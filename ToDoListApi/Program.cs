using Data;
using DbUp;
using Microsoft.EntityFrameworkCore;
using Migrations;
using ToDoListApi.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
  .AddJsonFile("appsettings.local.json", optional: true)
  .AddEnvironmentVariables();

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: MyAllowSpecificOrigins,
  builder =>
  {
    builder
      .AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowAnyMethod();
  });
});

builder.Services.AddDbContext<ToDoListDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetSection("DatabaseSettings:ConnectionString").Value);
});
builder.Services.AddControllers();

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<EmailService>();
builder.Services.AddHostedService<ConsumeScopedServiceHostedService>();
builder.Services.AddScoped<IScopedProcessingService, NotificationsScopedProcessingService>();
builder.Services.AddScoped<ToDoTaskService>();

var assemblies = AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "Migrations");

var upgrader = DeployChanges.To
    .SqlDatabase(builder.Configuration.GetSection("DatabaseSettings:ConnectionString").Value)
    .WithScriptsEmbeddedInAssembly(assemblies)
    .WithExecutionTimeout(TimeSpan.FromSeconds(180))
    .WithScriptNameComparer(new MigrationNameComparer())
    .Build();

var result = upgrader.PerformUpgrade();
if (!result.Successful)
{
  throw new ToDoListMigrationFailedException(result.Error);
}

var app = builder.Build();

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);

app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers();
});

app.Run();

public class ToDoListMigrationFailedException : Exception
{
  public ToDoListMigrationFailedException(Exception exception) : base($"Migration failed : {exception}") { }
}
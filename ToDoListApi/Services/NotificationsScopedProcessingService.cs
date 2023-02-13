using Data;

namespace ToDoListApi.Services
{
  internal interface IScopedProcessingService
  {
    Task DoWork(CancellationToken stoppingToken);
  }

  internal class NotificationsScopedProcessingService : IScopedProcessingService
  {
    private int executionCount = 0;
    private readonly ILogger _logger;
    private readonly EmailService _emailService;
    private readonly ToDoListDbContext _context;

    public NotificationsScopedProcessingService(
      ILogger<NotificationsScopedProcessingService> logger,
      EmailService emailService, 
      ToDoListDbContext context)
    {
      _logger = logger;
      _emailService = emailService;
      _context = context;
    }

    public async Task DoWork(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        executionCount++;

        var incomming = _context.ToDoTask
          .Where(x => x.State == Common.TaskState.ToDo && x.Day.Date <= DateTime.Today.AddDays(7) && x.Day.Date >= DateTime.Today)
          .Count();
        var delayed = _context.ToDoTask
          .Where(x => x.State == Common.TaskState.ToDo && x.Day.Date < DateTime.Today)
          .Count();

        var message = new Message(incomming, delayed);

        _emailService.SendEmail(message);

        _logger.LogInformation(
            "Scoped Processing Service is working. Count: {Count}", executionCount);
        await Task.Delay(86400000, stoppingToken);
      }
    }
  }

  public class ConsumeScopedServiceHostedService : BackgroundService
  {
    private readonly ILogger<ConsumeScopedServiceHostedService> _logger;

    public ConsumeScopedServiceHostedService(IServiceProvider services,
        ILogger<ConsumeScopedServiceHostedService> logger)
    {
      Services = services;
      _logger = logger;
    }

    public IServiceProvider Services { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(
          "Consume Scoped Service Hosted Service running.");

      await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
      _logger.LogInformation(
          "Consume Scoped Service Hosted Service is working.");

      using (var scope = Services.CreateScope())
      {
        var scopedProcessingService =
            scope.ServiceProvider
                .GetRequiredService<IScopedProcessingService>();

        await scopedProcessingService.DoWork(stoppingToken);
      }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation(
          "Consume Scoped Service Hosted Service is stopping.");

      await base.StopAsync(stoppingToken);
    }
  }

}

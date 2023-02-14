using DbUp;
using Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
  [CollectionDefinition("Database collection")]
  public class DatabaseCollection : ICollectionFixture<IntegrationTestsSetup>
  {
    // Class only for apply collection Fixture and all necessary interfaces
  }
  public class IntegrationTestsSetup : IDisposable
  {
    public string ConnectionString =>
             @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ToDoListTestDB; Integrated Security=True";

    private static bool _isInitialized;
    private static bool _isInitializing;

    public IntegrationTestsSetup()
    {
      if (_isInitialized)
      {
        return;
      }
      if (_isInitializing)
      {
        return;
      }
      _isInitializing = true;
      InitDb();
      _isInitialized = true;
    }
    private void InitDb()
    {
      DropDatabase.For.SqlDatabase(ConnectionString);
      EnsureDatabase.For.SqlDatabase(ConnectionString, "Polish_CI_AI");
      Thread.Sleep(new TimeSpan(0, 0, 5));

      UpdateDatabaseFromAssembly(new MigrationNameComparer());
    }

    private void UpdateDatabaseFromAssembly(MigrationNameComparer migrationNameComparer)
    {
      var assemblies = AppDomain.CurrentDomain.GetAssemblies();
      var databaseMigrationsAssembly = assemblies.Single(a => a.GetName().Name == "Migrations");
      UpdateFromAssembly(databaseMigrationsAssembly, migrationNameComparer);
    }

    private void UpdateFromAssembly(Assembly databaseMigrationsAssembly, IComparer<string> migrationNameComparer)
    {
      var upgrader = DeployChanges.To
          .SqlDatabase(ConnectionString)
          .WithScriptsEmbeddedInAssembly(databaseMigrationsAssembly)
          .WithExecutionTimeout(TimeSpan.FromSeconds(180))
          .WithScriptNameComparer(migrationNameComparer)
          .Build();

      var result = upgrader.PerformUpgrade();
      if (!result.Successful)
      {
        throw result.Error;
      }
    }
    public void Dispose()
    {

    }
  }
}

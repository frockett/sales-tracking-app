using DataAccess;
using Library;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace sales_app;

internal class Program
{
    static void Main(string[] args)
    {
        // selects implementation of IDbMethods
        var dataAccess = InitializeSqliteDatabase();

        //var stopwatchService = new StopwatchService();
        //var inputValidationService = new UserInputValidationService();
        var itemController = new ItemController(dataAccess);
        //var displayService = new DisplayService();
        var menuHandler = new MenuHandler(itemController);

        menuHandler.ShowMainMenu();
    }

    static IDataAccess InitializeSqliteDatabase()
    {
        string? connectionString;
        IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        connectionString = config.GetConnectionString("DefaultConnection");

        var sqliteDataAccess = new SqliteDataAccess(connectionString);
        sqliteDataAccess.InitDatabase();
        return sqliteDataAccess;
    }
}

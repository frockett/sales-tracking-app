using DataAccess;
using sales_app.Repositories;
using sales_app.Services;
using sales_app.UI;
using sales_app.Helpers;

namespace sales_app;

internal class Program
{
    static void Main(string[] args)
    {
        // selects implementation of IDbMethods
        var dataAccess = InitializeSqliteDatabase();

        //var stopwatchService = new StopwatchService();
        var inputValidation = new InputValidation();
        var itemController = new ItemService(dataAccess, inputValidation);
        var displayService = new DisplayService();
        var menuHandler = new MenuHandler(itemController, displayService);

        menuHandler.ShowMainMenu();
    }

    static IRepository InitializeSqliteDatabase()
    {
        string? connectionString;
        IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        connectionString = config.GetConnectionString("DefaultConnection");

        var sqliteDataAccess = new SqliteRepository(connectionString);
        sqliteDataAccess.InitDatabase();
        return sqliteDataAccess;
    }
}

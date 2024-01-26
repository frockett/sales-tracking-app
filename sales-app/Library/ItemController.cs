using DataAccess;

namespace Library;

public class ItemController
{
    private readonly IDataAccess dataAccess;
    //private readonly UserInputValidationService inputValidationService;
    //private readonly StopwatchService stopwatchService;

    public ItemController(IDataAccess dataAccess)
    {
        this.dataAccess = dataAccess;
    }

    public void SeedJanData()
    {
        int rowsAffected = dataAccess.SeedJanData();

        Console.WriteLine($"{rowsAffected} rows affected!");
    }

}

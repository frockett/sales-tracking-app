using Shared;

namespace DataAccess;

public interface IDataAccess
{
    public void InitDatabase();
    public void InsertItem(Item item);
    public void UpdateItem(Item item);
    public bool ValidateItemById(int id);
    public void DeleteItem(int id);
    public List<Item> GetAllItems();
    public List<Item> GetItems(int? year = null, int? month = null, string? groupBy = null, string? orderBy = null);
    /*
    public List<CodingSession> GetCustomCodingSessions(bool isListOfAverages, DateOnly date);
    public bool CheckForTableData(int year = 0, int month = 0);
    public void SeedRandomData(int iterations);
    */
    public int SeedJanData();
}

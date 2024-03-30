using sales_app.Models;

namespace sales_app.Repositories;

public class EFCoreSalesRepository : ISalesRepository
{
    public void DeleteItem(int id)
    {
        throw new NotImplementedException();
    }

    public void ExportToCSV()
    {
        throw new NotImplementedException();
    }

    public List<Item> GetAllItems()
    {
        throw new NotImplementedException();
    }

    public List<Item> GetItems(int? year = null, int? month = null, string? groupBy = null, string? orderBy = null)
    {
        throw new NotImplementedException();
    }

    public void InitDatabase()
    {
        throw new NotImplementedException();
    }

    public void InsertItem(Item item)
    {
        throw new NotImplementedException();
    }

    public int SeedJanData()
    {
        throw new NotImplementedException();
    }

    public void UpdateItem(Item item)
    {
        throw new NotImplementedException();
    }

    public bool ValidateItemById(int id)
    {
        throw new NotImplementedException();
    }
}

using sales_app.Models;

namespace sales_app.Repositories;

public class EFCoreSalesRepository : ISalesRepository
{
    private readonly SalesAndInventoryContext context;

    public EFCoreSalesRepository(SalesAndInventoryContext context)
    {
        this.context = context;
    }

    public void DeleteItem(int id)
    {
        throw new NotImplementedException();
    }

    public void ExportToCSV()
    {
        throw new NotImplementedException();
    }

    public List<Sale> GetAllItems()
    {
        return context.Sales.OrderBy(s => s.DateOfSale).ToList();
    }

    public List<Sale> GetItems(int? year = null, int? month = null, string? groupBy = null, string? orderBy = null)
    {
        throw new NotImplementedException();
    }

    public void InitDatabase()
    {
        throw new NotImplementedException();
    }

    public void InsertItem(Sale item)
    {
        throw new NotImplementedException();
    }

    public int SeedJanData()
    {
        throw new NotImplementedException();
    }

    public void UpdateItem(Sale item)
    {
        throw new NotImplementedException();
    }

    public bool ValidateItemById(int id)
    {
        throw new NotImplementedException();
    }
}

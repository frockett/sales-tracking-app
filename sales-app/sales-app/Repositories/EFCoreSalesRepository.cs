using sales_app.Models;
using System.Linq;

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
        var query = context.Sales.AsQueryable();

        if (year.HasValue)
        {
            query = query.Where(s => s.DateOfSale.Value.Year == year);
        }

        if (month.HasValue)
        {
            query = query.Where(s => s.DateOfSale.Value.Month == month);
        }

        // TODO implement other ordering schemes, currently it's always by date
        if (!String.IsNullOrEmpty(orderBy))
        {
            query.OrderBy(s => s.DateOfSale);
        }

        return query.ToList();
        
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

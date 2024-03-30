using Microsoft.EntityFrameworkCore;
using sales_app.Helpers;
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
        var sale = context.Sales.FirstOrDefault(s => s.Id == id);
        try
        {
            if (sale != null)
            {
                context.Sales.Remove(sale);
                context.SaveChanges();
            }
            else return;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
        
    }

    public void ExportToCSV()
    {
        string workingDirectory = Directory.GetCurrentDirectory();
        string csvFileName = "backupTEST.csv";
        string csvFilePath = Path.Combine(workingDirectory, csvFileName);

        var sales = context.Sales.ToList();

        if (sales.Any())
        {
            using var writer = new StreamWriter(csvFilePath, false);

            var header = string.Join(",", typeof(Sale).GetProperties().Select(p => p.Name));
            writer.WriteLine(header);

            foreach (var sale in sales)
            {
                var line = string.Join(",", typeof(Sale).GetProperties().Select(p => p.GetValue(sale)?.ToString()));
                writer.WriteLine(line);
            }
        }
    }

    public List<Sale> GetAllItems()
    {
        return context.Sales.OrderBy(s => s.DateOfSale).ToList();
    }

    public List<Sale> GetItems(DataOrder orderBy, int? year = null, int? month = null, string? groupBy = null)
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

        query = orderBy switch
        {
            DataOrder.Brand => query.OrderBy(s => s.Brand),
            DataOrder.Date_of_sale => query.OrderBy(s => s.DateOfSale),
            DataOrder.Type => query.OrderBy(s => s.Type),
            DataOrder.Cost => query.OrderBy(s => s.Cost),
            DataOrder.Sale_price => query.OrderBy(s => s.SalePrice),
            DataOrder.Profit => query.OrderBy(s => s.Profit),
            DataOrder.Margin => query.OrderBy(s => s.Margin),
            DataOrder.Platform => query.OrderBy(s => s.Platform),
            _ => query.OrderBy(s => s.DateOfSale)
        };

        return query.ToList();
    }

    public void InsertItem(Sale sale)
    {
        context.Sales.Add(sale);
        try
        {
            context.SaveChanges();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return;
        }
    }

    public void UpdateItem(Sale item)
    {
        throw new NotImplementedException();
    }

    public bool ValidateItemById(int id)
    {
        return context.Sales.Any(s => s.Id == id);
    }
}

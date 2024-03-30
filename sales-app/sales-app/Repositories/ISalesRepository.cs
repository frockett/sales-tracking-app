using sales_app.Models;

namespace sales_app.Repositories;

public interface ISalesRepository
{
    public void InsertItem(Sale item);
    public void UpdateItem(Sale item);
    public bool ValidateItemById(int id);
    public void DeleteItem(int id);
    public List<Sale> GetAllItems();
    public List<Sale> GetItems(int? year = null, int? month = null, string? groupBy = null, string? orderBy = null);
    public void ExportToCSV();
}

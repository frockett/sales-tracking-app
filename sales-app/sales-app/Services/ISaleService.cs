using sales_app.DTOs;
using sales_app.Models;

namespace sales_app.Services
{
    public interface ISaleService
    {
        SalesRecord CalculateSalesRecord(List<ItemDTO> items);
        void DeleteItem();
        void ExportToCSV();
        List<ItemDTO> FetchAllItems();
        List<ItemDTO> FetchItems(int? year = null, int? month = null, string? groupBy = null, string? orderBy = null);
        void InsertItem();
        void SeedJanData();
    }
}
using sales_app.DTOs;
using sales_app.Models;

namespace sales_app.Services
{
    public interface ISaleService
    {
        SalesRecord CalculateSalesRecord(List<SaleDTO> items);
        void DeleteItem();
        void ExportToCSV();
        List<SaleDTO> FetchAllItems();
        List<SaleDTO> FetchItems(int? year = null, int? month = null, string? groupBy = null, string? orderBy = null);
        void InsertItem();
        void SeedJanData();
    }
}
using sales_app.DTOs;
using sales_app.Helpers;
using sales_app.Models;
using sales_app.Repositories;
using Spectre.Console;

namespace sales_app.Services;

public class SaleService : ISaleService
{
    private readonly ISalesRepository repository;
    private readonly InputValidation inputValidation;

    public SaleService(ISalesRepository repository, InputValidation inputValidation)
    {
        this.repository = repository;
        this.inputValidation = inputValidation;
    }

    public void InsertItem()
    {
        SaleDTO itemDTOToConvert = inputValidation.GetItemInformation();
        Sale itemToInput = ItemMapper.ToDomainModel(itemDTOToConvert);
        repository.InsertItem(itemToInput);
    }

    public List<SaleDTO> FetchAllItems()
    {
        List<Sale> items = repository.GetAllItems();

        List<SaleDTO> itemDTOs = new List<SaleDTO>();

        foreach (Sale item in items)
        {
            SaleDTO itemDTO = ItemMapper.ToDTO(item);
            itemDTOs.Add(itemDTO);
        }
        return itemDTOs;
    }

    public List<SaleDTO> FetchItems(int? year = null, int? month = null, string? groupBy = null, string? orderBy = null) // I think these are unnecessary
    {
        List<Sale> items = new List<Sale>();
        string reportType = inputValidation.GetReportType();

        switch (reportType)
        {
            case "all items":
                items = repository.GetItems(inputValidation.GetOrderBy(), null, null, null);
                break;
            case "monthly":
                items = repository.GetItems(inputValidation.GetOrderBy(), DateTime.Now.Year, inputValidation.GetMonth().Month, null);
                break;
            case "yearly":
                items = repository.GetItems(inputValidation.GetOrderBy(), inputValidation.GetYear().Year, null, inputValidation.GetGroupBy());
                break;
            case "goal":
                break;
        }

        List<SaleDTO> itemDTOs = new List<SaleDTO>();

        foreach (Sale item in items)
        {
            SaleDTO itemDTO = ItemMapper.ToDTO(item);
            itemDTOs.Add(itemDTO);
        }
        return itemDTOs;
    }

    public SalesRecord CalculateSalesRecord(List<SaleDTO> items)
    {
        SalesRecord record = new SalesRecord();
        record.TotalSales = 0;
        record.GrossProfit = 0;

        if (!items.Any())
        {
            return record;
        }
        else
        {
            int? totalMargin = 0;

            record.Month = items.First().DateOfSale.Value.Month;
            record.Year = items.First().DateOfSale.Value.Year;

            foreach (SaleDTO item in items)
            {
                record.TotalSales += item.SalePrice;
                record.GrossProfit += item.Profit;
                totalMargin += item.Margin;
            }

            record.AvgRevenue = record.TotalSales / items.Count();
            record.AvgProfit = record.GrossProfit / items.Count();
            record.AvgMargin = totalMargin / items.Count();

            return record;
        }
    }

    public void ExportToCSV()
    {
        repository.ExportToCSV();
    }

    public void DeleteItem()
    {
        int idToDelete = inputValidation.GetIntData("Enter the Id of the item to delete, or enter 0 to cancel: ");
        if (idToDelete <= 0)
        {
            return;
        }

        Sale? itemToDelete = repository.ValidateItemById(idToDelete);

        while (itemToDelete == null)
        {
            AnsiConsole.WriteLine($"[red]Item with ID {idToDelete} does not exist");
            idToDelete = inputValidation.GetIntData("Enter a valid item ID: ");
        }

        if (!AnsiConsole.Confirm($"Delete {itemToDelete.Brand} {itemToDelete.Type} with description {itemToDelete.Description} sold on {itemToDelete.DateOfSale}?"))
        {
            return;
        }
        else repository.DeleteItem(itemToDelete.Id);
    }

    public List<SummaryDto> FetchSummaries()
    {
        var sales = repository.GetAllItems();
        List<SaleDTO> saleDtos = new List<SaleDTO>();

        foreach (Sale sale in sales)
        {
            SaleDTO itemDTO = ItemMapper.ToDTO(sale);
            saleDtos.Add(itemDTO);
        }

        var summaries = saleDtos.GroupBy(s => new { s.DateOfSale.Value.Year, s.DateOfSale.Value.Month })
                                .Select(g => new SummaryDto
                                {
                                    Year = g.Key.Year,
                                    Month = g.Key.Month,
                                    SalesRecord = CalculateSalesRecord(g.ToList())
                                }).ToList();

        return summaries;
    }
}

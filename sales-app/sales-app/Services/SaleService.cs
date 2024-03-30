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
                items = repository.GetItems(null, null, null, inputValidation.GetOrderBy());
                break;
            case "monthly":
                items = repository.GetItems(DateTime.Now.Year, inputValidation.GetMonth().Month, null, inputValidation.GetOrderBy());
                break;
            case "yearly":
                items = repository.GetItems(inputValidation.GetYear().Year, null, inputValidation.GetGroupBy(), inputValidation.GetOrderBy());
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

    public void SeedJanData()
    {
        int rowsAffected = repository.SeedJanData();

        Console.WriteLine($"{rowsAffected} rows affected!");
    }

    public void DeleteItem()
    {
        int itemToDelete = inputValidation.GetIntData("Enter the Id of the item to delete, or enter 0 to cancel: ");

        if (itemToDelete <= 0)
        {
            return;
        }

        if (!repository.ValidateItemById(itemToDelete))
        {
            AnsiConsole.WriteLine($"[red]Item with ID {itemToDelete} does not exist");
            itemToDelete = inputValidation.GetIntData("Enter a valid item ID: ");
        }

        repository.DeleteItem(itemToDelete);
    }
}

using sales_app.DTOs;
using sales_app.Helpers;
using sales_app.Models;
using sales_app.Repositories;
using Spectre.Console;

namespace sales_app.Services;

public class ItemService
{
    private readonly IRepository repository;
    private readonly InputValidation inputValidation;

    public ItemService(IRepository repository, InputValidation inputValidation)
    {
        this.repository = repository;
        this.inputValidation = inputValidation;
    }

    public void InsertItem()
    {
        ItemDTO itemDTOToConvert = inputValidation.GetItemInformation();
        Item itemToInput = ItemMapper.ToDomainModel(itemDTOToConvert);
        repository.InsertItem(itemToInput);
    }

    public List<ItemDTO> FetchAllItems()
    {
        List<Item> items = repository.GetAllItems();

        List<ItemDTO> itemDTOs = new List<ItemDTO>();

        foreach (Item item in items)
        {
            ItemDTO itemDTO = ItemMapper.ToDTO(item);
            itemDTOs.Add(itemDTO);
        }
        return itemDTOs;
    }

    public List<ItemDTO> FetchItems(int? year = null, int? month = null, string? groupBy = null, string? orderBy = null) // I think these are unnecessary
    {
        List<Item> items = new List<Item>();
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

        List<ItemDTO> itemDTOs = new List<ItemDTO>();

        foreach (Item item in items)
        {
            ItemDTO itemDTO = ItemMapper.ToDTO(item);
            itemDTOs.Add(itemDTO);
        }
        return itemDTOs;
    }

    public SalesRecord CalculateSalesRecord(List<ItemDTO> items)
    {
        SalesRecord record = new SalesRecord();

        if (!items.Any())
        {
            return record;
        }
        else
        {
            float totalMargin = 0f;

            record.Month = items.First().DateOfSale.Month;
            record.Year = items.First().DateOfSale.Year;

            foreach (ItemDTO item in items)
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

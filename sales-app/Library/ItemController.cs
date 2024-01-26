﻿using DataAccess;
using Shared;
using Spectre.Console;
using System.Data;

namespace Library;

public class ItemController
{
    private readonly IDataAccess dataAccess;
    private readonly InputValidation inputValidation;
    //private readonly StopwatchService stopwatchService;

    public ItemController(IDataAccess dataAccess, InputValidation inputValidation)
    {
        this.dataAccess = dataAccess;
        this.inputValidation = inputValidation;
    }

    public void InsertItem()
    {
        ItemDTO itemDTOToConvert = inputValidation.GetItemInformation();
        Item itemToInput = ItemMapper.ToDomainModel(itemDTOToConvert);
        dataAccess.InsertItem(itemToInput);
    }

    public List<ItemDTO> FetchAllItems()
    {
        List<Item> items = dataAccess.GetAllItems();

        List<ItemDTO> itemDTOs = new List<ItemDTO>();

        foreach (Item item in items)
        {
            ItemDTO itemDTO = ItemMapper.ToDTO(item);
            itemDTOs.Add(itemDTO);
        }
        return itemDTOs;
    }

    public List<ItemDTO> FetchItems(int? year = null, int? month = null, string? groupBy = null, string? orderBy = null)
    {
        List<Item> items = new List<Item>();        
        string reportType = inputValidation.GetReportType();

        switch (reportType)
        {
            case "all items":
                items = dataAccess.GetItems(null, null, null, inputValidation.GetOrderBy());
                break;
            case "monthly":
                items = dataAccess.GetItems(DateTime.Now.Year, inputValidation.GetMonth().Month, null, inputValidation.GetOrderBy());
                break;
            case "yearly":
                items = dataAccess.GetItems(inputValidation.GetYear().Year, null, inputValidation.GetGroupBy(), inputValidation.GetOrderBy());
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

    public void SeedJanData()
    {
        int rowsAffected = dataAccess.SeedJanData();

        Console.WriteLine($"{rowsAffected} rows affected!");
    }

    public void DeleteItem()
    {
        int itemToDelete = inputValidation.GetIntData("Enter the Id of the item to delete, or enter 0 to cancel: ");

        if (itemToDelete <= 0)
        {
            return;
        }

        if (!dataAccess.ValidateItemById(itemToDelete))
        {
            AnsiConsole.WriteLine($"[red]Item with ID {itemToDelete} does not exist");
            itemToDelete = inputValidation.GetIntData("Enter a valid item ID: ");
        }

        dataAccess.DeleteItem(itemToDelete);
    }

}

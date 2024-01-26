using DataAccess;
using Shared;

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

    public void SeedJanData()
    {
        int rowsAffected = dataAccess.SeedJanData();

        Console.WriteLine($"{rowsAffected} rows affected!");
    }

}

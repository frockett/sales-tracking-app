using Shared;

namespace Library;

public static class ItemMapper
{
    public static ItemDTO ToDTO(Item item)
    {
        return new ItemDTO
        {
            Id = item.Id,
            Brand = item.Brand,
            Type = item.Type,
            Cost = item.Cost,
            SalePrice = item.SalePrice,
            Profit = item.Profit,
            Margin = item.Margin,
            DateOfSale = item.DateOfSale,
            Platform = item.Platform,
            Description = item.Description,
        };
    }

    public static Item ToDomainModel(ItemDTO dto)
    {
        return new Item
        {
            Brand = dto.Brand,
            Type = dto.Type,
            Cost = dto.Cost,
            SalePrice = dto.SalePrice,
            Profit = dto.Profit,
            Margin = dto.Margin,
            DateOfSale = dto.DateOfSale,
            Platform = dto.Platform,
            Description = dto.Description,
        };
    }
}

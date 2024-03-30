using sales_app.DTOs;
using sales_app.Models;

namespace sales_app.Helpers;

public static class ItemMapper
{
    public static SaleDTO ToDTO(Sale item)
    {
        //Console.WriteLine($"Current item ID is {item.Id}");
        return new SaleDTO
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

    public static Sale ToDomainModel(SaleDTO dto)
    {
        return new Sale
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

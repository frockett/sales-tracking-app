﻿namespace sales_app.DTOs;

public class SaleDTO
{
    public int Id { get; set; }
    public string? Brand { get; set; }
    public string? Type { get; set; }
    public int? Cost { get; set; }
    public int? SalePrice { get; set; }
    public int? Profit { get; set; }
    public int? Margin { get; set; }
    public DateOnly? DateOfSale { get; set; }
    public string? Platform { get; set; }
    public string? Description { get; set; }
}

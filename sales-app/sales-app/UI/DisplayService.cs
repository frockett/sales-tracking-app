using sales_app.DTOs;
using sales_app.Models;
using Spectre.Console;

namespace sales_app.UI;

internal class DisplayService
{
    public void PrintItemList(List<ItemDTO> itemDTOs, SalesRecord? salesRecord = null, bool shouldWait = true)
    {
        Table table = new Table();
        table.Title("Sales Data");
        table.AddColumns(new[] { "Id", "Brand", "Type", "Cost", "Sale Price", "Profit", "Margin", "Date of Sale", "Platform", "Description" });

        foreach (ItemDTO item in itemDTOs)
        {
            table.AddRow(item.Id.ToString(), item.Brand, item.Type, item.Cost.ToString() + " rmb",
                        item.SalePrice.ToString() + " rmb", item.Profit.ToString() + " rmb", item.Margin.ToString("F1") + "%",
                        item.DateOfSale.ToString("MM-dd"), item.Platform, item.Description);
        }
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        if (salesRecord != null)
        {
            Table aggTable = new Table();
            aggTable.AddColumns(new[] { "Total Revenue", "Total Profit", "Avg Revenue", "Avg Profit", "Avg Markup" });

            aggTable.AddRow(salesRecord.TotalSales.ToString(), salesRecord.GrossProfit.ToString(), salesRecord.AvgRevenue.ToString(), salesRecord.AvgProfit.ToString(), salesRecord.AvgMargin.ToString("F1") + "%");
            AnsiConsole.Write(aggTable);
        }


        if (shouldWait)
        {
            AnsiConsole.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}

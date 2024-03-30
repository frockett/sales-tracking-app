using sales_app.DTOs;
using sales_app.Models;
using Spectre.Console;
using System.Globalization;

namespace sales_app.UI;

internal class DisplayService
{
    public void PrintItemList(List<SaleDTO> itemDTOs, SalesRecord? salesRecord = null, bool shouldWait = true)
    {
        Table table = new Table();
        table.Title("Sales Data");
        table.AddColumns(new[] { "Id", "Brand", "Type", "Cost", "Sale Price", "Profit", "Margin", "Date of Sale", "Platform", "Description" });

        foreach (SaleDTO item in itemDTOs)
        {
            table.AddRow(item.Id.ToString(), item.Brand, item.Type, item.Cost.ToString() + " rmb",
                        item.SalePrice.ToString() + " rmb", item.Profit.ToString() + " rmb", item.Margin.ToString() + "%",
                        item.DateOfSale.Value.ToString("yyyy-MM-dd"), item.Platform, item.Description);
        }
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        if (salesRecord != null)
        {
            Table aggTable = new Table();
            aggTable.AddColumns(new[] { "Total Revenue", "Total Profit", "Avg Revenue", "Avg Profit", "Avg Markup" });

            aggTable.AddRow(salesRecord.TotalSales.ToString(), salesRecord.GrossProfit.ToString(), salesRecord.AvgRevenue.ToString(), salesRecord.AvgProfit.ToString(), salesRecord.AvgMargin.ToString() + "%");
            AnsiConsole.Write(aggTable);
        }

        if (shouldWait)
        {
            AnsiConsole.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }

    public void PrintBarChart(List<SummaryDto> summaries)
    {
        var revenueChart = new BarChart()
                    .Width(200)
                    .Label("[green bold underline]Total Sales[/]")
                    .CenterLabel();

        foreach (SummaryDto summary in summaries)
        {
            int value = summary.SalesRecord.TotalSales.Value;
            string? monthName = CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(summary.Month);
            Color color = GetColorForValue(value, summaries.Min(s => s.SalesRecord.TotalSales.Value), summaries.Max(s => s.SalesRecord.TotalSales.Value));
            revenueChart.AddItem($"{monthName}", Convert.ToDouble(summary.SalesRecord.TotalSales), color);
            Console.WriteLine(color.ToString());
        }

        AnsiConsole.Write(revenueChart);
        AnsiConsole.WriteLine("\n\n");

        var profitChart = new BarChart()
            .Width(200)
            .Label("[green bold underline]Total Profit[/]")
            .CenterLabel();

        foreach (SummaryDto summary in summaries)
        {
            int value = summary.SalesRecord.GrossProfit.Value;
            string? monthName = CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(summary.Month);
            Color color = GetColorForValue(value, summaries.Min(s => s.SalesRecord.GrossProfit.Value), summaries.Max(s => s.SalesRecord.GrossProfit.Value));
            profitChart.AddItem($"{monthName}", Convert.ToDouble(summary.SalesRecord.GrossProfit), color);
            Console.WriteLine(color.ToString());
        }

        AnsiConsole.Write(profitChart);
        AnsiConsole.WriteLine("\n\n");

        Console.ReadLine();
    }

    private Color GetColorForValue(int value, int min, int max)
    {
        double ratio = Convert.ToDouble((value - min)) / Convert.ToDouble((max - min));

        byte red, green;  

        if (ratio <= 0.5)
        {
            red = 255;
            green = (byte)(ratio * 2 * 255); 
        }
        else
        {
            // Transition from yellow to green
            green = 255; 
            red = (byte)((1 - (ratio - 0.5) * 2) * 255); 
        }
        return new Color(red, green, 0);
    }
}

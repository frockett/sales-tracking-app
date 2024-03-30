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

    // TODO - implement ability to print out bar chart that displays user-chosen info by month. For example, profit by month.
    /* 1. Get all data from database
     * 2. Sort data by month somehow, whether LINQ or separate lists or what idk
     * 3. Get a SalesRecord for each month. Maybe I need a new data type for this that holds the month info too?
     * 4. Send the List of SalesRecords to the print chart method
     * 5. Get the highest number to set the width
     * 6. Do a ForEach to AddItem for each month that has data
     */
    public void PrintBarChart(List<SummaryDto> summaries)
    {
        var revenueChart = new BarChart()
                    .Width(200)
                    .Label("[green bold underline]Total Sales[/]")
                    .CenterLabel();

        foreach (SummaryDto summary in summaries)
        {
            string? monthName = CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(summary.Month);
            revenueChart.AddItem($"{monthName}", Convert.ToDouble(summary.SalesRecord.TotalSales), Color.Green);
        }

        AnsiConsole.Write(revenueChart);
        AnsiConsole.WriteLine("\n\n");

        var profitChart = new BarChart()
            .Width(200)
            .Label("[green bold underline]Number of fruits[/]")
            .CenterLabel();

        foreach (SummaryDto summary in summaries)
        {
            string? monthName = CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(summary.Month);
            profitChart.AddItem($"{monthName}", Convert.ToDouble(summary.SalesRecord.GrossProfit), Color.Green);
        }

        AnsiConsole.Write(profitChart);

        /*
        .AddItem("Apple", 12000, Color.Yellow)
        .AddItem("Orange", 5400, Color.Green)
        .AddItem("Banana", 3300, Color.Red);
        */

        Console.ReadLine();
    }
}

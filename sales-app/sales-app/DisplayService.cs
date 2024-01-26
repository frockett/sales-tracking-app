using Shared;
using Spectre.Console;

namespace sales_app;

internal class DisplayService
{
    //bool shouldWaitForInput;
    public void PrintItemList(List<ItemDTO> itemDTOs, bool shouldWait = true)
    {
        Table table = new Table();
        table.AddColumns(new[] { "Id", "Brand", "Type", "Cost", "Sale Price", "Profit", "Margin", "Date of Sale", "Platform", "Description" });

        foreach (ItemDTO item in itemDTOs)
        {
            table.AddRow(item.Id.ToString(),item.Brand, item.Type, item.Cost.ToString() + " rmb", 
                        item.SalePrice.ToString() + " rmb", item.Profit.ToString() + " rmb", item.Margin.ToString("F1") + "%", 
                        item.DateOfSale.ToString("MM-dd"), item.Platform, item.Description);
        }
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        if (shouldWait)
        {
            AnsiConsole.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
    /*
    public void PrintMonthlyAverages(List<CodingSession> sessions)
    {
        Table table = new Table();
        table.Title("Monthly Averages");
        table.AddColumn("Month");
        table.AddColumn("Total Time");
        table.AddColumn("Average Per Session");
        table.AddColumn("Average Per Day");

        foreach (var session in sessions)
        {
            table.AddRow(session.Month, session.TotalTime.ToString() + " hours total", session.AverageTime.ToString() + " hours per session", Math.Round((session.TotalTime / 30), 1).ToString() + " hours per day");
        }
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine("Press enter to continue");
        Console.ReadLine();
    }
    */

}

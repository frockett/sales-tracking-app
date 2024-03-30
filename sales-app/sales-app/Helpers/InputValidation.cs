using Spectre.Console;
using System.Globalization;
using sales_app.DTOs;

namespace sales_app.Helpers;

public class InputValidation
{
    public string GetReportType()
    {
        string[] reportMenuOptions = {"Display All Items in Database", "Display Sales for Month", "Display Annual Report",
                                     "Display Goal Report - UNDER CONSTRUCTION" };

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Which type of report do you want to see?")
            .PageSize(10)
            .MoreChoicesText("Keep scrolling for more options")
            .AddChoices(reportMenuOptions));

        int menuSelection = Array.IndexOf(reportMenuOptions, choice) + 1;

        string? reportType = "";

        switch (menuSelection)
        {
            case 1:
                reportType = "all items";
                break;
            case 2:
                reportType = "monthly";
                break;
            case 3:
                reportType = "yearly";
                break;
            case 4:
                reportType = "goal";
                break;
        }
        AnsiConsole.Clear();
        return reportType;
    }

    public DateTime GetMonth()
    {
        string month = AnsiConsole.Ask<string>("\nEnter the month in the format 'mm' : \n");
        if (!DateTime.TryParseExact(month, "MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime cleanMonth))
        {
            AnsiConsole.Markup($"\n[red]{month} is not a valid month[/], make sure you input a month in a format like '01' for January\n");
            month = AnsiConsole.Ask<string>("Please try again: ");
        }
        return cleanMonth;
    }

    public DateTime GetYear()
    {
        int year = AnsiConsole.Ask<int>("\nEnter the year you'd like to view in the format 'yyyy' : \n");

        if (!DateTime.TryParseExact(year.ToString(), "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime cleanYear))
        {
            AnsiConsole.Markup($"\n[red]{year} is not a valid year[/], make sure you input a month in a format like '2024'\n");
            year = AnsiConsole.Ask<int>("Please try again: ");
        }
        return cleanYear;
    }

    public string GetGroupBy()
    {
        string[] groupOptions = Enum.GetNames(typeof(DataOrderingEnum));

        string selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("\nWhich column do you want to group by?\n")
            .PageSize(10)
            .MoreChoicesText("Keep scrolling for more options")
            .AddChoices(groupOptions));

        AnsiConsole.Clear();
        return selection;
    }

    public string GetOrderBy()
    {
        string[] orderOptions = Enum.GetNames(typeof(DataOrderingEnum));

        string selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("\nWhich column do you want to order by?\n")
            .PageSize(10)
            .MoreChoicesText("Keep scrolling for more options")
            .AddChoices(orderOptions));

        AnsiConsole.Clear();
        return selection;
    }

    public ItemDTO GetItemInformation()
    {
        string? brand = GetStringData("Brand?\n");
        string? type = GetStringData("Type?\n");
        int cost = GetIntData("Cost to purchase?\n");
        int salePrice = GetIntData("Price at which the item sold?\n");
        int profit = GetProfit(cost, salePrice);
        int margin = GetMargin(cost, salePrice);
        DateOnly date = GetDateOfSale();
        string? platform = GetStringData("Platform of sale?\n");
        string? description = GetStringData("Enter brief description: ");

        return new ItemDTO
        {
            Brand = brand,
            Type = type,
            Cost = cost,
            SalePrice = salePrice,
            Profit = profit,
            Margin = margin,
            DateOfSale = date,
            Platform = platform,
            Description = description
        };
    }

    private int GetMargin(int cost, int revenue)
    {
        int profit = revenue - cost;
        int margin = profit / cost * 100;
        AnsiConsole.WriteLine($"Margin is {margin}%");
        return margin;
    }

    private int GetProfit(int expense, int revenue)
    {
        int profit = revenue - expense;
        return profit;
    }

    private DateOnly GetDateOfSale()
    {
        DateTime dateOfSale;

        string selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("\nWhen was the sale made? Use [green]arrow[/] and [green]enter[/] keys to make a selection.\n")
                    .PageSize(10)
                    .MoreChoicesText("Keep scrolling for more options")
                    .AddChoices(new[] { "Today", "Enter date manually" }));

        if (selection == "Today")
        {
            dateOfSale = DateTime.Today;
            return DateOnly.FromDateTime(dateOfSale);
        }
        else
        {
            string monthAndDate = AnsiConsole.Ask<string>("Enter month and day of sale as mm-dd");
            string year = DateTime.Now.Year.ToString();
            string sDate = $"{year}-{monthAndDate}";

            while (!DateTime.TryParseExact(sDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfSale))
            {
                AnsiConsole.WriteLine($"{monthAndDate} is not a valid date in mm-dd format. Please re-enter");
                monthAndDate = AnsiConsole.Ask<string>("Enter month and day of sale as mm-dd");
            }
            Console.WriteLine(dateOfSale.ToString());
            return DateOnly.FromDateTime(dateOfSale);
        }
    }

    public int GetIntData(string prompt)
    {
        int input = AnsiConsole.Prompt(
            new TextPrompt<int>(prompt).ValidationErrorMessage("Please input an integer"));

        return input;

        //int input = AnsiConsole.Ask<int>(prompt).ValidationErrorMessage("Please input an integer price in CNY");
    }

    private string? GetStringData(string prompt)
    {
        string input = AnsiConsole.Ask<string>(prompt);

        while (input == null)
        {
            input = AnsiConsole.Ask<string>("Input cannot be null");
        }
        return input;
    }
}

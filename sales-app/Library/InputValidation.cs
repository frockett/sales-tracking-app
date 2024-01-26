using Spectre.Console;
using Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System;

namespace Library;

public class InputValidation
{
    public ItemDTO GetItemInformation()
    {
        string? brand = GetStringData("Brand?\n");
        string? type = GetStringData("Type?\n");
        int cost = GetIntData("Cost to purchase?\n");
        int salePrice = GetIntData("Price at which the item sold?\n");
        int profit = GetProfit(cost, salePrice);
        float margin = GetMargin(cost, salePrice);
        DateTime date = GetDateOfSale();
        string? platform = GetStringData("Platform of sale?\n");
        string? description = GetStringData("Enter brief description: ");

        return new ItemDTO { Brand = brand, Type = type, Cost = cost, SalePrice = salePrice, Profit = profit, Margin = margin, DateOfSale = date, Platform = platform, Description = description };
    }

    private float GetMargin(int profit, int revenue)
    {
        float margin = ((float)profit / (float)revenue) * 100;
        AnsiConsole.WriteLine($"Margin is {margin}%");
        return margin;
    }

    private int GetProfit(int expense, int revenue)
    {
        int profit = revenue - expense;
        return profit;
    }

    private DateTime GetDateOfSale()
    {
        DateTime dateOfSale;

        string selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("\nWhen was the sale made? Use [green]arrow[/] and [green]enter[/] keys to make a selection.\n")
                    .PageSize(10)
                    .MoreChoicesText("Keep scrolling for more options")
                    .AddChoices(new[] {"Today", "Enter date manually"}));

        if (selection == "Today")
        {
            dateOfSale = DateTime.Today;
            return dateOfSale;
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
            return dateOfSale;
        }
    }

    private int GetIntData(string prompt)
    {
        int input = AnsiConsole.Prompt(
            new TextPrompt<int>(prompt).ValidationErrorMessage("Please input an integer price in CNY"));

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

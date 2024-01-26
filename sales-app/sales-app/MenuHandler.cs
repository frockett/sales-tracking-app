using Library;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sales_app;

internal class MenuHandler
{
    private readonly ItemController itemController;
    private readonly DisplayService displayService;

    public MenuHandler(ItemController itemController, DisplayService display)
    {
        this.itemController = itemController;
        displayService = display;
    }

    public void ShowMainMenu()
    {
        AnsiConsole.Clear();

        string[] menuOptions =
                {"Insert Item", "Modify Item",
                "Delete Item", "BLANK",
                "Generate Reports", "DEVELOPER TOOLS: Seed Data", "Exit Program",};

        string choice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("Which operation would you like to perform? Use [green]arrow[/] and [green]enter[/] keys to make a selection.")
                            .PageSize(10)
                            .MoreChoicesText("Keep scrolling for more options")
                            .AddChoices(menuOptions));

        /* Before, the menu selection was parsed based on an int.parse of the first character, which was a number. 
        *  But having the numbers could confuse the user, since you can't input a number in the menu.
        *  So instead, menuSelection is the index in the menu array + 1 (the +1 is for ease of human readability) */

        int menuSelection = Array.IndexOf(menuOptions, choice) + 1;

        switch (menuSelection)
        {
            case 1:
                HandleInsertItem();
                break;
            case 2:
                //HandleUpdateRecord();
                break;
            case 3:
                //HandleDeleteRecord();
                break;
            case 4:
                //HandleStartCodingSession();
                break;
            case 5:
                HandleReportSubmenu();
                break;
            case 6:
                HandleSeedData();
                break;
            case 7:
                Environment.Exit(0);
                break;
        }
    }

    private void HandleInsertItem()
    {
        itemController.InsertItem();
        ShowMainMenu();
    }

    private void HandleSeedData()
    {
        if (!AnsiConsole.Confirm("Are you sure you want to seed January's data? WARNING: only seed Jan data if database is empty"))
        {
            AnsiConsole.Markup("\nOkay! Press any key to return to the main menu\n");
            Console.ReadKey(true);
            ShowMainMenu();
        }
        else
        {
            itemController.SeedJanData();
            ShowMainMenu();
        }
    }

    private void HandleReportSubmenu()
    {
        string[] reportMenuOptions =
        {"Display All Items in Database", "Display Sales by Month", "Display Monthly Averages",
                "Display Goal Report - UNDER CONSTRUCTION", "Return to Main Menu",};

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Which operation would you like to perform? Use [green]arrow[/] and [green]enter[/] keys to make a selection.")
            .PageSize(10)
            .MoreChoicesText("Keep scrolling for more options")
            .AddChoices(reportMenuOptions));

        int menuSelection = Array.IndexOf(reportMenuOptions, choice) + 1;

        switch (menuSelection)
        {
            case 1:
                displayService.PrintItemList(itemController.FetchAllItems());
                ShowMainMenu();
                break;
            case 2:
                ShowMainMenu();
                break;
            case 3:
                ShowMainMenu();
                break;
            case 4:
                AnsiConsole.Markup("Sorry, this feature isn't ready yet! The developer hasn't met his goal either!!");
                Console.ReadLine();
                ShowMainMenu();
                break;
            case 5:
                ShowMainMenu();
                break;
            default:
                AnsiConsole.Markup("[red]Invalid input![/]");
                break;
        }
    }
}

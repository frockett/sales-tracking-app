﻿using sales_app.DTOs;
using sales_app.Models;
using sales_app.Services;
using Spectre.Console;

namespace sales_app.UI;

internal class MenuHandler
{
    private readonly ISaleService saleService;
    private readonly DisplayService displayService;

    public MenuHandler(ISaleService saleService, DisplayService display)
    {
        this.saleService = saleService;
        displayService = display;
    }

    public void ShowMainMenu()
    {
        AnsiConsole.Clear();

        string[] menuOptions =
                {"Insert Item",
                "Delete Item",
                "Generate Reports",
                "View Graph",
                "Export Database to CSV",
                "Exit Program",};

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
                HandleDeleteItem();
                break;
            case 3:
                HandleReports();
                break;
            case 4:
                HandleViewGraphs();
                break;
            case 5:
                HandleExportToCSV();
                break;
            case 6:
                Environment.Exit(0);
                break;
        }
    }

    private void HandleInsertItem()
    {
        saleService.InsertItem();
        ShowMainMenu();
    }

    private void HandleDeleteItem()
    {
        displayService.PrintItemList(saleService.FetchAllItems(), null, false);
        saleService.DeleteItem();
        ShowMainMenu();
    }

    private void HandleReports()
    {
        List<SaleDTO> itemsToDisplay = saleService.FetchItems();
        SalesRecord salesRecordToDisplay = saleService.CalculateSalesRecord(itemsToDisplay);

        if (!itemsToDisplay.Any() || !salesRecordToDisplay.HasInformation())
        {
            AnsiConsole.Markup("[red]There is no matching data to display[/], please try another query. \nPress enter to return to main menu...");
            Console.ReadLine();
            ShowMainMenu();
        }
        else
        {
            displayService.PrintItemList(itemsToDisplay, salesRecordToDisplay);
            ShowMainMenu();
        }
    }
    private void HandleViewGraphs()
    {
        List<SummaryDto> summaries = saleService.FetchSummaries();
        displayService.PrintBarCharts(summaries);
        ShowMainMenu();
    }

    private void HandleExportToCSV()
    {
        saleService.ExportToCSV();

        // This doesn't actually represent progress, it just looks cute.
        AnsiConsole.Status()
            .Start("Initiating backup", ctx =>
            {
                AnsiConsole.MarkupLine("Writing table");
                Thread.Sleep(1000);

                ctx.Status("Writing more data to table");
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("green"));

                AnsiConsole.MarkupLine("Finalizing backup...");
                Thread.Sleep(1000);
            });

        AnsiConsole.Markup("[green]Backup to CSV completed successfully.[/] Press enter to return to main menu...");
        Console.ReadLine();

        ShowMainMenu();
    }
}

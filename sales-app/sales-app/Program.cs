using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using sales_app.Repositories;
using sales_app.Services;
using sales_app.UI;
using sales_app.Models;
using sales_app.Helpers;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

builder.Logging.ClearProviders(); // Clears default logging providers
builder.Logging.AddConsole(); // Adds console logging
builder.Logging.SetMinimumLevel(LogLevel.Information); // Sets the global minimum log level to Information
// Specifically reduce EF Core command logging level to Warning
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

builder.Services.AddDbContext<SalesAndInventoryContext>(options =>
{
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ISalesRepository, EFCoreSalesRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<MenuHandler>();
builder.Services.AddScoped<UserInput>();
builder.Services.AddScoped<InputValidation>();
builder.Services.AddScoped<DisplayService>();

var app = builder.Build();

var scope = app.Services.CreateScope();

var menuHandler = app.Services.GetRequiredService<MenuHandler>();

menuHandler.ShowMainMenu();

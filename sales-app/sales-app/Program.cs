using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using sales_app.Repositories;
using sales_app.Services;
using sales_app.UI;
using sales_app.Models;

var builder = Host.CreateApplicationBuilder();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


builder.Services.AddDbContext<SalesAndInventoryContext>(options =>
{
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ISalesRepository, EFCoreSalesRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<MenuHandler>();
builder.Services.AddScoped<UserInput>();
builder.Services.AddScoped<DisplayService>();

var app = builder.Build();

var scope = app.Services.CreateScope();

var menuHandler = app.Services.GetRequiredService<MenuHandler>();

menuHandler.ShowMainMenu();


/*
IRepository InitializeSqliteDatabase()
{
    string? connectionString;
    connectionString = configuration.GetConnectionString("DefaultConnection");

    var sqliteDataAccess = new SqliteRepository(connectionString);
    //sqliteDataAccess.InitDatabase();
    return sqliteDataAccess;
}
*/
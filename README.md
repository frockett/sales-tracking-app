# Personal Vintage Clothing Sales Tracker
Perform simple CRUD operations on an SQLite database that tracks sales information, all in console.

## Features
- Automatically calculate profit and markup.
- Generate a variety of reports that display sales for a given period, total revenue, total profit, average markup per item, and other useful business information.
  
![image](https://github.com/frockett/sales-tracking-app/assets/119839633/4b60cd9a-6470-4e8a-9f06-4957818f238c)

- Analyze sales data in terms of brand, profit, and other relevant metrics.
- Export database contents to CSV file for portability or data back-up.
  
![image](https://github.com/frockett/sales-tracking-app/assets/119839633/156ff598-a493-4f91-b433-43c728bb1f01)

### Technologies/Packages Used
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Microsoft.Data.SQLite](https://www.nuget.org/packages/Microsoft.Data.Sqlite/)
	- Communication with SQLite database
- [Spectre.Console](https://github.com/spectreconsole/spectre.console)
	- User input, table generation, and QOL features

## Operation
- Use the arrow and enter keys to navigate menus and submenus
- Follow on-screen prompts to enter information
- Use the reports submenu to generate reports for the desired period


## Prospective Features
- More custom report functions including sales by brand, platform, etc.
- GUI for ease of use and more flexible input.

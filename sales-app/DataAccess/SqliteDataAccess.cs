using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public class SqliteDataAccess : IDataAccess
{
    private readonly string? connectionString;

    public SqliteDataAccess(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void InitDatabase()
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS sales(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Brand TEXT,
                    Type TEXT,
                    Cost INT,
                    Sale_price INT,
                    Profit INT,
                    Margin DECIMAL,
                    Date_of_sale TEXT,
                    Platform TEXT,
                    Description TEXT)";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    private string? GetConnectionStringFromSettings(string desiredString)
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
        IConfiguration configuration = builder.Build();
        return configuration.GetConnectionString(desiredString);
    }
    public void InsertItem()
    {
        throw new NotImplementedException();
    }
    public void UpdateItem()
    {
        throw new NotImplementedException ();
    }
    public bool ValidateItemById(int id)
    {
        throw new NotImplementedException();
    }
    public void DeleteItem()
    {
        throw new NotImplementedException();
    }

    public int SeedJanData()
    {
        using(SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            // This query contains all sales data from Jan 1st to Jan 26th
            command.CommandText = @"INSERT INTO sales (Brand, Type, Cost, Sale_price, Profit, Margin, Date_of_sale, Platform, Description) 
                                    VALUES ('Louis Vuitton', 'Bag', 2600, 3950, 1350, 52, '2024-01-01', 'Grailed', 'none'), 
                                            ('Balenciaga', 'Bag', 1000, 1450, 450, 45, '2024-01-01', 'Wechat', 'none'), 
                                            ('Loewe', 'Bag', 500, 780, 280, 56, '2024-01-01', 'Wechat', 'none'), 
                                            ('Prada', 'Bag', 700, 980, 280, 40, '2024-01-01', 'Wechat', 'none'), 
                                            ('ISSEY', 'Dress', 500, 598, 98, 20, '2024-01-01', 'Wechat', 'none'), 
                                            ('Max&co', 'Jacket', 280, 458, 178, 64, '2024-01-01', 'Wechat', 'none'), 
                                            ('Vivienne Westwood', 'Bag', 420, 748, 328, 78, '2024-01-01', 'Wechat', 'none'), 
                                            ('Vivienne Westwood', 'Bag', 100, 600, 500, 500, '2024-01-01', 'Wechat', 'none'), 
                                            ('Celine', 'Bag', 720, 998, 278, 39, '2024-01-01', 'Wechat', 'none'), 
                                            ('MM6', 'Bag', 800, 1300, 500, 63, '2024-01-01', 'Wechat', 'none'), 
                                            ('MM6', 'Bag', 400, 698, 298, 75, '2024-01-01', 'Wechat', 'none'), 
                                            ('Lemaire', 'Bag', 450, 1700, 1250, 278, '2024-01-01', 'Wechat', 'none'), 
                                            ('Vivienne Westwood', 'Jacket', 400, 648, 248, 62, '2024-01-01', 'Wechat', 'none'), 
                                            ('Vivienne Westwood', 'Bag', 600, 848, 248, 41, '2024-01-01', 'Wechat', 'none'), 
                                            ('Chloe', 'Bag', 300, 598, 298, 99, '2024-01-01', 'Wechat', 'none');";
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            return rowsAffected;
        }
    }
}

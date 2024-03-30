using Microsoft.Data.Sqlite;
using sales_app.Repositories;
using sales_app.Models;
using System.Text;
using sales_app.Helpers;

namespace DataAccess;

public class SqliteRepository : ISalesRepository
{
    private readonly string? connectionString;

    public SqliteRepository(string connectionString)
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

    public void InsertItem(Sale item)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO sales(Brand, Type, Cost, Sale_price, Profit, Margin, Date_of_sale, Platform, Description)
                                    VALUES ('{item.Brand}', '{item.Type}', {item.Cost}, {item.SalePrice}, {item.Profit}, {item.Margin}, '{item.DateOfSale.ToString()}', '{item.Platform}','{item.Description}')";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void UpdateItem(Sale item)
    {
        throw new NotImplementedException ();
    }
    public void DeleteItem(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM sales WHERE id = {id}";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public List<Sale> GetAllItems()
    {
        List<Sale> items = new List<Sale>();

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM sales";

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    items.Add(
                        new Sale
                        {
                            Id = reader.GetInt32(0),
                            Brand = reader.GetString(1),
                            Type = reader.GetString(2),
                            Cost = reader.GetInt32(3),
                            SalePrice = reader.GetInt32(4),
                            Profit = reader.GetInt32(5),
                            Margin  = reader.GetInt32(6),
                            DateOfSale = DateOnly.Parse(reader.GetString(7)),
                            Platform = reader.GetString(8),
                            Description = reader.GetString(9),
                        });
                }
            }
            connection.Close();
            return items;
        }
    }
    public List<Sale> GetItems(DataOrder orderBy, int? year = null, int? month = null, string? groupBy = null)
    {
        List<Sale> items = new List<Sale>();

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var query = new StringBuilder($"Select * FROM sales");

            if (year.HasValue)
            {
                query.Append($@" WHERE strftime('%Y', Date_of_sale) = '{year:D4}'");
            }

            if (month.HasValue)
            {
                query.Append($" AND strftime('%m', Date_of_sale) = '{month:00}'");
            }

            if (!string.IsNullOrEmpty(groupBy))
            {
                query.Append($" GROUP BY {groupBy}");
            }

            query.Append($" ORDER BY {orderBy.ToString()}");

            // for testing
            Console.WriteLine(query.ToString() + "\n");

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = query.ToString();

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    items.Add(new Sale
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Brand = reader.GetString(reader.GetOrdinal("Brand")),
                        Type = reader.GetString(reader.GetOrdinal("Type")),
                        Cost = reader.GetInt32(reader.GetOrdinal("Cost")),
                        SalePrice = reader.GetInt32(reader.GetOrdinal("Sale_price")),
                        Profit = reader.GetInt32(reader.GetOrdinal("Profit")),
                        Margin = reader.GetInt32(reader.GetOrdinal("Margin")),
                        DateOfSale = DateOnly.Parse(reader.GetString(reader.GetOrdinal("Date_of_sale"))),
                        Platform = reader.GetString(reader.GetOrdinal("Platform")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                    });
                }
            }
            connection.Close();
            return items;
        }
    }

    public bool ValidateItemById(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $@"SELECT EXISTS(SELECT 1 FROM sales WHERE Id = {id})";
            int checkQuery = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            if (checkQuery == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public void ExportToCSV()
    {
        string workingDirectory = Directory.GetCurrentDirectory();
        string csvFileName = "backup.csv";
        string databaseFilePath = Path.Combine(workingDirectory, "sales-and-inventory.db");
        string csvFilePath = Path.Combine(workingDirectory, csvFileName);

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM sales";
            using SqliteDataReader reader = command.ExecuteReader();
            using StreamWriter writer = new StreamWriter(csvFilePath, false);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                writer.Write(reader.GetName(i) + (i < reader.FieldCount - 1 ? "," : ""));
            }
            writer.WriteLine();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    writer.Write(reader[i].ToString() + (i < reader.FieldCount - 1 ? "," : ""));
                }
                writer.WriteLine();
            }
        }

        return;
    }
}

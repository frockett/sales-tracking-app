﻿using sales_app.Helpers;
using sales_app.Models;

namespace sales_app.Repositories;

public interface ISalesRepository
{
    public void InsertItem(Sale item);
    public void UpdateItem(Sale item);
    public Sale? ValidateItemById(int id);
    public void DeleteItem(int id);
    public List<Sale> GetAllItems();
    public List<Sale> GetItems(DataOrder orderBy, int? year = null, int? month = null, string? groupBy = null);
    public void ExportToCSV();
}

namespace sales_app.Models;

public class SalesRecord
{
    public int Month { get; set; }
    public int Year { get; set; }
    public int TotalSales { get; set; }
    public int GrossProfit { get; set; }
    public int AvgRevenue { get; set; }
    public int AvgProfit { get; set; }
    public float AvgMargin { get; set; }

    public bool HasInformation()
    {
        return Month != 0 && TotalSales != 0;
    }
}

using sales_app.Models;

namespace sales_app.DTOs;

public class SummaryDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public SalesRecord SalesRecord { get; set; }
}

using sales_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sales_app.DTOs;

public class SummaryDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public SalesRecord SalesRecord { get; set; }
}

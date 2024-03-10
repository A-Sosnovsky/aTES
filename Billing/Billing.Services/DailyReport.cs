using System;

namespace Billing.Services;

public class DailyReport
{
    public DateTime Date { get; set; }
    public decimal TotalCost { get; set; }
}
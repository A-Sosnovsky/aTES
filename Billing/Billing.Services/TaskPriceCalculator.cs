using System;

namespace Billing.Services;

public static class TaskPriceCalculator
{
    public static decimal CalculateAssignedPrice()
    {
        return GetTaskPrice(10, 20);
    }
    
    public static decimal CalculateCompetedPrice()
    {
        return GetTaskPrice(20, 40);
    }
    
    private static decimal GetTaskPrice(double minimum, double maximum)
    { 
        var random = new Random();
        return (decimal)(random.NextDouble() * (maximum - minimum) + minimum);
    }
}
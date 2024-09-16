using JOIEnergy.Domain;
using System.Collections.Generic;
using System;
using System.Linq;
namespace JOIEnergy.Utilities;
public static class CostCalculator
{
    private static decimal CalculateAverageReading(List<ElectricityReading> electricityReadings)
    {
        return electricityReadings.Average(reading => reading.Reading);
    }
    private static decimal CalculateTimeElapsed(List<ElectricityReading> electricityReadings)
    {
        var firstReading = electricityReadings.Min(reading => reading.Time);
        var lastReading = electricityReadings.Max(reading => reading.Time);
        return (decimal)(lastReading - firstReading).TotalHours;
    }
    public static decimal CalculateCost(List<ElectricityReading> electricityReadings, PricePlan pricePlan)
    {
        decimal average = CalculateAverageReading(electricityReadings);
        decimal timeElapsed = CalculateTimeElapsed(electricityReadings);
        if (average == 0 || timeElapsed == 0)
        {
            return 0;
        }
        var costPerUnit = average / timeElapsed * pricePlan.UnitRate;
        return Math.Round(costPerUnit, 3);
    }
}
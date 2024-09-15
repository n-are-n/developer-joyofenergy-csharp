using JOIEnergy.Domain;
using JOIEnergy.Utilities;
using System;
using System.Collections.Generic;
using Xunit;
namespace JOIEnergy.Tests.UtilityTests;
public class CostCalculatorTests
{
    [Fact]
    public void CalculateCost_ShouldReturnCorrectCost_WhenValidReadingsAndPlan()
    {
        List<ElectricityReading> readings =
        [
            new ElectricityReading { Reading = 10.0m, Time = DateTime.UtcNow },
            new ElectricityReading { Reading = 20.0m, Time = DateTime.UtcNow.AddHours(2) }
        ];
        PricePlan pricePlan = new() { UnitRate = 0.5m };
        decimal cost = CostCalculator.CalculateCost(readings, pricePlan);
        Assert.Equal(3.75m, cost);
    }
    [Fact]
    public void CalculateCost_ShouldReturnZero_WhenAverageReadingIsZero()
    {
        List<ElectricityReading> readings =
        [
            new ElectricityReading { Reading = 0.0m, Time = DateTime.UtcNow },
            new ElectricityReading { Reading = 0.0m, Time = DateTime.UtcNow.AddHours(2) }
        ];
        PricePlan pricePlan = new() { UnitRate = 0.5m };
        decimal cost = CostCalculator.CalculateCost(readings, pricePlan);
        Assert.Equal(0.0m, cost);
    }
}
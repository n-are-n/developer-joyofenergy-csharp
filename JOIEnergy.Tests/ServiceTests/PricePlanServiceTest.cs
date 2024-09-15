using JOIEnergy.Domain;
using JOIEnergy.Exceptions;
using JOIEnergy.Repositories;
using JOIEnergy.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace JOIEnergy.Tests.ServiceTests;
public class PricePlanServiceTests
{
    private readonly Mock<ILogger<PricePlanService>> _mockLogger;
    private readonly Mock<IMeterReadingService> _mockMeterReadingService;
    private readonly Mock<IPricePlanRepository> _mockPricePlanRepository;
    private readonly PricePlanService _pricePlanService;

    public PricePlanServiceTests()
    {
        _mockLogger = new();
        _mockMeterReadingService = new();
        _mockPricePlanRepository = new();
        _pricePlanService = new(_mockLogger.Object, _mockMeterReadingService.Object, _mockPricePlanRepository.Object);
    }
    [Fact]
    public void GetCostForEachPlan_ShouldReturnCosts_WhenValidSmartMeterId()
    {
        string smartMeterId = "smart-meter-0";
        List<ElectricityReading> electricityReadings =
        [
            new ElectricityReading { Time = DateTime.Now, Reading = 0.1m }
        ];
        List<PricePlan> pricePlans =
        [
            new PricePlan { PlanName = "price-plan-0", UnitRate = 0.2m },
            new PricePlan { PlanName = "price-plan-1", UnitRate = 0.3m }
        ];
        _mockMeterReadingService.Setup(service => service.GetReadings(smartMeterId)).Returns(electricityReadings);
        _mockPricePlanRepository.Setup(repo => repo.GetPricePlans()).Returns(pricePlans);
        var result = _pricePlanService.GetCostForEachPlan(smartMeterId);
        Assert.Equal(2, result.Count);
        Assert.Contains("price-plan-0", result.Keys);
        Assert.Contains("price-plan-1", result.Keys);
    }
    [Fact]
    public void GetCostForEachPlan_ShouldThrowException_WhenMeterReadingServiceFails()
    {
        string smartMeterId = "smartmeterid0";
        _mockMeterReadingService.Setup(service => service.GetReadings(smartMeterId)).Throws(new InvalidSmartMeterException("Test exception"));
        var result = Assert.Throws<InvalidSmartMeterException>(() => _pricePlanService.GetCostForEachPlan(smartMeterId));
        Assert.Equal("Test exception", result.Message);
    }
    [Fact]
    public void GetRecommendedPricePlans_ShouldReturnSortedPlans_WhenValidSmartMeterId()
    {
        string smartMeterId = "smart-meter-0";
        List<ElectricityReading> electricityReadings =
        [
            new ElectricityReading { Time = DateTime.Now, Reading = 0.1m },
            new ElectricityReading { Time = DateTime.Now, Reading = 0.2m },
        ];
        List<PricePlan> consumptionCosts =
        [
            new PricePlan { PlanName = "price-plan-0", UnitRate = 0.1m },
            new PricePlan { PlanName = "price-plan-1", UnitRate = 0.2m }
        ];
        _mockMeterReadingService.Setup(service => service.GetReadings(smartMeterId)).Returns(electricityReadings);
        _mockPricePlanRepository.Setup(repo => repo.GetPricePlans()).Returns(consumptionCosts);
        var result = _pricePlanService.GetRecommendedPricePlans(smartMeterId, null).ToList();
        Assert.Equal(2, result.Count);
        Assert.Equal("price-plan-0", result[0].Key);
        Assert.Equal("price-plan-1", result[1].Key);
    }
    [Fact]
    public void GetRecommendedPricePlans_ShouldLimitNumberOfPlans_WhenLimitIsProvided()
    {
        string smartMeterId = "smart-meter-0";
        List<ElectricityReading> electricityReadings =
        [
            new ElectricityReading { Time = DateTime.Now, Reading = 0.1m },
            new ElectricityReading { Time = DateTime.Now, Reading = 0.2m },
        ];
        List<PricePlan> consumptionCosts =
        [
            new PricePlan { PlanName = "price-plan-0", UnitRate = 0.1m },
            new PricePlan { PlanName = "price-plan-1", UnitRate = 0.2m }
        ];
        _mockMeterReadingService.Setup(service => service.GetReadings(smartMeterId)).Returns(electricityReadings);
        _mockPricePlanRepository.Setup(repo => repo.GetPricePlans()).Returns(consumptionCosts);
        var result = _pricePlanService.GetRecommendedPricePlans(smartMeterId, 1).ToList();
        Assert.Single(result);
        Assert.Equal("price-plan-0", result[0].Key);
    }
    [Fact]
    public void GetRecommendedPricePlans_ShouldThrowException()
    {
        var smartMeterId = "smartmeterid0";
        _mockMeterReadingService.Setup(service => service.GetReadings(smartMeterId)).Throws(new InvalidSmartMeterException("Invalid Smart Meter ID"));
        _mockPricePlanRepository.Setup(repo => repo.GetPricePlans()).Returns(new List<PricePlan>());
        var result = Assert.Throws<InvalidSmartMeterException>(() => _pricePlanService.GetRecommendedPricePlans(smartMeterId, null));
        Assert.Equal("Invalid Smart Meter ID", result.Message);
    }
}
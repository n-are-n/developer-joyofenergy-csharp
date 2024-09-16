using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using JOIEnergy.Data;
using JOIEnergy.Domain;
using JOIEnergy.Repositories;
using System.Collections.Generic;
using JOIEnergy.Enums;
namespace JOIEnergy.Tests.RepositoryTests;
public class PricePlanRepositoryTests
{
    private readonly Mock<ILogger<PricePlanRepository>> _mockLogger;
    private readonly Mock<IInMemoryContext> _mockContext;
    private readonly PricePlanRepository _repository;
    public PricePlanRepositoryTests()
    {
        _mockContext = new();
        _mockLogger = new();
        _repository = new(_mockLogger.Object, _mockContext.Object);
    }
    [Fact]
    public void GetPricePlans_ReturnsListOfPricePlans()
    {
        List<PricePlan> pricePlans =
        [
            new PricePlan { PlanName = "price-plan-0", EnergySupplier = Supplier.DrEvilsDarkEnergy, UnitRate = 10m },
            new PricePlan { PlanName = "price-plan-1", EnergySupplier = Supplier.TheGreenEco, UnitRate = 2m }
        ];
        Dictionary<string, PricePlan> pricePlanDictionary = new()
        {
            { "plan-1", pricePlans[0] },
            { "plan-2", pricePlans[1] }
        };
        _mockContext.Setup(c => c.PricePlans).Returns(pricePlanDictionary);
        List<PricePlan> result = _repository.GetPricePlans();
        Assert.Multiple(() => {
            Assert.Equal(pricePlans.Count, result.Count);
            Assert.Contains(result, p => p.PlanName == "price-plan-0");
            Assert.Contains(result, p => p.PlanName == "price-plan-1");
        });
    }
}
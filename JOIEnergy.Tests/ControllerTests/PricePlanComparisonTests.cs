using JOIEnergy.Controllers;
using JOIEnergy.Exceptions;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;
namespace JOIEnergy.Tests.ControllerTests;
public class PricePlanControllerTests
{
    private readonly Mock<ILogger<PricePlanController>> _mockLogger;
    private readonly Mock<IPricePlanService> _mockPricePlanService;
    private readonly Mock<IAccountService> _mockAccountService;
    private readonly PricePlanController _controller;
    public PricePlanControllerTests()
    {
        _mockLogger = new();
        _mockAccountService = new();
        _mockPricePlanService = new();
        _controller = new(_mockLogger.Object, _mockAccountService.Object, _mockPricePlanService.Object);
    }
     [Fact]
    public void GetCostForEachPlan_ShouldReturnOkResult_WhenValidSmartMeterId()
    {
        string smartMeterId = It.IsAny<string>();
        string pricePlanId = "price-plan-0";
        Dictionary<string, decimal> costPerPricePlan = new()
        {
            { "price-plan-1", 0.1m },
            { "price-plan-2", 0.2m }
        };
        _mockAccountService.Setup(service => service.GetPricePlanIdForSmartMeterId(smartMeterId)).Returns(pricePlanId);
        _mockPricePlanService.Setup(service => service.GetCostForEachPlan(smartMeterId)).Returns(costPerPricePlan);
        var result = _controller.GetCostForEachPlan(smartMeterId) as OkObjectResult;
        Assert.Multiple(() => {
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        });
    }
    [Fact]
    public void GetCostForEachPlan_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        string smartMeterId = It.IsAny<string>();
        _mockAccountService.Setup(service => service.GetPricePlanIdForSmartMeterId(smartMeterId)).Throws(new InvalidSmartMeterException("Test exception"));
        var result = _controller.GetCostForEachPlan(smartMeterId) as ObjectResult;
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }
    [Fact]
    public void RecommendPricePlans_ShouldReturnOkResult_WhenValidSmartMeterId()
    {
        string smartMeterId = It.IsAny<string>();
        List<KeyValuePair<string, decimal>> recommendedPlans = 
        [
            new KeyValuePair<string, decimal>("price-plan-0", 0.1m),
            new KeyValuePair<string, decimal>("price=plan-1", 0.2m)
        ];
        _mockPricePlanService.Setup(service => service.GetRecommendedPricePlans(smartMeterId, null)).Returns(recommendedPlans);
        var result = _controller.RecommendPricePlans(smartMeterId) as ObjectResult;
        Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<KeyValuePair<string, decimal>>>(result.Value);
        Assert.Equal(2, returnValue.Count);
    }
    [Fact]
    public void RecommendPricePlans_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        string smartMeterId = It.IsAny<string>();
        _mockPricePlanService.Setup(service => service.GetRecommendedPricePlans(smartMeterId, null)).Throws(new InvalidSmartMeterException("Test exception"));
        var result = _controller.RecommendPricePlans(smartMeterId) as ObjectResult;
        Assert.Multiple(() => {
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        });
    }
}
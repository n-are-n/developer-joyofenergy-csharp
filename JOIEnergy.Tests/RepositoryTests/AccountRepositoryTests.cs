using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using JOIEnergy.Data;
using JOIEnergy.Repositories;
using System.Collections.Generic;
namespace JOIEnergy.Tests.RepositoryTests;
public class AccountRepositoryTests
{
    private readonly Mock<ILogger<AccountRepository>> _mockLogger;
    private readonly Mock<IInMemoryContext> _mockContext;
    private readonly AccountRepository _repository;

    public AccountRepositoryTests()
    {
        _mockContext = new();
        _mockLogger = new();
        _repository = new(_mockLogger.Object, _mockContext.Object);
    }
    [Fact]
    public void GetPricePlanIdForSmartMeterId_ReturnsCorrectPricePlanId()
    {
        string smartMeterId = "smart-meter-0";
        string expectedPricePlanId = "price-plan-0";
        Dictionary<string, string> smartMeterToPricePlanAccounts = new()
        {
            { smartMeterId, expectedPricePlanId }
        };
        _mockContext.Setup(c => c.SmartMeterToPricePlanAccounts).Returns(smartMeterToPricePlanAccounts);
        var result = _repository.GetPricePlanIdForSmartMeterId(smartMeterId);
        Assert.Equal(expectedPricePlanId, result);
    }
    [Fact]
    public void GetPricePlanIdForSmartMeterId_ThrowsKeyNotFoundException_WhenSmartMeterIdNotFound()
    {
        string smartMeterId = It.IsAny<string>();
        _mockContext.Setup(c => c.SmartMeterToPricePlanAccounts).Throws(new KeyNotFoundException("Key Not Found"));
        var exception = Assert.Throws<KeyNotFoundException>(() => _repository.GetPricePlanIdForSmartMeterId(smartMeterId));
        Assert.Equal("Key Not Found", exception.Message);
    }
}
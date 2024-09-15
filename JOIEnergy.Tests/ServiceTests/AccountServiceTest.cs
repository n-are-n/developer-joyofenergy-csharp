using JOIEnergy.Services;
using JOIEnergy.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;
using JOIEnergy.Exceptions;
namespace JOIEnergy.Tests.ServiceTests;
public class AccountServiceTests
{
    private readonly Mock<ILogger<AccountService>> _mockLogger;
    private readonly Mock<IAccountRepository> _mockAccountRepository;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _mockLogger = new();
        _mockAccountRepository = new();
        _accountService = new(_mockLogger.Object, _mockAccountRepository.Object);
    }
    [Fact]
    public void GetPricePlanIdForSmartMeterId_ShouldReturnPricePlanId_WhenValidSmartMeterId()
    {
        string smartMeterId = "smart-meter-0";
        string expectedPricePlanId = It.IsAny<string>();
        _mockAccountRepository.Setup(repo => repo.GetPricePlanIdForSmartMeterId(smartMeterId)).Returns(expectedPricePlanId);
        var result = _accountService.GetPricePlanIdForSmartMeterId(smartMeterId);
        Assert.Equal(expectedPricePlanId, result);
    }
    [Fact]
    public void GetPricePlanIdForSmartMeterId_ShouldLogError_WhenExceptionThrown()
    {
        string smartMeterId = "smartmeterid0";
        var exception = Assert.Throws<InvalidSmartMeterException>(() => _accountService.GetPricePlanIdForSmartMeterId(smartMeterId));
        Assert.Equal("Invalid Smart Meter ID", exception.Message);
    }
}
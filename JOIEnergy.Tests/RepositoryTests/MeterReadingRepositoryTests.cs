using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using JOIEnergy.Data;
using JOIEnergy.Domain;
using JOIEnergy.Repositories;
using System.Collections.Generic;
namespace JOIEnergy.Tests.RepositoryTests;
public class MeterReadingRepositoryTests
{
    private readonly Mock<IInMemoryContext> _mockContext;
    private readonly Mock<ILogger<MeterReadingRepository>> _mockLogger;
    private readonly MeterReadingRepository _repository;
    public MeterReadingRepositoryTests()
    {
        _mockLogger = new();
        _mockContext = new();
        _repository = new(_mockLogger.Object, _mockContext.Object);
    }
    [Fact]
    public void GetReadings_ReturnsElectricityReadings_WhenSmartMeterIdExists()
    {
        string smartMeterId = It.IsAny<string>();
        List<ElectricityReading> expectedReadings =
        [
            new ElectricityReading() {}
        ];
        List<MeterReadings> meterReadings =
        [
            new MeterReadings { SmartMeterId = smartMeterId, ElectricityReadings = expectedReadings }
        ];
        _mockContext.Setup(c => c.MeterReadings).Returns(meterReadings);
        var result = _repository.GetReadings(smartMeterId);
        Assert.Equal(expectedReadings, result);
    }
    [Fact]
    public void StoreReadings_AddsNewReadings_WhenSmartMeterIdDoesNotExist()
    {
        MeterReadings meterReadings = new()
        {
            SmartMeterId = "smart-meter-0",
            ElectricityReadings =
            [
                new ElectricityReading {}
            ]
        };
        List<MeterReadings> existingReadings = [];
        _mockContext.Setup(c => c.MeterReadings).Returns(existingReadings);
        _repository.StoreReadings(meterReadings);
        Assert.Contains(meterReadings, existingReadings);
    }
    [Fact]
    public void StoreReadings_AddsToExistingReadings_WhenSmartMeterIdExists()
    {
        List<ElectricityReading> existingReadingsList =
        [
            new ElectricityReading {}
        ];
        MeterReadings existingMeterReadings = new()
        {
            SmartMeterId = "smart-meter-2",
            ElectricityReadings = existingReadingsList
        };
        MeterReadings newReadings = new()
        {
            SmartMeterId = "smart-meter-2",
            ElectricityReadings =
            [
                new ElectricityReading {}
            ]
        };
        List<MeterReadings> meterReadingsList = [existingMeterReadings];
        _mockContext.Setup(c => c.MeterReadings).Returns(meterReadingsList);
        _repository.StoreReadings(newReadings);
        Assert.Equal(2, existingReadingsList.Count);
    }
}
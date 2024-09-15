using JOIEnergy.Services;
using JOIEnergy.Repositories;
using JOIEnergy.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using JOIEnergy.Exceptions;
namespace JOIEnergy.Tests.ServiceTests;
public class MeterReadingServiceTests
{
    private readonly Mock<ILogger<MeterReadingService>> _mockLogger;
    private readonly Mock<IMeterReadingRepository> _mockMeterReadingRepository;
    private readonly MeterReadingService _meterReadingService;
    public MeterReadingServiceTests()
    {
        _mockLogger = new();
        _mockMeterReadingRepository = new();
        _meterReadingService = new(_mockLogger.Object, _mockMeterReadingRepository.Object);
    }
    [Fact]
    public void GetReadings_ShouldReturnReadings_WhenValidSmartMeterId()
    {
        string smartMeterId = "smart-meter-0";
        List<ElectricityReading> expectedReadings =
        [
            new ElectricityReading { Time = DateTime.UtcNow, Reading = 0.1m },
        ];
        _mockMeterReadingRepository.Setup(repo => repo.GetReadings(smartMeterId)).Returns(expectedReadings);
        var result = _meterReadingService.GetReadings(smartMeterId);
        Assert.Multiple(() => {
            Assert.Equal(expectedReadings.Count, result.Count);
            Assert.Equal(expectedReadings[0].Reading, result[0].Reading);
            Assert.Equal(expectedReadings[0].Time, result[0].Time);
        });
    }
    [Fact]
    public void GetReadings_ShouldThrowException_WhenInvalidSmartMeterId()
    {
        string invalidSmartMeterId = "smartmeterid0";
        var result = Assert.Throws<InvalidSmartMeterException>(() => _meterReadingService.GetReadings(invalidSmartMeterId));
        Assert.Equal("Invalid Smart Meter ID", result.Message);
    }
    [Fact]
    public void StoreReadings_ShouldStoreReadings_WhenValidMeterReadings()
    {
        var meterReadings = new MeterReadings
        {
            SmartMeterId = "smart-meter-0",
            ElectricityReadings =
            [
                new ElectricityReading { Time = DateTime.Now, Reading = 0.1m }
            ]
        };
        _mockMeterReadingRepository.Setup(repo => repo.StoreReadings(meterReadings));
        _meterReadingService.StoreReadings(meterReadings);
        _mockMeterReadingRepository.Verify(repo => repo.StoreReadings(meterReadings), Times.Once);
    }
    [Fact]
    public void StoreReadings_ShouldThrowException_WhenValidationFails()
    {
        MeterReadings invalidMeterReadings = new()
        {
            SmartMeterId = "smartmeterid0",
            ElectricityReadings =
            [
                new ElectricityReading { Time = DateTime.Now, Reading = 0.1m }
            ]
        };
        var result = Assert.Throws<InvalidSmartMeterException>(() => _meterReadingService.StoreReadings(invalidMeterReadings));
        Assert.Equal("Invalid Smart Meter ID", result.Message);
    }
}

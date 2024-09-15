using JOIEnergy.Controllers;
using JOIEnergy.Domain;
using JOIEnergy.Exceptions;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
namespace JOIEnergy.Tests.ControllerTests;
public class MeterReadingControllerTests
{
    private readonly Mock<ILogger<MeterReadingController>> _mockLogger;
    private readonly Mock<IMeterReadingService> _mockMeterReadingService;
    private readonly MeterReadingController _controller;
    public MeterReadingControllerTests()
    {
        _mockLogger = new();
        _mockMeterReadingService = new();
        _controller = new(_mockLogger.Object, _mockMeterReadingService.Object);
    }
    [Fact]
    public void GetReadings_ShouldReturnOkResult_WhenReadingsExist()
    {
        string smartMeterId = "smart-meter-5";
        List<ElectricityReading> mockReadings = [
            new() { Time = DateTime.Now, Reading = 0.1m }
        ];
        _mockMeterReadingService.Setup(service => service.GetReadings(smartMeterId)).Returns(mockReadings);
        var result = _controller.GetReadings(smartMeterId) as OkObjectResult;
        Assert.Multiple(() => {
            var returnValue = Assert.IsType<List<ElectricityReading>>(result.Value);
            Assert.Single(returnValue);
            Assert.IsType<OkObjectResult>(result);
        });
    }
    [Fact]
    public void GetReadings_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        string smartMeterId = It.IsAny<string>();
        _mockMeterReadingService.Setup(service => service.GetReadings(smartMeterId)).Throws(new InvalidSmartMeterException("Invalid Smart meter Id"));
        var result = _controller.GetReadings(smartMeterId) as ObjectResult;
        Assert.Multiple(() => {
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        });
    }
    [Fact]
    public void StoreReadings_ShouldReturnCreatedResult_WhenReadingsStoredSuccessfully()
    {
        var meterReadings = new MeterReadings
        {
            SmartMeterId = "12345",
            ElectricityReadings = [
                new() { Time = DateTime.Now, Reading = 0.1m },
            ]
        };
        _mockMeterReadingService.Setup(service => service.StoreReadings(meterReadings));
        var result = _controller.StoreReadings(meterReadings) as CreatedResult;
        Assert.Multiple(() => {
            Assert.IsType<CreatedResult>(result);
            Assert.Equal("Meter readings stored successfully.", result.Value);
        });
    }
    [Fact]
    public void StoreReadings_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        MeterReadings meterReadings = It.IsAny<MeterReadings>();
        _mockMeterReadingService.Setup(service => service.StoreReadings(meterReadings)).Throws(new InvalidSmartMeterException("Test exception"));
        var result = _controller.StoreReadings(meterReadings);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }
}
using System;
using Xunit;
using JOIEnergy.Validation;
using JOIEnergy.Domain;
using System.ComponentModel.DataAnnotations;
using JOIEnergy.Exceptions;
namespace JOIEnergy.Tests.ValidationTests;
public class MeterReadingValidatorBuilderTests
{
    private readonly MeterReadingValidatorBuilder validatorBuilder;
    public MeterReadingValidatorBuilderTests()
    {
        validatorBuilder = new();
    }
    [Fact]
    public void Validate_WithAllRules_DoesNotThrowException()
    {
        validatorBuilder.AddRule(new SmartMeterIdValidationRule()).AddRule(new ReadingValueValidationRule()).AddRule(new ReadingTimeValidationRule());
        MeterReadings meterReadings = new()
        {
            SmartMeterId = "smart-meter-0",
            ElectricityReadings =
            [
                new() { Time = DateTime.Now, Reading = 0.1m }
            ]
        };
        var exception = Record.Exception(() => validatorBuilder.Validate(meterReadings));
        Assert.Null(exception);
    }
    [Fact]
    public void Validate_WithSmartMeterIdRule_ThrowsInvalidSmartMeterException()
    {
        validatorBuilder.AddRule(new SmartMeterIdValidationRule());
        MeterReadings meterReadings = new()
        {
            SmartMeterId = "smartmeterid0"
        };
        var exception = Assert.Throws<InvalidSmartMeterException>(() => validatorBuilder.Validate(meterReadings));
        Assert.Equal("Invalid Smart Meter ID", exception.Message);
    }
    [Fact]
    public void Validate_WithReadingTimeRule_InvalidTime_ThrowsValidationException()
    {
        validatorBuilder.AddRule(new ReadingTimeValidationRule());
        MeterReadings meterReadings = new()
        {
            SmartMeterId = "smart-meter-0",
            ElectricityReadings =
            [
                new ElectricityReading { Time = default, Reading = 0.1m }
            ]
        };
        var exception = Assert.Throws<ValidationException>(() => validatorBuilder.Validate(meterReadings));
        Assert.Equal("Invalid reading time.", exception.Message);
    }
    [Fact]
    public void Validate_WithReadingValueRule_InvalidValue_ThrowsValidationException()
    {
        validatorBuilder.AddRule(new ReadingValueValidationRule());
        var meterReadings = new MeterReadings
        {
            SmartMeterId = "smart-meter-0",
            ElectricityReadings =
            [
                new ElectricityReading { Time = DateTime.Now, Reading = -1 }
            ]
        };
        var exception = Assert.Throws<ValidationException>(() => validatorBuilder.Validate(meterReadings));
        Assert.Equal("Reading value must be greater than 0.", exception.Message);
    }
    [Fact]
    public void Validate_WithMultipleRules_AllRulesApplied()
    {
        validatorBuilder.AddRule(new SmartMeterIdValidationRule()).AddRule(new ReadingValueValidationRule()).AddRule(new ReadingTimeValidationRule());
        MeterReadings meterReadings = new()
        {
            SmartMeterId = "smartmeterid0",
            ElectricityReadings =
            [
               new ElectricityReading { Time = default, Reading = -1 }
            ]
        };
        var invalidSmartMeterException = Assert.Throws<InvalidSmartMeterException>(() => validatorBuilder.Validate(meterReadings));
        Assert.Equal("Invalid Smart Meter ID", invalidSmartMeterException.Message);
        meterReadings.SmartMeterId = "smart-meter-0";
        var exception = Assert.Throws<ValidationException>(() => validatorBuilder.Validate(meterReadings));
        Assert.Equal("Reading value must be greater than 0.", exception.Message);
        meterReadings.ElectricityReadings[0].Reading = 0.1m;
        exception = Assert.Throws<ValidationException>(() => validatorBuilder.Validate(meterReadings));
        Assert.Equal("Invalid reading time.", exception.Message);
    }
}
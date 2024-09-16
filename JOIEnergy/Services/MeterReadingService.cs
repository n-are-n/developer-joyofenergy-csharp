using System;
using System.Collections.Generic;
using JOIEnergy.Domain;
using JOIEnergy.Repositories;
using JOIEnergy.Validation;
using Microsoft.Extensions.Logging;
namespace JOIEnergy.Services;
public class MeterReadingService(ILogger<MeterReadingService> logger, IMeterReadingRepository meterReadingRepository) : IMeterReadingService
{
    private readonly ILogger<MeterReadingService> _logger = logger;
    private readonly IMeterReadingRepository _meterReadingRepository = meterReadingRepository;
    public List<ElectricityReading> GetReadings(string smartMeterId)
    {
        try
        {
            _logger.LogInformation(string.Concat(nameof(MeterReadingService), ':', nameof(GetReadings)));
            new MeterReadingValidatorBuilder().AddRule(new SmartMeterIdValidationRule()).Validate(new MeterReadings() { SmartMeterId = smartMeterId });
            List<ElectricityReading> electricityReadings = _meterReadingRepository.GetReadings(smartMeterId);
            _logger.LogInformation($"Electricity readings for smart meter ID : {smartMeterId} retrived successfully");
            return electricityReadings;
        }
        catch(Exception e)
        {
            _logger.LogError(e, string.Concat(nameof(MeterReadingService), ':', nameof(GetReadings)));
            throw;
        }
    }
    public void StoreReadings(MeterReadings meterReadings)
    {
        try
        {
            _logger.LogInformation(string.Concat(nameof(MeterReadingService), ':', nameof(StoreReadings)));
            new MeterReadingValidatorBuilder().AddRule(new SmartMeterIdValidationRule()).AddRule(new ReadingValueValidationRule()).AddRule(new ReadingTimeValidationRule()).Validate(meterReadings);
            _meterReadingRepository.StoreReadings(meterReadings);
            _logger.LogInformation("Meter readings stored successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, string.Concat(nameof(MeterReadingService), ':', nameof(StoreReadings)));
            throw;
        }
    }
}
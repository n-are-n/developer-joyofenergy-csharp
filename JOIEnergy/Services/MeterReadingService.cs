using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using JOIEnergy.Domain;
using JOIEnergy.Exceptions;
using JOIEnergy.Repositories;
using JOIEnergy.Utilities;
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
            SmartMeterValidator.ValidateSmartMeterId(smartMeterId);
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
            SmartMeterValidator.ValidateSmartMeterId(meterReadings.SmartMeterId);
            if (!meterReadings.ElectricityReadings.Any())
            {
                _logger.LogWarning("Electricity readings cannot be empty.");
                throw new ValidationException("Electricity readings cannot be empty.");
            }
            foreach (var reading in meterReadings.ElectricityReadings)
            {
                if (reading.Reading <= 0)
                {
                    _logger.LogWarning("Invalid reading value.");
                    throw new ValidationException("Reading value must be greater than or equal to 0.");
                }
                if (reading.Time == default || reading.Time > DateTime.Now || reading.Time < DateTime.Today)
                {
                    _logger.LogWarning("Invalid reading time.");
                    throw new ValidationException("Reading time must be valid.");
                }
            }
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
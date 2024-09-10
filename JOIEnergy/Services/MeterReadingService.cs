using System;
using System.Collections.Generic;
using JOIEnergy.Domain;
using JOIEnergy.Repositories;
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
            List<ElectricityReading> electricityReadings = _meterReadingRepository.GetReadings(smartMeterId);
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
            _meterReadingRepository.StoreReadings(meterReadings);
        }
        catch(Exception e)
        {
            _logger.LogError(e, string.Concat(nameof(MeterReadingService), ':', nameof(StoreReadings)));
            throw;
        }
    }
}
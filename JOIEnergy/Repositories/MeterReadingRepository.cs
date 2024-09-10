using JOIEnergy.Data;
using JOIEnergy.Domain;
using JOIEnergy.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
namespace JOIEnergy.Repositories;
public class MeterReadingRepository(ILogger<MeterReadingRepository> logger, InMemoryContext context) : IMeterReadingRepository
{
    private readonly ILogger<MeterReadingRepository> _logger = logger;
    private readonly InMemoryContext _context = context;
    public List<ElectricityReading> GetReadings(string smartMeterId)
    {
        try
        {
            MeterReadings meterReading = _context.MeterReadings.FirstOrDefault(mr => mr.SmartMeterId == smartMeterId);
            return meterReading == null ? throw new InvalidSmartMeterException($"Invalid smart meter ID: {smartMeterId}") : meterReading.ElectricityReadings;
        }
        catch(Exception e)
        {
            _logger.LogError(e, string.Concat(nameof(MeterReadingRepository), ':', nameof(GetReadings)));
            throw;
        }
    }
    public void StoreReadings(MeterReadings meterReadings)
    {
        try
        {
            MeterReadings meterReading = _context.MeterReadings.FirstOrDefault(mr => mr.SmartMeterId == meterReadings.SmartMeterId);
            if (meterReading == null)
                _context.MeterReadings.Add(meterReadings);
            else
                meterReading.ElectricityReadings.AddRange(meterReadings.ElectricityReadings);
        }
        catch(Exception e)
        {
            _logger.LogError(e, string.Concat(nameof(MeterReadingRepository), ':', nameof(StoreReadings)));
            throw;
        }
    }
}

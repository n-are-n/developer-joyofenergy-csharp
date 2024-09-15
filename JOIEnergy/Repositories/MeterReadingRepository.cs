using JOIEnergy.Data;
using JOIEnergy.Domain;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
namespace JOIEnergy.Repositories;
public class MeterReadingRepository(ILogger<MeterReadingRepository> logger, IInMemoryContext context) : IMeterReadingRepository
{
    private readonly ILogger<MeterReadingRepository> _logger = logger;
    private readonly IInMemoryContext _context = context;
    public List<ElectricityReading> GetReadings(string smartMeterId)
    {
        _logger.LogInformation(string.Concat(nameof(MeterReadingRepository), ':', nameof(GetReadings)));
        MeterReadings meterReading = _context.MeterReadings.FirstOrDefault(mr => mr.SmartMeterId == smartMeterId);
        return meterReading.ElectricityReadings;
    }
    public void StoreReadings(MeterReadings meterReadings)
    {
        _logger.LogInformation(string.Concat(nameof(MeterReadingRepository), ':', nameof(StoreReadings)));
        MeterReadings meterReading = _context.MeterReadings.FirstOrDefault(mr => mr.SmartMeterId == meterReadings.SmartMeterId);
        if (meterReading == null) _context.MeterReadings.Add(meterReadings);
        else meterReading.ElectricityReadings.AddRange(meterReadings.ElectricityReadings);
    }
}
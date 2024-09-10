using JOIEnergy.Domain;
using System.Collections.Generic;
namespace JOIEnergy.Repositories;
public interface IMeterReadingRepository
{
    List<ElectricityReading> GetReadings(string smartMeterId);
    void StoreReadings(MeterReadings meterReadings);
}

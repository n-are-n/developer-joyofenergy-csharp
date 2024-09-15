using System.Collections.Generic;
using JOIEnergy.Domain;
namespace JOIEnergy.Data;
public interface IInMemoryContext
{
    List<MeterReadings> MeterReadings { get; }
    Dictionary<string, PricePlan> PricePlans { get; }
    Dictionary<string, string> SmartMeterToPricePlanAccounts { get; }
}

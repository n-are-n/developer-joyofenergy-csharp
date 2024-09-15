using JOIEnergy.Domain;
using JOIEnergy.Enums;
using JOIEnergy.Generator;
using JOIEnergy.Utilities;
using System.Collections.Generic;
using System.Linq;
namespace JOIEnergy.Data;
public class InMemoryContext : IInMemoryContext
{
    public List<MeterReadings> MeterReadings { get; }
    public Dictionary<string, PricePlan> PricePlans { get; }
    public Dictionary<string, string> SmartMeterToPricePlanAccounts
    {
        get
        {
            Dictionary<string, string> smartMeterToPricePlanAccounts = new()
            {
                { "smart-meter-0", SupplierPricePlanMapper.GetPricePlanId(Supplier.DrEvilsDarkEnergy) },
                { "smart-meter-1", SupplierPricePlanMapper.GetPricePlanId(Supplier.TheGreenEco) },
                { "smart-meter-2", SupplierPricePlanMapper.GetPricePlanId(Supplier.DrEvilsDarkEnergy) },
                { "smart-meter-3", SupplierPricePlanMapper.GetPricePlanId(Supplier.PowerForEveryone) },
                { "smart-meter-4", SupplierPricePlanMapper.GetPricePlanId(Supplier.TheGreenEco) }
            };
            return smartMeterToPricePlanAccounts;
        }
    }
    public InMemoryContext()
    {
        MeterReadings = [];
        PricePlans = [];
        SeedPricePlans();
        GenerateMeterElectricityReadings();
    }
    private void SeedPricePlans()
    {
        var pricePlans = new List<PricePlan> {
            new() {
                PlanName = SupplierPricePlanMapper.GetPricePlanId(Supplier.DrEvilsDarkEnergy),
                EnergySupplier = Supplier.DrEvilsDarkEnergy,
                UnitRate = 10m,
                PeakTimeMultiplier = []
            },
            new() {
                PlanName = SupplierPricePlanMapper.GetPricePlanId(Supplier.TheGreenEco),
                EnergySupplier = Supplier.TheGreenEco,
                UnitRate = 2m,
                PeakTimeMultiplier = []
            },
            new() {
                PlanName = SupplierPricePlanMapper.GetPricePlanId(Supplier.PowerForEveryone),
                EnergySupplier = Supplier.PowerForEveryone,
                UnitRate = 1m,
                PeakTimeMultiplier = []
            }
        };

        pricePlans.ForEach(x => PricePlans.Add(x.PlanName, x));
    }
    private MeterReadings GenerateMeterElectricityReadings()
    {
        MeterReadings readings = new();
        ElectricityReadingGenerator generator = new();
        var smartMeterIds = SmartMeterToPricePlanAccounts.Select(mtpp => mtpp.Key);
        foreach (var smartMeterId in smartMeterIds)
        {
            MeterReadings.Add(new()
            {
                SmartMeterId = smartMeterId,
                ElectricityReadings = generator.Generate(20)
            });
        }
        return readings;
    }
}
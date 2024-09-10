using JOIEnergy.Domain;
using JOIEnergy.Enums;
using JOIEnergy.Generator;
using JOIEnergy.Utilities;
using System.Collections.Generic;
using System.Linq;
namespace JOIEnergy.Data;
public class InMemoryContext
{
    public List<MeterReadings> MeterReadings { get; }
    public Dictionary<string, PricePlan> PricePlans { get; }
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
                PlanName = SupplierUtility.GetPricePlanId(Supplier.DrEvilsDarkEnergy),
                EnergySupplier = Supplier.DrEvilsDarkEnergy,
                UnitRate = 10m,
                PeakTimeMultiplier = []
            },
            new() {
                PlanName = SupplierUtility.GetPricePlanId(Supplier.TheGreenEco),
                EnergySupplier = Supplier.TheGreenEco,
                UnitRate = 2m,
                PeakTimeMultiplier = []
            },
            new() {
                PlanName = SupplierUtility.GetPricePlanId(Supplier.PowerForEveryone),
                EnergySupplier = Supplier.PowerForEveryone,
                UnitRate = 1m,
                PeakTimeMultiplier = []
            }
        };

        pricePlans.ForEach(x => PricePlans.Add(x.PlanName, x));
    }
    public Dictionary<string, string> SmartMeterToPricePlanAccounts
    {
        get
        {
            Dictionary<string, string> smartMeterToPricePlanAccounts = new Dictionary<string, string>();
            smartMeterToPricePlanAccounts.Add("smart-meter-0", SupplierUtility.GetPricePlanId(Supplier.DrEvilsDarkEnergy));
            smartMeterToPricePlanAccounts.Add("smart-meter-1", SupplierUtility.GetPricePlanId(Supplier.TheGreenEco));
            smartMeterToPricePlanAccounts.Add("smart-meter-2", SupplierUtility.GetPricePlanId(Supplier.DrEvilsDarkEnergy));
            smartMeterToPricePlanAccounts.Add("smart-meter-3", SupplierUtility.GetPricePlanId(Supplier.PowerForEveryone));
            smartMeterToPricePlanAccounts.Add("smart-meter-4", SupplierUtility.GetPricePlanId(Supplier.NullSupplier));
            return smartMeterToPricePlanAccounts;
        }
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
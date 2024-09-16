using System.Collections.Generic;
using JOIEnergy.Enums;
namespace JOIEnergy.Domain;
public class PricePlan
{
    public string PlanName { get; set; }
    public Supplier EnergySupplier { get; set; }
    public decimal UnitRate { get; set; }
}

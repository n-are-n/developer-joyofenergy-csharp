using JOIEnergy.Enums;

namespace JOIEnergy.Utilities;
public static class SupplierUtility
{
    public static string GetPricePlanId(Supplier supplier)
    {
        return supplier switch
        {
            Supplier.DrEvilsDarkEnergy => "price-plan-0",
            Supplier.TheGreenEco => "price-plan-1",
            Supplier.PowerForEveryone => "price-plan-2",
            _ => string.Empty
        };
    }
}

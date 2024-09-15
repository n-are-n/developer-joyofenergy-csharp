using JOIEnergy.Enums;
using JOIEnergy.Utilities;
using Xunit;
namespace JOIEnergy.Tests.UtilityTests;
public class SupplierPricePlanMapperTests
{
    [Theory]
    [InlineData(Supplier.DrEvilsDarkEnergy, "price-plan-0")]
    [InlineData(Supplier.TheGreenEco, "price-plan-1")]
    [InlineData(Supplier.PowerForEveryone, "price-plan-2")]
    public void GetPricePlanId_ShouldReturnCorrectPricePlanId_ForKnownSuppliers(Supplier supplier, string expectedPricePlanId)
    {
        string pricePlanId = SupplierPricePlanMapper.GetPricePlanId(supplier);
        Assert.Equal(expectedPricePlanId, pricePlanId);
    }
    [Fact]
    public void GetPricePlanId_ShouldReturnEmptyString_ForUnknownSupplier()
    {
        Supplier unknownSupplier = (Supplier)3;
        string pricePlanId = SupplierPricePlanMapper.GetPricePlanId(unknownSupplier);
        Assert.Equal(string.Empty, pricePlanId);
    }
}
using System.Collections.Generic;
namespace JOIEnergy.Services;
public interface IPricePlanService
{
    Dictionary<string, decimal> GetCostForEachPlan(string smartMeterId);
    IEnumerable<KeyValuePair<string, decimal>> GetRecommendedPricePlans(string smartMeterId, int? limit);
}
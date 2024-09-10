using JOIEnergy.Domain;
using System.Collections.Generic;

namespace JOIEnergy.Repositories;
public interface IPricePlanRepository
{
    List<PricePlan> GetPricePlans();
}

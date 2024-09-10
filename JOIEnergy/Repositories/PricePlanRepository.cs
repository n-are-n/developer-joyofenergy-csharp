using JOIEnergy.Data;
using JOIEnergy.Domain;
using System.Collections.Generic;
using System.Linq;

namespace JOIEnergy.Repositories;
public class PricePlanRepository(InMemoryContext context) : IPricePlanRepository
{
    private readonly InMemoryContext _context = context;
    public List<PricePlan> GetPricePlans()
    {
        return [.. _context.PricePlans.Values];
    }
}

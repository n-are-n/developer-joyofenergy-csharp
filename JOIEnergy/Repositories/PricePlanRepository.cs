using JOIEnergy.Data;
using JOIEnergy.Domain;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
namespace JOIEnergy.Repositories;
public class PricePlanRepository(ILogger<PricePlanRepository> logger, InMemoryContext context) : IPricePlanRepository
{
    private readonly ILogger<PricePlanRepository> _logger = logger;
    private readonly InMemoryContext _context = context;
    public List<PricePlan> GetPricePlans()
    {
        _logger.LogInformation(string.Concat(nameof(PricePlanRepository), ':', nameof(GetPricePlans)));
        return [.. _context.PricePlans.Values];
    }
}
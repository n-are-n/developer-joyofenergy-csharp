using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;
using JOIEnergy.Repositories;
using JOIEnergy.Utilities;
using Microsoft.Extensions.Logging;
namespace JOIEnergy.Services;
public class PricePlanService(ILogger<PricePlanService> logger, IMeterReadingService meterReadingService, IPricePlanRepository pricePlanRepository) : IPricePlanService
{
    private readonly ILogger<PricePlanService> _logger = logger;
    private readonly IMeterReadingService _meterReadingService = meterReadingService;
    private readonly IPricePlanRepository _pricePlanRepository = pricePlanRepository;
    public Dictionary<string, decimal> GetCostForEachPlan(string smartMeterId)
    {
        try
        {
            List<ElectricityReading> electricityReadings = _meterReadingService.GetReadings(smartMeterId);
            List<PricePlan> pricePlans = _pricePlanRepository.GetPricePlans();
            return pricePlans.ToDictionary(plan => plan.PlanName, plan => CostCalculator.CalculateCost(electricityReadings, plan));
        }
        catch(Exception e)
        {
            _logger.LogError(e, $"Error calculating consumption cost for Smart Meter ID: {smartMeterId}");
            throw;
        }
    }
    public IEnumerable<KeyValuePair<string, decimal>> GetRecommendedPricePlans(string smartMeterId, int? limit)
    {
        try
        {
            Dictionary<string, decimal> consumptionForPricePlans = GetCostForEachPlan(smartMeterId);
            var sortedRecommendations = consumptionForPricePlans.OrderBy(pricePlanComparison => pricePlanComparison.Value);
            if (limit.HasValue && limit.Value > 0 && limit.Value < sortedRecommendations.Count())
            {
                return sortedRecommendations.Take(limit.Value);
            }
            return sortedRecommendations;
        }
        catch(Exception e)
        {
            _logger.LogError(e, $"Error retrieving recommended price plans for Smart Meter ID: {smartMeterId}");
            throw;
        }
    }
}
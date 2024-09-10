using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;
using JOIEnergy.Repositories;
using Microsoft.Extensions.Logging;

namespace JOIEnergy.Services
{
    public class PricePlanService(ILogger<PricePlanService> logger, IMeterReadingService meterReadingService, IPricePlanRepository pricePlanRepository) : IPricePlanService
    {
        private readonly ILogger<PricePlanService> _logger = logger;
        private IMeterReadingService _meterReadingService = meterReadingService;
        private IPricePlanRepository _pricePlanRepository = pricePlanRepository;
        private decimal CalculateAverageReading(List<ElectricityReading> electricityReadings)
        {
            if (!electricityReadings.Any())
            {
                _logger.LogWarning("No readings available for average calculation.");
                return 0;
            }
            decimal totalReadings = electricityReadings.Sum(electricityReading => electricityReading.Reading);
            return totalReadings / electricityReadings.Count;
        }

        private decimal CalculateTimeElapsed(List<ElectricityReading> electricityReadings)
        {
            if(!electricityReadings.Any())
            {
                _logger.LogWarning("No readings available for average calculation.");
                return 0;
            }
            var firstReading = electricityReadings.Min(reading => reading.Time);
            var lastReading = electricityReadings.Max(reading => reading.Time);
            return (decimal)(lastReading - firstReading).TotalHours;
        }
        private decimal CalculateCost(List<ElectricityReading> electricityReadings, PricePlan pricePlan)
        {
            decimal average = CalculateAverageReading(electricityReadings);
            decimal timeElapsed = CalculateTimeElapsed(electricityReadings);
            if(timeElapsed == 0 || average == 0)
            {
                _logger.LogWarning($"Time elapsed or average reading is zero for Price Plan: {pricePlan.PlanName}");
                return 0;
            }
            var costPerUnit = (average / timeElapsed) * pricePlan.UnitRate;
            return Math.Round(costPerUnit, 3);
        }
        public Dictionary<string, decimal> GetCostForEachPlan(string smartMeterId)
        {
            try
            {
                List<ElectricityReading> electricityReadings = _meterReadingService.GetReadings(smartMeterId);
                if (!electricityReadings.Any())
                {
                    _logger.LogWarning($"No readings found for Smart Meter ID: {smartMeterId}");
                    return [];
                }
                List<PricePlan> pricePlans = _pricePlanRepository.GetPricePlans();
                return pricePlans.ToDictionary(plan => plan.PlanName, plan => CalculateCost(electricityReadings, plan));
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
                if (!consumptionForPricePlans.Any())
                {
                    _logger.LogWarning($"No consumption data found for Smart Meter ID: {smartMeterId}");
                    return [];
                }
                var sortedRecommendations = consumptionForPricePlans.OrderBy(pricePlanComparison => pricePlanComparison.Value);
                if (limit.HasValue && limit.Value < sortedRecommendations.Count())
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
}

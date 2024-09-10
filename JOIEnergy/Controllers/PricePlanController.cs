using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace JOIEnergy.Controllers
{
    [Route("price-plans")]
    public class PricePlanController(ILogger<PricePlanController> logger, IPricePlanService pricePlanService, IAccountService accountService) : Controller
    {
        private readonly ILogger<PricePlanController> _logger = logger;
        private readonly IPricePlanService _pricePlanService = pricePlanService;
        private readonly IAccountService _accountService = accountService;
        [HttpGet("compare-all/{smartMeterId}")]
        public IActionResult GetCostForEachPlan(string smartMeterId)
        {
            try
            {
                Dictionary<string, decimal> costPerPricePlan = _pricePlanService.GetCostForEachPlan(smartMeterId);
                string pricePlanId = _accountService.GetPricePlanIdForSmartMeterId(smartMeterId);
                return Ok(new { pricePlanId, costPerPricePlan});
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred while calculating costs for Smart Meter ID: {smartMeterId}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("recommend/{smartMeterId}")]
        public IActionResult RecommendPricePlans(string smartMeterId, int? limit = null)
        {
            try
            {
                IEnumerable<KeyValuePair<string, decimal>> recommandedPricePlans = _pricePlanService.GetRecommendedPricePlans(smartMeterId, limit);
                return Ok(recommandedPricePlans);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Error occurred while recommending cheapest price plans for Smart Meter ID: {smartMeterId}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}

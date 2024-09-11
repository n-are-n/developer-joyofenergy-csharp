using System;
using System.Collections.Generic;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace JOIEnergy.Controllers;
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
            _logger.LogInformation(string.Concat(nameof(PricePlanController), ':', nameof(GetCostForEachPlan)));
            string pricePlanId = _accountService.GetPricePlanIdForSmartMeterId(smartMeterId);
            Dictionary<string, decimal> costPerPricePlan = _pricePlanService.GetCostForEachPlan(smartMeterId);
            _logger.LogInformation("Cost per each plan retived successfully");
            return Ok(new { pricePlanId, costPerPricePlan});
        }
        catch (Exception e)
        {
            _logger.LogError(e, string.Concat(nameof(PricePlanController), ':', nameof(GetCostForEachPlan)));
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }

    [HttpGet("recommend/{smartMeterId}")]
    public IActionResult RecommendPricePlans(string smartMeterId, int? limit = null)
    {
        try
        {
            _logger.LogInformation(string.Concat(nameof(PricePlanController), ':', nameof(RecommendPricePlans)));
            IEnumerable<KeyValuePair<string, decimal>> recommandedPricePlans = _pricePlanService.GetRecommendedPricePlans(smartMeterId, limit);
            _logger.LogInformation("Recommended plans retrived successfully");
            return Ok(recommandedPricePlans);
        }
        catch(Exception e)
        {
            _logger.LogError(e, string.Concat(nameof(PricePlanController), ':', nameof(RecommendPricePlans)));
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
}
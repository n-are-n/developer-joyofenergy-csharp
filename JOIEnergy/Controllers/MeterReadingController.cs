using JOIEnergy.Domain;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace JOIEnergy.Controllers;
[ApiController]
[Route("readings")]
public class MeterReadingController(ILogger<MeterReadingController> logger, IMeterReadingService meterReadingService) : Controller
{
    private readonly ILogger<MeterReadingController> _logger = logger;
    private readonly IMeterReadingService _meterReadingService = meterReadingService;
    [HttpGet("read/{smartMeterId}")]
    public IActionResult GetReadings(string smartMeterId)
    {
        try
        {
            List<ElectricityReading> electricityReadings = _meterReadingService.GetReadings(smartMeterId);
            return Ok(electricityReadings);
        }
        catch(Exception e)
        {
            _logger.LogError(e, string.Concat(nameof(MeterReadingController), ':', nameof(GetReadings)));
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
    // POST api/values
    [HttpPost ("store")]
    public IActionResult StoreReadings([FromBody]MeterReadings meterReadings)
    {
        try
        {
            _meterReadingService.StoreReadings(meterReadings);
            return Created(nameof(StoreReadings), "Meter readings stored successfully.");
        }
        catch(Exception e)
        {
            _logger.LogError(e, string.Concat(nameof(MeterReadingController), ':', nameof(StoreReadings)));
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
}
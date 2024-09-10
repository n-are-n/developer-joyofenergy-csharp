using JOIEnergy.Data;
using Microsoft.Extensions.Logging;
using System;
namespace JOIEnergy.Repositories;
public class AccountRepository(ILogger<AccountRepository> logger, InMemoryContext context) : IAccountRepository
{
    private readonly ILogger<AccountRepository> _logger = logger;
    private readonly InMemoryContext _context = context;
    public string GetPricePlanIdForSmartMeterId(string smartMeterId)
    {
        try
        {
            if (_context.SmartMeterToPricePlanAccounts.TryGetValue(smartMeterId, out var pricePlanId))
            {
                return pricePlanId;
            }
            return null;
        }
        catch(Exception e)
        {
            _logger.LogError(e, string.Concat(nameof(AccountRepository), ':', nameof(GetPricePlanIdForSmartMeterId)));
            throw;
        }
    }
}

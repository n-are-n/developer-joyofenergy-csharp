using JOIEnergy.Data;
using Microsoft.Extensions.Logging;
namespace JOIEnergy.Repositories;
public class AccountRepository(ILogger<AccountRepository> logger, IInMemoryContext context) : IAccountRepository
{
    private readonly ILogger<AccountRepository> _logger = logger;
    private readonly IInMemoryContext _context = context;
    public string GetPricePlanIdForSmartMeterId(string smartMeterId)
    {
        _logger.LogInformation(string.Concat(nameof(AccountRepository), ':', nameof(GetPricePlanIdForSmartMeterId)));
        return _context.SmartMeterToPricePlanAccounts[smartMeterId];
    }
}
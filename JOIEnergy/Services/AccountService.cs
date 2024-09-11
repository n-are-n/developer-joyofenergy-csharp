using JOIEnergy.Exceptions;
using JOIEnergy.Repositories;
using JOIEnergy.Utilities;
using Microsoft.Extensions.Logging;
using System;
namespace JOIEnergy.Services;
public class AccountService(ILogger<AccountService> logger, IAccountRepository accountRepository) : IAccountService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly ILogger<AccountService> _logger = logger;
    public string GetPricePlanIdForSmartMeterId(string smartMeterId)
    {
        try
        {
            _logger.LogInformation(string.Concat(nameof(AccountService), ':', nameof(GetPricePlanIdForSmartMeterId)));
            SmartMeterValidator.ValidateSmartMeterId(smartMeterId);
            var pricePlanId = _accountRepository.GetPricePlanIdForSmartMeterId(smartMeterId);
            _logger.LogInformation("Price of the plan retrived successfully");
            return pricePlanId;
        }
        catch(Exception e)
        {
            _logger.LogError(e, string.Concat(nameof(AccountService), ':', nameof(GetPricePlanIdForSmartMeterId)));
            throw;
        }
    }
}
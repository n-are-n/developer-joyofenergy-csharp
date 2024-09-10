using JOIEnergy.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace JOIEnergy.Services
{
    public class AccountService(ILogger<AccountService> logger, IAccountRepository accountRepository) : IAccountService
    {
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ILogger<AccountService> _logger = logger;
        public string GetPricePlanIdForSmartMeterId(string smartMeterId)
        {
            try
            {
                var price = _accountRepository.GetPricePlanIdForSmartMeterId(smartMeterId);
                if (price == null)
                {
                    _logger.LogWarning($"No Price Plan found for Smart Meter ID: {smartMeterId}");
                    return null;
                }
                return price;
            }
            catch(Exception e)
            {
                _logger.LogError(e, string.Concat(nameof(AccountService), ':', nameof(GetPricePlanIdForSmartMeterId)));
                throw;
            }
        }
    }
}

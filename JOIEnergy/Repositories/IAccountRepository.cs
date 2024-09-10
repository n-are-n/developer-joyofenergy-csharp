namespace JOIEnergy.Repositories;
public interface IAccountRepository
{
    public string GetPricePlanIdForSmartMeterId(string smartMeterId);
}

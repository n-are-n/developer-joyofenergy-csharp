using JOIEnergy.Exceptions;
using System.Text.RegularExpressions;
namespace JOIEnergy.Utilities;
public static class SmartMeterValidator
{
    private const string pattern = @"^smart-meter-\d+$";
    public static void ValidateSmartMeterId(string smartMeterId)
    {
        if (!Regex.IsMatch(smartMeterId, pattern)) throw new InvalidSmartMeterException($"Invalid smart meter format: {smartMeterId}");
    }
}

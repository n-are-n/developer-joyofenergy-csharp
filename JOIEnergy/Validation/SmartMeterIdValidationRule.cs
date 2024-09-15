using System.Text.RegularExpressions;
using JOIEnergy.Domain;
using JOIEnergy.Exceptions;
namespace JOIEnergy.Validation;
public partial class SmartMeterIdValidationRule : IValidationRule<MeterReadings>
{
    private const string pattern = @"^smart-meter-\d+$";
    [GeneratedRegex(pattern)]
    private static partial Regex MyRegex();
    public void Validate(MeterReadings meterReadings)
    {
        if (!MyRegex().IsMatch(meterReadings.SmartMeterId)) throw new InvalidSmartMeterException("Invalid Smart Meter ID");
    }
}

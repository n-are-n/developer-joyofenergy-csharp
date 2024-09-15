using System.Collections.Generic;
using JOIEnergy.Domain;

namespace JOIEnergy.Validation;
public class MeterReadingValidatorBuilder
{
    private readonly List<IValidationRule<MeterReadings>> _rules = [];
    public MeterReadingValidatorBuilder AddRule(IValidationRule<MeterReadings> rule)
    {
        _rules.Add(rule);
        return this;
    }
    public void Validate(MeterReadings meterReadings)
    {
        foreach (var rule in _rules) rule.Validate(meterReadings);
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using JOIEnergy.Domain;
namespace JOIEnergy.Validation;
public class ReadingTimeValidationRule : IValidationRule<MeterReadings>
{
    public void Validate(MeterReadings meterReadings)
    {
        foreach (var reading in meterReadings.ElectricityReadings)
        { 
            if (reading.Time == default || reading.Time > DateTime.Now || reading.Time < DateTime.Today)
            {
                throw new ValidationException("Invalid reading time.");
            }
        }
    }
}
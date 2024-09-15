using System.ComponentModel.DataAnnotations;
using JOIEnergy.Domain;
namespace JOIEnergy.Validation;
public class ReadingValueValidationRule : IValidationRule<MeterReadings>
{
    public void Validate(MeterReadings meterReadings)
    {
        foreach (var reading in meterReadings.ElectricityReadings)
        {
            if (reading.Reading <= 0)
            {
                throw new ValidationException("Reading value must be greater than 0.");
            }
        }
    }
}

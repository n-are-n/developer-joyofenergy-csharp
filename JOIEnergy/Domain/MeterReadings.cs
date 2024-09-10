using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JOIEnergy.Domain
{
    public class MeterReadings
    {
        [StringLength(13, MinimumLength = 13)]
        public string SmartMeterId { get; set; }
        [Required, MinLength(1)]
        public List<ElectricityReading> ElectricityReadings { get; set; }
    }
}

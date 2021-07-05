using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CongestionTaxCalculatorApi.Models
{
    public class VehicleTaxModel
    {
        public string CityName { get; set; }
        public string VehicleType { get; set; }
        public DateTime[] Dates { get; set; }
    }
}

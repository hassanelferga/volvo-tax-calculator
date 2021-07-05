using System;
using System.Collections.Generic;
using System.Text;

namespace CongestionTaxCalulator.Data.Models
{
    public class TaxModel
    {
        public List<City> Cities { get; set; }
        public List<PublicHoliday> PublicHolidays { get; set; }
        public List<string> TollFreeVehicles { get; set; }
    }

    public class Fare
    {
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public decimal TollValue { get; set; }
    }

    public class City
    {
        public string CityName { get; set; }
        public decimal MaxTaxValue { get; set; }
        public List<Fare> Fares { get; set; }
    }

    public class PublicHoliday
    {
        public int Month { get; set; }
        public List<int> Days { get; set; }
    }
}

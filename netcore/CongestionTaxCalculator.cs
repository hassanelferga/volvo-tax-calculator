using System;
using System.Linq;
using System.Threading.Tasks;
using congestion.calculator;
using CongestionTaxCalulator.Data.Models;
using CongestionTaxCalulator.Data.Services;

public class CongestionTaxCalculator
{
    
    TaxModel _taxModel;
    CityFareService _cityFareService;
    public CongestionTaxCalculator(CityFareService cityFareService)
    {
        _cityFareService = cityFareService;
    }

    /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @param cityName - the city name
         * @return - the total congestion tax for that day
         */
    public async Task<decimal> GetTax(Vehicle vehicle, DateTime[] dates, string cityName)
    {
        _taxModel = await _cityFareService.GetCitiesFare();

        // Handle the exempted Vehicles types instead of looping
        if (IsTollFreeVehicle(vehicle))
            return 0;

        DateTime intervalStart = dates[0];
        decimal totalFee = 0;
        City city = _taxModel.Cities.FirstOrDefault(c => c.CityName == cityName);
        if (city == null)
            throw new ApplicationException(string.Format("City with name {0} does not exists.", cityName));
        foreach (DateTime date in dates)
        {
            decimal nextFee = GetTollFee(date, vehicle, city);
            decimal tempFee = GetTollFee(intervalStart, vehicle, city);

            // In some environments Millisecond always 0
            //long diffInMillies = date.Millisecond - intervalStart.Millisecond;

            // Use ticks instead
            long diffInTicks = date.Ticks - intervalStart.Ticks;
            //Millisecond = 10,000 ticks
            long minutes = diffInTicks / 10000 / 1000 / 60;

            if (minutes <= 60)
            {
                if (totalFee > 0)
                {
                    totalFee -= tempFee;
                }
                if (nextFee >= tempFee)
                {
                    tempFee = nextFee;
                }
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }
        if (totalFee > city.MaxTaxValue)
        {
            totalFee = city.MaxTaxValue;
        }            
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
            return false;
        String vehicleType = vehicle.GetVehicleType();
        return _taxModel.TollFreeVehicles.Contains(vehicleType);
    }

    public decimal GetTollFee(DateTime date, Vehicle vehicle, City city)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
            return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        var x = city.Fares.Where(f =>
        // Single Entry match
        (f.TimeFrom.Hour == hour && f.TimeFrom.Minute <= minute && f.TimeTo.Hour == hour && f.TimeTo.Minute >= minute)
        // date fall in a range same day
        || (f.TimeTo.Hour > f.TimeFrom.Hour && f.TimeFrom.Hour <= hour && hour <= f.TimeTo.Hour)
        // date fall in a range which in two differnt dates
        || (f.TimeTo.Hour < f.TimeFrom.Hour && f.TimeFrom.Hour <= hour)
        ).FirstOrDefault();

        return city.Fares.Where(f =>
        // Single Entry match
        (f.TimeFrom.Hour == hour && f.TimeFrom.Minute <= minute && f.TimeTo.Hour == hour && f.TimeTo.Minute >= minute)
        // date fall in a range same day
        || (f.TimeTo.Hour > f.TimeFrom.Hour && f.TimeFrom.Hour <= hour && hour <= f.TimeTo.Hour)
        // date fall in a range which in two differnt dates
        || (f.TimeTo.Hour < f.TimeFrom.Hour && f.TimeFrom.Hour <= hour)
        ).FirstOrDefault().TollValue;
    }

    private Boolean IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        if (year == 2013)
        {
            return _taxModel.PublicHolidays.Any(x => (x.Month == month && x.Days.Contains(day))
            // Whole Month is represented as Zero
                || (x.Month == month && x.Days.Contains(0)));
            
        }
        return false;
    }
}
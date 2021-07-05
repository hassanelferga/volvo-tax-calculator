using CongestionTaxCalulator.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CongestionTaxCalulator.Data.Services
{
    public class CityFareService
    {
        TaxModel _model;
        public CityFareService()
        {

            string json = string.Empty;
            using (System.IO.StreamReader reader = new System.IO.StreamReader("CitiesData.json"))
            {
                json = reader.ReadToEnd();
            }
            _model = JsonConvert.DeserializeObject<TaxModel>(json);
        }
        
        public async Task<TaxModel> GetCitiesFare()
        {
            return _model;
        }
    }
}

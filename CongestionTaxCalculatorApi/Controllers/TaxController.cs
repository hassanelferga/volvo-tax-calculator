using congestion.calculator;
using CongestionTaxCalculatorApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CongestionTaxCalculatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        CongestionTaxCalculator _taxCalculator;
        public TaxController(CongestionTaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }
        [HttpPost]
        [Route("CalculateTax")]
        public async Task<IActionResult> CalculateTax(VehicleTaxModel model)
        {
            try
            {                
                // Not the best appraoch, use Factory patteren instead
                Vehicle vehicle;
                switch(model.VehicleType)
                {
                    case "Car":
                        vehicle = new Car();
                        break;
                    case "Motorbike":
                        vehicle = new Motorbike();
                        break;
                    default:
                        vehicle = new Emergency();
                        break;
                };

                decimal value = await _taxCalculator.GetTax(vehicle, model.Dates, model.CityName);
                return Ok(value);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }
    }
}

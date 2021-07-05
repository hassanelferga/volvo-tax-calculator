Author: Hassan Ibrahim
Email: hassan.ibrahim@poolia.se

## Project Stucture

 1. CongestionTaxtCalculatorAPI: 
	 An ASP.NET Core Web API project that will be the entry for the 	Congestion Tax Clacultor. The project has Swagger enabled to test the API. The project has one API Endpoint fos simplifying the test.	 
 2. CongestionTaxCalulator.Data: A class libaray project that read the data from the json file.
 3. congestion-tax-calculator-net-core: The project from Github with the task modification and ehancement.

## Changes
I have made the following changes:

 - Added Emergency class: an inherited class from Vehicle.
 - Added CongestionTaxCalculator.CongestionTaxCalculator constructor.
 - Changed CongestionTaxCalculator.GetTax to take the city as arguments.
 - Changed the logic and removed the static if statements with dynamic data from data source (json).
 - Fixed the bug of Millisecond.
## Json Schema
I haved used the json file as the data source to simplify the test of the application. The structures as follows:
 
 - Cities array which contains City object with array of toll fares.
 - Public Holidays array for adding the public holiday.
 - TollFreeVehicles arrays for adding exmpeted vehicles.
## How to test
To test the application:
 
 - Run the Web API project. It will open the browser with Swagger by default.
 - Click on /api/Tax/CalculateTax and Click Try it out
 - Fill in the body request data as the following sample:
 

    {
  "cityName": "Gothenburg",
  "vehicleType": "Motorbike",
  "dates": [
		  "2013-02-07T06:23:27",
	"2013-02-07T15:27:00"
	]
}

 - The expected result will be a decimal value or an error.
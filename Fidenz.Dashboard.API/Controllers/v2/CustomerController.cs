using Asp.Versioning;
using BusinessLogicLayer.Common.Interfaces;
using BusinessLogicLayer.Utility;
using DataAccessLayer.Data;
using DataAccessLayer.DTOs;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fidenz.Dashboard.API.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [Authorize]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Index()
        {
            var CustomerData = "../DataAccessLayer/Data/UserData.json";

            // Read JSON file
            StreamReader r = new StreamReader(CustomerData);
            string json = r.ReadToEnd();
            List<CustomerDTO> customersList = JsonConvert.DeserializeObject<List<CustomerDTO>>(json);

            foreach (var customer in customersList)
            {
                // Check if the customer with the same ID already exists in the database
                var existingCustomer = _unitOfWork.Customer.Get(c => c.Id == customer._id);

                if (existingCustomer == null)
                {
                    // Insert the customer into the database
                    Customer cust = new Customer();
                    cust.CustomerId = customer.Index;
                    cust.Id = customer._id;
                    cust.Index = customer.Index;
                    cust.Age = customer.Age;
                    cust.EyeColor = customer.EyeColor;
                    cust.Name = customer.Name;
                    cust.Gender = customer.Gender;
                    cust.Company = customer.Company;
                    cust.Email = customer.Email;
                    cust.Phone = customer.Phone;
                    cust.Address = customer.Address;
                    cust.About = customer.About;
                    cust.Registered = customer.Registered;
                    cust.Longitude = customer.Longitude;
                    cust.Latitude = customer.Latitude;
                    _unitOfWork.Customer.Add(cust);
                }
                else
                {
                    // Handle duplicate (e.g., log a message, skip, or update existing record)
                    Console.WriteLine($"Duplicate customer ID found: {customer._id}");
                }
            }

            _unitOfWork.Customer.Save();

            //return View(await _context.Customers.ToArrayAsync());
            return View(_unitOfWork.Customer.GetAll().ToArray());
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerDTO customerDto)
        {
            if (customerDto == null || customerDto._id == null)
            {
                return BadRequest("Invalid data provided");
            }

            try
            {
                Customer cust = _unitOfWork.Customer.Get(c => c.Id == customerDto._id);

                if (cust == null)
                {
                    return NotFound($"Customer with ID {customerDto._id} not found");
                }
                cust.Name = customerDto.Name;
                cust.Email = customerDto.Email;
                cust.Phone = customerDto.Phone;

                _unitOfWork.Customer.Update(cust);
                _unitOfWork.Customer.Save();

                // Fetch the updated customer from the database
                Customer updatedCustomer = _unitOfWork.Customer.Get(c => c.Id == customerDto._id);
                return Ok(updatedCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("getdistance")]
        public async Task<IActionResult> GetDistance([FromBody] GetDistanceRequestDTO requestBody)
        {
            const double EarthRadius = 6371; // Earth radius in kilometers

            if (requestBody == null || requestBody.CustomerId == null)
            {
                return BadRequest("Invalid data provided");
            }
            try
            {
                Customer cust = _unitOfWork.Customer.Get(c => c.Id == requestBody.CustomerId);
                if (cust == null)
                {
                    return NotFound($"Customer with ID {requestBody.CustomerId} not found");
                }

                var customerLatitude = cust.Latitude;
                var customerLongitude = cust.Longitude;

                var locLatitude = requestBody.Latitude;
                var locLongitude = requestBody.Longitude;

                var dLat = DegreeToRadian(customerLatitude - locLatitude);
                var dLon = DegreeToRadian(customerLongitude - locLongitude);

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreeToRadian(locLatitude)) * Math.Cos(DegreeToRadian(customerLatitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                var distance = EarthRadius * c;

                return Ok(distance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchCustomers(string strPart)
        {
            if (string.IsNullOrEmpty(strPart))
            {
                return BadRequest("Invalid or empty search parameter");
            }

            try
            {
                // Search for customers where any property contains the specified partial string
                List<Customer> matchingCustomers = _unitOfWork.Customer.GetAll()
                    .Where(c =>
                        c.Id.Contains(strPart) ||
                        c.EyeColor.Contains(strPart) ||
                        c.Name.Contains(strPart) ||
                        c.Company.Contains(strPart) ||
                        c.Email.Contains(strPart) ||
                        c.Phone.Contains(strPart) ||
                        c.About.Contains(strPart)
                    )
                    .ToList();

                return Ok(matchingCustomers.ToArray());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("groupedByZip")]
        public IActionResult GetCustomersGroupedByZip()
        {
            try
            {
                // Group customers by zip code
                var groupedCustomers = _unitOfWork.Customer.GetAll()
                    .GroupBy(c => c.Address.Zipcode)
                    .Select(group => new
                    {
                        ZipCode = group.Key,
                        Customers = group.ToList()
                    })
                    .ToList();

                return Ok(groupedCustomers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180.0;
        }
    }
}

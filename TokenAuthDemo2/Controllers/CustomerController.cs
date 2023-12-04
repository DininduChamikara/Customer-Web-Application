using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TokenAuthDemo2.Models;

namespace TokenAuthDemo2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    /*[Authorize]*/
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var CustomerData = "./Data/UserData.json";

            // Read JSON file
            StreamReader r = new StreamReader(CustomerData);
            string json = r.ReadToEnd();
            List<Customer> customersList = JsonConvert.DeserializeObject<List<Customer>>(json);

            foreach (var customer in customersList)
            {
                // Check if the customer with the same ID already exists in the database
                var existingCustomer = _context.Customers.FirstOrDefault(c => c._id == customer._id);

                if (existingCustomer == null)
                {
                    // Insert the customer into the database
                    _context.Customers.Add(customer);
                }
                else
                {
                    // Handle duplicate (e.g., log a message, skip, or update existing record)
                    Console.WriteLine($"Duplicate customer ID found: {customer._id}");
                }
            }

            _context.SaveChanges();

            return View(await _context.Customers.ToArrayAsync());
        }

        [HttpPost]
        [Route("api/update")]
        public async Task<IActionResult> UpdateCustomer([FromBody] Customer customer)
        {
            if (customer == null || customer._id == null)
            {
                return BadRequest("Invalid data provided");
            }

            try
            {
                Customer cust = await _context.Customers.FirstAsync(c => c._id == customer._id);

                if (cust == null)
                {
                    return NotFound($"Customer with ID {customer._id} not found");
                }
                cust.Name = customer.Name;
                cust.Email = customer.Email;
                cust.Phone = customer.Phone;

                await _context.SaveChangesAsync();

                // Fetch the updated customer from the database
                Customer updatedCustomer = await _context.Customers.FirstAsync(c => c._id == customer._id);
                return Ok(updatedCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/getdistance")]
        public async Task<IActionResult> GetDistance([FromBody] GetDistanceRequest requestBody)
        {
            const double EarthRadius = 6371; // Earth radius in kilometers

            if (requestBody == null || requestBody.CustomerId == null)
            {
                return BadRequest("Invalid data provided");
            }
            try
            {
                Customer cust = await _context.Customers.FirstAsync(c => c._id == requestBody.CustomerId);
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
        [Route("api/search")]
        public async Task<IActionResult> SearchCustomers(string strPart)
        {
            if (string.IsNullOrEmpty(strPart))
            {
                return BadRequest("Invalid or empty search parameter");
            }

            try
            {
                // Search for customers where any property contains the specified partial string
                List<Customer> matchingCustomers = _context.Customers
                    .Where(c =>
                        c._id.Contains(strPart) ||
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
        [Route("api/groupedByZip")]
        public IActionResult GetCustomersGroupedByZip()
        {
            try
            {
                // Group customers by zip code
                var groupedCustomers = _context.Customers
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

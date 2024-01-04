using Asp.Versioning;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Utility;
using DataAccessLayer.Common.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.DTOs;
using DataAccessLayer.Models;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fidenz.Dashboard.API.Controllers.v3
{
    [ApiVersion("3.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [Authorize]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [MapToApiVersion("3.0")]
        [HttpGet]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Index()
        {
            var importedCustomers = await _customerService.ImportCustomersFromJsonAsync();
            return View(importedCustomers.ToArray());
        }

        [MapToApiVersion("3.0")]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerDTO customerDto)
        {
            if (customerDto == null || customerDto._id == null)
            {
                return BadRequest("Invalid data provided");
            }
            if (_customerService.UpdateCustomerAsync(customerDto) != null)
            {
                Customer updatedCustomer = _customerService.UpdateCustomerAsync(customerDto);
                return Ok(updatedCustomer);
            }else
            {
                return NotFound($"Customer with ID {customerDto._id} not found");
            }
        }

        [MapToApiVersion("3.0")]
        [HttpPost]
        [Route("getdistance")]
        public async Task<IActionResult> GetDistance([FromBody] GetDistanceRequestDTO requestBody)
        {
            if (requestBody == null || requestBody.CustomerId == null)
            {
                return BadRequest("Invalid data provided");
            }
            try
            {
                var distance = _customerService.GetDistance(requestBody);
                return Ok(distance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }

}

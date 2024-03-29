﻿using Asp.Versioning;
using Fidenz.Customers.Application.Services;
using Fidenz.Customers.Application.Utility;
using Fidenz.Customers.Domain.Common.Interfaces;
using Fidenz.Customers.Domain.Data;
using Fidenz.Customers.Domain.DTOs;
using Fidenz.Customers.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fidenz.Customers.API.Controllers.v1
{
    [Route("")]
    [ApiVersion("1.0")]
    [Authorize]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Index()
        {
            var importedCustomers = await _customerService.ImportCustomersFromJsonAsync();
            return View(importedCustomers.ToArray());
        }

        [HttpPost]
        [Route("api/update")]
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
            }
            else
            {
                return NotFound($"Customer with ID {customerDto._id} not found");
            }
        }

        [HttpPost]
        [Route("api/getdistance")]
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
                var matchingCustomers = _customerService.SearchCustomer(strPart);
                return Ok(matchingCustomers);
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
                var groupedCustomers = _customerService.GetCustomersGroupedByZip();
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

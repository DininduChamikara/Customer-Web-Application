using Fidenz.Customers.Domain.DTOs;
using Fidenz.Customers.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fidenz.Customers.Application.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> ImportCustomersFromJsonAsync();
        Customer UpdateCustomerAsync(CustomerDTO customerDto);
        double GetDistance(GetDistanceRequestDTO getDistanceRequestDTO);
        List<Customer> SearchCustomer(string strPart);
        List<ZipCodeCustomerGroup> GetCustomersGroupedByZip();
    }
}

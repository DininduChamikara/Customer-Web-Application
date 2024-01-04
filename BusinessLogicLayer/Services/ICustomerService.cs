using DataAccessLayer.DTOs;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> ImportCustomersFromJsonAsync();
        Customer UpdateCustomerAsync(CustomerDTO customerDto);
        double GetDistance (GetDistanceRequestDTO getDistanceRequestDTO);
    }
}

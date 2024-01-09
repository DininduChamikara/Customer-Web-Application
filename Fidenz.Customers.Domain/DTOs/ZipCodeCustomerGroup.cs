using Fidenz.Customers.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fidenz.Customers.Domain.DTOs
{
    public class ZipCodeCustomerGroup
    {
        public int ZipCode { get; set; }
        public List<Customer> Customers { get; set; }
    }
}

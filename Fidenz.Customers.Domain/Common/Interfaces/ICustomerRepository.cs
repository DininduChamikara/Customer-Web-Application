using Fidenz.Customers.Domain.Repository;
using Fidenz.Customers.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fidenz.Customers.Domain.Common.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Update(Customer customer);
        void Save();
    }
}

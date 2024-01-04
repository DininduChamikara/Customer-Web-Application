using DataAccessLayer.Models;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Update(Customer customer);
        void Save();
    }
}

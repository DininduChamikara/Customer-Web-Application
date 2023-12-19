using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Common.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Update(Customer customer);
        void Save();
    }
}

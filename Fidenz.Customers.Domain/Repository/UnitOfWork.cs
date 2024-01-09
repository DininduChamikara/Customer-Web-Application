using Fidenz.Customers.Domain.Common.Interfaces;
using Fidenz.Customers.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fidenz.Customers.Domain.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICustomerRepository Customer { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Customer = new CustomerRepository(_context);
        }
    }
}

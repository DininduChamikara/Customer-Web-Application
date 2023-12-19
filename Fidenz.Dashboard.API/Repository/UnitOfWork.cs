using BusinessLogicLayer.Common.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.Repository;

namespace Fidenz.Dashboard.API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICustomerRepository Customer {  get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Customer = new CustomerRepository(_context);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fidenz.Customers.Domain.Common.Interfaces
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customer { get; }
    }
}

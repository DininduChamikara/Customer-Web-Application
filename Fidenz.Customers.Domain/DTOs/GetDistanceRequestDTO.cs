﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fidenz.Customers.Domain.DTOs
{
    public class GetDistanceRequestDTO
    {
        public string CustomerId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

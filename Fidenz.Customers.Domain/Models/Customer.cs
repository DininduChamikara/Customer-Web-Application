using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Fidenz.Customers.Domain.Models
{
    public class Customer
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        //public string CustomerId { get; set; }
        //public int Index { get; set; }
        //public int Age { get; set; }
        //public string? EyeColor { get; set; }
        //public string Name { get; set; }
        //public string? Gender { get; set; }
        //public string? Company { get; set; }
        //public string Email { get; set; }
        //public string Phone { get; set; }
        //public Address? Address { get; set; }
        //public string? About { get; set; }
        //public string? Registered { get; set; }
        //public double Latitude { get; set; }
        //public double Longitude { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string CustomerId { get; set; }

        [Required]
        public int Index { get; set; }

        [Required]
        public int Age { get; set; }

        public string EyeColor { get; set; }

        [Required]
        [MaxLength]
        public string Name { get; set; }

        public string Gender { get; set; }

        public string Company { get; set; }

        [Required]
        [MaxLength]
        public string Email { get; set; }

        [Required]
        [MaxLength]
        public string Phone { get; set; }

        public Address? Address { get; set; }

        public string About { get; set; }

        public string Registered { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}

using DataAccessLayer.Common.Interfaces;
using DataAccessLayer.DTOs;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Customer>> ImportCustomersFromJsonAsync()
        {
            var jsonFilePath = "../DataAccessLayer/Data/UserData.json";

            // Read JSON file
            StreamReader r = new StreamReader(jsonFilePath);
            string json = r.ReadToEnd();
            List<CustomerDTO> customersList = JsonConvert.DeserializeObject<List<CustomerDTO>>(json);

            List<Customer> importedCustomers = new List<Customer>();

            foreach (var customer in customersList)
            {
                // Check if the customer with the same ID already exists in the database
                var existingCustomer = _unitOfWork.Customer.Get(c => c.Id == customer._id);

                if (existingCustomer == null)
                {
                    // Insert the customer into the database
                    Customer cust = new Customer();
                    cust.CustomerId = customer.Index;
                    cust.Id = customer._id;
                    cust.Index = customer.Index;
                    cust.Age = customer.Age;
                    cust.EyeColor = customer.EyeColor;
                    cust.Name = customer.Name;
                    cust.Gender = customer.Gender;
                    cust.Company = customer.Company;
                    cust.Email = customer.Email;
                    cust.Phone = customer.Phone;
                    cust.Address = customer.Address;
                    cust.About = customer.About;
                    cust.Registered = customer.Registered;
                    cust.Longitude = customer.Longitude;
                    cust.Latitude = customer.Latitude;
                    _unitOfWork.Customer.Add(cust);
                }
                else
                {
                    // Handle duplicate (e.g., log a message, skip, or update existing record)
                    Console.WriteLine($"Duplicate customer ID found: {customer._id}");
                }
            }

            _unitOfWork.Customer.Save();

            //return importedCustomers;
            return _unitOfWork.Customer.GetAll();
        }

        public Customer UpdateCustomerAsync(CustomerDTO customerDto)
        {
            Customer cust = _unitOfWork.Customer.Get(c => c.Id == customerDto._id);
            if (cust == null)
            {
                return null;
            }
            cust.Name = customerDto.Name;
            cust.Email = customerDto.Email;
            cust.Phone = customerDto.Phone;

            _unitOfWork.Customer.Update(cust);
            _unitOfWork.Customer.Save();

            // Fetch the updated customer from the database
            Customer updatedCustomer = _unitOfWork.Customer.Get(c => c.Id == customerDto._id);

            return updatedCustomer;
        }

        public double GetDistance(GetDistanceRequestDTO getDistanceRequestDTO)
        {
            const double EarthRadius = 6371;

            Customer cust = _unitOfWork.Customer.Get(c => c.Id == getDistanceRequestDTO.CustomerId);
            if (cust == null)
            {
                return 0;
            }

            var customerLatitude = cust.Latitude;
            var customerLongitude = cust.Longitude;

            var locLatitude = getDistanceRequestDTO.Latitude;
            var locLongitude = getDistanceRequestDTO.Longitude;

            var dLat = DegreeToRadian(customerLatitude - locLatitude);
            var dLon = DegreeToRadian(customerLongitude - locLongitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreeToRadian(locLatitude)) * Math.Cos(DegreeToRadian(customerLatitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = EarthRadius * c;

            return distance;
        }

        private double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180.0;
        }
    }
}

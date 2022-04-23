using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.Models
{
    public class Restaurant
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.Models
{
    public class Order
    {

        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Customer Customer { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}

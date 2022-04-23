using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.Models
{
    public class Robot
    {
        public int CourierID { get; set; }
        public string StateOfRobot { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
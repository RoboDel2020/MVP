using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.Models
{
    public class RobotStatistics
    {
        public int CourierID { get; set; }
        public DateTime DateTime { get; set; }
        public double Speed { get; set; }
        public double Battery { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
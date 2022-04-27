using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.Models
{
    public class CourierForDelivery
    {

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Delivery Delivery { get; set; }
        public Robot Robot { get; set; }
        public double Distance { get; set; }
    }
}

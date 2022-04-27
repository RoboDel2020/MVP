using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.Models
{
    public class Delivery
    {

        public int ID { get; set; }
        public DateTime ToRestaurant { get; set; }
        public DateTime AtRestaurant { get; set; }
        public DateTime Loaded { get; set; }
        public DateTime FromRestaurant { get; set; }
        public DateTime AtCustomer { get; set; }
        public DateTime CustomerInformed { get; set; }
        public DateTime Delivered { get; set; }
        public Order Order { get; set; }
    }
}

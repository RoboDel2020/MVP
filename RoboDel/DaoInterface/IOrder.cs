using RoboDel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.DaoInterface
{
    public interface IOrder
    {
        public List<Order> GetAllOrders(out string error);
        public List<Order> GetAllOrdersByRestaurantEmail(string restaurantEmail, out string error);
        public Order GetOrderDetails(int orderID, out string error);
        public bool AddOrder(String pickupTime, bool readyForPickup, string restaurantEmail, int customerID, out string error);
    }
}

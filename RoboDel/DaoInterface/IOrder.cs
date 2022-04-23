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
        public Order GetOrderDetails(int orderID, out string error);
    }
}

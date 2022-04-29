using RoboDel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.DaoInterface
{
    public interface IDelivery
    {
        public bool AddDelivery(int orderId, out string error);
        public int GetActiveDeliveryByOrderID(int orderId);
    }
}

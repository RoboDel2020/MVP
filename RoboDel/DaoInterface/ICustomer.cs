using RoboDel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.DaoInterface
{
    public interface ICustomer
    {
        
        public Customer GetCustomerDetails(int customerID, out string error);
    }
}

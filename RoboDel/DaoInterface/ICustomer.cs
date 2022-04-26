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
        public int CustomerExists(string firstName, string lastName, string email, string phoneNumber, string address, string apartment, string city, string zip, string state, string country, out string error);
        public bool AddCustomer(string firstName, string lastName, string email, string phoneNumber, string address, string apartment, string city, string zip, string state, string country, out string error);
        public int GetLastInsertedCustomerID();
    }
}

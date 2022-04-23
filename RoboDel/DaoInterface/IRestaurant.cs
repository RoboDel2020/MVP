using RoboDel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.DaoInterface
{
    public interface IRestaurant
    {
        public bool RestaurantExists(string email);
        //public bool RegisterUser(string email, string password, string nickname, string firstName, string lastName, string postalCode, out string error);
        public bool ValidateRestaurant(string emailInput, out string error);
        public Restaurant ValidatePassword(string email, string password, out string error);
        public Restaurant GetRestaurantDetails(string restaurantEmail, out string error);
        //public User GetRegisteredInfoByEmail(string userEmail, out string error);
        //public bool UpdateUserInfoByEmail(string email, string password, string nickname, string firstName, string lastName, string postalCode, out string error);
    }
}

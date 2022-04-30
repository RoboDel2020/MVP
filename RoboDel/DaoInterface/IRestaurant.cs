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
        public bool RegisterRestaurant(string name, string email, string password, string type, double price, string phoneNumber, string address, string city, string state, string postalCode, string country, double latitude, double longitude, out string error);
        public bool ValidateRestaurant(string emailInput, out string error);
        public Restaurant ValidatePassword(string email, string password, out string error);
        public Restaurant GetRestaurantDetails(string restaurantEmail, out string error);
        public List<Restaurant> GetAllRestaurants(out string error);
        //public User GetRegisteredInfoByEmail(string userEmail, out string error);
        //public bool UpdateUserInfoByEmail(string email, string password, string nickname, string firstName, string lastName, string postalCode, out string error);
    }
}

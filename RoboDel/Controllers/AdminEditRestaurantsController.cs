using RoboDel.Dao;
using RoboDel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http;
using System.IO;

namespace RoboDel.Controllers
{
    public class AdminEditRestaurantsController : Controller
    {
        public static string email { get; private set; }

        public AdminEditRestaurantsController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            ViewBag.restaurant = GetRestaurantByEmail(email);
            return View();
        }


        public IActionResult GoToEditRestaurant(string restaurantEmail)
        {
            email = restaurantEmail;
            return Json(new { errMsg = "" });
        }

        public IActionResult EditRestaurant(string status, string name, string email, string password, string type, string price, string phoneNumber, string address, string city, string state, string postalCode, string country, double latitude,double longitude)
        {
            RestaurantDao restaurantDao = new RestaurantDao();

            _ = restaurantDao.UpdateRestaurant(status, name, email, password, type, double.Parse(price), phoneNumber, address, city, state, postalCode, country, latitude, longitude, out string error);
          
            return Json(new { errMsg = error });
        }

        public Restaurant GetRestaurantByEmail(string email)
        {
            RestaurantDao restaurantDao = new RestaurantDao();
            return restaurantDao.GetRestaurantDetails(email, out string error);
        }

    }
}

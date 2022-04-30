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
    public class AdminAddRestaurantsController : Controller
    {
        
        public AdminAddRestaurantsController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            return View();
        }


        public IActionResult GoToAddRestaurant()
        {
            return Json(new { errMsg = "" });
        }

        public IActionResult AddRestaurant(string name, string email, string password, string type, string price, string phoneNumber, string address, string city, string state, string postalCode, string country, double latitude,double longitude)
        {
            RestaurantDao restaurantDao = new RestaurantDao();

            if (restaurantDao.RestaurantExists(email))
            {
                return Json(new { errMsg = "Restaurant account with that email already exists." });
            }
            _ = restaurantDao.RegisterRestaurant(name, email, password, type, double.Parse(price), phoneNumber, address, city, state, postalCode, country, latitude, longitude, out string error);
          
            return Json(new { errMsg = error });
        }

    }
}

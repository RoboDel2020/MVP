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

namespace RoboDel.Controllers
{
    public class RestaurantLoginController : Controller
    {
        public static Restaurant restaurant { get; private set; }


        public RestaurantLoginController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            return View();
        }


        public IActionResult ValidateLogin(string credentials, string password)
        {
            RestaurantDao restaurantDao = new RestaurantDao();
            if (restaurantDao.ValidateRestaurant(credentials, out string error))
            {
                restaurant = restaurantDao.ValidatePassword(credentials, password, out error);
                if (restaurant != null)
                {
                    HttpContext.Session.SetString("restaurant_email", restaurant.Email);
                    HttpContext.Session.SetString("restaurant_name", restaurant.Name);
                }
            }
            return Json(new { errMsg = error });
        }


    }
}

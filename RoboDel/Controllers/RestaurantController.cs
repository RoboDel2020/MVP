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
    public class RestaurantController : Controller
    {
        public static Restaurant restaurant { get; private set; }
        public static List<Order> order { get; private set; }

        public RestaurantController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            OrderDao orderDao = new OrderDao();
            ViewBag.allOrders = orderDao.GetAllOrdersByRestaurantEmail(HttpContext.Session.GetString("restaurant_email"), out string error);
            return View();
        }



        public IActionResult GoToLoginAsARestaurant()
        {
            return Json(new { errMsg = "" });
        }

        public IActionResult Logout(string itemNumber)
        {
            HttpContext.Session.SetString("restaurant_email", "");
            HttpContext.Session.SetString("restaurant_name", "");
            return Json(new { errMsg = "" });
        }

        public List<Restaurant> GetAllRestaurants()
        {
            RestaurantDao restaurantDao = new RestaurantDao();
            List<Restaurant> allRestaurants = restaurantDao.GetAllRestaurants(out string error);
            return allRestaurants;
        }

        
    }
}

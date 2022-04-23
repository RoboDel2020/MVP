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
    public class ViewRestaurantController : Controller
    {
        public static Restaurant restaurant { get; private set; }

        public ViewRestaurantController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            ViewBag.restaurant = restaurant;
            return View();
        }



        public IActionResult GoToViewRestaurant(string restaurantEmail)
        {
            _ = GetRestaurantDetails(restaurantEmail, out string error);
            return Json(new { errMsg = error });
        }

        public IActionResult GetRestaurantDetails(string restaurantEmail, out string error)
        {
            RestaurantDao restaurantDao = new RestaurantDao();
            restaurant = restaurantDao.GetRestaurantDetails(restaurantEmail, out  error);
            return Json(new { errMsg = error });
        }
    }
}

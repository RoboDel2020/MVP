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
    public class AdminRestaurantsController : Controller
    {
        
        public AdminRestaurantsController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            ViewBag.restaurants = this.GetAllRestaurants();
            return View();
        }


        public IActionResult GoToAdminRestaurants()
        {
            return Json(new { errMsg = "" });
        }

        public List<Restaurant> GetAllRestaurants()
        {
            RestaurantDao restaurantDao = new RestaurantDao();
            List<Restaurant>  restaurants = restaurantDao.GetAllRestaurants(out string error);
            return restaurants;
        }

        
    }
}

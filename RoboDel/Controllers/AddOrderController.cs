using RoboDel.Dao;
using RoboDel.Models;
using RoboDel.Controllers;
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
    public class AddOrderController : Controller
    {
        public static List<Order> order { get; private set; }

        public AddOrderController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            RestaurantController restaurantController = new RestaurantController();
            ViewBag.restaurants = restaurantController.GetAllRestaurants();
            return View();
        }



        public IActionResult GoToAddOrder()
        {
            return Json(new { errMsg = "" });
        }

        

        public IActionResult AddAnOrder(string restaurantEmail, string pickupTime, bool readyForPickup, string firstName, string lastName,  string phoneNumber, string email, string address, string apartment, string city,  string zip, string state, string country)
        {
            CustomerDao customerDao = new CustomerDao();
            OrderDao orderDao = new OrderDao();
            int customerIDCheck = customerDao.CustomerExists(firstName, lastName, email, phoneNumber, address, apartment, city, zip, state, country, out string error);
            if (customerIDCheck != -1)
            {
                int customerID = customerIDCheck;
                _ = orderDao.AddOrder(pickupTime, readyForPickup, restaurantEmail, customerID, out error);
            }
            else
            {
                if (customerDao.AddCustomer(firstName, lastName, email, phoneNumber, address, apartment, city, zip, state, country, out  error))
                {
                    int customerID = customerDao.GetLastInsertedCustomerID();
                    _ = orderDao.AddOrder(pickupTime, readyForPickup, restaurantEmail, customerID, out error);
                }
            }
            return Json(new { errMsg = error });
        }



    }
}

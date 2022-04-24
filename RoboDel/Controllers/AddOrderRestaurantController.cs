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
    public class AddOrderRestaurantController : Controller
    {
        public static List<Order> order { get; private set; }

        public AddOrderRestaurantController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            return View();
        }



        public IActionResult GoToAddOrder()
        {
            return Json(new { errMsg = "" });
        }

        

        public IActionResult AddAnOrderRestaurant(string pickupTime, bool readyForPickup, string firstName, string lastName,  string phoneNumber, string email, string address, string city,  string zip, string state, string country)
        {
            string restaurantEmail = HttpContext.Session.GetString("restaurant_email");
            CustomerDao customerDao = new CustomerDao();
            OrderDao orderDao = new OrderDao();
            int customerIDCheck = customerDao.CustomerExists(firstName, lastName, email, phoneNumber, address, city, zip, state, country, out string error);
            if (customerIDCheck != -1)
            {
                int customerID = customerIDCheck;
                _ = orderDao.AddOrder(pickupTime, readyForPickup, restaurantEmail, customerID, out error);
            }
            else
            {
                if (customerDao.AddCustomer(firstName, lastName, email, phoneNumber, address, city, zip, state, country, out  error))
                {
                    int customerID = customerDao.GetLastInsertedCustomerID();
                    _ = orderDao.AddOrder(pickupTime, readyForPickup, restaurantEmail, customerID, out error);
                }
            }
            return Json(new { errMsg = error });
        }



    }
}

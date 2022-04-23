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
    public class AddOrderController : Controller
    {
        public static List<Order> order { get; private set; }

        public AddOrderController()
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

        public IActionResult AddAnOrder(string restaurantName, string pickupTime, bool readyForPickup, string firstName, string lastName,  string phoneNumber, string email, string address, string city,  string zip, string state, string country)
        {
            //PostalCodeDao postalCodeDao = new PostalCodeDao();
            //UserDao userDao = new UserDao();
            //PhoneDao phoneDao = new PhoneDao();

            //if (userDao.UserExists(email))
            //{
            //    return Json(new { errMsg = "User account with that email already exists." });
            //}

            //if (postalCodeDao.ValidatePostalCode(postalCode, city, state, out string error))
            //{
            //    if (phoneDao.ValidatePhone(phoneNumber, email, out error))
            //    {
            //        if (userDao.RegisterUser(email, password, nickname, firstName, lastName, postalCode, out error))
            //        {
            //            _ = phoneDao.AddPhone(phoneNumber, type, ifShare, email, out error);
            //        }
            //    }
            //}
            //if (error == "")
            //{
            //    HttpContext.Session.SetString("session_email", email);
            //    HttpContext.Session.SetString("session_firstName", firstName);
            //    HttpContext.Session.SetString("session_lastName", lastName);
            //}
            string error = "";
            return Json(new { errMsg = error });
        }



    }
}

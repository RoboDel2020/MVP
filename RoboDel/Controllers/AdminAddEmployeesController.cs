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
    public class AdminAddEmployeesController : Controller
    {
        
        public AdminAddEmployeesController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            return View();
        }


        public IActionResult GoToAddAddEmployee()
        {
            return Json(new { errMsg = "" });
        }

        public IActionResult AddEmployee(string username, string email, string password, string phoneNumber, string address, string city, string state, string postalCode, string country, string firstName, string lastName, bool operatorRole, bool adminRole, bool courierRole)
        {
            UserDao userDao = new UserDao();

            if (userDao.UserExists(username))
            {
                return Json(new { errMsg = "User account with that username already exists." });
            }
            if (userDao.RegisterUser(username,  email,  password,  phoneNumber,  address,  city,  state,  postalCode,  country,  firstName,  lastName, out string error))
            {
                if (adminRole)
                {
                    userDao.AddUserRole(username, "admin", out error);
                }
                if (courierRole)
                {
                    userDao.AddUserRole(username, "courier", out error);
                }
                if (operatorRole)
                {
                    userDao.AddUserRole(username, "operator", out error);
                }
            }
            return Json(new { errMsg = error });
        }

    }
}

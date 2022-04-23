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
    public class LoginController : Controller
    {
        public static User user { get; private set; }

        public LoginController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            return View();
        }


        public IActionResult ValidateLogin(string credentials, string password)
        {
            UserDao userDao = new UserDao();
            if (userDao.ValidateUser(credentials, out string error))
            {
                user = userDao.ValidatePassword(credentials, password, out error);
                if (user != null)
                {
                    HttpContext.Session.SetString("session_username", user.Username);
                    HttpContext.Session.SetString("session_firstName", user.FirstName);
                    HttpContext.Session.SetString("session_lastName", user.LastName);
                }
            }
            return Json(new { errMsg = error });
        }


        public IActionResult Logout(string itemNumber)
        {
            HttpContext.Session.SetString("session_username", "");
            return Json(new { errMsg = "" });
        }
    }
}

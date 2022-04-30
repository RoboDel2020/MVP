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
    public class AdminEmployeesController : Controller
    {
        
        public AdminEmployeesController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            ViewBag.users = this.GetAllUsers();
            ViewBag.blankspace = " ";
            return View();
        }


        public IActionResult GoToAdminEmployees()
        {
            return Json(new { errMsg = "" });
        }

        public List<User> GetAllUsers()
        {
            UserDao userDao = new UserDao();
            List<User> users = userDao.GetAllUsers(out string error);
            return users;
        }

        
    }
}

using RoboDel.Dao;
using RoboDel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.Controllers
{
    public class MainMenuController : Controller
    {
        public IActionResult Index()
        {
            Database.Init();


            ViewBag.currentUserEmail = HttpContext.Session.GetString("session_username");
            return View();
        }



        public IActionResult GoMainMenu()
        {
            return Json(new { errMsg = "" });
        }
    }
}

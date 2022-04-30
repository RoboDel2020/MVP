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
    public class AdminPanelController : Controller
    {
        public IActionResult Index()
        {
            Database.Init();

            return View();
        }



        public IActionResult GoToAdminPanel()
        {
            return Json(new { errMsg = "" });
        }
    }
}

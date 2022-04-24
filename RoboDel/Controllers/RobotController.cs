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
    public class RobotController : Controller
    {
        public static List<Robot> robot { get; private set; }

        public RobotController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            ViewBag.allRobots = this.GetAllRobotsWithCurrentStatistics();
            return View();
        }



        public IActionResult GoToAllRobots()
        {
            return Json(new { errMsg = "" });
        }

        public List<Robot> GetAllRobots()
        {
            RobotDao robotDao = new RobotDao();
            List<Robot> allRobots = robotDao.GetAllRobots(out string error);
            return allRobots;
        }

        public List<Robot> GetAllRobotsWithCurrentStatistics()
        {
            RobotDao robotDao = new RobotDao();
            List<Robot> allRobots = robotDao.GetAllRobotsWithCurrentStatistics(out string error);
            return allRobots;
        }

    }
}

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
    public class ViewOrderController : Controller
    {
        public static Order order { get; private set; }

        public ViewOrderController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            ViewBag.order = order;
            return View();
        }



        public IActionResult GoToViewOrder(string orderID)
        {
            _ = GetOrderDetails(Int16.Parse(orderID), out string error);
            return Json(new { errMsg = error });
        }

        public IActionResult GetOrderDetails(int orderID, out string error)
        {
            OrderDao orderDao = new OrderDao();
            order = orderDao.GetOrderDetails(orderID, out  error);
            return Json(new { errMsg = error });
        }
    }
}

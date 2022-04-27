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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RoboDel.Controllers
{
    public class OrderController : Controller
    {
        public static List<Order> order { get; private set; }

        public OrderController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            ViewBag.allPendingOrders = this.GetAllPendingOrders();
            ViewBag.allCanceledOrders = this.GetAllCanceledOrders();
            ViewBag.allDeliveredOrders = this.GetAllDeliveredOrders();
            ViewBag.allInProgressOrders = this.GetAllInProgressOrders();
            return View();
        }


        public IActionResult GoToAllOrders()
        {
            return Json(new { errMsg = "" });
        }

        public List<Order> GetAllOrders()
        {
            OrderDao orderDao = new OrderDao();
            List<Order> allOrders = orderDao.GetAllOrders(out string error);
            return allOrders;
        }

        public List<Order> GetAllPendingOrders()
        {
            OrderDao orderDao = new OrderDao();
            List<Order> allOrders = orderDao.GetAllPendingOrders(out string error);
            return allOrders;
        }

        public List<Order> GetAllCanceledOrders()
        {
            OrderDao orderDao = new OrderDao();
            List<Order> allOrders = orderDao.GetAllCanceledOrders(out string error);
            return allOrders;
        }

        public List<Order> GetAllDeliveredOrders()
        {
            OrderDao orderDao = new OrderDao();
            List<Order> allOrders = orderDao.GetAllDeliveredOrders(out string error);
            return allOrders;
        }

        public List<Order> GetAllInProgressOrders()
        {
            OrderDao orderDao = new OrderDao();
            List<Order> allOrders = orderDao.GetAllInProgressOrders(out string error);
            return allOrders;
        }

        public List<Order> GetAllOrdersByRestaurantEmail( string restaurantEmail)
        {
            OrderDao orderDao = new OrderDao();
            List<Order> allOrders = orderDao.GetAllOrdersByRestaurantEmail(restaurantEmail, out string error);
            return allOrders;
        }

        public IActionResult AddCoordinates(string orderId, string longitude, string latitude)
        {

            OrderDao orderDao = new OrderDao();
             _ = orderDao.AddCoordinates(Int16.Parse(orderId), double.Parse(longitude), double.Parse(latitude), out string error);
            return Json(new { errMsg = error });
        }

    }
}

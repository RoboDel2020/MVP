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
    public class AssignRobotController : Controller
    {
        public static string orderID { get; private set; }
        public static string orderLongitude { get; private set; }
        public static string orderLatitude { get; private set; }
        public static string restaurantLongitude { get; private set; }
        public static string restaurantLatitude { get; private set; }
        public static double distanceFromRestaurantToCustomer { get; private set; }
        public static double durationFromRestaurantToCustomer { get; private set; }
        private readonly HttpClient _httpClient = new HttpClient();
        public static List<CourierForDelivery> couriersForDelivery { get; private set; }
        public AssignRobotController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            ViewBag.orderID = orderID;
            ViewBag.distanceFromRestaurantToCustomer = distanceFromRestaurantToCustomer;
            ViewBag.durationFromRestaurantToCustomer = durationFromRestaurantToCustomer;
            ViewBag.orderLongitude = orderLongitude;
            ViewBag.orderLatitude = orderLatitude;
            ViewBag.restaurantLongitude = restaurantLongitude;
            ViewBag.restaurantLatitude = restaurantLatitude;
            ViewBag.allActiveCouriersForDelivery = couriersForDelivery;
            ViewBag.robots = GetOnlyRobots();
            return View();
        }

        public string get_web_content(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string output = reader.ReadToEnd();
            response.Close();

            return output;
        }



        public IActionResult GoToAssignRobots(string inputOrderID, string inputOrderLongitude, string inputOrderLatitude, string inputRestaurantLongitude, string inputRestaurantLatitude)
        {
            orderID = inputOrderID;
            orderLongitude = inputOrderLongitude;
            orderLatitude = inputOrderLatitude;
            restaurantLongitude = inputRestaurantLongitude;
            restaurantLatitude = inputRestaurantLatitude;
            this.GetDistanceAndDurationOfRestaurantAndCustomer();
            this.GetDistanceAndDuration();
            return Json(new { errMsg = "" });
        }

        public List<CourierForDelivery> GetAllActiveRobotsWithCurrentStatisticsAndOrders()
        {
            RobotDao robotDao = new RobotDao();
            List<CourierForDelivery> couriersForDelivery = robotDao.GetAllActiveRobotsWithCurrentStatisticsAndOrders(out string error);
            return couriersForDelivery;
        }

        public void GetDistanceAndDuration()
        {
            List<CourierForDelivery> initialCouriers = this.GetAllActiveRobotsWithCurrentStatisticsAndOrders();
            List<CourierForDelivery> courierssForDelivery = new List<CourierForDelivery>();
            foreach (CourierForDelivery courierForDelivery in initialCouriers)
            {
                try
                {
                    string url = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=5b3ce3597851110001cf62488303e1e1bcb64deb91105af7032e30d4&start=" + courierForDelivery.Robot.CurrentRobotStatistics.Longitude.ToString() + "," + courierForDelivery.Robot.CurrentRobotStatistics.Latitude.ToString()  + "&end=" + restaurantLongitude + "," + restaurantLatitude;
                    try
                    {
                        if (courierForDelivery.Delivery.Order.Longitude != 0 && courierForDelivery.Delivery.Order.Latitude != 0)
                        {
                            url = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=5b3ce3597851110001cf62488303e1e1bcb64deb91105af7032e30d4&start=" + courierForDelivery.Delivery.Order.Longitude.ToString() + "," + courierForDelivery.Delivery.Order.Latitude.ToString()  + "&end=" + restaurantLongitude + "," + restaurantLatitude ;
                        }
                    }
                    catch
                    {

                    }
                    Console.WriteLine(url);
                    string json = get_web_content(url);
                    dynamic array = JsonConvert.DeserializeObject(json);
                    courierForDelivery.Distance = array.features[0].properties.summary.distance / 1000;
                    courierForDelivery.Duration = array.features[0].properties.summary.duration / 60;
                }
                catch
                {
                    courierForDelivery.Distance = 1000;
                    courierForDelivery.Duration = 1000;
                }
                
                courierssForDelivery.Add(courierForDelivery);
            }
            couriersForDelivery = courierssForDelivery;
        }

        public void GetDistanceAndDurationOfRestaurantAndCustomer()
        {
            try
            {
                string url = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=5b3ce3597851110001cf62488303e1e1bcb64deb91105af7032e30d4&start=" + orderLongitude.ToString() + "," + orderLatitude.ToString()  + "&end=" + restaurantLongitude + "," + restaurantLatitude  ;
                string json = get_web_content(url);
                dynamic array = JsonConvert.DeserializeObject(json);
                distanceFromRestaurantToCustomer = array.features[0].properties.summary.distance / 1000;
                durationFromRestaurantToCustomer = array.features[0].properties.summary.duration / 60;
            }
            catch
            {
                distanceFromRestaurantToCustomer = 1000;
                durationFromRestaurantToCustomer = 1000;
            }
        }

        public List<Robot> GetOnlyRobots()
        {
            List<Robot> robots = new List<Robot>();
            foreach (CourierForDelivery courierForDelivery in couriersForDelivery)
            {
                robots.Add(courierForDelivery.Robot);
            }
            return robots;
        }

        public IActionResult AssignRobotToOrder(int inputOrderID, int inputCourierID, string state)
        {
            OrderDao orderDao = new OrderDao();
            DeliveryDao deliveryDao = new DeliveryDao();

            if (deliveryDao.AddDelivery(inputOrderID, out string error))
            {
                int newDeliveryID = deliveryDao.GetActiveDeliveryByOrderID(inputOrderID);
                if (deliveryDao.AddCourierForDelivery(newDeliveryID, inputCourierID, out  error))
                {
                    _ = orderDao.UpdateOrderStatusToInProgress(inputOrderID, out error);
                }
                
            }
            return Json(new { errMsg = error });
        }
    }
}

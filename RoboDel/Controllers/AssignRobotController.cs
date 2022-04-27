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
        private readonly HttpClient _httpClient = new HttpClient();
        public static List<CourierForDelivery> couriersForDelivery { get; private set; }
        public AssignRobotController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            List<CourierForDelivery> initialCouriers = this.GetAllActiveRobotsWithCurrentStatisticsAndOrders();
            ViewBag.orderID = orderID;
            ViewBag.orderLongitude = orderLongitude;
            ViewBag.orderLatitude = orderLatitude;
            ViewBag.restaurantLongitude = restaurantLongitude;
            ViewBag.restaurantLatitude = restaurantLatitude;
            List<CourierForDelivery> couriersForDelivery = new List<CourierForDelivery>();
            foreach (CourierForDelivery courierForDelivery in initialCouriers)
            {
                string url = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=5b3ce3597851110001cf62488303e1e1bcb64deb91105af7032e30d4&start=" + courierForDelivery.Robot.CurrentRobotStatistics.Latitude.ToString() + "," + courierForDelivery.Robot.CurrentRobotStatistics.Longitude.ToString() + "&end=" + restaurantLatitude + "," + restaurantLongitude;
                try
                {
                    if (courierForDelivery.Delivery.Order.Longitude !=0 && courierForDelivery.Delivery.Order.Latitude!=0) {
                        url = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=5b3ce3597851110001cf62488303e1e1bcb64deb91105af7032e30d4&start=" + courierForDelivery.Delivery.Order.Latitude.ToString() + "," + courierForDelivery.Delivery.Order.Longitude.ToString() + "&end=" + restaurantLatitude + "," + restaurantLongitude;
                    }
                }
                catch
                {

                }
                Console.WriteLine(url);
                string json = get_web_content(url);
                dynamic array = JsonConvert.DeserializeObject(json);
                courierForDelivery.Distance = array.features[0].properties.summary.duration;
                couriersForDelivery.Add(courierForDelivery);
            }

            ViewBag.allActiveCouriersForDelivery = couriersForDelivery;

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
            return Json(new { errMsg = "" });
        }

        public List<CourierForDelivery> GetAllActiveRobotsWithCurrentStatisticsAndOrders()
        {
            RobotDao robotDao = new RobotDao();
            List<CourierForDelivery> couriersForDelivery = robotDao.GetAllActiveRobotsWithCurrentStatisticsAndOrders(out string error);
            return couriersForDelivery;
        }
    }
}

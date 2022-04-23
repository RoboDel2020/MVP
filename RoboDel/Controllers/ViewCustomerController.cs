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
    public class ViewCustomerController : Controller
    {
        public static Customer customer { get; private set; }

        public ViewCustomerController()
        {
        }

        public IActionResult Index()
        {
            Database.Init();
            ViewBag.customer = customer;
            return View();
        }



        public IActionResult GoToViewCustomer(string customerID)
        {
            _ = GetCustomerDetails(Int16.Parse(customerID), out string error);
            return Json(new { errMsg = error });
        }

        public IActionResult GetCustomerDetails(int customerID, out string error)
        {
            CustomerDao customerDao = new CustomerDao();
            customer = customerDao.GetCustomerDetails(customerID, out  error);
            return Json(new { errMsg = error });
        }
    }
}

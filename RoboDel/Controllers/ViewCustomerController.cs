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
            string zip = "";
            string apartment = "";
            string state = "";
            try
            {
                if (@ViewBag.customer.Apartment.Length > 0)
                {
                    apartment = ", Apt " + @ViewBag.customer.Apartment;
                }
            }
            catch { }
            
            try
            {
                if (@ViewBag.customer.State.Length > 0)
                {
                    state = ", " + @ViewBag.customer.State;
                }
            }
            catch { }
            
            try
            {
                if (@ViewBag.customer.Zip.Length > 0)
                {
                    zip = ", " + @ViewBag.customer.Zip;
                }
            }
            catch { }
            
            ViewBag.apartment = apartment;
            ViewBag.state = state;
            ViewBag.zip = zip;

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

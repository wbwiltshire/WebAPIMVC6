using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SeagullConsulting.WebAPIMVC6Website.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Title"] = "About";
            ViewData["Message"] = "A sample MVC 6 WebAPI application written using ASP.Net Core, Bootstrap, Handlebars, and TurboTables: version";
            ViewData["Version"] = "(v0.9.0)";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

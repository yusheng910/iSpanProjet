using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Member()
        {
            return View();
        }
        public IActionResult Order()
        {
            var q = from p in (new 鮮蔬果季Context()).Orders
                    select p;
            return View(q);
        }
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Customer()
        {
            var q = from p in (new 鮮蔬果季Context()).Members
                    select p;
            return View(q);
        }

        public IActionResult Event()
        {
            return View();
        }

    }
}

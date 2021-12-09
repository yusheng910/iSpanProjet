using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }
    }
}

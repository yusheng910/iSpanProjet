using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace 鮮蔬果季_前台.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult AboutUS()
        {
            return View();
        }
    }
}

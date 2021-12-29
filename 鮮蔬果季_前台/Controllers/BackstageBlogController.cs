using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.ViewModels;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageBlogController : Controller
    {
        public IActionResult BlogList()
        {
            return View();
        }
    }
}

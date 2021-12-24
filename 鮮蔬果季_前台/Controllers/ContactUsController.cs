using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
namespace 鮮蔬果季_前台.Controllers
{
    public class ContactUsController : Controller
    {
        
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Feebacks()
        {
            return View();
        }
    }
    
}

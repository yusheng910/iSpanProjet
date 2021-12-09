using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace 鮮蔬果季_前台.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult OrderDetail()
        {
            return View();
        }
        public IActionResult MyFavoriteList()
        {
            return View();
        }
        public IActionResult CouponsList()
        {
            return View();
        }
        public IActionResult MemberCenter()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            UserLogin.member = null;
            return RedirectToAction("Index","Home");
        }
        public IActionResult MyFavoriteList()
        {
            return View();
        }
        public IActionResult MemberCenter()
        {
            return View();
        }
    }
}

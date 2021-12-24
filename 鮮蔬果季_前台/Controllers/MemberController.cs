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
        private readonly 鮮蔬果季Context db;
        public MemberController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            UserLogin.member = null;
            return RedirectToAction("Index","Home");
        }
        public IActionResult MyFavoriteList()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }
            return View();
        }
        public IActionResult MemberCenter()
        {
            MemberViewModel mv = null;
            var mc = (from i in db.Members
                      join a in db.Cities on i.CityId equals a.CityId
                      where i.UserId == UserLogin.member.UserId
                      select new { i, a.CityName }).FirstOrDefault();
            if(HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.CITY = mc.CityName;
                ViewBag.GENDER = mc.i.Gender;
                mv = new MemberViewModel()
                {
                    member = mc.i,
                    city = mc.CityName,
                   
                };
                
            }
            else
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login","Login");
            }

            return View(mv);
        }
        [HttpPost]
        public IActionResult MemberCenter(Member p)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }

            else
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }

            return View();
        }
    }
}

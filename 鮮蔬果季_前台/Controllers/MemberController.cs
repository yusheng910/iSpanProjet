using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class MemberController : Controller
    {
        //IWebHostEnvironment _enviroment;
        private readonly 鮮蔬果季Context db;
        public MemberController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        //public MemberController(IWebHostEnvironment p)
        //{
        //    _enviroment = p;
        //}
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
                ViewBag.userID = UserLogin.member.MemberId;
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
        public IActionResult MemberCenter(MemberViewModel p)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
                var cityid = (from i in db.Cities
                              where p.city==i.CityName
                              select i.CityId).FirstOrDefault();

                Member cust = db.Members.FirstOrDefault(c => c.MemberId == p.MemberId);
                if(cust!=null)
                {
                    cust.UserId = p.UserId;
                    cust.Mobile = p.Mobile;
                    cust.MemberAddress = p.MemberAddress;
                    cust.CityId = cityid;
                    cust.Gender = p.Gender;
                    cust.MemberName = p.MemberName;
                    //cust.MemberPhotoPass = p.MemberPhotoPass;
                    db.SaveChanges();
                    
                };
                

            }

            else
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }

            return View();
        }
        //[HttpPost] //上傳檔案(待測試)

        //public IActionResult uploadPhoto(IFormFile photo)
        //{
        //    photo.CopyTo(new FileStream(_enviroment.WebRootPath+@"\MemberPhoto\test.jpg",FileMode.Create));
        //    return View();
        //}
    }
}

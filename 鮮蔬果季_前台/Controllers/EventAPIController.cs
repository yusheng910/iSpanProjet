using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class EventAPIController : Controller
    {

        // IWebHostEnvironment 介面，提供應用程式執行所在之伺服器的相關環境資訊
        private readonly IWebHostEnvironment _hostingEnvironment;
        public EventAPIController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        // 使用注入的方式啟用db (注入程式在Startup內)
        // 後續引用資料庫,直接使用以下設定的變數db即可
        // 在使用LINQ時,每次都要有斷點(可以用ToList / FirstOrDefault)
        private readonly 鮮蔬果季Context db;

        public EventAPIController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }



        [HttpPost]
        public IActionResult EventSignUpAPI(EventRegistration SignUpForm)  //怪怪的
        {                                      
            // 判斷會員是否登入
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                EventRegistration 送出報名資料 = new EventRegistration()
                {
                    MemberId = UserLogin.member.MemberId,
                    ParticipantNumber = SignUpForm.ParticipantNumber,
                    FoodPreference = SignUpForm.FoodPreference,
                    ContactName = SignUpForm.ContactName,
                    ContactEmail = SignUpForm.ContactEmail,
                    ContactMobile = SignUpForm.ContactMobile,
                    SubmitDate = DateTime.Now,
                };

                db.Add(送出報名資料);
                db.SaveChanges();
                return Content("AA"); //回傳值做判斷寫入用
            }

            else  //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");   //返回登入頁面
            }
        }


      





        public IActionResult Index()
        {
            return View();
        }
    }
}

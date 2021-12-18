using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel LOGIN)
        {
            Member user = (new 鮮蔬果季Context()).Members.FirstOrDefault(t => t.UserId.Equals(LOGIN.username) && t.Password.Equals(LOGIN.password));
            if (user != null)
            {
                if(user.UserId.Equals(LOGIN.username)&& user.Password.Equals(LOGIN.password))
                {
                    string json = "";

                    json = JsonSerializer.Serialize(user);
                    HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);
                    UserLogin.member = user;
                    return RedirectToAction("Index","Home");

                }
            }
            return View();
        }
        
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(MemberViewModel p)
        {
            鮮蔬果季Context db = new 鮮蔬果季Context();
            DateTime rt = new DateTime(2019, 01, 01);  //註冊日期固定
            var CityID =(from a in db.Cities
                         where p.city.Equals(a.CityName)
                         select a.CityId).FirstOrDefault();
            var newMember = new Member
            {
                UserId = p.UserId,
                Password = p.Password,
                MemberName = p.MemberName,
                Gender = p.Gender,
                CityId = CityID,
                MemberAddress=p.MemberAddress,
                BirthDate=p.BirthDate,
                Mobile=p.Mobile,
                RegisteredDate=rt,
                MemberPhotoPass= "inihead.png"
            };
            
            db.Add(newMember);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        public IActionResult AccountVerification(string email)
        {
            Member user = (new 鮮蔬果季Context()).Members.FirstOrDefault(t => t.UserId.Equals(email));
            if (user != null)
            {
                return Content("帳號已被註冊");
            }
            return Content("");
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class LoginController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public LoginController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel LOGIN)
        {
            Member user = db.Members.FirstOrDefault(t => t.UserId.Equals(LOGIN.username) && t.Password.Equals(LOGIN.password));
            Supplier supplier = db.Suppliers.FirstOrDefault(t => t.SupplierAccount.Equals(LOGIN.username) && t.SupplierPassword.Equals(LOGIN.password));
            if (user != null)
            {
                  if (user.UserId.Equals("adminuser") && user.Password.Equals("a12345"))
                {
                    string json = "";

                    json = JsonSerializer.Serialize(user);
                    HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);
                    UserLogin.member = user;
                    return RedirectToAction("Home", "Backstage");
                }
                else if (user.UserId.Equals(LOGIN.username) && user.Password.Equals(LOGIN.password))
                {
                    string json = "";

                    json = JsonSerializer.Serialize(user);
                    HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);
                    UserLogin.member = user;
                    return RedirectToAction("Index", "Home");

                }
               
                else if(user==null)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            //else if (supplier != null)
            //{
            //    if (supplier.SupplierAccount.Equals(LOGIN.username) && supplier.SupplierPassword.Equals(LOGIN.password))
            //    {
            //        string json = "";
            //        json = JsonSerializer.Serialize(user);
            //        HttpContext.Session.SetString(CDictionary.SK_LOGINED_SUPPLIER, json);
            //        UserLogin.supplier = supplier;
            //        return RedirectToAction("Index", "Home");

            //    }
            //    else if (supplier == null)
            //    {
            //        return RedirectToAction("Login", "Login");
            //    }


            //}
            return View();
        }
        
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(MemberViewModel p)
        {
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
            Member user = db.Members.FirstOrDefault(t => t.UserId.Equals(email));

            if (email != null)
            {
                if ((new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")).IsMatch(email))
                {
                    if (user != null)
                    {
                        return Content("帳號已被註冊");
                    }
                    return Content("");
                }
            }
            return Content("帳號格式不正確");
        }
        public IActionResult PasswordRegex(string Password)
        {
            if (Password != null)
            {
                if (Password.Length >= 6)
                {
                    if ((new Regex(@"^[a-zA-Z]\w{5,17}$")).IsMatch(Password))
                    {
                        return Content("");
                    }
                }
                return Content("密碼格式不正確");
            }
            return Content("");
        }
        public IActionResult MemberNameRegex(string MemberName)
        {
            if (MemberName != null)
            {
                if (MemberName.Length< 2)
                {
                    return Content("字數不能小於兩字");
                }
            }
            return Content("");
        }
        public IActionResult MobileRegex(string Mobile)
        {
            if (Mobile != null)
            {
                if ((new Regex(@"^[0-9]*$")).IsMatch(Mobile))
                {
                    if (Mobile.Length < 10)
                    {
                        return Content("號碼長度不能小於10碼");
                    }
                    return Content("");
                }
                return Content("請輸入數字");
            }
            return Content("");
        }
        public IActionResult BirthDateRegex(DateTime birthdate)
        {
            
            if (birthdate >DateTime.Now)
            {
                return Content("出生日期大於今日");
            }
            return Content("");
        }
        public IActionResult LoadCities()
        {
            
            var cities = db.Cities.Select(a => new
            {
                a.CityName
            }).Distinct().OrderBy(a => a.CityName);
            return Json(cities);
        }
        public IActionResult ExistPassword(string ExistuserName, string ExistuserPassword)
        {
            var cust = (from p in db.Members
                            where p.UserId == ExistuserName
                            select new {p.UserId ,p.Password }).FirstOrDefault();
            if (ExistuserName != null && ExistuserPassword!=null )
            {
                if (cust != null)
                {
                    if (cust.Password != ExistuserPassword)
                    {
                        return Content("1");
                    }
                    return Content("2");
                }
                return Content("3");
                
            }
            return Content("0");
        }
        public IActionResult forgetPassword(string acount)
        {

            return PartialView();
        }

    }
}

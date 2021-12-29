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
    public class MemberController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly 鮮蔬果季Context db;
        public MemberController(IWebHostEnvironment webHost,鮮蔬果季Context dbContext )
        {
            _hostingEnvironment = webHost;//取得wwwroot的路徑
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
            MemberViewModel mv = null;
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
                var mc = (from i in db.Members
                          join a in db.Cities on i.CityId equals a.CityId
                          where i.UserId == UserLogin.member.UserId
                          select new { i, a.CityName }).FirstOrDefault();
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
                return RedirectToAction("Login", "Login");
            }

            return View(mv);
        }
        public IActionResult PasswordChange()
        {
            MemberViewModel mv = null;
            var mc = (from i in db.Members
                     where i.MemberId == UserLogin.member.MemberId
                     select new { i.MemberId }).FirstOrDefault();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                mv = new MemberViewModel()
                {
                    MemberId=mc.MemberId
                };
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }

            return View(mv);
        }
        [HttpPost]
        public IActionResult PasswordChange(MemberViewModel p)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                Member cust = db.Members.FirstOrDefault(c => c.MemberId == p.MemberId);
                if (cust != null)
                {
                    cust.Password = p.Password;
                    db.SaveChanges();
                };
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
            return RedirectToAction("Login", "Login");
        }
        public IActionResult CheckPassword(string oldPassword)
        {
            if (oldPassword != null)
            {
                if (oldPassword != UserLogin.member.Password)
                {
                    return Content("密碼不相符");
                }
            }
            return Content("");
        }
        public IActionResult CheckNewPassword(string newPassword)
        {
            if (newPassword != null)
            {
                if (newPassword != UserLogin.member.Password)
                {
                    if((new Regex(@"^[a-zA-Z]\w{5,17}$")).IsMatch(newPassword))
                    {
                        return Content("");
                    }
                    return Content("新密碼格式錯誤");
                }
                else if (newPassword==UserLogin.member.Password)
                {
                    return Content("與舊密碼相同");
                }
                
            }
            return Content("");
        }
        //public IActionResult partialPassword()
        //{
        //    return PartialView();
        //}
        //public IActionResult partialMemberinfo()
        //{
        //    MemberViewModel mv = null;
        //    var mc = (from i in db.Members
        //              join a in db.Cities on i.CityId equals a.CityId
        //              where i.UserId == UserLogin.member.UserId
        //              select new { i, a.CityName }).FirstOrDefault();
        //    if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
        //    {
        //        ViewBag.USER = UserLogin.member.MemberName;
        //        ViewBag.userID = UserLogin.member.MemberId;
        //        mv = new MemberViewModel()
        //        {
        //            member = mc.i,
        //            city = mc.CityName,
        //        };

        //    }
        //    else
        //    {
        //        ViewBag.USER = null;
        //        UserLogin.member = null;
        //        return RedirectToAction("Login", "Login");
        //    }
        //    return PartialView(mv);
        //}
        //[HttpPost]
        //public IActionResult partialMemberinfo(MemberViewModel p)
        //{
        //    MemberViewModel mv = null;
        //    if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
        //    {
        //        ViewBag.USER = UserLogin.member.MemberName;
        //        ViewBag.userID = UserLogin.member.MemberId;
        //        var cityid = (from i in db.Cities
        //                      where p.city == i.CityName
        //                      select i.CityId).FirstOrDefault();

        //        Member cust = db.Members.FirstOrDefault(c => c.MemberId == p.MemberId);
        //        if (cust != null)
        //        {
        //            cust.UserId = p.UserId;
        //            cust.Mobile = p.Mobile;
        //            cust.MemberAddress = p.MemberAddress;
        //            cust.CityId = cityid;
        //            cust.Gender = p.Gender;
        //            cust.MemberName = p.MemberName;
        //            cust.MemberPhotoPass = p.MemberPhotoPass;
        //            db.SaveChanges();

        //        };
        //        var mc = (from i in db.Members
        //                  join a in db.Cities on i.CityId equals a.CityId
        //                  where i.UserId == UserLogin.member.UserId
        //                  select new { i, a.CityName }).FirstOrDefault();
        //        mv = new MemberViewModel()
        //        {
        //            member = mc.i,
        //            city = mc.CityName,
        //        };
        //    }
        //    else
        //    {
        //        ViewBag.USER = null;
        //        UserLogin.member = null;
        //    }

        //    return PartialView(mv);
        //}
        public IActionResult uploadPhoto(MemberViewModel memberphoto)
        {
            if (memberphoto.photo != null)
            {
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/MemberPhoto/" + photoName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))//創造新圖片,如果已存在就覆寫
                {
                    memberphoto.photo.CopyTo(fileStream);//上傳指令
                }
                Member cust= db.Members.FirstOrDefault(c => c.MemberId == memberphoto.MemberId);
                if (cust!= null)
                {
                    cust.MemberPhotoPass = photoName;
                    db.SaveChanges();
                }
            }
            else
            {
                return Content("0");
            }
            return Content("1");
         }
        public IActionResult Photoload(int id)
        {
            var q = db.Members.Where(p => p.MemberId == id).FirstOrDefault();
            return PartialView(q);
        }
    }
}

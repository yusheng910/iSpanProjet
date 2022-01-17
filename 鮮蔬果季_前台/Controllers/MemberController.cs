using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
            //廣告商品列表
            List<MyFavoriteViewModel> 商品列表 = new List<MyFavoriteViewModel>();
            var 所有產品 = (from prod in db.Products
                        join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                        join c in db.MyFavorites
                        on prod.ProductId equals c.ProductId
                        where c.MemberId == UserLogin.member.MemberId
                        orderby c.MyFavoriteId
                        select new { prod, supp, c }).ToList();
            foreach (var item in 所有產品)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.prod.ProductId);
                相片List.Add(封面相片);
                商品列表.Add(new MyFavoriteViewModel()
                {
                    product = item.prod,
                    supplier = item.supp,
                    photoBank = 相片List,
                    myFavorite = item.c,
                });
            }
            return View(商品列表);
        }        
        public IActionResult RemoveFavorite(int id)
        {
            Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            UserLogin.member = user;
            var q = db.MyFavorites.FirstOrDefault(a => a.MyFavoriteId == id);
            db.MyFavorites.Remove(q);
            db.SaveChanges();
            List<MyFavoriteViewModel> 商品列表 = new List<MyFavoriteViewModel>();
            var 所有產品 = (from prod in db.Products
                        join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                        join c in db.MyFavorites
                        on prod.ProductId equals c.ProductId
                        where c.MemberId == UserLogin.member.MemberId
                        orderby c.MyFavoriteId
                        select new { prod, supp, c }).ToList();
            foreach (var item in 所有產品)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.prod.ProductId);
                相片List.Add(封面相片);
                商品列表.Add(new MyFavoriteViewModel()
                {
                    product = item.prod,
                    supplier = item.supp,
                    photoBank = 相片List,
                    myFavorite = item.c,
                });
            }
            return PartialView(商品列表);
        }
        public IActionResult MemberCenter()
        {
            try
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                MemberViewModel mv = null;
                var mc = (from i in db.Members
                          join a in db.Cities on i.CityId equals a.CityId
                          where i.UserId == UserLogin.member.UserId
                          select new { i, a.CityName }).FirstOrDefault();
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
                {
                    ViewBag.USER = UserLogin.member.MemberName;
                    ViewBag.userID = UserLogin.member.MemberId;
                    ViewBag.cityname = mc.CityName;
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
            catch
            {
                return RedirectToAction("Login", "Login");
            }

           
        }
        [HttpPost]
        public IActionResult MemberCenter(MemberViewModel p)
        {
            MemberViewModel mv = null;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
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
            Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            UserLogin.member = user;
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
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
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
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
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
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
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
                Member cust = db.Members.FirstOrDefault(c => c.MemberId == memberphoto.MemberId);
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/MemberPhoto/" + photoName);
                string oldfilePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/MemberPhoto/" + cust.MemberPhotoPass);
                bool result = System.IO.File.Exists(oldfilePath);
                if (cust.MemberPhotoPass != "inihead.png")
                {
                    if (result == true)
                    {
                        System.IO.File.Delete(oldfilePath);
                    }
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))//創造新圖片,如果已存在就覆寫
                {
                    memberphoto.photo.CopyTo(fileStream);//上傳指令
                }
                
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

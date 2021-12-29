﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    // 備註: 如要顯示多張照片,需要使用陣列,可參考shoppinglist

    public class EventController : Controller
    {
        // 使用注入的方式啟用db (注入程式在Startup內)
        // 後續引用資料庫,直接使用以下設定的變數db即可
        // 在使用LINQ時,每次都要有斷點(可以用ToList / FirstOrDefault)
        private readonly 鮮蔬果季Context db;

    public EventController(鮮蔬果季Context dbContext)
    {
        db = dbContext;
    }


        public IActionResult EventBlog(int id)
        {
            // 判斷會員是否登入
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

            List<EventListViewModel> 所有活動列表 = new List<EventListViewModel>();
            //鮮蔬果季Context db = new 鮮蔬果季Context();          //引用注入就不用new db Context
            var 所有活動 = (from E in db.Events
                        join supp in db.Suppliers
                       on E.SupplierId equals supp.SupplierId
                        select new { E, supp }).ToList();

            if (id == 1)
            { 
                所有活動 = (from E in db.Events
                join supp in db.Suppliers
                on E.SupplierId equals supp.SupplierId
                where E.LableId == 1
                select  new {E,supp }).ToList();
            }

            if (id == 2)
            {
                所有活動 = (from E in db.Events
                        join supp in db.Suppliers
                        on E.SupplierId equals supp.SupplierId
                        where E.LableId == 2
                        select new { E, supp }).ToList();
            }

            if (id == 3)
            {
                所有活動 = (from E in db.Events
                        join supp in db.Suppliers
                        on E.SupplierId equals supp.SupplierId
                        where E.LableId == 3
                        select new { E, supp }).ToList();
            }

            if (id == 4)
            {
                所有活動 = (from E in db.Events
                        join supp in db.Suppliers
                        on E.SupplierId equals supp.SupplierId
                        where E.LableId == 4
                        select new { E, supp }).ToList();
            }

            if (id == 5)
            {
                所有活動 = (from E in db.Events
                        join supp in db.Suppliers
                        on E.SupplierId equals supp.SupplierId
                        where E.LableId == 5
                        select new { E, supp }).ToList();
            }



            foreach (var item in 所有活動)
            {

                List<EventPhotoBank> 相片list = new List<EventPhotoBank>();
                //db = new 鮮蔬果季Context();                                  //使用注入,故不用在new db

                //原本LINQ的寫法,下面是轉換為Landa寫法(較簡潔也較抽象)
                //var 城市資料 = (from C in db.Cities
                //            where C.CityId == item.supp.CityId
                //            select C).FirstOrDefault();

                var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == item.supp.CityId);
                var 照片資料 = db.EventPhotoBanks.FirstOrDefault(P => P.EventId == item.E.EventId);
                相片list.Add(照片資料);
                //ViewBag.活動數量 =  db.Events.Count().ToString() ;            //活動數量總計
                //var 照片資料 = db.EventPhotoBanks.Where(P => P.EventId == item.E.EventId).ToList();
                所有活動列表.Add(new EventListViewModel()              
                {
                    Event = item.E,
                    City = 城市資料,
                    EventPhoto = 相片list,
                });
            }
            return View(所有活動列表);
        }

        //舊的留存
        ////鮮蔬果季Context db = new 鮮蔬果季Context();          //引用注入就不用new db Context
        //var datas = (from E in db.Events
        //                 //join 照片 in db.EventPhotoBanks on E.EventId equals 照片.EventId
        //             select E).ToList();

        //List<EventListViewModel> list = new List<EventListViewModel>();
        //    foreach (var item in datas)
        //    {
        //        //db = new 鮮蔬果季Context();                                  //使用注入,故不用在new db
        //        var 活動供應商與城市 = (from SI in db.Suppliers
        //                        join C in db.Cities on SI.CityId equals C.CityId
        //                        where SI.SupplierId == item.SupplierId
        //                        select new { SI, C }).FirstOrDefault();                            //.FirstOrDefault跟ToList相同有斷點效果
        //list.Add(new EventListViewModel()
        //{
        //    Event = item,
        //            Supplier = 活動供應商與城市.SI,
        //            City = 活動供應商與城市.C
        //        });
        //    }
        //    return View(list);



    public IActionResult EventSignUp_1(int id)
        {

            // 判斷會員是否登入
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
                ViewBag.姓名 = UserLogin.member.MemberName.ToString();   //前台報名表單,帶入會員資料
                ViewBag.電郵 = UserLogin.member.UserId.ToString();
                ViewBag.電話 = UserLogin.member.Mobile.ToString();

            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");   //返回登入頁面
            }

            var 活動及供應商明細 = (from E in db.Events
                      join supp in db.Suppliers on E.SupplierId equals supp.SupplierId
                      where id ==E.EventId           //回傳的id與活動id相等
                      select new { E,supp }).FirstOrDefault();

            //進到指定的活動頁(單筆活動),故不使用list,透過回傳的ID僅只一筆對應資料
            EventListViewModel 單筆活動 = new EventListViewModel();   
            List<EventPhotoBank> 相片list = new List<EventPhotoBank>();
             

            單筆活動.Event = 活動及供應商明細.E;
            單筆活動.Supplier = 活動及供應商明細.supp;
            var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == 單筆活動.Event.Supplier.CityId);
            //因為是多張照片,故使用Where(找全部),型態List
            var 照片資料 = db.EventPhotoBanks.Where(EP => EP.EventId == id).ToList();
            //把上面找到的照片加入到 單筆活動(物件),因EventPhoto ViewModel型態為list,故可以用以下list加法
            foreach (var 照片 in 照片資料)    單筆活動.EventPhoto.Add(照片);

            return View(單筆活動);
        }


        //public IActionResult EventRegistration()
        //{ 
        //    //var 活動報名資料 = (from ER in db.EventRegistrations
        //    //              join M in db.Members
        //    //              on ER.MemberId equals M.MemberId
        //    //              where ER.EventId == id
        //    //              select new { M, ER }).FirstOrDefault();
        //    return View();
        //}
        [HttpPost]  //同名方法
            public IActionResult EventSignUp_1(EventRegistration XXX)
        { 
            // 判斷會員是否登入
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                EventRegistration 送出報名資料 = new EventRegistration()
                {

                    MemberId = UserLogin.member.MemberId,
                    EventId = XXX.EventId,
                    ParticipantNumber = XXX.ParticipantNumber,
                    ContactName = XXX.ContactName,
                    ContactEmail = XXX.ContactEmail,
                    ContactMobile = XXX.ContactMobile,
                    FoodPreference =XXX.FoodPreference,
                    SubmitDate = DateTime.Now,

                    //EventId = XXX.EventId,
                    //ParticipantNumber = XXX.ParticipantNumber,
                    //ContactName = XXX.ContactName,
                    //ContactEmail = XXX.ContactEmail,
                    //ContactMobile = XXX.ContactMobile,
                    //FoodPreference = XXX.FoodPreference,

                };
                    db.Add(送出報名資料);
                   db.SaveChanges();
               return   RedirectToAction("EventSignUp_1"/* new { XXX.EventId }*/);
            }

            else  //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");   //返回登入頁面
            }
        }
    }
}

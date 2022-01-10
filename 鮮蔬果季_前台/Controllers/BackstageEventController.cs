﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.ViewModels;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageEventController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageEventController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }

        public IActionResult EventList()
        {
            List<EventListViewModel> 所有活動列表 = new List<EventListViewModel>();
            var 所有活動 = (from E in db.Events
                        join supp in db.Suppliers
                        on E.SupplierId equals supp.SupplierId
                        select new { E, supp }).ToList();

            foreach (var item in 所有活動)
            {
                List<EventPhotoBank> 相片list = new List<EventPhotoBank>();
                var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == item.supp.CityId);
                var 照片資料 = db.EventPhotoBanks.FirstOrDefault(P => P.EventId == item.E.EventId);
                相片list.Add(照片資料);

                所有活動列表.Add(new EventListViewModel()  //加入EventListViewModel (new新記憶體空間)
                {
                    Event = item.E,
                    City = 城市資料,
                    EventPhoto = 相片list
                });
            }
            return View(所有活動列表);
        }

        public IActionResult EventEdit()
        {
            return View();
        }
        


        
        public IActionResult EventCreate()
        {

            return View();
        }
        [HttpPost]
        public IActionResult EventCreate(Event FormData)   //回傳名稱要使用form的name同名
        {

            Event 活動新增資料 = new Event()
            {

                //EventId = FormData.EventId,
                EventName = FormData.EventName,
                SupplierId = FormData.SupplierId,
                Lable = FormData.Lable,
                EventParticipantCap = FormData.EventParticipantCap,
                EventPrice = FormData.EventPrice,
                EventLocation = FormData.EventLocation,
                EventStartDate = FormData.EventStartDate,
                EventEndDate = FormData.EventEndDate,
                EventDescription = FormData.EventDescription,
            };
            db.Add(活動新增資料);
            db.SaveChanges();

            return RedirectToAction("EventCreate");
        }

        public IActionResult EventCreateAPI(Event FormData)   //回傳名稱要使用form的name同名
        {

            Event 活動新增資料 = new Event()
            {

                //EventId = FormData.EventId,
                EventName = FormData.EventName,
                SupplierId = FormData.SupplierId,
                Lable = FormData.Lable,
                EventParticipantCap = FormData.EventParticipantCap,
                EventPrice = FormData.EventPrice,
                EventLocation = FormData.EventLocation,
                EventStartDate = FormData.EventStartDate,
                EventEndDate = FormData.EventEndDate,
                EventDescription = FormData.EventDescription,
            };
            db.Add(活動新增資料);
            db.SaveChanges();

            return RedirectToAction("EventCreate");
        }


        //後台修改的顯示頁面
        public IActionResult BackstageEvevtDetailPartial(int id)
        {
            var 活動及供應商明細 = (from E in db.Events
                            join supp in db.Suppliers on E.SupplierId equals supp.SupplierId
                            where id == E.EventId           //回傳的id與活動id相等
                            select new { E, supp }).FirstOrDefault();

            //進到指定的活動頁(單筆活動),故不使用list,透過回傳的ID僅只一筆對應資料
            EventListViewModel 單筆活動 = new EventListViewModel();


            //單筆資料的加入(屬性:物件)
            單筆活動.Event = 活動及供應商明細.E;
            單筆活動.Supplier = 活動及供應商明細.supp;

            //List<EventPhotoBank> 相片list = new List<EventPhotoBank>();
            //var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == 單筆活動.Event.Supplier.CityId);
            ////因為是多張照片,故使用Where(找全部),型態List
            //var 照片資料 = db.EventPhotoBanks.Where(EP => EP.EventId == id).ToList();
            ////把上面找到的照片加入到 單筆活動(物件),因EventPhoto ViewModel型態為list,故可以用以下list加法
            //foreach (var 照片 in 照片資料) 單筆活動.EventPhoto.Add(照片);

            return PartialView(單筆活動);
        }


    }
}

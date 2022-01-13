using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.ViewModels;
using 鮮蔬果季_前台.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageEventController : Controller
    {
        private readonly 鮮蔬果季Context db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BackstageEventController(IWebHostEnvironment webHost, 鮮蔬果季Context dbContext)
        {
            _hostingEnvironment = webHost; //取的wwwroot的路徑
            db = dbContext;
        }

        //顯示後台Event活動
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
                var 已報名人數 = from ER in db.EventRegistrations
                            where item.E.EventId == ER.EventId
                            select new { ER.ParticipantNumber, ER.Event.EventParticipantCap };
                int? count = 0;
                int? all = 0;
                foreach (var item2 in 已報名人數)
                {
                    count += item2.ParticipantNumber;
                    all = item2.EventParticipantCap;
                }
                if (all == 0) //如果沒有人報名過就要去活動找名額
                {
                    var 活動人數 = db.Events.FirstOrDefault(a => a.EventId == item.E.EventId);
                    all = 活動人數.EventParticipantCap;
                }
                var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == item.supp.CityId);
                var 照片資料 = db.EventPhotoBanks.FirstOrDefault(P => P.EventId == item.E.EventId);
                相片list.Add(照片資料);

                所有活動列表.Add(new EventListViewModel()  //加入EventListViewModel (new新記憶體空間)
                {
                    Event = item.E,
                    City = 城市資料,
                    EventPhoto = 相片list,
                    已報名人數=count
                });
            }
            return View(所有活動列表);
        }

   
        //後台 活動修改的顯示頁面
        public IActionResult bEvevtEditPartial(int id)
        {
            var 活動及供應商明細 = (from E in db.Events
                            join supp in db.Suppliers on E.SupplierId equals supp.SupplierId
                            where id == E.EventId           //回傳的id與活動id相等
                            select new { E, supp }).FirstOrDefault();
            var 活動照片 = db.EventPhotoBanks.Where(a => a.EventId == id).ToList();
            ViewBag.Photo = 活動照片;
            //進到指定的活動頁(單筆活動),故不使用list,透過回傳的ID僅只一筆對應資料
            EventListViewModel 單筆活動 = new EventListViewModel();

            //單筆資料的加入(屬性:物件)
            單筆活動.Event = 活動及供應商明細.E;
            單筆活動.Supplier = 活動及供應商明細.supp;

            return PartialView("EventEditPartial",單筆活動);
        }              //如果要導入與方法不同名稱的view, 要用分號指定帶入的顯示的頁面   "EventEditPartial"
      
        
        [HttpPost]        //修改寫入資料庫
        public IActionResult bEvevtEditPartial(Event EventEdit)
        {
            var 活動修改資料 = db.Events.FirstOrDefault(E => E.EventId == EventEdit.EventId);

            if (活動修改資料 != null)
            {
                //活動修改資料.EventParticipantCap = EventEdit.EventParticipantCap;
                活動修改資料.EventLocation = EventEdit.EventLocation;
                活動修改資料.EventStartDate = EventEdit.EventStartDate;
                活動修改資料.EventEndDate = EventEdit.EventEndDate;
                活動修改資料.EventPrice = EventEdit.EventPrice;
                活動修改資料.Subtitle = EventEdit.Subtitle;
                活動修改資料.EventDescription = EventEdit.EventDescription;
                db.SaveChanges();
            };

            return Content("1");
        }



        public IActionResult EventListPartial()
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
            return PartialView(所有活動列表);
        }




        // 新增活動
        public IActionResult EventCreate()
        {
            var 共應商 = db.Suppliers.ToList();
            ViewBag.AllSupp = 共應商;
            return View();
        }
        [HttpPost]
        public IActionResult EventCreate(EventListViewModel CreatEventForm)   //回傳名稱要使用form的name同名
        {
            var supid = db.Suppliers.FirstOrDefault(a => a.SupplierName == CreatEventForm.SupplierName);

            Event 活動新增資料 = new Event()
            {

                //EventId = FormData.EventId,
                EventName = CreatEventForm.EventName,
                SupplierId = supid.SupplierId,
                Lable = CreatEventForm.Lable,
                EventParticipantCap = CreatEventForm.Event.EventParticipantCap,
                EventPrice = CreatEventForm.EventPrice,
                EventLocation = CreatEventForm.EventLocation,
                EventStartDate = CreatEventForm.EventStartDate,
                EventEndDate = CreatEventForm.EventEndDate,
                EventDescription = CreatEventForm.EventDescription,
            };
            db.Add(活動新增資料);
            db.SaveChanges();
            var 新的活動 = db.Events.OrderByDescending(p => p.EventId).FirstOrDefault(a => a.EventName == CreatEventForm.EventName);
            if (CreatEventForm.photo != null)
            {
                int count = 0;
                foreach (var item in CreatEventForm.photo)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/活動照片/" + photoName); //取得路徑+要上傳的圖片檔案名稱
                    using (var fileStream = new FileStream(filePath, FileMode.Create)) //Create新增圖片，如果已存在就覆寫
                    {
                        CreatEventForm.photo[count].CopyTo(fileStream); //上傳指令
                    }
                    count++;
                    var q = new EventPhotoBank()
                    {
                        EventId = 新的活動.EventId,
                        PhotoPath = photoName
                    };
                    db.Add(q);
                }
                db.SaveChanges();
            }

            return RedirectToAction("EventList");                //待解決,如何回到活動後台首頁,同時回傳Context

        }






        //目前寫在eventController,以下找不到問題原因
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



        //[HttpPost]
        //public IActionResult EventSignUp_1(EventRegistration SignUpForm)  //回傳前台form的資料(name為FormData)
        //{
        //    // 判斷會員是否登入
        //    if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
        //    {
        //        ViewBag.USER = UserLogin.member.MemberName;
        //        ViewBag.userID = UserLogin.member.MemberId;

        //        EventRegistration 送出報名資料 = new EventRegistration()
        //        {
        //            MemberId = UserLogin.member.MemberId,
        //            EventId = SignUpForm.EventId,
        //            ParticipantNumber = SignUpForm.ParticipantNumber,
        //            ContactName = SignUpForm.ContactName,
        //            ContactEmail = SignUpForm.ContactEmail,
        //            ContactMobile = SignUpForm.ContactMobile,
        //            FoodPreference = SignUpForm.FoodPreference,
        //            SubmitDate = DateTime.Now,
        //        };
        //        db.Add(送出報名資料);
        //        db.SaveChanges();
        //        return Content("0");
        //    }

        //    else  //Seesion沒找到
        //    {
        //        ViewBag.USER = null;
        //        UserLogin.member = null;
        //        return RedirectToAction("Login", "Login");   //返回登入頁面
        //    }
        //}







        public IActionResult EventEdit()
        {
            return View();
        }
    }
}

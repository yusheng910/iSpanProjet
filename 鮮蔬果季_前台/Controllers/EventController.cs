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


        public IActionResult EventBlog()
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



            foreach (var item in 所有活動)
            {
                //計算該活動已報名人數
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
                string s= deleteHtml(item.E.EventDescription);
                所有活動列表.Add(new EventListViewModel()
                {
                    Event = item.E,
                    活動描述去除html = s,
                    City = 城市資料,
                    EventPhoto = 相片list,
                    活動滿額人數 = all,
                    已報名人數 = count,
                    剩餘名額 = all - count
                }) ;
            }
            return View(所有活動列表);
        }

        public string deleteHtml(string xxx) {
            string regex = @"(<.+?>|&nbsp;)";
            var result = Regex.Replace(xxx, regex, "").Trim();
            return result;
        }

        //活動首頁標籤的Partialview 所有活動
        public IActionResult AllBlog()
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
            var 所有活動 = (from E in db.Events
                        join supp in db.Suppliers
                       on E.SupplierId equals supp.SupplierId
                        select new { E, supp }).ToList();

          
            foreach (var item in 所有活動)
            {
                //計算該活動已報名人數
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
                List<EventPhotoBank> 相片list = new List<EventPhotoBank>();

                var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == item.supp.CityId);
                var 照片資料 = db.EventPhotoBanks.FirstOrDefault(P => P.EventId == item.E.EventId);
                相片list.Add(照片資料);
                string s = deleteHtml(item.E.EventDescription);
                所有活動列表.Add(new EventListViewModel()
                {
                    Event = item.E,
                    活動描述去除html = s,
                    City = 城市資料,
                    EventPhoto = 相片list,
                    活動滿額人數 = all,
                    已報名人數 = count,
                    剩餘名額 = all - count
                });
            }
            return PartialView("BlogPartial",所有活動列表);
        }





        //活動首頁標籤的Partialview DIY標籤
        public IActionResult BlogTag_DIY()
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
            var 所有活動 = (from E in db.Events
                        where E.Lable == "DIY體驗"
                        join supp in db.Suppliers
                       on E.SupplierId equals supp.SupplierId
                        select new { E, supp }).ToList();


            foreach (var item in 所有活動)
            {
                //計算該活動已報名人數
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
                List<EventPhotoBank> 相片list = new List<EventPhotoBank>();

                var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == item.supp.CityId);
                var 照片資料 = db.EventPhotoBanks.FirstOrDefault(P => P.EventId == item.E.EventId);
                相片list.Add(照片資料);
                string s = deleteHtml(item.E.EventDescription);
                所有活動列表.Add(new EventListViewModel()
                {
                    Event = item.E,
                    活動描述去除html = s,
                    City = 城市資料,
                    EventPhoto = 相片list,
                    活動滿額人數 = all,
                    已報名人數 = count,
                    剩餘名額 = all - count
                });
            }
            return PartialView("BlogPartial", 所有活動列表);
        }


        //活動首頁標籤的Partialview 可愛動物
        public IActionResult BlogTag_CuteAnimal()
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
            var 所有活動 = (from E in db.Events
                        where E.Lable == "可愛動物"
                        join supp in db.Suppliers
                       on E.SupplierId equals supp.SupplierId
                        select new { E, supp }).ToList();


            foreach (var item in 所有活動)
            {
                //計算該活動已報名人數
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
                List<EventPhotoBank> 相片list = new List<EventPhotoBank>();

                var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == item.supp.CityId);
                var 照片資料 = db.EventPhotoBanks.FirstOrDefault(P => P.EventId == item.E.EventId);
                相片list.Add(照片資料);
                string s = deleteHtml(item.E.EventDescription);
                所有活動列表.Add(new EventListViewModel()
                {
                    Event = item.E,
                    活動描述去除html = s,
                    City = 城市資料,
                    EventPhoto = 相片list,
                    活動滿額人數 = all,
                    已報名人數 = count,
                    剩餘名額 = all - count
                });
            }
            return PartialView("BlogPartial", 所有活動列表);
        }




        //活動首頁標籤的Partialview 絕美風景
        public IActionResult BlogTag_Landscape()
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
            var 所有活動 = (from E in db.Events
                        where E.Lable == "絕美風景"
                        join supp in db.Suppliers
                       on E.SupplierId equals supp.SupplierId
                        select new { E, supp }).ToList();


            foreach (var item in 所有活動)
            {
                //計算該活動已報名人數
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
                List<EventPhotoBank> 相片list = new List<EventPhotoBank>();

                var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == item.supp.CityId);
                var 照片資料 = db.EventPhotoBanks.FirstOrDefault(P => P.EventId == item.E.EventId);
                相片list.Add(照片資料);
                string s = deleteHtml(item.E.EventDescription);
                所有活動列表.Add(new EventListViewModel()
                {
                    Event = item.E,
                    活動描述去除html = s,
                    City = 城市資料,
                    EventPhoto = 相片list,
                    活動滿額人數 = all,
                    已報名人數 = count,
                    剩餘名額 = all - count
                });
            }
            return PartialView("BlogPartial", 所有活動列表);
        }

        //活動首頁標籤的Partialview 戶外露營
        public IActionResult BlogTag_Camping()
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
            var 所有活動 = (from E in db.Events
                        where E.Lable == "戶外露營"
                        join supp in db.Suppliers
                       on E.SupplierId equals supp.SupplierId
                        select new { E, supp }).ToList();

            foreach (var item in 所有活動)
            {
                //計算該活動已報名人數
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
                List<EventPhotoBank> 相片list = new List<EventPhotoBank>();

                var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == item.supp.CityId);
                var 照片資料 = db.EventPhotoBanks.FirstOrDefault(P => P.EventId == item.E.EventId);
                相片list.Add(照片資料);
                string s = deleteHtml(item.E.EventDescription);
                所有活動列表.Add(new EventListViewModel()
                {
                    Event = item.E,
                    活動描述去除html = s,
                    City = 城市資料,
                    EventPhoto = 相片list,
                    活動滿額人數 = all,
                    已報名人數 = count,
                    剩餘名額 = all - count
                });
            }
            return PartialView("BlogPartial", 所有活動列表);
        }

        //活動首頁標籤的Partialview 專業課程
        public IActionResult BlogTag_ProfessionalClass()
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
            var 所有活動 = (from E in db.Events
                        where E.Lable == "專業課程"
                        join supp in db.Suppliers
                       on E.SupplierId equals supp.SupplierId
                        select new { E, supp }).ToList();

            foreach (var item in 所有活動)
            {
                //計算該活動已報名人數
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
                List<EventPhotoBank> 相片list = new List<EventPhotoBank>();

                var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == item.supp.CityId);
                var 照片資料 = db.EventPhotoBanks.FirstOrDefault(P => P.EventId == item.E.EventId);
                相片list.Add(照片資料);
                string s = deleteHtml(item.E.EventDescription);
                所有活動列表.Add(new EventListViewModel()
                {
                    Event = item.E,
                    活動描述去除html = s,
                    City = 城市資料,
                    EventPhoto = 相片list,
                    活動滿額人數 = all,
                    已報名人數 = count,
                    剩餘名額 = all - count
                });
            }
            return PartialView("BlogPartial", 所有活動列表);
        }



        // 載入對應活動頁面
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
            //計算該活動已報名人數
            var 已報名人數 = from ER in db.EventRegistrations
                         where id == ER.EventId
                         select new { ER.ParticipantNumber,ER.Event.EventParticipantCap };
            int? count = 0;
            int? all = 0;
            foreach (var item in 已報名人數) { 
             count += item.ParticipantNumber;
            all =  item.EventParticipantCap;
            }
            if (all == 0) {
                var 活動人數 = db.Events.FirstOrDefault(a => a.EventId == id);
                all = 活動人數.EventParticipantCap;
            }
            ViewBag.已報名人數 = count;
            ViewBag.活動人數 = all;
            ViewBag.剩餘名額 = all - count;
            var 活動及供應商明細 = (from E in db.Events
                      join supp in db.Suppliers on E.SupplierId equals supp.SupplierId
                      where id ==E.EventId           //回傳的id與活動id相等
                      select new { E,supp }).FirstOrDefault();

            ViewBag.活動地址 = 活動及供應商明細.supp.SupplierAddress.ToString();

            //進到指定的活動頁(單筆活動),故不使用list,透過回傳的ID僅只一筆對應資料
            EventListViewModel 單筆活動 = new EventListViewModel();   
            List<EventPhotoBank> 相片list = new List<EventPhotoBank>();

            //單筆資料的加入(屬性:物件)
            單筆活動.Event = 活動及供應商明細.E;
            單筆活動.Supplier = 活動及供應商明細.supp;

            var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == 單筆活動.Event.Supplier.CityId);
            //因為是多張照片,故使用Where(找全部),型態List
            var 照片資料 = db.EventPhotoBanks.Where(EP => EP.EventId == id).ToList();
            //把上面找到的照片加入到 單筆活動(物件),因EventPhoto ViewModel型態為list,故可以用以下list加法
            foreach (var 照片 in 照片資料)    單筆活動.EventPhoto.Add(照片);

             return View(單筆活動);
        }

        //報名送出
        [HttpPost]  
        public IActionResult EventSignUp_1(EventRegistration SignUpForm)  //回傳前台form的資料(name為FormData)
        {
            // 判斷會員是否登入
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                EventRegistration 送出報名資料 = new EventRegistration()
                {
                    MemberId = UserLogin.member.MemberId,
                    EventId = SignUpForm.EventId,
                    ParticipantNumber = SignUpForm.ParticipantNumber,
                    ContactName = SignUpForm.ContactName,
                    ContactEmail = SignUpForm.ContactEmail,
                    ContactMobile = SignUpForm.ContactMobile,
                    FoodPreference = SignUpForm.FoodPreference,
                    SubmitDate = DateTime.Now,
                };
                db.Add(送出報名資料);
                db.SaveChanges();
                return Content("0");
            }

            else  //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");   //返回登入頁面
            }
        }


        //我的活動
        public IActionResult MyEventList()
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
                return RedirectToAction("Login", "Login");
            }

            List<EventListViewModel> 活動報名資料 = new List<EventListViewModel>();
            var 登入會員報名資料 = (from ER in db.EventRegistrations
                            join E in db.Events
                            on ER.EventId  equals E.EventId
                            where ER.MemberId == UserLogin.member.MemberId
                            select new { ER,E }).ToList();

            foreach (var item in 登入會員報名資料)
            { 
                活動報名資料.Add(new EventListViewModel()
                {
                    Event = item.E,
                    _EventRegistration = item.ER,
                });
            }
                return View(活動報名資料);
        }

        public IActionResult RemoveMyEvent(int id) 
        {
            var 取消的報名資料 = db.EventRegistrations.FirstOrDefault(ERI => ERI.EventRegistrationId == id);
            if (取消的報名資料 != null)
            {
                db.Remove(取消的報名資料);
                db.SaveChanges();
            }
            return RedirectToAction("MyEventList");   //目前怪怪的
        }



    }
}

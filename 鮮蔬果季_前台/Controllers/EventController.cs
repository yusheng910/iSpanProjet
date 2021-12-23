using Microsoft.AspNetCore.Mvc;
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


        public IActionResult EventBlog()
        {
            //鮮蔬果季Context db = new 鮮蔬果季Context();          //引用注入就不用new db Context
            var datas = (from E in db.Events
                       select E).ToList();

            List<EventListViewModel> list = new List<EventListViewModel>();
            foreach (var item in datas)
            {
                //db = new 鮮蔬果季Context();                                  //使用注入,故不用在new db
                var EventPhotos = (from EI in db.EventPhotoBanks
                         where EI.EventId == item.EventId
                         select EI).FirstOrDefault();                            //.FirstOrDefault跟ToList相同有斷點效果
                list.Add(new EventListViewModel()              
                {
                    Event = item,
                    EventPhotoBank = EventPhotos
                });
            }
            return View(list);
            //鮮蔬果季Context db = new 鮮蔬果季Context();
            //var datas = from E in db.Events
            //            select E;
            //return View(datas);
        }



        public IActionResult EventSignUp_1(int id)
        {
            //鮮蔬果季Context db = new 鮮蔬果季Context();
            var datas = (from E in db.Events
                        where id ==E.EventId           //回傳的id與活動id相等
                        select E).ToList();

            List<EventListViewModel> list = new List<EventListViewModel>();
            foreach (var item in datas)
            {
                //db = new 鮮蔬果季Context();
                var EventPhotos = (from EI in db.EventPhotoBanks
                                   where EI.EventId == item.EventId
                                   select EI).FirstOrDefault();
                list.Add(new EventListViewModel()
                {
                    Event = item,
                    EventPhotoBank = EventPhotos
                });
            }
            return View(list);
        }

    }
}

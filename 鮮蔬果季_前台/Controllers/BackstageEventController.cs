using Microsoft.AspNetCore.Mvc;
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
    }
}

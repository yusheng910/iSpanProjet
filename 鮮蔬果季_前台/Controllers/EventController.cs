using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class EventController : Controller
    {
        public IActionResult EventBlog()
        {
            鮮蔬果季Context db = new 鮮蔬果季Context();
            var datas = from E in db.Events
                       select E;

            List<EventListViewModel> list = new List<EventListViewModel>();
            foreach (var item in datas)
            {
                db = new 鮮蔬果季Context();
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



            //鮮蔬果季Context db = new 鮮蔬果季Context();
            //var datas = from E in db.Events
            //            select E;
            //return View(datas);
        }











        public IActionResult EventSignUp_1()
        {
            return View();
        }

        public IActionResult EventSignUp_2()
        {
            return View();
        }
        public IActionResult EventSignUp_3()
        {
            return View();
        }

        public IActionResult EventSignUp_4()
        {
            return View();
        }

    }
}

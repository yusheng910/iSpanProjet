using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Member()
        {
            var 會員資料 = (from m in db.Members
                        join c in db.Cities
                        on m.CityId equals c.CityId
                        select new { m, c }).ToList();
            List<MemberViewModel> member = new List<MemberViewModel>();

            foreach (var item in 會員資料) {
                member.Add(new MemberViewModel()
                {
                    member=item.m,
                    city=item.c.CityName
                });
            }
            return View(member);
        }
        public IActionResult Order()
        {
            var q = from p in (new 鮮蔬果季Context()).Orders
                    select p;
            return View(q);
        }
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Customer()
        {
            var q = from p in (new 鮮蔬果季Context()).Members
                    select p;
            return View(q);
        }

        public IActionResult Event()
        {
            return View();
        }

        public IActionResult EventEdit()
        {
            return View();
        }


        public IActionResult PowerBIReport_1()
        {
            return View();
        }

        public IActionResult PowerBIReport_2()
        {
            return View();
        }

        public IActionResult PowerBIReport_3()
        {
            return View();
        }



    }
}

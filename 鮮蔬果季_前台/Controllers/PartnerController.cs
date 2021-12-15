using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.Controllers
{
    public class PartnerController : Controller
    {
        public IActionResult PartnerBlog()
        {
            //鮮蔬果季Context db = new 鮮蔬果季Context();
            //var datas = from E in db.Events
            //            select E;
            //return View(datas);

            鮮蔬果季Context db = new 鮮蔬果季Context();
            var datas = from P in db.Suppliers
                        select P;
            return View(datas);
        }

        public IActionResult PartnerBlog_1()
        {
            return View();
        }

    }
}

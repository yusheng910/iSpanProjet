using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageCouponsController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageCouponsController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Coupons()
        {
            var 酷碰詳細 = from cp in db.Coupons
                        select cp;
            return View(酷碰詳細);
        }
    }
}

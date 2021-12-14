using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Orders()
        {
            IEnumerable<Order> orders = null;
            orders = from p in (new 鮮蔬果季Context()).Orders
                     where p.MemberId == 2
                     select p;
            List<OrderListViewModel> list = new List<OrderListViewModel>();

            foreach (Order o in orders)
                list.Add(new OrderListViewModel() { order = o });
            return View(list);
        }
        public IActionResult OrderDetail()
        {
            return View();
        }
        public IActionResult MyFavoriteList()
        {
            return View();
        }
        public IActionResult CouponsList()
        {
            鮮蔬果季Context db = new 鮮蔬果季Context();
            var qall = from p in db.Coupons
                    select p;
            //var q = from p in db.Coupons
            //        join x in db.CouponDetails
            //        on p.CouponId equals x.CouponId
            //        where x.MemberId == 2
            //        select new { x, p };
            List<CouponsListViewModel> list = new List<CouponsListViewModel>(); 
            foreach(var item in qall)
            {
                db = new 鮮蔬果季Context();
                var q = (from cd in db.CouponDetails
                        where cd.CouponId==item.CouponId
                        select cd).FirstOrDefault();
                list.Add(new CouponsListViewModel()
                {
                    coupon = item,
                    couponDetail = q
                });
            }
            return View(list);
            //var q = from p in new 鮮蔬果季Context().Coupons
            //        select p;
            //return View(q);

        }
        public IActionResult MemberCenter()
        {
            return View();
        }
    }
}

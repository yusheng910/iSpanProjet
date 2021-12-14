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
            //IEnumerable<Order> orders = null;
            鮮蔬果季Context db = new 鮮蔬果季Context();
            List<OrderListViewModel> list = new List<OrderListViewModel>();
            var orders = (from ord in db.Orders
                          join stat in db.Statuses
                          on ord.StatusId equals stat.StatusId
                          where ord.MemberId == 2
                          select new { ord, stat });

            db = new 鮮蔬果季Context();
            foreach (var o in orders)
            {
                var 訂單總價 = (from od in db.OrderDetails
                          join pro in db.Products
                          on od.ProductId equals pro.ProductId
                          where od.OrderId == o.ord.OrderId
                          group new { od, pro } by od.OrderId into g
                          select g.Sum(p => p.od.UnitsPurchased*p.pro.ProductUnitPrice)).FirstOrDefault();
                list.Add(new OrderListViewModel() { order = o.ord, status = o.stat, 總價 = 訂單總價});               
            }
            return View(list);
            //var 訂單編號 = db.Orders.FirstOrDefault(p => p.MemberId == 2);
            //var 訂單狀態 = 

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
            var q = from p in db.Coupons
                    join x in db.CouponDetails
                    on p.CouponId equals x.CouponId
                    where x.MemberId == 2
                    select new { x, p };
            List<CouponsListViewModel> list = new List<CouponsListViewModel>(); 
            foreach(var item in q)
            {
                list.Add(new CouponsListViewModel()
                {
                    coupon = item.p,
                    couponDetail = item.x
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

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageOrderController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageOrderController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult Order()
        {
            List<OrderListViewModel> orderList = new List<OrderListViewModel>();
            var orders = (from ord in db.Orders
                          join stat in db.Statuses
                          on ord.StatusId equals stat.StatusId
                          join mem in db.Members
                          on ord.MemberId equals mem.MemberId
                          join payby in db.PayMethods
                          on ord.PayMethodId equals payby.PayMethodId
                          orderby ord.OrderId descending
                          select new { ord, stat, mem, payby }).ToList();

            foreach (var o in orders)
            {
                var 訂單總價 = (from od in db.OrderDetails
                            join pro in db.Products
                            on od.ProductId equals pro.ProductId
                            where od.OrderId == o.ord.OrderId
                            group new { od, pro } by od.OrderId into g
                            select g.Sum(p => p.od.UnitsPurchased * p.pro.ProductUnitPrice)).FirstOrDefault();

                if (o.ord.CouponId == null)
                {
                    orderList.Add(new OrderListViewModel()
                    {
                        order = o.ord,
                        status = o.stat,
                        member = o.mem,
                        paymethod = o.payby,
                        總價 = 訂單總價,
                        coupon = null
                    });
                }
                else {
                    var q = (from c in db.Coupons
                             where c.CouponId == o.ord.CouponId
                            select c).FirstOrDefault();
                    orderList.Add(new OrderListViewModel()
                    {
                        order = o.ord,
                        status = o.stat,
                        member = o.mem,
                        paymethod = o.payby,
                        總價 = 訂單總價 - q.CouponDiscount,
                        coupon = q
                    });
                }
            }
            return View(orderList);
        }
    }
}

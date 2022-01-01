using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageOrderApiController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageOrderApiController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult ChangeToShipped(int id)
        {
            Order order = db.Orders.FirstOrDefault(i => i.OrderId == id);
            order.StatusId = 5;
            order.ShippedDate = DateTime.Now;
            db.SaveChanges();

            List<OrderListViewModel> orderList = new List<OrderListViewModel>();
            
            var 後台表列訂單 = (from ord in db.Orders
                          join stat in db.Statuses
                          on ord.StatusId equals stat.StatusId
                          join mem in db.Members
                          on ord.MemberId equals mem.MemberId
                          join payby in db.PayMethods
                          on ord.PayMethodId equals payby.PayMethodId
                          orderby ord.OrderId descending
                          select new { ord, stat, mem, payby }).ToList();

            foreach (var o in 後台表列訂單)
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
                else
                {
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
            return PartialView(orderList);
        }
        public IActionResult ChangeToDelivered(int id)
        {
            Order order = db.Orders.FirstOrDefault(i => i.OrderId == id);
            order.StatusId = 6;
            db.SaveChanges();

            List<OrderListViewModel> orderList = new List<OrderListViewModel>();
            var 後台表列訂單 = (from ord in db.Orders
                          join stat in db.Statuses
                          on ord.StatusId equals stat.StatusId
                          join mem in db.Members
                          on ord.MemberId equals mem.MemberId
                          join payby in db.PayMethods
                          on ord.PayMethodId equals payby.PayMethodId
                          orderby ord.OrderId descending
                          select new { ord, stat, mem, payby }).ToList();       
            
            foreach (var o in 後台表列訂單)
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
                else
                {
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
            return PartialView(orderList);
        }

        public IActionResult OrderDetailPartial(int id)
        {
            List<OrderListViewModel> 訂單細項列表 = new List<OrderListViewModel>();
            var 所有訂單細項 = (from od in db.OrderDetails
                          join p in db.Products
                          on od.ProductId equals p.ProductId
                          join sup in db.Suppliers
                          on p.SupplierId equals sup.SupplierId
                          where od.OrderId == id
                          orderby od.ProductId
                          select new { od, p, sup }).ToList();

            var 訂單 = db.Orders.FirstOrDefault(a => a.OrderId == id);
            ViewBag.cpd = (from cp in db.Coupons
                           where cp.CouponId == 訂單.CouponId
                           select cp.CouponDiscount).FirstOrDefault();

            foreach (var o in 所有訂單細項)
            {
                var 封面相片 = db.ProductPhotoBanks.Where(p => p.ProductId == o.p.ProductId).FirstOrDefault();
                訂單細項列表.Add(new OrderListViewModel()
                {
                    odetail = o.od,
                    product = o.p,
                    supplier = o.sup,
                    photoBank = 封面相片,
                    //單筆訂單細項總價 = 訂單細項總價
                });
            }
            return PartialView(訂單細項列表);
        }
    }
}

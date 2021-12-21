using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Orders()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                //=============================
                鮮蔬果季Context db = new 鮮蔬果季Context();
                List<OrderListViewModel> list = new List<OrderListViewModel>();
                var orders = (from ord in db.Orders
                              join stat in db.Statuses
                              on ord.StatusId equals stat.StatusId
                              where ord.MemberId == UserLogin.member.MemberId
                              select new { ord, stat });

                db = new 鮮蔬果季Context();
                foreach (var o in orders)
                {
                    var 訂單總價 = (from od in db.OrderDetails
                                join pro in db.Products
                                on od.ProductId equals pro.ProductId
                                where od.OrderId == o.ord.OrderId
                                group new { od, pro } by od.OrderId into g
                                select g.Sum(p => p.od.UnitsPurchased * p.pro.ProductUnitPrice)).FirstOrDefault();
                    list.Add(new OrderListViewModel() { order = o.ord, status = o.stat, 總價 = 訂單總價 });
                }
                return View(list);
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult OrderDetail(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion會員有登入
            {
                ViewBag.USER = UserLogin.member.MemberName;
                鮮蔬果季Context db = new 鮮蔬果季Context();
                List<OrderListViewModel> 訂單細項列表 = new List<OrderListViewModel>();
                var 所有訂單細項 = (from od in db.OrderDetails
                              join p in db.Products
                              on od.ProductId equals p.ProductId
                              where od.OrderId == id
                              select new { od, p });
                //todo
                db = new 鮮蔬果季Context();
                foreach (var o in 所有訂單細項)
                {
                    var 封面相片 = db.ProductPhotoBanks.Where(p => p.ProductId == o.p.ProductId).FirstOrDefault();
                    訂單細項列表.Add(new OrderListViewModel()
                    {
                        odetail = o.od,
                        product = o.p,
                        photoBank = 封面相片,
                        //單筆訂單細項總價 = 訂單細項總價
                    });
                }

                return View(訂單細項列表);
            }                         
            else //Seesion會員沒登入
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
        }
       
        public IActionResult AddReview(ReviewViewModel r)
        {
             

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion會員有登入
            {
                ViewBag.USER = UserLogin.member.MemberName;
                鮮蔬果季Context db = new 鮮蔬果季Context();
                Review review = new Review()
                {
                    OrderDetailId = r.AddId,
                    Comments = r.AddComments,
                    ReviewDate = DateTime.Now,
                    StarRanking = r.AddStarRanking,

                };
                db.Add(review);
                db.SaveChanges();

                var 供應商 = (from rr in db.Reviews
                           join od in db.OrderDetails
                           on rr.OrderDetailId equals od.OrderDetailId
                           join p in db.Products
                           on od.ProductId equals p.ProductId
                           join s in db.Suppliers
                           on p.SupplierId equals s.SupplierId
                           where rr.OrderDetailId == r.AddId
                           select s.SupplierName).FirstOrDefault();
            }
            else //Seesion會員沒登入
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }


            return RedirectToAction("OrderDetail", "Order");
        }
    }
}

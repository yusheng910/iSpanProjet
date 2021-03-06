using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class OrderController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public OrderController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult Orders()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                //=============================
                List<OrderListViewModel> list = new List<OrderListViewModel>();
                var orders = (from ord in db.Orders
                              join stat in db.Statuses
                              on ord.StatusId equals stat.StatusId
                              join pm in db.PayMethods
                              on ord.PayMethodId equals pm.PayMethodId
                              where ord.MemberId == UserLogin.member.MemberId                        
                              orderby ord.OrderId descending
                              select new { ord, stat, pm }).ToList();

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
                        list.Add(new OrderListViewModel()
                        {
                            order = o.ord,
                            status = o.stat,
                            paymethod = o.pm,
                            總價 = 訂單總價,
                            coupon = null
                        });
                    }
                    else
                    {
                        var q = (from c in db.Coupons
                                 where c.CouponId == o.ord.CouponId
                                 select c).FirstOrDefault();

                        list.Add(new OrderListViewModel()
                        {
                            order = o.ord,
                            status = o.stat,
                            paymethod = o.pm,
                            總價 = 訂單總價 - q.CouponDiscount,
                        });
                    }
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

        public IActionResult OrdersAll()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                //=============================
                List<OrderListViewModel> list = new List<OrderListViewModel>();
                var orders = (from ord in db.Orders
                              join stat in db.Statuses
                              on ord.StatusId equals stat.StatusId
                              join pm in db.PayMethods
                              on ord.PayMethodId equals pm.PayMethodId
                              where ord.MemberId == UserLogin.member.MemberId
                              orderby ord.OrderId descending
                              select new { ord, stat, pm }).ToList();

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
                        list.Add(new OrderListViewModel()
                        {
                            order = o.ord,
                            status = o.stat,
                            paymethod = o.pm,
                            總價 = 訂單總價,
                            coupon = null
                        });
                    }
                    else
                    {
                        var q = (from c in db.Coupons
                                 where c.CouponId == o.ord.CouponId
                                 select c).FirstOrDefault();

                        list.Add(new OrderListViewModel()
                        {
                            order = o.ord,
                            status = o.stat,
                            paymethod = o.pm,
                            總價 = 訂單總價 - q.CouponDiscount,
                        });
                    }
                }
                return PartialView(list);
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult OrdersDelivered()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                //=============================
                List<OrderListViewModel> list = new List<OrderListViewModel>();
                var orders = (from ord in db.Orders
                          join stat in db.Statuses
                          on ord.StatusId equals stat.StatusId
                          join pm in db.PayMethods
                          on ord.PayMethodId equals pm.PayMethodId
                          where ord.MemberId == UserLogin.member.MemberId && ord.StatusId == 6
                          orderby ord.OrderId descending
                          select new { ord, stat, pm }).ToList();

                //db = new 鮮蔬果季Context();
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
                        list.Add(new OrderListViewModel()
                        {
                            order = o.ord,
                            status = o.stat,
                            paymethod = o.pm,
                            總價 = 訂單總價,
                            coupon = null
                        });
                    }
                    else
                    {
                        var q = (from c in db.Coupons
                                 where c.CouponId == o.ord.CouponId
                                 select c).FirstOrDefault();

                        list.Add(new OrderListViewModel()
                        {
                            order = o.ord,
                            status = o.stat,
                            paymethod = o.pm,
                            總價 = 訂單總價 - q.CouponDiscount,
                        });
                    }
                }
                return PartialView(list);
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult OrdersShipped()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                //=============================
                List<OrderListViewModel> list = new List<OrderListViewModel>();
                var orders = (from ord in db.Orders
                          join stat in db.Statuses
                          on ord.StatusId equals stat.StatusId
                          join pm in db.PayMethods                              
                          on ord.PayMethodId equals pm.PayMethodId
                          where ord.MemberId == UserLogin.member.MemberId && ord.StatusId == 5
                          orderby ord.OrderId descending
                          select new { ord, stat, pm }).ToList();

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
                        list.Add(new OrderListViewModel()
                        {
                            order = o.ord,
                            status = o.stat,
                            paymethod = o.pm,
                            總價 = 訂單總價,
                            coupon = null
                        });
                    }
                    else
                    {
                        var q = (from c in db.Coupons
                                 where c.CouponId == o.ord.CouponId
                                 select c).FirstOrDefault();

                        list.Add(new OrderListViewModel()
                        {
                            order = o.ord,
                            status = o.stat,
                            paymethod = o.pm,
                            總價 = 訂單總價 - q.CouponDiscount,
                        });
                    }
                }
                return PartialView(list);
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult OrdersNotShipped()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                //=============================
                List<OrderListViewModel> list = new List<OrderListViewModel>();
                var orders = (from ord in db.Orders
                          join stat in db.Statuses
                          on ord.StatusId equals stat.StatusId
                          join pm in db.PayMethods
                          on ord.PayMethodId equals pm.PayMethodId
                          where ord.MemberId == UserLogin.member.MemberId && ord.StatusId == 4
                          orderby ord.OrderId descending
                          select new { ord, stat, pm }).ToList();

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
                        list.Add(new OrderListViewModel()
                        {
                            order = o.ord,
                            status = o.stat,
                            paymethod = o.pm,
                            總價 = 訂單總價,
                            coupon = null
                        });
                    }
                    else
                    {
                        var q = (from c in db.Coupons
                                 where c.CouponId == o.ord.CouponId
                                 select c).FirstOrDefault();

                        list.Add(new OrderListViewModel()
                        {
                            order = o.ord,
                            status = o.stat,
                            paymethod = o.pm,
                            總價 = 訂單總價 - q.CouponDiscount,
                        });
                    }
                }
                return PartialView(list);
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
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

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
                    var 是否回饋 = db.FeedbackResponses.Where(p => p.OrderDetailId == o.od.OrderDetailId).FirstOrDefault();
                    var 封面相片 = db.ProductPhotoBanks.Where(p => p.ProductId == o.p.ProductId).FirstOrDefault();
                    if (是否回饋 != null)
                    {
                        訂單細項列表.Add(new OrderListViewModel()
                        {
                            odetail = o.od,
                            product = o.p,
                            supplier = o.sup,
                            photoBank = 封面相片,
                            回饋 = true
                            //單筆訂單細項總價 = 訂單細項總價
                        });
                    }
                    else {
                        訂單細項列表.Add(new OrderListViewModel()
                        {
                            odetail = o.od,
                            product = o.p,
                            supplier = o.sup,
                            photoBank = 封面相片,
                            回饋 = false
                            //單筆訂單細項總價 = 訂單細項總價
                        });
                    }

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
       
        //public IActionResult AddReview(ReviewViewModel r)
        //{
        //    if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion會員有登入
        //    {
        //        ViewBag.USER = UserLogin.member.MemberName;
        //        ViewBag.userID = UserLogin.member.MemberId;

        //        Review review = new Review()
        //        {
        //            OrderDetailId = r.AddId,
        //            Comments = r.AddComments,
        //            ReviewDate = DateTime.Now,
        //            StarRanking = r.AddStarRanking

        //        };
        //        db.Add(review);
        //        db.SaveChanges();

        //        var 供應商 = (from rr in db.Reviews
        //                   join od in db.OrderDetails
        //                   on rr.OrderDetailId equals od.OrderDetailId
        //                   join p in db.Products
        //                   on od.ProductId equals p.ProductId
        //                   join s in db.Suppliers
        //                   on p.SupplierId equals s.SupplierId
        //                   where rr.OrderDetailId == r.AddId
        //                   select s.SupplierName).FirstOrDefault();
        //    }
        //    else //Seesion會員沒登入
        //    {
        //        ViewBag.USER = null;
        //        UserLogin.member = null;
        //        return RedirectToAction("Login", "Login");
        //    }
        //    return RedirectToAction("OrderDetail", "Order");
        //}

        public IActionResult AddReviewPartial(int id,int starrank,string AddComments)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion會員有登入
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                List<OrderListViewModel> 訂單細項列表 = new List<OrderListViewModel>();
                var q = (from od in db.OrderDetails
                         where od.OrderDetailId == id
                         select od.OrderId).FirstOrDefault();

                var 所有訂單細項 = (from od in db.OrderDetails
                              join p in db.Products
                              on od.ProductId equals p.ProductId
                              join sup in db.Suppliers
                              on p.SupplierId equals sup.SupplierId
                              where od.OrderId == q
                              orderby od.ProductId
                              select new { od, p, sup }).ToList();
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
                Review review = new Review()
                {
                    OrderDetailId = id,
                    Comments = AddComments,
                    ReviewDate = DateTime.Now,
                    StarRanking = starrank
                };
                var 是否評論 = (from x in db.OrderDetails
                            where x.OrderDetailId == id
                            select x).FirstOrDefault();
                是否評論.HaveReviews = true;
                db.Add(review);
                db.SaveChanges();
                return PartialView(訂單細項列表);
            }
            else //Seesion會員沒登入
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult ShowSuplierName(int id)
        {
            var 供應商 = (from od in db.OrderDetails
                       join p in db.Products
                       on od.ProductId equals p.ProductId
                       join s in db.Suppliers
                       on p.SupplierId equals s.SupplierId
                       where od.OrderDetailId == id
                       select s.SupplierName).FirstOrDefault();

            return Content($"【{供應商}】");
        }

        public IActionResult ShowProdName(int id)
        {
            var 商品名 = (from od in db.OrderDetails
                       join p in db.Products
                       on od.ProductId equals p.ProductId
                       where od.OrderDetailId == id
                       select p.ProductName).FirstOrDefault();

            return Content($"{商品名}");
        }
    }
}

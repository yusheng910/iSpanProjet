using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{

    public class CouponsController : Controller
    {
        private readonly 鮮蔬果季Context db;

        public CouponsController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult CouponsList()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }

            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }
            var qall = (from p in db.Coupons
                        select p).ToList();

            List<CouponsListViewModel> list = new List<CouponsListViewModel>();
            foreach (var item in qall)
            {
                var q = (from cd in db.CouponDetails
                         where cd.CouponId == item.CouponId &&
                         cd.MemberId == UserLogin.member.MemberId
                         select cd).FirstOrDefault();
                list.Add(new CouponsListViewModel()
                {
                    coupon = item,
                    couponDetail = q
                });
            }
            return View(list);
        }
        public IActionResult AddCoupons(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
                
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
            var q = (from p in db.Coupons
                     where p.CouponId == id
                     select p.CouponQuantityIssued).FirstOrDefault();

            CouponDetail couponDetail = (new CouponDetail() { CouponId = id, MemberId = UserLogin.member.MemberId, CouponQuantity = q });
            db.Add(couponDetail);
            db.SaveChanges();
            return RedirectToAction("CouponsList");
        }

        public IActionResult PartialHavent()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }
            var qall = (from p in db.Coupons
                        select p).ToList();

            List<CouponsListViewModel> list = new List<CouponsListViewModel>();
            foreach (var item in qall)
            {
                var q = (from cd in db.CouponDetails
                         where cd.CouponId == item.CouponId &&
                         cd.MemberId == UserLogin.member.MemberId
                         select cd).FirstOrDefault();
                list.Add(new CouponsListViewModel()
                {
                    coupon = item,
                    couponDetail = q
                });
            }

            return PartialView(list);
        }

        public IActionResult PartialAll()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }
            var qall = (from p in db.Coupons
                        select p).ToList();

            List<CouponsListViewModel> list = new List<CouponsListViewModel>();
            foreach (var item in qall)
            {
                var q = (from cd in db.CouponDetails
                         where cd.CouponId == item.CouponId &&
                         cd.MemberId == UserLogin.member.MemberId
                         select cd).FirstOrDefault();
                list.Add(new CouponsListViewModel()
                {
                    coupon = item,
                    couponDetail = q
                });
            }
            return PartialView(list);
        }
        public IActionResult partialexpired()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }
            var qall = (from p in db.Coupons
                        where p .CouponEndDate < DateTime.Now
                        select p).ToList();

            List<CouponsListViewModel> list = new List<CouponsListViewModel>();
            foreach (var item in qall)
            {
                var q = (from cd in db.CouponDetails
                         where cd.CouponId == item.CouponId &&
                         cd.MemberId == UserLogin.member.MemberId
                         select cd).FirstOrDefault();
                list.Add(new CouponsListViewModel()
                {
                    coupon = item,
                    couponDetail = q
                });
            }
            return PartialView(list);
        }

        public IActionResult partialHad()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }
            var couponsHad = (from p in db.Coupons
                              join cd in db.CouponDetails
                              on p.CouponId equals cd.CouponId
                              where cd.CouponQuantity >= 0 &&
                              cd.CouponId != 0 &&
                              cd.MemberId == UserLogin.member.MemberId
                              select new {p,cd }).ToList();
            List<CouponsListViewModel> list = new List<CouponsListViewModel>();
            foreach (var item in couponsHad)
            {
                list.Add(new CouponsListViewModel()
                {
                    coupon = item.p,
                    couponDetail=item.cd
                });
            }

            return PartialView(list);
        }

        public IActionResult partialAdd(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return NoContent();
            }
            var q = (from p in db.Coupons
                     where p.CouponId == id
                     select p.CouponQuantityIssued).FirstOrDefault();
            var qall = (from p in db.Coupons
                        select p).ToList();
            CouponDetail couponDetail = (new CouponDetail() { CouponId = id, MemberId = UserLogin.member.MemberId, CouponQuantity = q });

            db.Add(couponDetail);
            db.SaveChanges();
            List<CouponsListViewModel> list = new List<CouponsListViewModel>();
            foreach (var item in qall)
            {
                var q2 = (from cd in db.CouponDetails
                          where cd.CouponId == item.CouponId &&
                          cd.MemberId == UserLogin.member.MemberId
                          select cd).FirstOrDefault();
                list.Add(new CouponsListViewModel()
                {
                    coupon = item,
                    couponDetail = q2
                });
            }
            return PartialView(list);
        }
    }
}

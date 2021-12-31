using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageCouponsApiController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageCouponsApiController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult CouponsPartial(int id)
        {
            var 酷碰詳細 = (from cp in db.Coupons
                       where cp.CouponId == id
                       select cp).FirstOrDefault();
            CouponsListViewModel list = new CouponsListViewModel
            {
                coupon = 酷碰詳細
            };



            string year = (DateTime.Now.Year).ToString();
            string month = (DateTime.Now.Month).ToString();
            string date = (DateTime.Now.Date).ToString();

            string fullymd = System.DateTime.Now.ToString("yyyy-MM-dd");

            CalDate.strdate = fullymd+"T00:00";

            return PartialView(list);
        }

        public IActionResult CouponsEditPartial(CouponsListViewModel c)
        {
            var 酷碰詳細 = (from cp in db.Coupons
                        where cp.CouponId == c.CouponId
                        select cp).FirstOrDefault();
            酷碰詳細.CouponName = c.CouponName;
            酷碰詳細.CouponDiscount = c.CouponDiscount;
            酷碰詳細.DiscountCondition = c.DiscountCondition;
            酷碰詳細.CouponDescription = c.CouponDescription;
            酷碰詳細.CouponStartDate = c.CouponStartDate;
            酷碰詳細.CouponEndDate = c.CouponEndDate;
            酷碰詳細.CouponQuantityIssued = c.CouponQuantityIssued;
            db.SaveChanges();

            return Content("1");
        }
        public IActionResult CouponsListPartial()
        {
            List<CouponsListViewModel> list = new List<CouponsListViewModel>();
            var 酷碰詳細 = (from cp in db.Coupons
                        select cp).ToList();
            foreach (var item in 酷碰詳細)
            {
                list.Add(new CouponsListViewModel()
                {
                    coupon = item,
                });
            }
            return PartialView(list);
        }
    }
}

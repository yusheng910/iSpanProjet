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
        public IActionResult CouponsEditPartial(int id)
        {
            //限制日期
            string year = (DateTime.Now.Year).ToString();
            string month = (DateTime.Now.Month).ToString();
            string date = (DateTime.Now.Date).ToString();
            string fullymd = System.DateTime.Now.ToString("yyyy-MM-dd");
            CalDate.strdate = fullymd + "T00:00";
            //SHOW編輯畫面
            var 酷碰詳細 = (from cp in db.Coupons
                       where cp.CouponId == id
                       select cp).FirstOrDefault();
            CouponsListViewModel list = new CouponsListViewModel
            {
                coupon = 酷碰詳細
            };
            return PartialView(list);
        }
        [HttpPost]
        public IActionResult CouponsEditPartial(CouponsListViewModel c)
        {
            //編輯
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
            //編輯後載入的partial表
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
        public IActionResult CouponsCreatePartial()
        {
            //限制日期
            string year = (DateTime.Now.Year).ToString();
            string month = (DateTime.Now.Month).ToString();
            string date = (DateTime.Now.Date).ToString();
            string fullymd = System.DateTime.Now.ToString("yyyy-MM-dd");
            CalDate.strdate = fullymd + "T00:00";
            //SHOW新增畫面
            var 酷碰詳細 = (from cp in db.Coupons
                        select cp).FirstOrDefault();
            CouponsListViewModel list = new CouponsListViewModel
            {
                coupon = 酷碰詳細
            };
            return PartialView();
        }
        [HttpPost]
        public IActionResult CouponsCreatePartial(CouponsListViewModel c)
        {
            //新增
            var 酷碰詳細 = (from cp in db.Coupons
                        select cp).FirstOrDefault();

            Coupon coupon = new Coupon()
            {
                CouponName = c.CouponName,
                CouponDiscount = c.CouponDiscount,
                DiscountCondition = c.DiscountCondition,
                CouponDescription =c.CouponDescription,
                CouponStartDate = c.CouponStartDate,
                CouponEndDate = c.CouponEndDate,
                CouponQuantityIssued = c.CouponQuantityIssued
            };
            db.Add(coupon);
            db.SaveChanges();

            return Content("1");
        }



        //public IActionResult CouponsDeletePartial(int id)
        //{
        //    //刪除
        //    var 先刪除酷碰明細 = (from cpd in db.CouponDetails
        //                   where cpd.CouponId == id
        //                   select cpd).FirstOrDefault();

        //    var 再刪除單筆酷碰 = (from cp in db.Coupons
        //                join cpd in db.CouponDetails
        //                on cp.CouponId equals cpd.CouponId
        //                where cp.CouponId == id
        //                select cp).FirstOrDefault();
        //    if(先刪除酷碰明細!=null)
        //    {
        //        db.CouponDetails.Remove(先刪除酷碰明細);
        //    }
        //    db.Coupons.Remove(再刪除單筆酷碰);

        //    List<CouponsListViewModel> list = new List<CouponsListViewModel>();
        //    var 酷碰詳細 = (from cp in db.Coupons
        //                select cp).ToList();
        //    foreach (var item in 酷碰詳細)
        //    {
        //        list.Add(new CouponsListViewModel()
        //        {
        //            coupon = item,
        //        });
        //    }
        //    return PartialView("CouponsListPartial",list);
        //}
    }
}

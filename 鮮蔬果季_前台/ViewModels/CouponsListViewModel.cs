using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class CouponsListViewModel
    {
        public Coupon _coupon = null;
        public Coupon coupon
        {
            get
            {
                if (_coupon == null)
                    _coupon = new Coupon();
                return _coupon;
            }
            set
            {
                _coupon = value;
            }
        }
        public CouponDetail _couponDetail  = null;
        public CouponDetail couponDetail
        {
            get
            {
                if (_couponDetail == null)
                    _couponDetail = new CouponDetail();
                return _couponDetail;
            }
            set
            {
                _couponDetail = value;
            }
        }

        public int CouponId { get { return this.coupon.CouponId; } set { this.coupon.CouponId = value; } }
        [DisplayName("酷碰券名稱")]
        public string CouponName { get { return this.coupon.CouponName; } set { this.coupon.CouponName = value; } }
        [DisplayName("折扣金額")]
        public int CouponDiscount { get { return this.coupon.CouponDiscount; } set { this.coupon.CouponDiscount = value; } }
        [DisplayName("折扣條件")]
        public int? DiscountCondition { get { return this.coupon.DiscountCondition; } set { this.coupon.DiscountCondition = value; } }
        [DisplayName("酷碰券描述")]
        public string CouponDescription { get { return this.coupon.CouponDescription; } set { this.coupon.CouponDescription = value; } }
        [DisplayName("開始日")]
        public DateTime CouponStartDate { get { return this.coupon.CouponStartDate; } set { this.coupon.CouponStartDate = value; } }
        [DisplayName("結束日")]
        public DateTime CouponEndDate { get { return this.coupon.CouponEndDate; } set { this.coupon.CouponEndDate = value; } }
        [DisplayName("每人發放數量")]
        public int CouponQuantityIssued { get { return this.coupon.CouponQuantityIssued; } set { this.coupon.CouponQuantityIssued = value; } }

        public int CouponDatilId { get { return this.couponDetail.CouponDatilId; } set { this.couponDetail.CouponDatilId = value; } }
        public int MemberId { get { return this.couponDetail.MemberId; } set { this.couponDetail.MemberId = value; } }
        public int CouponQuantity { get { return this.couponDetail.CouponQuantity; } set { this.couponDetail.CouponQuantity = value; } }

    }
}

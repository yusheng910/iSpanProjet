using System;
using System.Collections.Generic;
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
        public string CouponName { get { return this.coupon.CouponName; } set { this.coupon.CouponName = value; } }
        public int CouponDiscount { get { return this.coupon.CouponDiscount; } set { this.coupon.CouponDiscount = value; } }
        public int? DiscountCondition { get { return this.coupon.DiscountCondition; } set { this.coupon.DiscountCondition = value; } }
        public string CouponDescription { get { return this.coupon.CouponDescription; } set { this.coupon.CouponDescription = value; } }
        public DateTime CouponStartDate { get { return this.coupon.CouponStartDate; } set { this.coupon.CouponStartDate = value; } }
        public DateTime CouponEndDate { get { return this.coupon.CouponEndDate; } set { this.coupon.CouponEndDate = value; } }
        public int CouponQuantityIssued { get { return this.coupon.CouponQuantityIssued; } set { this.coupon.CouponQuantityIssued = value; } }

        public int CouponDatilId { get { return this.couponDetail.CouponDatilId; } set { this.couponDetail.CouponDatilId = value; } }
        public int MemberId { get { return this.couponDetail.MemberId; } set { this.couponDetail.MemberId = value; } }
        public int CouponQuantity { get { return this.couponDetail.CouponQuantity; } set { this.couponDetail.CouponQuantity = value; } }

    }
}

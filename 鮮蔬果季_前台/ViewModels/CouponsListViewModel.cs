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
        public int cid { get; set; }
    }
}

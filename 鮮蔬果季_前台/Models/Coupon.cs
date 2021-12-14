using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class Coupon
    {
        public Coupon()
        {
            CouponDetails = new HashSet<CouponDetail>();
            Orders = new HashSet<Order>();
        }

        public int CouponId { get; set; }
        public string CouponName { get; set; }
        public int CouponDiscount { get; set; }
        public int? DiscountCondition { get; set; }
        public string CouponDescription { get; set; }
        public DateTime CouponStartDate { get; set; }
        public DateTime CouponEndDate { get; set; }

        public virtual ICollection<CouponDetail> CouponDetails { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}

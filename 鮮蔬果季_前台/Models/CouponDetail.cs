using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class CouponDetail
    {
        public int CouponDatilId { get; set; }
        public int CouponId { get; set; }
        public int MemberId { get; set; }
        public DateTime? CouponStartDate { get; set; }
        public DateTime CouponEndDate { get; set; }
        public int CouponQuantity { get; set; }

        public virtual Coupon Coupon { get; set; }
        public virtual Member Member { get; set; }
    }
}

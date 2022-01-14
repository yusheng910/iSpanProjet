using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class NewsletterViewModel
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
        public CouponDetail _couponDetail = null;
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
        public Order _order = null;
        public Order order
        {
            get
            {
                if (_order == null)
                    _order = new Order();
                return _order;
            }
            set
            {
                _order = value;
            }
        }

        public Member _member = null;
        public Member member
        {
            get
            {
                if (_member == null)
                    _member = new Member();
                return _member;
            }
            set
            {
                _member = value;
            }
        }
        [DisplayName("近一年是否有消費")]
        public int 是否有消費 { get; set; }
        public int MemberId { get { return this.member.MemberId; } set { this.member.MemberId = value; } }
        [DisplayName("Email")]
        public string UserId { get { return this.member.UserId; } set { this.member.UserId = value; } }
        [DisplayName("性別")]
        public string Gender { get { return this.member.Gender; } set { this.member.Gender = value; } }
        [DisplayName("會員名稱")]
        public string MemberName { get { return this.member.MemberName; } set { this.member.MemberName = value; } }
    }
}

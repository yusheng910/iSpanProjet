using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class Member
    {
        public Member()
        {
            CouponDetails = new HashSet<CouponDetail>();
            EventRegistrations = new HashSet<EventRegistration>();
            MyFavorites = new HashSet<MyFavorite>();
            Orders = new HashSet<Order>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int MemberId { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string MemberName { get; set; }
        public string Gender { get; set; }
        public string MemberAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public string Mobile { get; set; }
        public int CityId { get; set; }
        public DateTime RegisteredDate { get; set; }
        public byte[] MemberPhoto { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<CouponDetail> CouponDetails { get; set; }
        public virtual ICollection<EventRegistration> EventRegistrations { get; set; }
        public virtual ICollection<MyFavorite> MyFavorites { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}

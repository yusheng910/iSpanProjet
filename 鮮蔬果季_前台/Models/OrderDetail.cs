using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            Reviews = new HashSet<Review>();
        }

        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int UnitsPurchased { get; set; }
        public bool HaveReviews { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}

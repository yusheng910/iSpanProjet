using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class MyFavorite
    {
        public int MyFavoriteId { get; set; }
        public int? MemberId { get; set; }
        public int? SupplierId { get; set; }
        public int? ProductId { get; set; }

        public virtual Member Member { get; set; }
        public virtual Product Product { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}

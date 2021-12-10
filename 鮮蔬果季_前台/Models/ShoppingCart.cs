using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public int MemberId { get; set; }
        public int ProductId { get; set; }
        public int UnitsInCart { get; set; }
        public int StatusId { get; set; }

        public virtual Member Member { get; set; }
        public virtual Product Product { get; set; }
        public virtual Status Status { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class ProductAdvertise
    {
        public int ProductAdvertiseId { get; set; }
        public int ProductId { get; set; }
        public string Tag { get; set; }

        public virtual Product Product { get; set; }
    }
}

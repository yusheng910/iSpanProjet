using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class ProductPriceChange
    {
        public int ProductPriceChangeId { get; set; }
        public int? ProductId { get; set; }
        public int? CostPriceAtTheTime { get; set; }
        public int? UnitPriceAtTheTime { get; set; }
        public DateTime? DateChanged { get; set; }

        public virtual Product Product { get; set; }
    }
}

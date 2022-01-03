using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class CategoryPowerBi
    {
        public int CategoryPowerBiId { get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual Product Product { get; set; }
    }
}

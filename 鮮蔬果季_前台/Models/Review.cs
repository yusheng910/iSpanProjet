using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int? OrderDetailId { get; set; }
        public string Comments { get; set; }
        public DateTime ReviewDate { get; set; }
        public int StarRanking { get; set; }
        public bool Shield { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
    }
}

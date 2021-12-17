using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class BlogDetail
    {
        public int BlogDetailId { get; set; }
        public int SupplierId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Maintext { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string Label { get; set; }
        public string PhotoPath { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class Category
    {
        public Category()
        {
            CategoryDetails = new HashSet<CategoryDetail>();
            Events = new HashSet<Event>();
            InverseFatherCategory = new HashSet<Category>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? FatherCategoryId { get; set; }

        public virtual Category FatherCategory { get; set; }
        public virtual ICollection<CategoryDetail> CategoryDetails { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Category> InverseFatherCategory { get; set; }
    }
}

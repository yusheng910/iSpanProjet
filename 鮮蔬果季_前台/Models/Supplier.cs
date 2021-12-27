using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Events = new HashSet<Event>();
            MyFavorites = new HashSet<MyFavorite>();
            Products = new HashSet<Product>();
        }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string BusinessOwner { get; set; }
        public string SupplierAddress { get; set; }
        public string Mobile { get; set; }
        public int CityId { get; set; }
        public string SupplierProfile { get; set; }
        public string SupplierAccount { get; set; }
        public string SupplierPassword { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<MyFavorite> MyFavorites { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}

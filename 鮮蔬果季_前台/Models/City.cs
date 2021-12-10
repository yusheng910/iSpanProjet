using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class City
    {
        public City()
        {
            Members = new HashSet<Member>();
            Suppliers = new HashSet<Supplier>();
        }

        public int CityId { get; set; }
        public string CityName { get; set; }

        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}

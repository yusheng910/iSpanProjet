using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class ProductOnSale
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public string SaleTitle { get; set; }
        public string SaleDescription { get; set; }
        public int PriceOff { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual Product Product { get; set; }
    }
}

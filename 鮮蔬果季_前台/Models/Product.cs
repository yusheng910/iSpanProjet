using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class Product
    {
        public Product()
        {
            CategoryDetails = new HashSet<CategoryDetail>();
            MyFavorites = new HashSet<MyFavorite>();
            OrderDetails = new HashSet<OrderDetail>();
            PhotoBanks = new HashSet<PhotoBank>();
            ProductAdvertises = new HashSet<ProductAdvertise>();
            ProductOnSales = new HashSet<ProductOnSale>();
            ProductPhotoBanks = new HashSet<ProductPhotoBank>();
            ProductPriceChanges = new HashSet<ProductPriceChange>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public int ProductUnitPrice { get; set; }
        public int ProductCostPrice { get; set; }
        public int ProductUnitsInStock { get; set; }
        public DateTime? ProduceDate { get; set; }
        public string ProductDescription { get; set; }
        public string ProductSize { get; set; }
        public bool DefectiveGood { get; set; }
        public bool InShop { get; set; }
        public byte[] ProductQrcode { get; set; }
        public bool HotProduct { get; set; }
        public double? ProductDisCount { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<CategoryDetail> CategoryDetails { get; set; }
        public virtual ICollection<MyFavorite> MyFavorites { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<PhotoBank> PhotoBanks { get; set; }
        public virtual ICollection<ProductAdvertise> ProductAdvertises { get; set; }
        public virtual ICollection<ProductOnSale> ProductOnSales { get; set; }
        public virtual ICollection<ProductPhotoBank> ProductPhotoBanks { get; set; }
        public virtual ICollection<ProductPriceChange> ProductPriceChanges { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}

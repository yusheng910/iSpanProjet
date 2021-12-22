using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class ShoppingListViewModel
    {
        public Product _prod = null;
        public Product product {
            get
            {
                if (_prod == null)
                    _prod = new Product();
                return _prod;
            }
            set
            {
                _prod = value;
            }
        }
        public Supplier _supp = null;
        public Supplier supplier
        {
            get
            {
                if (_supp == null)
                    _supp = new Supplier();
                return _supp;
            }
            set
            {
                _supp = value;
            }
        }

        public ProductPhotoBank _prodphotoforCart = null;
        public ProductPhotoBank photoforCart
        {
            get
            {
                if (_prodphotoforCart == null)
                    _prodphotoforCart = new ProductPhotoBank();
                return _prodphotoforCart;
            }
            set
            {
                _prodphotoforCart = value;
            }
        }

        public List<ProductPhotoBank> _prodphoto = null;
        public List<ProductPhotoBank> photoBank
        {
            get
            {
                if (_prodphoto == null)
                    _prodphoto = new List<ProductPhotoBank>();
                return _prodphoto;
            }
            set
            {
                _prodphoto = value;
            }
        }
        public Status _stat = null;
        public Status status
        {
            get
            {
                if (_stat == null)
                    _stat = new Status();
                return _stat;
            }
            set
            {
                _stat = value;
            }
        }
        public int 出售量 { get; set; }

        public MyFavorite _favorite = null;
        public MyFavorite myFavorite
        {
            get
            {
                if (_favorite == null)
                    _favorite = new MyFavorite();
                return _favorite;
            }
            set
            {
                _favorite = value;
            }
        }

        public List<Review> _review = null;
        public List<Review> review
        {
            get
            {
                if (_review == null)
                    _review = new List<Review>();
                return _review;
            }
            set
            {
                _review = value;
            }
        }
        public List<Member> _Member = null;
        public List<Member> Member
        {
            get
            {
                if (_Member == null)
                    _Member = new List<Member>();
                return _Member;
            }
            set
            {
                _Member = value;
            }
        }
        public ShoppingCart _cart = null;
        public ShoppingCart shopCart
        {
            get
            {
                if (_cart == null)
                    _cart = new ShoppingCart();
                return _cart;
            }
            set
            {
                _cart = value;
            }
        }

        public int ProductId { get { return this.product.ProductId; } set { this.product.ProductId = value; } }
        public string ProductName { get { return this.product.ProductName; } set { this.product.ProductName = value; } }
        public int SupplierId { get { return this.product.SupplierId; } set { this.product.SupplierId = value; } }
        public int ProductUnitPrice { get { return this.product.ProductUnitPrice; } set { this.product.ProductUnitPrice = value; } }
        public int ProductCostPrice { get { return this.product.ProductCostPrice; } set { this.product.ProductCostPrice = value; } }
        public int ProductUnitsInStock { get { return this.product.ProductUnitsInStock; } set { this.product.ProductUnitsInStock = value; } }
        public DateTime? ProduceDate { get { return this.product.ProduceDate; } set { this.product.ProduceDate = value; } }
        public string ProductDescription { get { return this.product.ProductDescription; } set { this.product.ProductDescription = value; } }
        public string ProductSize { get { return this.product.ProductSize; } set { this.product.ProductSize = value; } }
        public bool DefectiveGood { get { return this.product.DefectiveGood; } set { this.product.DefectiveGood = value; } }
        public bool InShop { get { return this.product.InShop; } set { this.product.InShop = value; } }
        public byte[] ProductQrcode { get { return this.product.ProductQrcode; } set { this.product.ProductQrcode = value; } }
        public bool HotProduct { get { return this.product.HotProduct; } set { this.product.HotProduct = value; } }
        public string SupplierName { get {return this.supplier.SupplierName; } set {this.supplier.SupplierName=value; } }
        //public int ProductId { get; set; }
        //public string ProductName { get; set; }
        //public int SupplierId { get; set; }
        //public int ProductUnitPrice { get; set; }
        //public int ProductCostPrice { get; set; }
        //public int ProductUnitsInStock { get; set; }
        //public DateTime? ProduceDate { get; set; }
        //public string ProductDescription { get; set; }
        //public string ProductSize { get; set; }
        //public bool DefectiveGood { get; set; }
        //public bool InShop { get; set; }
        //public byte[] ProductQrcode { get; set; }
        //public bool HotProduct { get; set; }

    }
}

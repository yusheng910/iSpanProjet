using System;
using System.Collections.Generic;
using 鮮蔬果季_前台.Models;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace 鮮蔬果季_前台.ViewModels
{
    public class SupplierViewModel
    {
        public Supplier _supplier = null;
        public Supplier supplier { 
            get { if(supplier == null)
                    supplier = new Supplier();
                return _supplier; }
            set { _supplier = value; }
        }
        //public SupplierViewModel()
        //{
        //    Events = new HashSet<Event>();
        //    MyFavorites = new HashSet<MyFavorite>();
        //    Products = new HashSet<Product>();
        //}

        public int SupplierId { get {return this._supplier.SupplierId; } set {this._supplier.SupplierId=value; } }
        [DisplayName("供應商名稱")]
        public string SupplierName { get {return this._supplier.SupplierName; } set { this._supplier.SupplierName = value; } }
        [DisplayName("負責人")]
        public string BusinessOwner { get { return this._supplier.BusinessOwner; } set {this._supplier.BusinessOwner=value; } }
        [DisplayName("供應商地址")]
        public string SupplierAddress { get { return this._supplier.SupplierAddress; } set {this._supplier.SupplierAddress=value; } }
        [DisplayName("電話")]
        public string Mobile { get {return this._supplier.Mobile; } set { this._supplier.Mobile = value; } }
        [DisplayName("城市ID")]
        public int CityId { get {return this._supplier.CityId; } set {this._supplier.CityId=value; } }
        [DisplayName("供應商履歷")]
        public string SupplierProfile { get { return this._supplier.SupplierProfile; } set {this._supplier.SupplierName=value; } }

        public virtual City City { get { return this._supplier.City; } set { this._supplier.SupplierProfile = value;} }
        public virtual ICollection<Event> Events { get { return this._supplier.Events; } set { this._supplier.Events = value; } }
        public virtual ICollection<MyFavorite> MyFavorites { get { return this._supplier.MyFavorites; } set { this._supplier.MyFavorites = value; } }
        public virtual ICollection<Product> Products { get {return this._supplier.Products; } set { this._supplier.Products=value; } }
    }
}

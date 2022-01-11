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
            get { if(_supplier == null)
                    _supplier = new Supplier();
                return _supplier; }
            set { _supplier = value; }
        }
        public City _city = null;
        public City city
        {
            get
            {
                if (_city== null)
                    _city = new City();
                return _city;
            }
            set { _city = value; }
        }

        public string cities { get; set; }
        public int SupplierId { get {return this.supplier.SupplierId; } set {this.supplier.SupplierId=value; } }
        [DisplayName("供應商名稱")]
        public string SupplierName { get {return this.supplier.SupplierName; } set { this.supplier.SupplierName = value; } }
        [DisplayName("負責人")]
        public string BusinessOwner { get { return this.supplier.BusinessOwner; } set {this.supplier.BusinessOwner=value; } }
        [DisplayName("供應商地址")]
        public string SupplierAddress { get { return this.supplier.SupplierAddress; } set {this.supplier.SupplierAddress=value; } }
        [DisplayName("電話")]
        public string Mobile { get {return this.supplier.Mobile; } set { this.supplier.Mobile = value; } }
        [DisplayName("城市ID")]
        public int CityId { get {return this.supplier.CityId; } set {this.supplier.CityId=value; } }
        public String CityName { get { return this.city.CityName; } set { this.city.CityName = value; } }
        [DisplayName("供應商履歷")]
        public string SupplierProfile { get { return this.supplier.SupplierProfile; } set {this.supplier.SupplierName=value; } }
        

    }
}

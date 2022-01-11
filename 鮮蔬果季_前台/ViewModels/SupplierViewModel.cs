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
        

    }
}

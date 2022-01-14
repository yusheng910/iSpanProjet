using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using System.Globalization;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace 鮮蔬果季_前台.ViewModels
{
    public class BlogDetailListViewModel
    {
        public BlogDetail _BlogDetail  = null;
        public BlogDetail BlogDetail
        {
            get
            {
                if (_BlogDetail == null)
                    _BlogDetail = new BlogDetail();
                return _BlogDetail;
            }
            set
            {
                _BlogDetail = value;
            }
        }



        public Supplier _Supplier = null;
        public Supplier Supplier
        {
            get
            {
                if (_Supplier == null)
                    _Supplier = new Supplier();
                return _Supplier;
            }
            set
            {
                _Supplier = value;
            }
        }

        public City _City = null;
        public City City
        {
            get
            {
                if (_City == null)
                    _City = new City();
                return _City;
            }
            set
            {
                _City = value;
            }
        }

        public IFormFile photo { get; set; }   

        [DisplayName("部落格ID")]
        public int BlogDetailID { get { return this.BlogDetail.BlogDetailId; } set { this.BlogDetail.BlogDetailId = value; } }

        [DisplayName("主標題")]
        public string Title { get { return this.BlogDetail.Title; } set { this.BlogDetail.Title = value; } }

        [DisplayName("次標題")]
        public string Subtitle { get { return this.BlogDetail.Subtitle; } set { this.BlogDetail.Subtitle = value; } }

        [DisplayName("部落格文章")]
        public string Maintext { get { return this.BlogDetail.Maintext; } set { this.BlogDetail.Maintext = value; } }

        [DisplayName("發布日期")]    //問號代表資料庫,菲必填值
        public DateTime? PublishedDate { get { return this.BlogDetail.PublishedDate; } set { this.BlogDetail.PublishedDate = value; } }

        [DisplayName("標籤分類")]
        public string Label { get { return this.BlogDetail.Label; } set { this.BlogDetail.Label = value; } }

        [DisplayName("供應商名稱")]
        public string SupplierName { get { return this.Supplier.SupplierName; } set { this.Supplier.SupplierName = value; } }

        public string 部落格描述去除html { get; set; }

    }
}

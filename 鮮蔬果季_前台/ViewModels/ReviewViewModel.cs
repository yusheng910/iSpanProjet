using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class ReviewViewModel
    {

        public Review _review = null;
        public Review review
        {
            get
            {
                if (_review == null)
                    _review = new Review();
                return _review;
            }
            set
            {
                _review = value;
            }
        }


        public OrderDetail _odetail = null;
        public OrderDetail odetail
        {
            get
            {
                if (_odetail == null)
                    _odetail = new OrderDetail();
                return _odetail;
            }
            set
            {
                _odetail = value;
            }
        }
        public Product _prod = null;
        public Product product
        {
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
        public Supplier _sup = null;
        public Supplier supplier
        {
            get
            {
                if (_sup == null)
                    _sup = new Supplier();
                return _sup;
            }
            set
            {
                _sup = value;
            }
        }

        //評論
        public int ReviewId { get { return this.review.ReviewId; } set { this.review.ReviewId = value; } }
        [DisplayName("訂單明細編號")]
        public int? OrderDetailId { get { return this.review.OrderDetailId; } set { this.review.OrderDetailId = value; } }
        [DisplayName("評論內容")]
        public string Comments { get { return this.review.Comments; } set { this.review.Comments = value; } }
        [DisplayName("評論日期")]
        public DateTime ReviewDate { get { return this.review.ReviewDate; } set { this.review.ReviewDate = value; } }
        [DisplayName("評論分數")]
        public int StarRanking { get { return this.review.StarRanking; } set { this.review.StarRanking = value; } }
        [DisplayName("屏蔽狀態")]
        public bool Shield { get { return this.review.Shield; } set { this.review.Shield = value; } }

        //產品
        [DisplayName("產品名稱")]
        public string ProductName { get { return this.product.ProductName; } set { this.product.ProductName = value; } }

        //供應商
        [DisplayName("供應商名稱")]
        public string SupplierName { get { return this.supplier.SupplierName; } set { this.supplier.SupplierName = value; } }
        [DisplayName("評論會員")]
        public string MemberName { get; set; } 

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class OrderListViewModel
    {
        public Order order { get; set; }
        public int OrderId {
            get { return this.order.OrderId; }
            set { this.order.OrderId = value; } 
        }
        public DateTime OrderDate {
            get { return this.order.OrderDate; }
            set { this.order.OrderDate = value; }
        }
        public string ShippedTo {
            get { return this.order.ShippedTo; }
            set { this.order.ShippedTo = value; }
        }
        public DateTime? ShippedDate {
            get { return this.order.ShippedDate; }
            set { this.order.ShippedDate = value; }
        }

        public int StatusId {//todo 測試導覽屬性
            get { return this.order.StatusId; }
            set { this.order.StatusId = value; }
        }

        public int MemberId {
            get { return this.order.MemberId; }
            set { this.order.MemberId = value; }
        }
        public int? CouponId
        {
            get { return this.order.CouponId; }
            set { this.order.CouponId = value; }
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
        public ProductPhotoBank _prodphoto = null;
        public ProductPhotoBank photoBank
        {
            get
            {
                if (_prodphoto == null)
                    _prodphoto = new ProductPhotoBank();
                return _prodphoto;
            }
            set
            {
                _prodphoto = value;
            }
        }


        public int 總價 { get; set; }

        public int 單筆訂單細項總價 { get; set; }

        public int 訂單細項商品單價 { get; set; }
        //public int OrderDetailId {
        //    get { return this.order.CouponId; }
        //    set { this.order.CouponId = value; }
        //}
        //public int OrderId { get; set; } //todo OrderDetail
        public int ProductId { get; set; }
        public int UnitsPurchased { get; set; }
        public bool HaveReviews { get; set; }

        public int PayMethodId { get; set; }

        public virtual Coupon coupon { get; set; }
        public virtual Member member { get; set; }
        public virtual ICollection<OrderDetail> orderDetails { get; set; }
    }
}

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

        public Member _member = null;
        public Member member
        {
            get
            {
                if (_member == null)
                    _member = new Member();
                return _member;
            }
            set
            {
                _member = value;
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

        public PayMethod _paymethod = null;
        public PayMethod paymethod
        {
            get
            {
                if (_paymethod == null)
                    _paymethod = new PayMethod();
                return _paymethod;
            }
            set
            {
                _paymethod = value;
            }
        }

        public Coupon _coupon = null;
        public Coupon coupon
        {
            get
            {
                if (_coupon == null)
                    _coupon = new Coupon();
                return _coupon;
            }
            set
            {
                _coupon = value;
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

        public virtual ICollection<OrderDetail> orderDetails { get; set; }

    }
}

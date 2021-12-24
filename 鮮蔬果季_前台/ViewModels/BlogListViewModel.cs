using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using System.Globalization;

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


    }
}

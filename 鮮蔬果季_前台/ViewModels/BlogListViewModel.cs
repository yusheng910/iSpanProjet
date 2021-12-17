using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class OrderListViewModel
    {
        public Order _order = null;
        public Order order
        {
            get
            {
                if (_order == null)
                    _order = new Order();
                return _order;
            }
            set
            {
                _order = value;
            }
        }
        public Status _stat = null;
        public Status stat
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace 鮮蔬果季_前台.Models
{
    public class UserLogin
    {
        public static Member _memb = null;
        public static Member member
        {
            get
            {
                if (_memb == null)
                    _memb = new Member();
                return _memb;
            }
            set
            {
                _memb = value;
            }
        }
        public static Supplier _supp = null;
        public static Supplier supplier
        {
            get
            {
                if (_supp == null)
                    _supp = new Supplier();
                return _supp;
            }
            set
            {
                _supp = value;
            }
        }

    }
}

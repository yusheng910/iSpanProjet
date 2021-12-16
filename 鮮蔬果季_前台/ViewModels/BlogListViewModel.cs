using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class BlogListViewModel
    {

        public EventPhotoBank _EventPhotoBank = null;
        public EventPhotoBank EventPhotoBank
        {
            get
            {
                if (_EventPhotoBank == null)
                    _EventPhotoBank = new EventPhotoBank();
                return _EventPhotoBank;
            }
            set
            {
                _EventPhotoBank = value;
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

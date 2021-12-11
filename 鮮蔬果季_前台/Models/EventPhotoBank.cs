using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class EventPhotoBank
    {
        public int EventPhotoId { get; set; }
        public int EventId { get; set; }
        public string PhotoPath { get; set; }

        public virtual Event Event { get; set; }
    }
}

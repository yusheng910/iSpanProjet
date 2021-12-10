using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class PhotoBank
    {
        public int PhotoId { get; set; }
        public int ProductId { get; set; }
        public byte[] Photo { get; set; }

        public virtual Product Product { get; set; }
    }
}

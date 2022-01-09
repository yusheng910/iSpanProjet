using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace 鮮蔬果季_前台.ViewModels
{
    public class Cashier
    {
        public int TotalAmount { get; set; }
        public string ItemName { get; set; }
        public string CheckMac { get; set; }
        public string Date { get; set; }
        public string TradeNo { get; set; }
        public string BacktoUrl { get; set; }
    }
}

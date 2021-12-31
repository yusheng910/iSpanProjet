using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace 鮮蔬果季_前台.ViewModels
{
    public class ReviewViewModel
    {
        public string AddComments { get; set; }
        public int AddId { get; set; }
        public int  AddStarRanking {get;set;}

        public static double Avgstr { get; set; }
    }
}

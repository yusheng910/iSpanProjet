using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class FeedbackResponseViewModel
    {
        
        public int FeedbackResponseId { get; set; }

        public int OrderDetailId { get; set; }

        public int FeedbackId { get; set; }

        public string FeedbackComment { get; set; }
    }
}

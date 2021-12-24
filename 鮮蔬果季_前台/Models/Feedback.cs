using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public string FeedbackName { get; set; }
        public int? FatherFeedbackId { get; set; }
    }
}

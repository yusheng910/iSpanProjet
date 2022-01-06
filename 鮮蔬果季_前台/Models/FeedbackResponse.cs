using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class FeedbackResponse
    {
        public int FeedbackResponseId { get; set; }
        public int OrderDetailId { get; set; }
        public int FeedbackId { get; set; }
        public string FeedbackComment { get; set; }
        public bool HaveResponses { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class EventRegistration
    {
        public int EventRegistrationId { get; set; }
        public int? EventId { get; set; }
        public int? MemberId { get; set; }
        public int? ParticipantNumber { get; set; }
        public DateTime? SubmitDate { get; set; }

        public virtual Event Event { get; set; }
        public virtual Member Member { get; set; }
    }
}

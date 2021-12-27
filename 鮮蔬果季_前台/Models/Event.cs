using System;
using System.Collections.Generic;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class Event
    {
        public Event()
        {
            EventPhotoBanks = new HashSet<EventPhotoBank>();
            EventRegistrations = new HashSet<EventRegistration>();
        }

        public int EventId { get; set; }
        public int? SupplierId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public int? EventParticipantCap { get; set; }
        public string EventLocation { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public int? CategoryId { get; set; }
        public int? EventPrice { get; set; }
        public string Subtitle { get; set; }
        public int? LableId { get; set; }
        public string Lable { get; set; }

        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<EventPhotoBank> EventPhotoBanks { get; set; }
        public virtual ICollection<EventRegistration> EventRegistrations { get; set; }
    }
}

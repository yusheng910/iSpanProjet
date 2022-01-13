using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class EventListViewModel
    {
        public Event _Event = null;
        public Event Event
        {
            get
            {
                if (_Event == null)
                    _Event = new Event();
                return _Event;
            }
            set
            {
                _Event = value;
            }
        }


        public List <EventPhotoBank> _EventPhotoBank = null;
        public  List<EventPhotoBank> EventPhoto
        {
            get
            {
                if (_EventPhotoBank == null)
                    _EventPhotoBank = new List<EventPhotoBank>();
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


        public EventRegistration _EventRegistration = null;
        public EventRegistration EventRegistration
        {
            get
            {
                if (_EventRegistration == null)
                    _EventRegistration = new EventRegistration();
                return _EventRegistration;
            }
            set
            {
                _EventRegistration = value;
            }
        }

        public Supplier _Supplier = null;
        public Supplier Supplier
        {
            get
            {
                if (_Supplier == null)
                    _Supplier = new Supplier();
                return _Supplier;
            }
            set
            {
                _Supplier = value;
            }
        }

        public Member _member = null;
        public Member member
        {
            get
            {
                if (_member == null)
                    _member = new Member();
                return _member;
            }
            set
            {
                _member = value;
            }
        }

        public List<IFormFile> photo { get; set; }           //不知道運作,目前是用在後臺活動照片寫入  ("~/BackstageEventAPI/PhotoLoad")


        //定義欄位名稱  (DisplayName會需要using)
        //此處設定DisplayName後, 在view裡面的asp-for="SQL欄位名稱",就會帶出以下DisplayName的設定
        [DisplayName("活動名稱")]         
        public string EventName { get { return this.Event.EventName; } set { this.Event.EventName = value;  } }
        public string EventName2 { get; set; }
        
        [DisplayName("活動ID")]
        public int EventId { get { return this.Event.EventId; } set { this.Event.EventId = value; } }

        [DisplayName("供應商名稱")]
        public string SupplierName { get { return this.Supplier.SupplierName; } set { this.Supplier.SupplierName = value; } }

        [DisplayName("活動描述")]
        public string EventDescription { get { return this.Event.EventDescription; } set { this.Event.EventDescription = value; } }
        
        //怪怪的
        //[DisplayName("活動人數上限")] 
        //public int EventParticipantCap { get { return (int)this.Event.EventParticipantCap; } set { (this.Event.EventDescription = value; } }

        [DisplayName("活動地點")]
        public string EventLocation { get { return this.Event.EventLocation; } set { this.Event.EventLocation = value; } }

        [DisplayName("活動開始時間")]     //部落格上傳圖片,這邊會出錯!
        public DateTime EventStartDate { get { return (DateTime)this.Event.EventStartDate; } set { this.Event.EventStartDate = value; } }

        [DisplayName("活動結束時間")]
        public DateTime EventEndDate { get { return (DateTime)this.Event.EventEndDate; } set { this.Event.EventEndDate = value; } }

        [DisplayName("活動副標題")]
        public string Subtitle { get { return this.Event.Subtitle; } set { this.Event.Subtitle = value; } }

        [DisplayName("活動標籤")]
        public string Lable { get { return this.Event.Lable; } set { this.Event.Lable = value; } }


        [DisplayName("活動費用")]
        public int EventPrice { get { return (int)this.Event.EventPrice; } set { this.Event.EventPrice = value; } }

        public int? 活動滿額人數 { get; set; }

        public int? 已報名人數 { get; set; }
        public int? 剩餘名額 { get; set; }
    }
}

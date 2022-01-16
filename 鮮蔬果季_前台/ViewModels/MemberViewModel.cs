using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class MemberViewModel
    {
        private  Member _memb = null;
        public Member member
        {
            get
            {
                if (_memb == null)
                    _memb = new Member();
                return _memb;
            }
            set { _memb = value; }
        }
        public IFormFile photo { get; set; }
        public string  city { get; set; }
        public int MemberId { get { return this.member.MemberId; } set { this.member.MemberId = value; }  }
        public string UserId { get {return this.member.UserId; } set {this.member.UserId=value; } }
        public string Password { get { return this.member.Password; } set {this.member.Password=value; } }
        public string MemberName { get { return this.member.MemberName; } set { this.member.MemberName = value; } }
        public string Gender { get { return this.member.Gender; } set { this.member.Gender = value; } }
        public string MemberAddress { get { return this.member.MemberAddress; } set { this.member.MemberAddress = value; } }
        public DateTime BirthDate { get { return this.member.BirthDate; } set { this.member.BirthDate = value; } }
        public string Mobile { get { return this.member.Mobile; } set { this.member.Mobile = value; } }
        public int CityId { get { return this.member.CityId; } set { this.member.CityId = value; } }
        public DateTime RegisteredDate { get { return this.member.RegisteredDate; } set { this.member.RegisteredDate = value; } }
        public string MemberPhotoPass { get { return this.member.MemberPhotoPass; } set { this.member.MemberPhotoPass = value; } }
        public string BlackList { get {return this.member.BlackList; } set { this.member.BlackList = value; } }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageMemberController : Controller
    {
        private readonly 鮮蔬果季Context db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BackstageMemberController(IWebHostEnvironment webHost, 鮮蔬果季Context dbContext)
        {
            _hostingEnvironment = webHost; //取的wwwroot的路徑
            db = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MemberDetailPartial(int id)
        {
            MemberViewModel 單筆會員 = new MemberViewModel();
            var 會員資料 = (from p in db.Members
                        join s in db.Cities
                        on p.CityId equals s.CityId
                        where p.MemberId == id
                        select new { p, s}).FirstOrDefault();

            單筆會員.member = 會員資料.p;
            單筆會員.city = 會員資料.s.CityName;
            var 縣市 = db.Cities.ToList();
            ViewBag.AllCities = 縣市;
            return PartialView(單筆會員);
        }
        public IActionResult uploadPhoto(MemberViewModel memberphoto)
        {
            if (memberphoto.photo != null)
            {
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/MemberPhoto/" + photoName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))//創造新圖片,如果已存在就覆寫
                {
                    memberphoto.photo.CopyTo(fileStream);//上傳指令
                }
                Member cust = db.Members.FirstOrDefault(c => c.MemberId == memberphoto.MemberId);
                if (cust != null)
                {
                    cust.MemberPhotoPass = photoName;
                    db.SaveChanges();
                }
            }
            else
            {
                return Content("0");
            }
            return Content("1");
        }
        public IActionResult Photoload(int id)
        {
            var q = db.Members.Where(p => p.MemberId == id).FirstOrDefault();
            return PartialView(q);
        }
        public IActionResult submitMemberDetil(MemberViewModel p)
        {
           
            var cityid = (from i in db.Cities
                           where i.CityName == p.city
                           select i.CityId).FirstOrDefault();
            if (p != null)
            {
                Member cust = db.Members.FirstOrDefault(c => c.MemberId == p.MemberId);
                if (cust != null)
                {
                    cust.UserId = p.UserId;
                    cust.Mobile = p.Mobile;
                    cust.MemberAddress = p.MemberAddress;
                    cust.CityId = cityid;
                    cust.Gender = p.Gender;
                    cust.MemberName = p.MemberName;
                    cust.Password = p.Password;
                    db.SaveChanges();
                    return Content("1");
                }
                return Content("0");
            }
            return Content("0");
        }
        public IActionResult showNewMemberDetil()
        {
            var 會員資料 = (from m in db.Members
                        join c in db.Cities
                        on m.CityId equals c.CityId
                        select new { m, c }).ToList();
            List<MemberViewModel> member = new List<MemberViewModel>();

            foreach (var item in 會員資料)
            {
                member.Add(new MemberViewModel()
                {
                    member = item.m,
                    city = item.c.CityName
                });
            }
            return PartialView(member);
        }
    }
}

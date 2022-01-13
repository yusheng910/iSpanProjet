using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class BackstageEventAPIController : Controller
    {

        private readonly 鮮蔬果季Context db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BackstageEventAPIController(IWebHostEnvironment webHost, 鮮蔬果季Context dbContext)
        {
            _hostingEnvironment = webHost; //取的wwwroot的路徑
            db = dbContext;
        }

        public IActionResult PhotoLoad(EventListViewModel EventEdit)
        {
            //如果該產品資料庫有無商品照片，會移除
            var 移除noimage = db.EventPhotoBanks.Where(E => E.EventId == EventEdit.Event.EventId && E.PhotoPath == "nprod.jpg").FirstOrDefault();
            if (移除noimage != null)
                db.Remove(移除noimage);
            if (EventEdit.photo != null)
            {
                int count = 0;
                foreach (var item in EventEdit.photo)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";          //Guid這段,作用為產生不重複亂數,作為照片名稱
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/活動照片/" + photoName); //取得路徑+要上傳的圖片檔案名稱
                    using (var fileStream = new FileStream(filePath, FileMode.Create)) //Create新增圖片，如果已存在就覆寫
                    {
                        EventEdit.photo [count].CopyTo(fileStream); //上傳指令
                    }
                    count++;
                    var q = new EventPhotoBank()
                    {
                        EventId = EventEdit.Event.EventId,
                        PhotoPath = photoName
                    };
                    db.Add(q);
                    db.SaveChanges();
                }
            }
            else
            {
                return Content("0");
            }
            return Content("1");
        }

        //================= 參加活動會員明細 =================
        public IActionResult EventParticipantModalPartial(int id)
        {
            List<EventListViewModel> 參加單一活動會員列表 = new List<EventListViewModel>();
            var 所有參加活動會員 = (from er in db.EventRegistrations
                          where er.EventId == id
                          orderby er.Member.MemberId
                          select new { er, er.Event ,er.Member}).ToList();
            foreach(var item in 所有參加活動會員)
            {
                參加單一活動會員列表.Add(new EventListViewModel()
                {
                    member = item.er.Member,
                    EventName2 =item.er.Event.EventName,
                    EventParticipantCap = item.er.Event.EventParticipantCap,
                    EventRegistration = item.er
                });
            }
            return PartialView(參加單一活動會員列表);
        }


        public IActionResult imgLoad(int id)         //此處為獨立的view (同名稱的view才能連接到)
        {
            var q = db.EventPhotoBanks.Where(E => E.EventId == id).ToList();
            if (q == null)
            {
                return Content("請上傳圖片");
            }
            return PartialView(q);    //與方法不同名的view要用""註明view的名稱  
        }



        public IActionResult ClearImg(int id)
        {
            var q = db.EventPhotoBanks.Where(E => E.EventId == id).ToList();
            foreach (var 圖片 in q)
            {
                if (圖片.PhotoPath != "nprod.jpg")
                {
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/活動照片/" + 圖片.PhotoPath); //取得路徑
                    bool result = System.IO.File.Exists(filePath);
                    if (result == true)
                    {
                        System.IO.File.Delete(filePath);             
                        db.EventPhotoBanks.Remove(圖片);
                        db.SaveChanges();
                    }
                    else
                    {
                        return Content("0");
                    }
                }
                else
                {
                    db.Remove(圖片);//如果按下清空按鈕還有無商品的照片只清除資料庫的資料，但是專案底下的檔案還是要在。避免有多張無商品照片
                }
            }
            //最後清空完成後會留一張無商品的照片
            var q2 = new EventPhotoBank()
            {
                EventId = id,
                PhotoPath = "nprod.jpg"
            };
            db.Add(q2);
            db.SaveChanges();
            return Content("1");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

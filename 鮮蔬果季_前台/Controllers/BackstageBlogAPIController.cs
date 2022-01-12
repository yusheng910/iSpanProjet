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
    public class BackstageBlogAPIController : Controller
    {

        private readonly 鮮蔬果季Context db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BackstageBlogAPIController(IWebHostEnvironment webHost, 鮮蔬果季Context dbContext)
        {
            _hostingEnvironment = webHost; //取的wwwroot的路徑
            db = dbContext;
        }






        public IActionResult uploadPhoto(BlogDetailListViewModel EventEdit)
        {
            if (EventEdit.photo != null)
            {
                BlogDetail cust = db.BlogDetails.FirstOrDefault(c => c.BlogDetailId == EventEdit.BlogDetailID);
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/農友部落格照/" + photoName);
                string oldfilePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/農友部落格照/" + cust.PhotoPath);
                bool result = System.IO.File.Exists(oldfilePath);
                if (cust.PhotoPath != null)
                {
                    if (result == true)
                    {
                        System.IO.File.Delete(oldfilePath);
                    }
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))  //創造新圖片,如果已存在就覆寫
                {
                    //EventEdit.photo.CopyTo(fileStream); //上傳指令 怪怪的
                }

                if (cust != null)
                {
                    cust.PhotoPath = photoName;
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
            var q = db.BlogDetails.Where(p => p.BlogDetailId == id).FirstOrDefault();
            return PartialView(q);
        }























        public IActionResult PhotoLoad(BlogDetailListViewModel EventEdit)
        {
            //如果該產品資料庫有無商品照片，會移除
            var 移除noimage = db.BlogDetails.Where(E => E.BlogDetailId == EventEdit.BlogDetail.BlogDetailId && E.PhotoPath == "nprod.jpg").FirstOrDefault();
            if (移除noimage != null)
                db.Remove(移除noimage);
            if (EventEdit.photo != null)
            {
                int count = 0;
                foreach (var item in EventEdit.photo)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";          //Guid這段,作用為產生不重複亂數,作為照片名稱
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/農友部落格照/" + photoName); //取得路徑+要上傳的圖片檔案名稱
                    using (var fileStream = new FileStream(filePath, FileMode.Create)) //Create新增圖片，如果已存在就覆寫
                    {
                        EventEdit.photo[count].CopyTo(fileStream); //上傳指令
                    }
                    count++;
                    var q = new BlogDetail()
                    {
                        BlogDetailId = EventEdit.BlogDetail.BlogDetailId,
                        PhotoPath = photoName
                    };
                    db.Add(q);          //偵錯這邊會有問題
                    db.SaveChanges();
                }
            }
            else
            {
                return Content("0");
            }
            return Content("1");
        }



        public IActionResult imgLoad(int id)         //此處為獨立的view (同名稱的view才能連接到)
        {
            var q = db.BlogDetails.Where(E => E.BlogDetailId == id).ToList();
            if (q == null)
            {
                return Content("請上傳圖片");
            }
            return PartialView(q);    //與方法不同名的view要用""註明view的名稱  
        }



        public IActionResult ClearImg(int id)
        {
            var q = db.BlogDetails.Where(E => E.BlogDetailId == id).ToList();
            foreach (var 圖片 in q)
            {
                if (圖片.PhotoPath != "nprod.jpg")
                {
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/農友部落格照/" + 圖片.PhotoPath); //取得路徑
                    bool result = System.IO.File.Exists(filePath);
                    if (result == true)
                    {
                        System.IO.File.Delete(filePath);
                        db.BlogDetails.Remove(圖片);
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
            var q2 = new BlogDetail()
            {
                BlogDetailId = id,
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

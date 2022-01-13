using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.ViewModels;
using 鮮蔬果季_前台.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageBlogController : Controller
    {
        private readonly 鮮蔬果季Context db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BackstageBlogController(IWebHostEnvironment webHost, 鮮蔬果季Context dbContext)
        {
            _hostingEnvironment = webHost; //取的wwwroot的路徑
            db = dbContext;
        }

        public IActionResult BlogList()
        {

            //鮮蔬果季Context db = new 鮮蔬果季Context();
            var datas = (from E in db.BlogDetails
                         orderby E.BlogDetailId
                         select E).ToList();

            List<BlogDetailListViewModel> list = new List<BlogDetailListViewModel>();
            foreach (var item in datas)
            {
                //db = new 鮮蔬果季Context();
                var 供應商與城市 = (from Sl in db.Suppliers
                              join C in db.Cities on Sl.CityId equals C.CityId   //關聯第3個資料表
                              where Sl.SupplierId == item.SupplierId
                              select new { Sl, C }).FirstOrDefault();     //抓取兩個資料表

                ViewBag.活動日期 = item.PublishedDate.Value.ToLongDateString();  //僅顯示年月日

                list.Add(new BlogDetailListViewModel()
                {
                    BlogDetail = item,
                    Supplier = 供應商與城市.Sl,
                    City = 供應商與城市.C

                });
            }
            return View(list);
        }




        public IActionResult BlogEditPartial(int id)
        {

            //鮮蔬果季Context db = new 鮮蔬果季Context();
            var 部落格及供應商資料 = (from E in db.BlogDetails
                         join supp in db.Suppliers on E.SupplierId equals supp.SupplierId
                         where id == E.BlogDetailId
                         select new { E, supp}).FirstOrDefault()  ;

            BlogDetailListViewModel 單筆部落格 = new BlogDetailListViewModel();

            單筆部落格.BlogDetail = 部落格及供應商資料.E;
            單筆部落格.Supplier = 部落格及供應商資料.supp;

            return PartialView("BlogEditPartial", 單筆部落格);
        }

  //資料寫入有問題
        public IActionResult BlogEditPartial2(BlogDetailListViewModel  EventEdit )
        {

            var 部落格修改資料 = (from d in db.BlogDetails where d.BlogDetailId == EventEdit.BlogDetailID select d).FirstOrDefault();
                部落格修改資料.Title = EventEdit.Title;
                部落格修改資料.Subtitle = EventEdit.Subtitle;
                部落格修改資料.Maintext = EventEdit.Maintext;
                部落格修改資料.Label = EventEdit.Label;

            if (EventEdit.photo != null) {
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/農友部落格照/" + photoName);
                string oldfilePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/農友部落格照/" + 部落格修改資料.PhotoPath);
                bool result = System.IO.File.Exists(oldfilePath);
                if (result == true)
                {
                    System.IO.File.Delete(oldfilePath);
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))//創造新圖片,如果已存在就覆寫
                {
                    EventEdit.photo.CopyTo(fileStream);//上傳指令
                    部落格修改資料.PhotoPath = photoName;
                }
            }
                db.SaveChanges();


            return Content("1");
        }



        public IActionResult BlogListPartial()
        {

            var datas = (from E in db.BlogDetails
                         orderby E.BlogDetailId
                         select E).ToList();

            List<BlogDetailListViewModel> list = new List<BlogDetailListViewModel>();
            foreach (var item in datas)
            {
                //db = new 鮮蔬果季Context();
                var 供應商與城市 = (from Sl in db.Suppliers
                              join C in db.Cities on Sl.CityId equals C.CityId   //關聯第3個資料表
                              where Sl.SupplierId == item.SupplierId
                              select new { Sl, C }).FirstOrDefault();     //抓取兩個資料表

                ViewBag.活動日期 = item.PublishedDate.Value.ToLongDateString();  //僅顯示年月日

                list.Add(new BlogDetailListViewModel()
                {
                    BlogDetail = item,
                    Supplier = 供應商與城市.Sl,
                    City = 供應商與城市.C

                });
            }
            return PartialView(list);
        }


        public IActionResult BlogCreatePartial()
        {

            return PartialView();
        }






    }
}

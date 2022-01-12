using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.ViewModels;
using 鮮蔬果季_前台.Models;
namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageBlogController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageBlogController(鮮蔬果季Context dbContext)
        {
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

                ViewBag.活動日期 = item.PublishedDate.Value.ToLongDateString();

                list.Add(new BlogDetailListViewModel()
                {
                    BlogDetail = item,
                    Supplier = 供應商與城市.Sl,
                    City = 供應商與城市.C

                });
            }
            return View(list);

        }








        //public IActionResult BlogList()
        //{
        //    return View();
        //}
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class PartnerController : Controller
    {
        public IActionResult PartnerBlog(int Id)
        {
            鮮蔬果季Context db = new 鮮蔬果季Context();
            var datas = from E in db.BlogDetails orderby E.PublishedDate descending
                        select E;

            //if (Id==1)       目前卡關,想透過前端回傳的id值,來判斷使用哪種LINQ篩選
            //{ 
            // var datas = from E in db.BlogDetails
            //            where E.LabelId == 1
            //            select E;
            //}


            List<BlogDetailListViewModel> list = new List<BlogDetailListViewModel>();
            foreach (var item in datas)
            {
                db = new 鮮蔬果季Context();
                var 供應商 = (from Sl in db.Suppliers
                           join C in db.Cities on Sl.CityId equals C.CityId   //關聯第3個資料表,不確定是否是這樣
                           where Sl.SupplierId == item.SupplierId
                           select Sl).FirstOrDefault();
                list.Add(new BlogDetailListViewModel()
                {
                    BlogDetail = item,
                    Supplier = 供應商,
                });
            }
            return View(list);
        }


        public IActionResult PartnerBlogSelectTag(int id)
            {
                 鮮蔬果季Context db = new 鮮蔬果季Context();
                  var datas = from E in db.BlogDetails
                        where E.LabelId==1
                        select E;

                List<BlogDetailListViewModel> list = new List<BlogDetailListViewModel>();
                foreach (var item in datas)
                     {
                    db = new 鮮蔬果季Context();
                    var 供應商 = (from Sl in db.Suppliers
                           where Sl.SupplierId == item.SupplierId
                           select Sl).FirstOrDefault();
                    list.Add(new BlogDetailListViewModel()
                     {
                         BlogDetail = item,
                         Supplier = 供應商
                     });
                     }
                return View(list);
               }


        public IActionResult PartnerBlog_1(int id)     //此處的id為前台回傳的該農友ID
        {
            鮮蔬果季Context db = new 鮮蔬果季Context();
            var datas = from E in db.BlogDetails
                        where id == E.SupplierId
                        select E;

            List<BlogDetailListViewModel> list = new List<BlogDetailListViewModel>();
            foreach (var item in datas)
            {
                db = new 鮮蔬果季Context();
                var 供應商 = (from Sl in db.Suppliers
                           where Sl.SupplierId == item.SupplierId  
                           select Sl).FirstOrDefault();
                list.Add(new BlogDetailListViewModel()
                {
                    BlogDetail = item,
                    Supplier = 供應商
                });
            }
            return View(list);
        }
    }
}

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

        // 使用注入的方式啟用db (注入程式在Startup內)
        // 後續引用資料庫,直接使用以下設定的變數db即可
        // 在使用LINQ時,每次都要有斷點(可以用ToList / FirstOrDefault)
        private readonly 鮮蔬果季Context db;

        public PartnerController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }


        public IActionResult PartnerBlog(int Id)
        {

            // 判斷會員是否登入
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
                ViewBag.USER = UserLogin.member.MemberName;
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }

            //鮮蔬果季Context db = new 鮮蔬果季Context();
            var datas = (from E in db.BlogDetails orderby E.PublishedDate descending
                        select E).ToList();

            if (Id == 1) //目前卡關,想透過前端回傳的id值,來判斷使用哪種LINQ篩選
            {    //不另外宣告,用原本的datas覆蓋原本查詢 (以下也是foreach也是datas,故無法用新的)
                 datas = (from E in db.BlogDetails       
                            where E.LabelId == 1
                         orderby E.PublishedDate descending       //須留意使用orderby會變更資料型態,故這邊也要orderby
                         select E).ToList();
            }

            List<BlogDetailListViewModel> list = new List<BlogDetailListViewModel>();
            foreach (var item in datas)
            {
                //db = new 鮮蔬果季Context();
                var 供應商與城市 = (from Sl in db.Suppliers
                           join C in db.Cities on Sl.CityId equals C.CityId   //關聯第3個資料表,不確定是否是這樣
                           where Sl.SupplierId == item.SupplierId  
                           select new { Sl, C } ).FirstOrDefault();     //抓取兩個資料表
                list.Add(new BlogDetailListViewModel()          
                {
                    BlogDetail = item,
                    Supplier = 供應商與城市.Sl, 
                    City = 供應商與城市.C
                });
            }
            return View(list);
        }


        public IActionResult PartnerBlogSelectTag(int id)
            {
            // 判斷會員是否登入
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
                ViewBag.USER = UserLogin.member.MemberName;
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }


            //鮮蔬果季Context db = new 鮮蔬果季Context();
            var datas = (from E in db.BlogDetails
                        where E.LabelId==1
                        select E).ToList();

                List<BlogDetailListViewModel> list = new List<BlogDetailListViewModel>();
                foreach (var item in datas)
                     {
                    //db = new 鮮蔬果季Context();
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
            // 判斷會員是否登入
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
                ViewBag.USER = UserLogin.member.MemberName;
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }

            //鮮蔬果季Context db = new 鮮蔬果季Context();
            var datas = (from E in db.BlogDetails
                        where id == E.SupplierId
                        select E).ToList();

            List<BlogDetailListViewModel> list = new List<BlogDetailListViewModel>();
            foreach (var item in datas)
            {
                //db = new 鮮蔬果季Context();
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

﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }

            //鮮蔬果季Context db = new 鮮蔬果季Context();
            var datas = (from E in db.BlogDetails orderby E.PublishedDate descending
                        select E).ToList();

            if (Id == 1)   //對應標籤進行LINQ查詢
            {
                 datas = (from E in db.BlogDetails       
                            where E.LabelId == 1
                         orderby E.PublishedDate descending     
                         select E).ToList();
            }

            else if (Id == 2)
            {    
                datas = (from E in db.BlogDetails
                         where E.LabelId == 2
                         orderby E.PublishedDate descending       
                         select E).ToList();
            }


            else if (Id == 3)
            {    
                datas = (from E in db.BlogDetails
                         where E.LabelId == 3
                         orderby E.PublishedDate descending    
                         select E).ToList();
            }

            else if (Id == 3)
            {
                datas = (from E in db.BlogDetails
                         where E.LabelId == 3
                         orderby E.PublishedDate descending
                         select E).ToList();
            }

            else if (Id == 11)                 //查詢文章月份  等於 當下月份
            {
                datas = (from E in db.BlogDetails
                         where E.PublishedDate.Value.Month == DateTime.Now.Month
                         orderby E.PublishedDate descending
                         select E).ToList();
            }

            else if (Id == 12)                 //查詢文章月份  等於 當下月份-1
            {
                datas = (from E in db.BlogDetails
                         where E.PublishedDate.Value.Month == DateTime.Now.AddMonths(-1).Month
                         orderby E.PublishedDate descending
                         select E).ToList();
            }

            else if (Id == 13)                 //查詢文章月份  等於 當下月份-2
            {
                datas = (from E in db.BlogDetails
                         where E.PublishedDate.Value.Month == DateTime.Now.AddMonths(-2).Month
                         orderby E.PublishedDate descending
                         select E).ToList();
            }

            else       
            { 
                
                datas = (from E in db.BlogDetails
                         orderby E.PublishedDate descending
                         select E).ToList();
            }
            List<BlogDetailListViewModel> list = new List<BlogDetailListViewModel>();
            foreach (var item in datas)
            {
                //db = new 鮮蔬果季Context();
                var 供應商與城市 = (from Sl in db.Suppliers
                           join C in db.Cities on Sl.CityId equals C.CityId   //關聯第3個資料表
                           where Sl.SupplierId == item.SupplierId  
                           select new { Sl, C } ).FirstOrDefault();     //抓取兩個資料表
                list.Add(new BlogDetailListViewModel()          
                {
                    BlogDetail = item,
                    Supplier = 供應商與城市.Sl , 
                    City = 供應商與城市.C
                });
            }
            return View(list);
        }



        public IActionResult PartnerBlog_1(int id)     //此處的id為前台回傳的該農友ID
        {

            // 判斷會員是否登入
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
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
                var 供應商與城市 = (from Sl in db.Suppliers
                           join C in db.Cities on Sl.CityId equals C.CityId
                           where Sl.SupplierId == item.SupplierId  
                           select new { Sl,C}).FirstOrDefault();
                list.Add(new BlogDetailListViewModel()
                {
                    BlogDetail = item,
                    Supplier = 供應商與城市.Sl,
                    City = 供應商與城市.C,
                });
            }
            return View(list);
        }

















        //public IActionResult PartnerBlogSelectTag(int id)
        //    {
        //    // 判斷會員是否登入
        //    if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
        //        ViewBag.USER = UserLogin.member.MemberName;
        //    else //Seesion沒找到
        //    {
        //        ViewBag.USER = null;
        //        UserLogin.member = null;
        //    }


        //    //鮮蔬果季Context db = new 鮮蔬果季Context();
        //    var datas = (from E in db.BlogDetails
        //                where E.LabelId==1
        //                select E).ToList();

        //        List<BlogDetailListViewModel> list = new List<BlogDetailListViewModel>();
        //        foreach (var item in datas)
        //             {
        //            //db = new 鮮蔬果季Context();
        //            var 供應商 = (from Sl in db.Suppliers
        //                   where Sl.SupplierId == item.SupplierId
        //                   select Sl).FirstOrDefault();
        //            list.Add(new BlogDetailListViewModel()
        //             {
        //                 BlogDetail = item,
        //                 Supplier = 供應商
        //             });
        //             }
        //        return View(list);
        //       }
         }
}

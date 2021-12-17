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
        public IActionResult PartnerBlog()
        {
            鮮蔬果季Context db = new 鮮蔬果季Context();
            var datas = from E in db.BlogDetails
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


            //鮮蔬果季Context db = new 鮮蔬果季Context();
            //var datas = from P in db.Suppliers
            //            select P;
            //return View(datas);
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
                           where Sl.SupplierId == item.SupplierId  //== id  //??
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

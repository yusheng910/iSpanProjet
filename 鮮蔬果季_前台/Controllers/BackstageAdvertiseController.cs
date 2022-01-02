using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageAdvertiseController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageAdvertiseController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult Advertise()
        {
            List<AdvertiseViewModel> 所有商品列表 = new List<AdvertiseViewModel>();
            var 所有產品 = (from prod in db.Products
                        join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                        join c in db.CategoryDetails
                        on prod.ProductId equals c.ProductId
                        where c.CategoryId == 2
                        orderby prod.ProductId
                        select new { prod, supp }).ToList();
            foreach (var item in 所有產品)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.prod.ProductId);
                相片List.Add(封面相片);
                所有商品列表.Add(new AdvertiseViewModel()
                {
                    product = item.prod,
                    supplier = item.supp,
                    photoBank = 相片List,
                });
            }
            var 商品次類別 = db.Categories.Where(c => c.FatherCategoryId != 8).ToList();
            ViewBag.次類別 = 商品次類別;

            List<AdvertiseViewModel> 廣告區域 = new List<AdvertiseViewModel>();
            var 所有產品2 = (from prod in db.Products
                        join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                        join c in db.ProductAdvertises
                        on prod.ProductId equals c.ProductId
                        where c.Tag == "首頁輪播"
                        orderby c.ProductAdvertiseId
                         select new { prod, supp }).ToList();
            foreach (var item in 所有產品2)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.prod.ProductId);
                相片List.Add(封面相片);
                廣告區域.Add(new AdvertiseViewModel()
                {
                    product = item.prod,
                    supplier = item.supp,
                    photoBank = 相片List,
                });
            }
            ViewBag.首頁輪播商品 = 廣告區域;
            return View(所有商品列表);
        }




     }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageAdvertiseApiController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageAdvertiseApiController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult CateLoad(int id)
        {
            List<AdvertiseViewModel> 所有商品列表 = new List<AdvertiseViewModel>();
            var 所有產品 = (from prod in db.Products
                        join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                        join c in db.CategoryDetails
                        on prod.ProductId equals c.ProductId
                        where c.CategoryId == id
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

            return PartialView(所有商品列表);
        }        
        public IActionResult AdvertiseLoad(string tag)
        {
            List<AdvertiseViewModel> 所有商品列表 = new List<AdvertiseViewModel>();
            var 所有產品 = (from prod in db.Products
                        join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                        join c in db.ProductAdvertises
                        on prod.ProductId equals c.ProductId
                        where c.Tag == tag
                        orderby c.ProductAdvertiseId
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
            return PartialView("CateLoad", 所有商品列表);
        }       
        public IActionResult AdvertiseAdd(string pid , string AdvertisSel)
        {
            string[] textid = pid.Split(",");
            int[] productid= Array.ConvertAll<string, int>(textid, s => int.Parse(s)); 

            var 現有廣告商品 = db.ProductAdvertises.Where(a => a.Tag == AdvertisSel).ToList();
            if (現有廣告商品 != null) {
                foreach (var item in 現有廣告商品)
                {
                    db.Remove(item);
                }
                db.SaveChanges();
            }

            foreach (var id in productid) {
                ProductAdvertise 新的廣告商品 = new ProductAdvertise()
                {
                    ProductId=id,
                    Tag= AdvertisSel
                };
                db.ProductAdvertises.Add(新的廣告商品);
            }
            db.SaveChanges();

            List<AdvertiseViewModel> 所有商品列表 = new List<AdvertiseViewModel>();
            var 所有產品 = (from prod in db.Products
                        join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                        join c in db.ProductAdvertises
                        on prod.ProductId equals c.ProductId
                        where c.Tag == AdvertisSel
                        orderby c.ProductAdvertiseId
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
            return PartialView("CateLoad", 所有商品列表);
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageApiController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageApiController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult ProdDetailPartial(int id)
        {
            ShoppingListViewModel 單筆商品 = new ShoppingListViewModel();
            var 商品明細 = (from p in db.Products
                        join s in db.Suppliers
                        on p.SupplierId equals s.SupplierId
                        where p.ProductId == id
                        select new { p, s }).FirstOrDefault();
            var 產品相片 = db.ProductPhotoBanks.Where(p => p.ProductId == id).ToList();
            單筆商品.product = 商品明細.p;
            單筆商品.supplier = 商品明細.s;
            foreach (var 照片 in 產品相片)
                單筆商品.photoBank.Add(照片);
            var 共應商 = db.Suppliers.ToList();
            ViewBag.AllSupp = 共應商;
            return PartialView(單筆商品);
        }
        public IActionResult ProdListPartial()
        {
            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            var 所有產品 = (from prod in db.Products
                        join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                        join cd in db.CategoryDetails
                        on prod.ProductId equals cd.ProductId
                        join c in db.Categories
                        on cd.CategoryId equals c.CategoryId
                        where cd.CategoryId > 1 && cd.CategoryId < 7
                        select new { prod, supp, cd, c }).ToList();
            foreach (var item in 所有產品)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.Where(p => p.ProductId == item.prod.ProductId);
                foreach (var 相片 in 封面相片)
                    相片List.Add(相片);
                var 欲加購物車商品 = db.ShoppingCarts.FirstOrDefault(c => c.MemberId == UserLogin.member.MemberId && c.ProductId == item.prod.ProductId);
                var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.prod.ProductId); /*TODO 目前會員ID寫死的*/
                所有商品列表.Add(new ShoppingListViewModel()
                {
                    product = item.prod,
                    supplier = item.supp,
                    photoBank = 相片List,
                    myFavorite = 最愛商品,
                    shopCart = 欲加購物車商品,
                    category = item.c,
                    categoryDetail = item.cd
                });
            }

            var 商品主類別 = db.Categories.Where(c => !c.CategoryName.Contains("活動類") && c.FatherCategoryId == null).OrderByDescending(c => c.CategoryId).ToList();
            var 商品次類別 = db.Categories.Where(c => c.FatherCategoryId != 8).ToList();
            var 商品次類別2 = db.Categories.Where(c => c.FatherCategoryId != 8).ToList();
            var 商品分類明細 = (from p in db.CategoryDetails
                          group p by p.CategoryId into g
                          select new { CategoryId = g.Key, Total = g.Count(p => p.CategoryId == g.Key) }).ToList();
            List<C商品各類別總數> 分類list = new List<C商品各類別總數>();
            foreach (var 分類 in 商品分類明細)
            {
                分類list.Add(new C商品各類別總數()
                {
                    分類ID = 分類.CategoryId,
                    總數 = 分類.Total
                });
            }

            ViewBag.主類別 = 商品主類別;
            ViewBag.次類別 = 商品次類別;
            ViewBag.次類別2 = 商品次類別2;
            ViewBag.分類明細 = 分類list;


            return PartialView(所有商品列表);
        }

        public IActionResult ProdEditPartial(ShoppingListViewModel ProdEdit)
        {
            var product = db.Products.FirstOrDefault(a => a.ProductId == ProdEdit.ProductId);
            product.ProductName = ProdEdit.ProductName;
            product.ProductUnitPrice = ProdEdit.ProductUnitPrice;
            product.ProductUnitsInStock = ProdEdit.ProductUnitsInStock;
            product.ProductCostPrice = ProdEdit.ProductCostPrice;
            product.ProductDescription = ProdEdit.ProductDescription;
            db.SaveChanges();

            return Content("1");
        }




    }
}

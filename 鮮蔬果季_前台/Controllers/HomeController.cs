﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class HomeController : Controller
    {
        private readonly 鮮蔬果季Context db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, 鮮蔬果季Context dbContext)
        {
            db = dbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_SUPPLIER)) //Seesion有找到
            {
                ViewBag.SUPP = UserLogin.supplier.SupplierName;
                ViewBag.userID = UserLogin.supplier.SupplierAccount;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }
            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            var 銷售排行 = (from prod in db.Products
                        join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                        join od in db.OrderDetails on prod.ProductId equals od.ProductId
                        join o in db.Orders on od.OrderId equals o.OrderId
                        where o.OrderDate>DateTime.Now.AddDays(-7)
                        group new { prod, supp, od } by new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock } into g
                        select new { g.Key.ProductId, g.Key.ProductName, g.Key.ProductUnitPrice, g.Key.ProductSize, g.Key.SupplierName, g.Key.ProductUnitsInStock, 銷售量 = g.Sum(a => a.od.UnitsPurchased) }).OrderByDescending(a => a.銷售量).ToList();
            foreach (var item in 銷售排行)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.ProductId);
                var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.ProductId);
                相片List.Add(封面相片);
                所有商品列表.Add(new ShoppingListViewModel()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductUnitPrice = item.ProductUnitPrice,
                    ProductSize = item.ProductSize,
                    SupplierName = item.SupplierName,
                    ProductUnitsInStock = item.ProductUnitsInStock,
                    myFavorite = 最愛商品,
                    photoBank = 相片List,
                    出售量=item.銷售量
                });
            }


            return View(所有商品列表);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

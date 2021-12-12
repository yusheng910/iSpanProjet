using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class ShoppingController : Controller
    {

        public IActionResult List()
        {
            鮮蔬果季Context db = new 鮮蔬果季Context();
            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            var 所有產品 = from prod in db.Products
                       join supp in db.Suppliers 
                       on prod.SupplierId equals supp.SupplierId 
                       select new { prod,supp};
            db = new 鮮蔬果季Context();
            foreach (var item in 所有產品)
            {            
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
               var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.prod.ProductId);
                相片List.Add(封面相片);
                var 最愛商品= db.MyFavorites.FirstOrDefault(f => f.MemberId == 2 &&f.ProductId==item.prod.ProductId); /*TODO 目前會員ID寫死的*/
                所有商品列表.Add(new ShoppingListViewModel()
                {
                    product=item.prod,
                    supplier = item.supp,
                    photoBank= 相片List,
                    myFavorite= 最愛商品
                }) ; 
            }
            return View(所有商品列表);
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult ShopDetail(int id)
        {
            鮮蔬果季Context db = new 鮮蔬果季Context();
            ShoppingListViewModel 單筆商品 = new ShoppingListViewModel();
            var 商品明細 = (from p in db.Products
                       join s in db.Suppliers
                       on p.SupplierId equals s.SupplierId
                       where p.ProductId == id
                       select new { p, s }).FirstOrDefault();
            if (商品明細 == null)
                return RedirectToAction("List");
            //db = new 鮮蔬果季Context();
            var 封面相片 = db.ProductPhotoBanks.Where(p => p.ProductId ==id);
            var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == 2 && f.ProductId == id); /*TODO 目前會員ID寫死的*/
            var 商品出售數量 = (from p in db.OrderDetails
                         where p.ProductId == id
                         group p by p.ProductId into g
                         select  g.Sum(p => p.UnitsPurchased)).FirstOrDefault();
            單筆商品.product = 商品明細.p;
            單筆商品.supplier = 商品明細.s;
            foreach (var 照片 in 封面相片)
                單筆商品.photoBank.Add(照片);
            單筆商品.出售量 = 商品出售數量;
            單筆商品.myFavorite = 最愛商品;
            return View(單筆商品);
        }

    }
}

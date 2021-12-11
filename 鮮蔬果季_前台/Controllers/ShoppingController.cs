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
            foreach (var item in 所有產品)
            {
                所有商品列表.Add(new ShoppingListViewModel()
                {
                    product=item.prod,
                    supplier = item.supp
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
            單筆商品.product = 商品明細.p;
            單筆商品.supplier = 商品明細.s;

            return View(單筆商品);
        }

    }
}

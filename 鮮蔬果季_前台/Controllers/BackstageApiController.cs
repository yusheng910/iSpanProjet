using Microsoft.AspNetCore.Mvc;
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
            return PartialView(單筆商品);
        }
    }
}

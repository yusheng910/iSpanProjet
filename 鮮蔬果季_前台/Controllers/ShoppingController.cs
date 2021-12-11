using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.Controllers
{
    public class ShoppingController : Controller
    {
        public IActionResult List()
        {
            var 所有產品 = from prod in (new 鮮蔬果季Context()).Products
                       select prod;
            return View(所有產品);
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
            Product prod = db.Products.FirstOrDefault(p => p.ProductId == id);
            if (prod == null)
                return RedirectToAction("List");
            return View(prod);
        }

    }
}

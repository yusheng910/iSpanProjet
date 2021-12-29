using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.Controllers
{
    public class ShoppingApiController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public ShoppingApiController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult LayoutCart()
        {
            return Json(db.ShoppingCarts);
        }

        public IActionResult Cities()
        {
            var cities = db.Cities.Select(c => new
            {
                c.CityId,
                c.CityName
            }).OrderBy(c => c.CityId);
            return Json(cities);
        }
        public IActionResult ChangeCartQty(int id, int qty)
        {
                var q = (from i in db.ShoppingCarts
                         join pro in db.Products
                         on i.ProductId equals pro.ProductId
                        where i.ShoppingCartId == id
                        select new { i, pro }).FirstOrDefault();

                ShoppingCart Cart = db.ShoppingCarts.FirstOrDefault(i => i.ShoppingCartId == id);
                Cart.UnitsInCart = qty;
                db.SaveChanges();

            var unit = Cart.UnitsInCart;
            var uPrice = q.pro.ProductUnitPrice;
            var price = unit * uPrice;
            return Content(price.ToString()); 
        }

        public IActionResult RemoveCart(int id)
        {
            //var q = (from i in db.ShoppingCarts
            //         join pro in db.Products
            //         on i.ProductId equals pro.ProductId
            //         where i.ShoppingCartId == id
            //         select new { i, pro }).FirstOrDefault();

            //ShoppingCart Cart = db.ShoppingCarts.FirstOrDefault(i => i.ShoppingCartId == id);
            ////todo
            //db.SaveChanges();

            //var unit = Cart.UnitsInCart;
            //var uPrice = q.pro.ProductUnitPrice;
            //var price = unit * uPrice;
            //return Content(price.ToString());
            return View();
        }
    }
}

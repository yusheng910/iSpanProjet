using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            UserLogin.member = user;
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
            Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            UserLogin.member = user;
            var q = (from cart in db.ShoppingCarts
                     join pro in db.Products
                     on cart.ProductId equals pro.ProductId
                     join stat in db.Statuses
                     on cart.StatusId equals stat.StatusId
                     where cart.ShoppingCartId == id
                     select new { cart, pro, stat }).FirstOrDefault();

            ShoppingCart Cart = db.ShoppingCarts.FirstOrDefault(i => i.ShoppingCartId == id);
            Cart.StatusId = 2;
            db.SaveChanges();

            var 購物車品量 = (from c in db.ShoppingCarts
                         where c.StatusId==1 && c.MemberId == UserLogin.member.MemberId
                         select c).Count();
            return Content(購物車品量.ToString());
        }
    }
}

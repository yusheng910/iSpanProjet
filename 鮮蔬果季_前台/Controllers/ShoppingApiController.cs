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
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageReviewsController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageReviewsController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult Reviews()
        {
            List<ReviewViewModel> list = new List<ReviewViewModel>();
            var 評論詳細 = (from r in db.Reviews
                       join od in db.OrderDetails
                       on r.OrderDetailId equals od.OrderDetailId
                       join p in db.Products
                       on od.ProductId equals p.ProductId
                       join sp in db.Suppliers
                       on p.SupplierId equals sp.SupplierId
                       select new { r, od, p, sp }).ToList();
            foreach(var item in 評論詳細)
            {
                list.Add(new ReviewViewModel()
                {
                    review = item.r,
                    odetail = item.od,
                    product = item.p,
                    supplier = item.sp
                });
            }
            return View(list);
        }
    }
}

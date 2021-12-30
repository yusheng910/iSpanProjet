using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageOrderApiController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageOrderApiController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult ChangeToShipped(int id)
        {
            var 訂單列表 = (from ord in db.Orders
                       join stat in db.Statuses
                       on ord.StatusId equals stat.StatusId
                       select new { ord, stat }).FirstOrDefault();
            //傳入OrderID 更改送貨狀態 (status) 
            Order order = db.Orders.FirstOrDefault(i => i.OrderId == id);
            if(order.StatusId==4)
            {
                order.StatusId = 5;
                db.SaveChanges();
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
        public IActionResult ChangeToDelivered(int id)
        {
            var 訂單列表 = (from ord in db.Orders
                        join stat in db.Statuses
                        on ord.StatusId equals stat.StatusId
                        select new { ord, stat }).FirstOrDefault();
            //傳入OrderID 更改送貨狀態 (status) 
            Order order = db.Orders.FirstOrDefault(i => i.OrderId == id);
            order.StatusId = 6;
            db.SaveChanges(); return PartialView();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Orders()
        {
            IEnumerable<Order> orders = null;
            orders = from p in (new 鮮蔬果季Context()).Orders
                     where p.MemberId == 2
                     select p;
            List<OrderListViewModel> list = new List<OrderListViewModel>();

            foreach (Order o in orders)
                list.Add(new OrderListViewModel() { order = o });
            return View(list);
        }
        public IActionResult OrderDetail()
        {
            return View();
        }
        public IActionResult MyFavoriteList()
        {
            return View();
        }
        public IActionResult CouponsList()
        {
            return View();
        }
        public IActionResult MemberCenter()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageSupplierController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageSupplierController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult SupplierList()
        {
            List<SupplierViewModel> list = new List<SupplierViewModel>();
            var suplist = (from sup in db.Suppliers
                           join cid in db.Cities
                           on sup.CityId equals cid.CityId
                           select new { sup, cid }
                         ).ToList();
            foreach (var item in suplist)
            {
                list.Add(new SupplierViewModel()
                {
                    city = item.cid,
                    supplier = item.sup
                });
            }
            return View(list);
        }

        public IActionResult SupplierEditPartail(int id)
        {
            List<SupplierViewModel> supedit = new List<SupplierViewModel>();
            var suplist = (
                from sup in db.Suppliers
                select new { sup }).ToList();
            var suptxt = db.Suppliers.FirstOrDefault(a => a.SupplierId == id);
            return PartialView(supedit);
        }
    }
}

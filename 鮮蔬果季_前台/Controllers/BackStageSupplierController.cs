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
        public IActionResult SupplierListPartial()
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
            return PartialView(list);
        }


        public IActionResult SupplierEditPartail(int id)
        {

            var suptxt = db.Suppliers.FirstOrDefault(a => a.SupplierId == id);
            SupplierViewModel supedit = new SupplierViewModel() {
                supplier = suptxt
            };
            return PartialView(supedit);
        }
        [HttpPost]
        public IActionResult SupplierEditPartail(SupplierViewModel s)
        {
            var supedit = (from a in db.Suppliers
                           where a.SupplierId==s.SupplierId
                           select a  
                           ).FirstOrDefault();
            //var supcity = (from c in db.Cities
            //               where c.CityId == s.CityId
            //              select s).FirstOrDefault();
            supedit.SupplierAddress = s.SupplierAddress;
            supedit.BusinessOwner = s.BusinessOwner;
            supedit.Mobile = s.Mobile;
            //supcity.CityName = s.CityName;
            db.SaveChanges();
            //return RedirectToAction("BackstageSupplier","Supplier");
            return Content("1");
        }

        public IActionResult SupplierAddPartial()
        {
            var suplist = (from a in db.Suppliers
                           select  a).FirstOrDefault();
            List<SupplierViewModel> supadd = new List<SupplierViewModel>();
            {
                        
            };
            return PartialView();
        }
        [HttpPost]
        public IActionResult SupplierAddPartial(SupplierViewModel sup)
        {
            //新增
            var suplist = (from a in db.Suppliers
                           join b in db.Cities
                           on a.CityId equals b.CityId
                          select new{ a,b}).FirstOrDefault();
            Supplier supplier = new Supplier()
            {
                SupplierName=sup.SupplierName,
                BusinessOwner=sup.BusinessOwner,
                SupplierAddress=sup.SupplierAddress,
                Mobile=sup.Mobile,    
            }; 
            db.Add(supplier);
            db.SaveChanges();
            return Content("1");
        }
    }
}

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
        //表單
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
        //=============== 重新載入頁面 =====================================================
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

        //==============  修改功能  ===========================================================
        public IActionResult SupplierEditPartail(int id)
        {

            var suptxt = db.Suppliers.FirstOrDefault(a => a.SupplierId == id);
            SupplierViewModel supedit = new SupplierViewModel() {
                supplier = suptxt
            };
            var 縣市 = db.Cities.ToList();
            ViewBag.AllCities = 縣市;
            return PartialView(supedit);
        }
        [HttpPost]
        public IActionResult SupplierEditPartail(SupplierViewModel s)
        {
            var city = db.Cities.FirstOrDefault(a => a.CityName == s.CityName);
            var supedit = (from a in db.Suppliers
                           where a.SupplierId==s.SupplierId
                           select a  
                           ).FirstOrDefault();
            supedit.SupplierName = s.SupplierName;
            supedit.SupplierAddress = s.SupplierAddress;
            supedit.BusinessOwner = s.BusinessOwner;
            supedit.Mobile = s.Mobile;
            supedit.CityId = city.CityId;
            db.SaveChanges();
            return Content("1");
        }
        //============= 新增功能      ========================================================
        public IActionResult SupplierAddPartial()
        {
            var 縣市 = db.Cities.ToList();
            ViewBag.AllCities = 縣市;
            return PartialView();
        }
        [HttpPost]
        public IActionResult SupplierAddPartial(SupplierViewModel sup)
        {
            var city = db.Cities.FirstOrDefault(a => a.CityName == sup.CityName);

            //新增
            var suplist = (from a in db.Suppliers
                           select a).FirstOrDefault();
            Supplier supplier = new Supplier()
            {
                SupplierName=sup.SupplierName,
                BusinessOwner=sup.BusinessOwner,
                SupplierAddress=sup.SupplierAddress,
                Mobile=sup.Mobile,
                CityId= city.CityId,
            };
            var 縣市 = db.Cities.ToList();
            ViewBag.AllCities = 縣市;
            db.Add(supplier);
            db.SaveChanges();
            return Content("1");
        }
        //=========   防呆驗證  ==================================================//
        //public IActionResult SupplierNameRegex(string SupplierName)
        //{
        //    if (SupplierName != null)
        //    {
        //        if (SupplierName.Length < 4)
        //        {
        //            return Content("字數不的小於4字");
        //        }
        //    }
        //    return Content("");
        //}
    }
}

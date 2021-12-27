using Microsoft.AspNetCore.Mvc;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace 鮮蔬果季_前台.Controllers
{
    public class SupplierController : Controller
    {
       
        public IActionResult ProductList(KeywordViewModel kvm)
        {
            string keyword = kvm.txtKeyword;
            IEnumerable<Supplier> suppliers = null;
            if (string.IsNullOrEmpty(keyword))
                suppliers = from s in (new 鮮蔬果季Context()).Suppliers
                            select s;
            else suppliers = from s in (new 鮮蔬果季Context()).Suppliers
                             where s.SupplierName.Contains(keyword)
                             select s;
            List<SupplierViewModel> list=new List<SupplierViewModel>();
            foreach (Supplier s in suppliers)
                list.Add(new SupplierViewModel() { supplier = s });
            return View(list);
        }
    }
}

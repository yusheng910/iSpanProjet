using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackStageSupplierController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackStageSupplierController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult SupplierList()
        {
            
            return View();
        }
    }
}

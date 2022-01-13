using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageNewsletterController : Controller
    {
        private readonly 鮮蔬果季Context db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BackstageNewsletterController(IWebHostEnvironment webHost, 鮮蔬果季Context dbContext)
        {
            _hostingEnvironment = webHost; //取的wwwroot的路徑
            db = dbContext;
        }
        public IActionResult Newsletter()
        {
            List<MemberViewModel> list = new List<MemberViewModel>();
            var 所有會員email = (from m in db.Members
                             where m.CityId ==4
                             select m.UserId).ToList();
            string email = "";
            foreach (var item in 所有會員email)
            {
                email += item+",";
            }
            email=email.Substring(0, email.Length - 1); 

            ViewBag.email =email;
            ViewBag.demo = "freshveg132@gmail.com";

            return View();
        }
    }



}

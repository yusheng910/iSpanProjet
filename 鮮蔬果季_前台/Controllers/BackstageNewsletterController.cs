using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
            return View();
        }
        public IActionResult GetDemoNews()

        {
            try
            {
                string uploadsFolder = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "textdemo"); //c:\......\uploads
                string file = System.IO.Path.Combine(uploadsFolder, "demo.txt");
                string result = "";
                if (System.IO.File.Exists(file))
                {
                    using (var sr = new StreamReader(file))
                    {
                        result = sr.ReadToEnd();
                    }
                }
                return Content(result);
            }
            catch
            {
                return Content("");
            }
        }
        public IActionResult GetDemoNews2()

        {
            try
            {
                string uploadsFolder = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "textdemo"); //c:\......\uploads
                string file = System.IO.Path.Combine(uploadsFolder, "demo2.txt");
                string result = "";
                if (System.IO.File.Exists(file))
                {
                    using (var sr = new StreamReader(file))
                    {
                        result = sr.ReadToEnd();
                    }
                }
                return Content(result);
            }
            catch
            {
                return Content("");
            }
        }
        public IActionResult GetDemoNews3()

        {
            try
            {
                string uploadsFolder = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "textdemo"); //c:\......\uploads
                string file = System.IO.Path.Combine(uploadsFolder, "demo3.txt");
                string result = "";
                if (System.IO.File.Exists(file))
                {
                    using (var sr = new StreamReader(file))
                    {
                        result = sr.ReadToEnd();
                    }
                }
                return Content(result);
            }
            catch
            {
                return Content("");
            }
        }
    }
}

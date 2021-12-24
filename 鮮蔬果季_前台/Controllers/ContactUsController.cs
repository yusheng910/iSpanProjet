using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
namespace 鮮蔬果季_前台.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly 鮮蔬果季Context _context;
        
        public ContactUsController(IWebHostEnvironment hostEnvironment,鮮蔬果季Context context)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        //讀取回應項目
        public IActionResult FeedbackNames() {
            var feedbacknames = _context.Feedbacks.Select(a => new
            {
                a.FeedbackName
            }).Distinct().OrderBy(a => a.FeedbackName);
            return Json(feedbacknames);
        }
        //讀取細項類別
        public IActionResult FatherFeedbackIds()
        {
            var fatherfeedackids = _context.Feedbacks.Select(b => new
            {
                b.FatherFeedbackId
            }).Distinct().OrderBy(b => b.FatherFeedbackId);
            return Json(fatherfeedackids);
        }
    }
    
}

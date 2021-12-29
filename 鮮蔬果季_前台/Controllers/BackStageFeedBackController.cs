using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackStageFeedBackController : Controller
    {
        private readonly 鮮蔬果季Context _context;
        public BackStageFeedBackController(鮮蔬果季Context context)
        {
           
            _context = context;
        }
        public IActionResult List()
        {
            
            return View(_context.FeedbackResponses);
        }
    }
}

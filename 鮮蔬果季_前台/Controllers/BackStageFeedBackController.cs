using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackStageFeedBackController : Controller
    {
        private readonly 鮮蔬果季Context _context;
        public BackStageFeedBackController(鮮蔬果季Context context)
        {
           
            _context = context;
        }
        public IActionResult FeedbackList()
        {
            //List<FeedbackResponseViewModel> 所有意見回饋 = new List<FeedbackResponseViewModel>() ;
            
            return View(_context.FeedbackResponses);
        }
        public IActionResult FeedbackCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult FeedbackCreate(FeedbackResponse _response)
        {
            FeedbackResponse feedback = new FeedbackResponse()
            {
                OrderDetailId = _response.OrderDetailId,
                FeedbackResponseId = _response.FeedbackResponseId,
                FeedbackId = _response.FeedbackId,
                FeedbackComment = _response.FeedbackComment
            };
            _context.Add(feedback);
            _context.SaveChanges();
            return RedirectToAction("FeedbackList");
        }
        public IActionResult FeedbackDelete(int id)
        {
            var message = _context.FeedbackResponses.FirstOrDefault(m => m.FeedbackResponseId == id);
            if (message != null)
            {
                _context.FeedbackResponses.Remove(message);
                _context.SaveChanges();
            }
            return RedirectToAction("FeedbackList");
        }
        public IActionResult FeedbackEdit(int id)
        {
            FeedbackResponse message = new 鮮蔬果季Context().FeedbackResponses.FirstOrDefault(m => m.FeedbackResponseId == id);
            if (message == null)
                return RedirectToAction("FeedbackList");
            return View(message);
        }
        [HttpPost]
        public IActionResult FeedbackEdit(FeedbackResponse _feedback)
        {
            FeedbackResponse message = new 鮮蔬果季Context().FeedbackResponses.FirstOrDefault(m => m.FeedbackResponseId == _feedback.FeedbackResponseId);
            if (message != null)
            {
                message.FeedbackResponseId = _feedback.FeedbackResponseId;
                message.FeedbackId = _feedback.FeedbackId;
                message.OrderDetailId = _feedback.OrderDetailId;
                message.FeedbackComment = _feedback.FeedbackComment;
            }
            return RedirectToAction("FeedbackList");
        }
    }
}

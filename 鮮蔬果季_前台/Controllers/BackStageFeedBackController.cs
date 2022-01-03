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
        public IActionResult FeedbackList(FeedbackResponseViewModel message)
        {
            List<FeedbackResponseViewModel> 所有意見回饋列表 = new List<FeedbackResponseViewModel>() ;
            var feedbackname = (
                          from fb in _context.Feedbacks
                          join fbr1 in _context.FeedbackResponses
                          on fb.FeedbackId equals fbr1.FeedbackId                   
                          join od2 in _context.OrderDetails
                          on fbr1.OrderDetailId equals od2.OrderDetailId
                          join prod in _context.Products
                          on od2.ProductId equals prod.ProductId
                          join sup in _context.Suppliers
                          on prod.SupplierId equals sup.SupplierId
                          select new {fbr1,fb,od2,prod,sup}
                      ).ToList();
            
            

            foreach (var item in feedbackname)
            {
                所有意見回饋列表.Add(new FeedbackResponseViewModel()
                {
                    feedback = item.fb,
                    feedbackResponse = item.fbr1,
                    product=item.prod,
                    supplier=item.sup
                });

            }
            return View(所有意見回饋列表);
        }
        public IActionResult FeedbackDetailPartail(int id)
        {
            FeedbackResponseViewModel 單筆回應 = new FeedbackResponseViewModel();
            
            return PartialView(單筆回應);
        }
        //public IActionResult FeedbackCreate()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult FeedbackCreate(FeedbackResponse _response)
        //{
        //    FeedbackResponse feedback = new FeedbackResponse()
        //    {
        //        OrderDetailId = _response.OrderDetailId,
        //        FeedbackResponseId = _response.FeedbackResponseId,
        //        FeedbackId = _response.FeedbackId,
        //        FeedbackComment = _response.FeedbackComment
        //    };
        //    _context.Add(feedback);
        //    _context.SaveChanges();
        //    return RedirectToAction("FeedbackList");
        //}



        //public IActionResult FeedbackDelete(int id)
        //{
        //    var message = _context.FeedbackResponses.FirstOrDefault(m => m.FeedbackResponseId == id);
        //    if (message != null)
        //    {
        //        _context.FeedbackResponses.Remove(message);
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("FeedbackList");
        //}
        //public IActionResult FeedbackEdit(int id)
        //{
        //    var message = _context.FeedbackResponses.FirstOrDefault(m => m.FeedbackResponseId == id);
        //    if (message == null)
        //        return RedirectToAction("FeedbackList");
        //    return View(message);
        //}
        //[HttpPost]
        //public IActionResult FeedbackEdit(FeedbackResponse editmessage)
        //{
        //    var message = _context.FeedbackResponses.FirstOrDefault(m => m.FeedbackResponseId == editmessage.FeedbackResponseId);
        //    if (message != null)
        //    {
        //        message.FeedbackId = editmessage.FeedbackId;
        //        message.OrderDetailId = editmessage.OrderDetailId;
        //        message.FeedbackComment = editmessage.FeedbackComment;
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("FeedbackList");
        //}
    }
}

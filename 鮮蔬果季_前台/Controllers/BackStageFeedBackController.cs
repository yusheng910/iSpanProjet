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
                          join o in _context.Orders
                          on od2.OrderId equals o.OrderId
                          join m in _context.Members
                          on o.MemberId equals m.MemberId
                          select new {fbr1,fb,od2,prod,sup,o,m}
                      ).ToList();                       
            foreach (var item in feedbackname)
            {
                所有意見回饋列表.Add(new FeedbackResponseViewModel()
                {
                    member=item.m,
                    feedback = item.fb,
                    feedbackResponse = item.fbr1,
                    product=item.prod,
                    supplier=item.sup,                    
                });             
            }
            return View(所有意見回饋列表);
        }
        public IActionResult FeedbackDetailPartail(int id)
        {
            FeedbackResponseViewModel 單筆回應 = new FeedbackResponseViewModel();
            var 會員資料 = (
                 from fb in _context.Feedbacks
                 join fbr1 in _context.FeedbackResponses
                 on fb.FeedbackId equals fbr1.FeedbackId
                 join od2 in _context.OrderDetails
                 on fbr1.OrderDetailId equals od2.OrderDetailId
                 join prod in _context.Products
                 on od2.ProductId equals prod.ProductId
                 join sup in _context.Suppliers
                 on prod.SupplierId equals sup.SupplierId
                 join o in _context.Orders
                 on od2.OrderId equals o.OrderId
                 join m in _context.Members
                 on o.MemberId equals m.MemberId
                 where fbr1.FeedbackResponseId == id
                 select new { fbr1, fb, od2, prod, sup, o, m }
                ).FirstOrDefault();
            單筆回應.member=會員資料.m;
            單筆回應.FeedbackResponseID = 會員資料.fbr1.FeedbackResponseId;
            單筆回應.FeedbackName = 會員資料.fb.FeedbackName;
            return PartialView(單筆回應);
        }     
    }
}

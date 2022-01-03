using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly 鮮蔬果季Context _context;

        public ContactUsController(IWebHostEnvironment hostEnvironment, 鮮蔬果季Context context)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }
        public IActionResult ContactUs(int id,FeedbackResponseViewModel title )
        {
            List<FeedbackResponseViewModel> 標題 = new List<FeedbackResponseViewModel>();
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
                          select new { fbr1, fb, od2, prod, sup, o, m }
                      ).ToList();



           

            ViewBag.suppliername =null ;//供應商名稱
            ViewBag.productname = title.ProductName;//產品名
            ViewBag.orderdetailname = id;//訂單編號
            //========================引用帳號登入屏蔽================================================//
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");//修改完後解除
            }
            return View(id);
        }
        //==============================列出回報選項===============================================//
        public IActionResult FeedbackNames()
        {
            var feedbacknames = _context.Feedbacks.Where(a => a.FatherFeedbackId == null).OrderBy(a => a.FeedbackId).Select(a => new
            {
                a.FeedbackId,
                a.FeedbackName
            }).ToList();
            return Json(feedbacknames);
        }
        
        public IActionResult FatherFeedbackIds(int id)
        {
            var feedackids = _context.Feedbacks.OrderBy(b => b.FeedbackId).Select(b => new
            {
                b.FeedbackId,
                b.FeedbackName
            }).ToList();
            var fatherfeedackids = _context.Feedbacks.Where(a => a.FatherFeedbackId == id).OrderBy(b => b.FatherFeedbackId).Select(b => new
            {
                b.FeedbackId,
                b.FeedbackName
            }).ToList();
            if (fatherfeedackids == null)
            {
                return Json(feedackids);
            }
            return Json(fatherfeedackids);
        }
        //將產品意見回饋傳入資料庫

        //===================問題回報傳回資料庫===========================================================//
        public IActionResult Send(FeedbackResponse _respose)
        {
            FeedbackResponse q = new FeedbackResponse()
            {
                OrderDetailId = _respose.OrderDetailId,
                FeedbackComment = _respose.FeedbackComment,
                FeedbackResponseId = _respose.FeedbackResponseId,
                FeedbackId = _respose.FeedbackId
            };

            _context.FeedbackResponses.Add(q);
            _context.SaveChanges();
            return RedirectToAction("Orders", "Order");
        }
       


    }

}

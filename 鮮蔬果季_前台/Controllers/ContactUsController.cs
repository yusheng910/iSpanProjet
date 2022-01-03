using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using 鮮蔬果季_前台.Models;

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
        public IActionResult ContactUs(int id)
        {
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
                //return RedirectToAction("Login", "Login");//修改完後解除
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

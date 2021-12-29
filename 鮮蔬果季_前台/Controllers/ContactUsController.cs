﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

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
        public IActionResult ContactUs(int id)
        {
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
        //測試讀取回應項目是否成功
        public IActionResult FeedbackNames() {
            var feedbacknames = _context.Feedbacks.Where(a=>a.FatherFeedbackId==null).OrderBy(a=>a.FeedbackId).Select(a => new
            {
                a.FeedbackId,
                a.FeedbackName
            }).ToList();
            return Json(feedbacknames);
        }
        //測試讀取細項類別是否成功
        public IActionResult FatherFeedbackIds(int id)
        {
            var fatherfeedackids = _context.Feedbacks.Where(a=>a.FatherFeedbackId==id).OrderBy(b => b.FatherFeedbackId).Select(b => new
            {
                b.FeedbackId,
                b.FeedbackName
            }).ToList();
            return Json(fatherfeedackids);
        }
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
            //return View();
            return RedirectToAction("Index", "Home");
        }

        

    }
    
}

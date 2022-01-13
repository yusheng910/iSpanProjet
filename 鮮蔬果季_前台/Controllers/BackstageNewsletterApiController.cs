﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageNewsletterApiController : Controller
    {
        private readonly 鮮蔬果季Context db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BackstageNewsletterApiController(IWebHostEnvironment webHost, 鮮蔬果季Context dbContext)
        {
            _hostingEnvironment = webHost; //取的wwwroot的路徑
            db = dbContext;
        }
        public IActionResult Privacy()
        {
            //獲取當前請求完整的Url地址
            var GetCompleteUrlStr = GetCompleteUrl();

            return View();
        }
        private string GetCompleteUrl()
        {
            return new System.Text.StringBuilder()
                 .Append(HttpContext.Request.Scheme)
                 .Append("://")
                 .Append(HttpContext.Request.Host)
                 //.Append(HttpContext.Request.PathBase)
                 //.Append(HttpContext.Request.Path)
                 //.Append(HttpContext.Request.QueryString)
                 .ToString();
        }
        [HttpPost]
        public async Task<IActionResult> UploadF()
        {
            int code = 0;
            var files = Request.Form.Files;
            string filename = files[0].FileName;
            string fileExtention = System.IO.Path.GetExtension(files[0].FileName);
            string path = Guid.NewGuid().ToString() + fileExtention;
            string basepath = _hostingEnvironment.WebRootPath;
            string path_server = "/images/NewsletterImg/" + path;
            var GetCompleteUrlStr = GetCompleteUrl();
            string full_url = (GetCompleteUrl()).ToString() + path_server;
            using (FileStream fstream = new FileStream(basepath + path_server, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //  using (FileStream fstream = System.IO.File.Create(newFile)) //兩種都可以使用
            {
                await files[0].CopyToAsync(fstream);
                code = 1;
            }
            return Json(new { success=code, url= full_url, message = "上傳成功！" });
        }


        //[HttpPost]
        //public IActionResult uploadimg(IFormFile image)
        //{
        //    string photoName = Guid.NewGuid().ToString() + ".jpg";
        //    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "/images/NewsletterImg/" + photoName); //取得路徑+要上傳的圖片檔案名稱
        //    using (var fileStream = new FileStream(filePath, FileMode.Create)) //Create新增圖片，如果已存在就覆寫
        //    {
        //        image.CopyTo(fileStream); //上傳指令
        //    }
        //    return Json(new {sucess = 1,url = filePath,message ="上傳成功!" });
        //}
    }



}


using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class BackstageController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public BackstageController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Member()
        {
            var 會員資料 = (from m in db.Members
                        join c in db.Cities
                        on m.CityId equals c.CityId
                        select new { m, c }).ToList();
            List<MemberViewModel> member = new List<MemberViewModel>();

            foreach (var item in 會員資料) {
                member.Add(new MemberViewModel()
                {
                    member=item.m,
                    city=item.c.CityName
                });
            }
            return View(member);
        }
        
        public IActionResult Product()
        {
            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            var 所有產品 = (from prod in db.Products
                        join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                        join cd in db.CategoryDetails
                        on prod.ProductId equals  cd.ProductId
                        join c in db.Categories
                        on cd.CategoryId equals c.CategoryId
                        where cd.CategoryId>1 && cd.CategoryId<6 || cd.CategoryId==7
                        select new { prod, supp,cd,c }).ToList();
            foreach (var item in 所有產品)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.Where(p => p.ProductId == item.prod.ProductId);
                foreach(var 相片 in 封面相片)
                相片List.Add(相片);
                var 欲加購物車商品 = db.ShoppingCarts.FirstOrDefault(c => c.MemberId == UserLogin.member.MemberId && c.ProductId == item.prod.ProductId);
                var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.prod.ProductId); /*TODO 目前會員ID寫死的*/
                所有商品列表.Add(new ShoppingListViewModel()
                {
                    product = item.prod,
                    supplier = item.supp,
                    photoBank = 相片List,
                    myFavorite = 最愛商品,
                    shopCart = 欲加購物車商品,
                    category=item.c,
                    categoryDetail=item.cd
                });
            }

            var 商品主類別 = db.Categories.Where(c => !c.CategoryName.Contains("活動類") && c.FatherCategoryId == null).OrderByDescending(c => c.CategoryId).ToList();
            var 商品次類別 = db.Categories.Where(c => c.FatherCategoryId != 8).ToList();
            var 商品次類別2 = db.Categories.Where(c => c.FatherCategoryId != 8).ToList();
            var 商品分類明細 = (from p in db.CategoryDetails
                          group p by p.CategoryId into g
                          select new { CategoryId = g.Key, Total = g.Count(p => p.CategoryId == g.Key) }).ToList();
            List<C商品各類別總數> 分類list = new List<C商品各類別總數>();
            foreach (var 分類 in 商品分類明細)
            {
                分類list.Add(new C商品各類別總數()
                {
                    分類ID = 分類.CategoryId,
                    總數 = 分類.Total
                });
            }

            ViewBag.主類別 = 商品主類別;
            ViewBag.次類別 = 商品次類別;
            ViewBag.次類別2 = 商品次類別2;
            ViewBag.分類明細 = 分類list;


            return View(所有商品列表);
        }
        public IActionResult Customer()
        {
            var q = from p in (new 鮮蔬果季Context()).Members
                    select p;
            return View(q);
        }



        public IActionResult Event()
        {
            List<EventListViewModel> 所有活動列表 = new List<EventListViewModel>();
            var 所有活動 = (from E in db.Events
                        join supp in db.Suppliers
                        on E.SupplierId equals supp.SupplierId
                        select new { E, supp }).ToList();

            foreach (var item in 所有活動)
            {
                List<EventPhotoBank> 相片list = new List<EventPhotoBank>();
                var 城市資料 = db.Cities.FirstOrDefault(C => C.CityId == item.supp.CityId);
                var 照片資料 = db.EventPhotoBanks.FirstOrDefault(P => P.EventId == item.E.EventId);
                相片list.Add(照片資料);

                所有活動列表.Add(new EventListViewModel()  //加入EventListViewModel (new新記憶體空間)
                {
                    Event = item.E,
                    City = 城市資料,
                    EventPhoto = 相片list
                });
            }
            return View(所有活動列表);
        }


        public IActionResult EventEdit()
        {
            return View();
        }

        public IActionResult EventCreate()
        {
            return View();
        }


        public IActionResult PowerBIReport_1()
        {
            return View();
        }

        public IActionResult PowerBIReport_2()
        {
            return View();
        }

        public IActionResult PowerBIReport_3()
        {
            return View();
        }
        


    }
}

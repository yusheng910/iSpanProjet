using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using 鮮蔬果季_前台.Models;
using 鮮蔬果季_前台.ViewModels;

namespace 鮮蔬果季_前台.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly 鮮蔬果季Context db;
        public ShoppingController(鮮蔬果季Context dbContext)
        {
            db = dbContext;
        }
        public IActionResult List(int? id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_SUPPLIER)) //Seesion有找到
            {
                ViewBag.SUPP = UserLogin.supplier.SupplierName;
                ViewBag.userID = UserLogin.supplier.SupplierAccount;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }
            //鮮蔬果季Context db = new 鮮蔬果季Context();
            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            var 所有產品 = (from prod in db.Products
                       join supp in db.Suppliers
                       on prod.SupplierId equals supp.SupplierId
                       orderby prod.ProductId
                       select new { prod, supp }).ToList();
            if (id != null)
            {
                所有產品 = (from prod in db.Products
                        join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                        join c in db.CategoryDetails
                        on prod.ProductId equals c.ProductId
                        where c.CategoryId == id
                        orderby prod.ProductId
                        select new { prod, supp }).ToList();
            }
            foreach (var item in 所有產品)
            {            
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
               var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.prod.ProductId);
                相片List.Add(封面相片);
                var 欲加購物車商品 = db.ShoppingCarts.FirstOrDefault(c => c.MemberId == UserLogin.member.MemberId && c.ProductId == item.prod.ProductId);
                var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.prod.ProductId); /*TODO 目前會員ID寫死的*/
                string 分類 = "";
                if (id != null)
                {
                    var 分類名稱 = db.Categories.FirstOrDefault(a => a.CategoryId == id);
                   分類 = 分類名稱.CategoryName;
                }
                所有商品列表.Add(new ShoppingListViewModel()
                {
                    product=item.prod,
                    supplier = item.supp,
                    photoBank= 相片List,
                    myFavorite= 最愛商品,
                    shopCart = 欲加購物車商品,
                    分類名稱=分類
                }) ;


            }

            var 商品主類別 = db.Categories.Where(c => !c.CategoryName.Contains("活動類") && c.FatherCategoryId == null).OrderByDescending(c => c.CategoryId).ToList();
            var 商品次類別 = db.Categories.Where(c => c.FatherCategoryId != 8).ToList();
            var 商品次類別2 = db.Categories.Where(c => c.FatherCategoryId != 8).ToList();
            var 商品分類明細 = (from p in db.CategoryDetails
                          join a in db.Products
                          on p.ProductId equals a.ProductId
                          where a.InShop==true
                          group p by p.CategoryId into g
                          select new { CategoryId=g.Key, Total = g.Count(p => p.CategoryId == g.Key) }).ToList();
            List<C商品各類別總數> 分類list = new List<C商品各類別總數>();
            foreach (var 分類 in 商品分類明細) {
                分類list.Add(new C商品各類別總數() { 
                      分類ID=分類.CategoryId,
                      總數=分類.Total
                });
            }

            ViewBag.主類別 = 商品主類別;
            ViewBag.次類別 = 商品次類別;
            ViewBag.次類別2 = 商品次類別2;
            ViewBag.分類明細 = 分類list;

            List<ShoppingListViewModel> 限時商品列表 = new List<ShoppingListViewModel>();
            var 限時商品 = (from prod in db.Products
                        join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                        join sa in db.ProductOnSales
                        on prod.ProductId equals sa.ProductId
                        orderby prod.ProductId
                        select new { prod, supp, sa }).ToList();

            foreach (var item in 限時商品)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.prod.ProductId);
                相片List.Add(封面相片);
                var 欲加購物車商品 = db.ShoppingCarts.FirstOrDefault(c => c.MemberId == UserLogin.member.MemberId && c.ProductId == item.prod.ProductId);
                var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.prod.ProductId); /*TODO 目前會員ID寫死的*/
                限時商品列表.Add(new ShoppingListViewModel()
                {
                    product = item.prod,
                    supplier = item.supp,
                    photoBank = 相片List,
                    myFavorite = 最愛商品,
                    shopCart = 欲加購物車商品,
                    productOnSale= item.sa
                });
            }
            ViewBag.限時特價 = 限時商品列表;



            return View(所有商品列表);
        }
        #region List的PartialView商業邏輯查詢
        //PartialView 開始
        public IActionResult AllProductPartial()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
            }
            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            var 所有商品 = (from prod in db.Products
                        join supp in db.Suppliers
                       on prod.SupplierId equals supp.SupplierId
                        orderby prod.ProductId
                        select new
                        {
                            prod.ProductId,
                            prod.ProductName,
                            prod.ProductUnitPrice,
                            prod.ProductSize,
                            supp.SupplierName,
                            prod.ProductUnitsInStock,
                            prod.DefectiveGood,
                            prod.InShop,
                            prod.ProductDisCount
                        }).ToList();
            foreach (var item in 所有商品)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.ProductId);
                var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.ProductId);
                相片List.Add(封面相片);
                所有商品列表.Add(new ShoppingListViewModel()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductUnitPrice = item.ProductUnitPrice,
                    ProductSize = item.ProductSize,
                    SupplierName = item.SupplierName,
                    ProductUnitsInStock=item.ProductUnitsInStock,
                    InShop=item.InShop,
                    DefectiveGood=item.DefectiveGood,
                    ProductDisCount=item.ProductDisCount,
                    myFavorite = 最愛商品,
                    photoBank = 相片List
                });
            }
            return PartialView("ProductSearchPartial", 所有商品列表);
        }
        public IActionResult CategoryArray(string cateSel, int min, int max)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
            }
            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            //如果有篩選
            if (cateSel != null)
            {
                string[] textid = cateSel.Split(",");
                int[] cateid = Array.ConvertAll<string, int>(textid, s => int.Parse(s));
                var 所有商品2 = db.CategoryDetails
                    .Where(o => o.CategoryId == cateid[0] && o.Product.ProductUnitPrice > min && o.Product.ProductUnitPrice < max);
                int count = 0,count2=0; //count2為產品數量
                bool flag1=true, flag2=true, flag3=true, flag4=true;
                foreach (var id in cateid)
                {
                    if (flag1&&(id == 3 || id == 4 || id == 5 || id == 7)) {
                        count2++;
                        flag1 = false;
                    }
                    if (flag2 && (id == 20 || id == 21))
                    {
                        count2++;
                        flag2 = false;
                    }
                    if (flag3 && (id == 17 || id == 18))
                    {
                        count2++;
                        flag3 = false;
                    }
                    if (flag4 && (id == 12 || id == 13 || id == 14 || id == 15 || id == 23))
                    {
                        count2++;
                        flag4= false;
                    }
                    count++;
                    if (count == 1) continue;
                    var 所有商品 = db.CategoryDetails
                    .Where(o => o.CategoryId == id && o.Product.ProductUnitPrice > min && o.Product.ProductUnitPrice < max);
                    所有商品2 = 所有商品2.Union(所有商品);
                }
                var 所有商品3 = 所有商品2.GroupBy(x => x.ProductId).Select(x=>new  {pid=x.Key,數量=x.Count() }).ToList();
                foreach (var item in 所有商品3) {
                    if (item.數量 >= count2) {
                        var 所有商品4 = (from prod in db.Products
                                     join supp in db.Suppliers
                                    on prod.SupplierId equals supp.SupplierId
                                     orderby prod.ProductId
                                     where prod.ProductId == item.pid
                                     select new
                                     {
                                         prod.ProductId,
                                         prod.ProductName,
                                         prod.ProductUnitPrice,
                                         prod.ProductSize,
                                         supp.SupplierName,
                                         prod.DefectiveGood,
                                         prod.InShop,
                                         prod.ProductDisCount,
                                         prod.ProductUnitsInStock
                                     }).FirstOrDefault();
                        List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                        var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.pid);
                        var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.pid);
                        相片List.Add(封面相片);
                        所有商品列表.Add(new ShoppingListViewModel()
                        {
                            ProductId = 所有商品4.ProductId,
                            ProductName = 所有商品4.ProductName,
                            ProductUnitPrice = 所有商品4.ProductUnitPrice,
                            ProductSize = 所有商品4.ProductSize,
                            SupplierName = 所有商品4.SupplierName,
                            InShop = 所有商品4.InShop,
                            DefectiveGood = 所有商品4.DefectiveGood,
                            myFavorite = 最愛商品,
                            ProductDisCount = 所有商品4.ProductDisCount,
                            ProductUnitsInStock = 所有商品4.ProductUnitsInStock,
                            photoBank = 相片List
                        });
                    }             
                }
            }
            else { 
            
            }

            return PartialView("ProductSearchPartial", 所有商品列表);
        }
        /*
             public IActionResult CategoryPartial(int id, int min, int max)
         {
             List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
             var 所有商品 = (from prod in db.Products
                         join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                         join c in db.CategoryDetails
                         on prod.ProductId equals c.ProductId
                         where c.CategoryId == id && prod.ProductUnitPrice > min && prod.ProductUnitPrice < max
                         orderby prod.ProductId
                         select new
                         {
                             c.CategoryId,
                             prod.ProductId,
                             prod.ProductName,
                             prod.ProductUnitPrice,
                             prod.ProductSize,
                             supp.SupplierName,
                             prod.DefectiveGood,
                             prod.InShop,
                             prod.ProductUnitsInStock
                         }).ToList();
             foreach (var item in 所有商品)
             {
                 List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                 var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.ProductId);
                 var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.ProductId);
                 相片List.Add(封面相片);
                 所有商品列表.Add(new ShoppingListViewModel()
                 {
                     ProductId = item.ProductId,
                     ProductName = item.ProductName,
                     ProductUnitPrice = item.ProductUnitPrice,
                     ProductSize = item.ProductSize,
                     SupplierName = item.SupplierName,
                     ProductUnitsInStock = item.ProductUnitsInStock,
                     InShop = item.InShop,
                     DefectiveGood = item.DefectiveGood,
                     myFavorite = 最愛商品,
                     photoBank = 相片List,
                     CategoryId=item.CategoryId
                 });
             }
             return PartialView("ProductSearchPartial", 所有商品列表);
         }
         */

        public IActionResult PricePartial(int min, int max)
        {
            //if (categetoryId == 0)
            //{
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
            }
            List<ShoppingListViewModel> 商品列表 = new List<ShoppingListViewModel>();
            var 所有商品2 = (from prod in db.Products
                         join supp in db.Suppliers
                        on prod.SupplierId equals supp.SupplierId
                         where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max
                         orderby prod.ProductId
                         select new
                         {
                             prod.ProductId,
                             prod.ProductName,
                             prod.ProductUnitPrice,
                             prod.ProductSize,
                             supp.SupplierName,
                             prod.ProductUnitsInStock,
                             prod.DefectiveGood,
                             prod.ProductDisCount,
                             prod.InShop
                         }).ToList();
            foreach (var item in 所有商品2)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.ProductId);
                var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.ProductId);
                相片List.Add(封面相片);
                商品列表.Add(new ShoppingListViewModel()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductUnitPrice = item.ProductUnitPrice,
                    ProductSize = item.ProductSize,
                    SupplierName = item.SupplierName,
                    InShop = item.InShop,
                    DefectiveGood = item.DefectiveGood,
                    myFavorite = 最愛商品,
                    ProductDisCount = item.ProductDisCount,
                    ProductUnitsInStock = item.ProductUnitsInStock,
                    photoBank = 相片List
                });
            }
            return PartialView("ProductSearchPartial", 商品列表);
            //    }

            //    List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            //    var 所有商品 = (from prod in db.Products
            //                join supp in db.Suppliers
            //               on prod.SupplierId equals supp.SupplierId
            //                join c in db.CategoryDetails
            //                on prod.ProductId equals c.ProductId
            //                where c.CategoryId == categetoryId && prod.ProductUnitPrice > min && prod.ProductUnitPrice < max
            //                orderby prod.ProductId
            //                select new
            //                {
            //                    c.CategoryId,
            //                    prod.ProductId,
            //                    prod.ProductName,
            //                    prod.ProductUnitPrice,
            //                    prod.ProductSize,
            //                    supp.SupplierName,
            //                    prod.ProductUnitsInStock,
            //                    prod.DefectiveGood,
            //                    prod.InShop
            //                }).ToList();

            //    foreach (var item in 所有商品)
            //    {
            //        List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
            //        var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.ProductId);
            //        var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.ProductId);
            //        相片List.Add(封面相片);
            //        所有商品列表.Add(new ShoppingListViewModel()
            //        {
            //            ProductId = item.ProductId,
            //            ProductName = item.ProductName,
            //            ProductUnitPrice = item.ProductUnitPrice,
            //            ProductSize = item.ProductSize,
            //            SupplierName = item.SupplierName,
            //            ProductUnitsInStock = item.ProductUnitsInStock,
            //            InShop = item.InShop,
            //            DefectiveGood = item.DefectiveGood,
            //            myFavorite = 最愛商品,
            //            photoBank = 相片List,
            //            CategoryId = item.CategoryId
            //        });
            //    }
            //    return PartialView("ProductSearchPartial", 所有商品列表);
            //}
        }
        public IActionResult ProductSearchPartial(string prodName)
        {

            Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            UserLogin.member = user;
            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            var 所有商品 = (from prod in db.Products
                        join supp in db.Suppliers
                       on prod.SupplierId equals supp.SupplierId
                        where   prod.ProductName.Contains(prodName)
                        orderby prod.ProductId
                        select new
                        {
                            prod.ProductId,
                            prod.ProductName,
                            prod.ProductUnitPrice,
                            prod.ProductSize,
                            supp.SupplierName,
                            prod.ProductUnitsInStock,
                            prod.DefectiveGood,
                            prod.ProductDisCount,
                            prod.InShop
                        }).ToList();

            foreach (var item in 所有商品)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.ProductId);
                var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.ProductId);
                相片List.Add(封面相片);
                所有商品列表.Add(new ShoppingListViewModel()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductUnitPrice = item.ProductUnitPrice,
                    ProductSize = item.ProductSize,
                    SupplierName = item.SupplierName,
                    InShop = item.InShop,
                    DefectiveGood = item.DefectiveGood,
                    myFavorite = 最愛商品,
                    ProductDisCount = item.ProductDisCount,
                    ProductUnitsInStock = item.ProductUnitsInStock,
                    photoBank = 相片List
                });
            }
            return PartialView("ProductSearchPartial", 所有商品列表);
        }
        public IActionResult OrderCateArrayPartial(int id, int min, int max, string cateSel)
        {
            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
            }
            var 所有商品 = (from prod in db.Products
                        join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                        orderby prod.ProductId
                        select new
                        {
                            prod.ProductId,
                            prod.ProductName,
                            prod.ProductUnitPrice,
                            prod.ProductSize,
                            supp.SupplierName,
                            prod.ProductUnitsInStock,
                            prod.DefectiveGood,
                            prod.ProductDisCount,
                            prod.InShop
                        }).ToList();
            if (id == 0)
            {
                if (cateSel != null)
                {
                    string[] textid = cateSel.Split(",");
                    int[] cateid = Array.ConvertAll<string, int>(textid, s => int.Parse(s));
                    var 所有商品2 = db.CategoryDetails
                        .Where(o => o.CategoryId == cateid[0] && o.Product.ProductUnitPrice > min && o.Product.ProductUnitPrice < max);
                    int count = 0, count2 = 0; //count2為產品數量
                    bool flag1 = true, flag2 = true, flag3 = true, flag4 = true;
                    foreach (var cid in cateid)
                    {
                        if (flag1 && (cid == 3 || cid == 4 || cid == 5 || cid == 7))
                        {
                            count2++;
                            flag1 = false;
                        }
                        if (flag2 && (cid == 20 || cid == 21))
                        {
                            count2++;
                            flag2 = false;
                        }
                        if (flag3 && (cid == 17 || cid == 18))
                        {
                            count2++;
                            flag3 = false;
                        }
                        if (flag4 && (cid == 12 || cid == 13 || cid == 14 || cid == 15 || cid == 23))
                        {
                            count2++;
                            flag4 = false;
                        }
                        count++;
                        if (count == 1) continue;
                        var 所有商品a = db.CategoryDetails
                        .Where(o => o.CategoryId == cid && o.Product.ProductUnitPrice > min && o.Product.ProductUnitPrice < max);
                        所有商品2 = 所有商品2.Union(所有商品a);
                    }
                    var 所有商品3 = 所有商品2.GroupBy(x => x.ProductId).Select(x => new { pid = x.Key, 數量 = x.Count() }).ToList();
                    foreach (var item in 所有商品3)
                    {
                        if (item.數量 >= count2)
                        {
                            var 所有商品4 = (from prod in db.Products
                                         join supp in db.Suppliers
                                        on prod.SupplierId equals supp.SupplierId
                                         orderby prod.ProductId
                                         where prod.ProductId == item.pid
                                         select new
                                         {
                                             prod.ProductId,
                                             prod.ProductName,
                                             prod.ProductUnitPrice,
                                             prod.ProductSize,
                                             supp.SupplierName,
                                             prod.DefectiveGood,
                                             prod.InShop,
                                             prod.ProductDisCount,
                                             prod.ProductUnitsInStock
                                         }).FirstOrDefault();
                            List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                            var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.pid);
                            var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.pid);
                            相片List.Add(封面相片);
                            所有商品列表.Add(new ShoppingListViewModel()
                            {
                                ProductId = 所有商品4.ProductId,
                                ProductName = 所有商品4.ProductName,
                                ProductUnitPrice = 所有商品4.ProductUnitPrice,
                                ProductSize = 所有商品4.ProductSize,
                                SupplierName = 所有商品4.SupplierName,
                                InShop = 所有商品4.InShop,
                                DefectiveGood = 所有商品4.DefectiveGood,
                                myFavorite = 最愛商品,
                                ProductDisCount = 所有商品4.ProductDisCount,
                                ProductUnitsInStock = 所有商品4.ProductUnitsInStock,
                                photoBank = 相片List
                            });
                        }
                    }
                    return PartialView("ProductSearchPartial", 所有商品列表);
                }
                else
                {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max 
                            orderby prod.ProductId
                            select new
                            {
                                prod.ProductId,
                                prod.ProductName,
                                prod.ProductUnitPrice,
                                prod.ProductSize,
                                supp.SupplierName,
                                prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
            }
            //最新商品
            if (id == 1)
            {
                if (cateSel != null)
                {
                    string[] textid = cateSel.Split(",");
                    int[] cateid = Array.ConvertAll<string, int>(textid, s => int.Parse(s));
                    var 所有商品2 = db.CategoryDetails
                        .Where(o => o.CategoryId == cateid[0] && o.Product.ProductUnitPrice > min && o.Product.ProductUnitPrice < max);
                    int count = 0, count2 = 0; //count2為產品數量
                    bool flag1 = true, flag2 = true, flag3 = true, flag4 = true;
                    foreach (var cid in cateid)
                    {
                        if (flag1 && (cid == 3 || cid == 4 || cid == 5 || cid == 7))
                        {
                            count2++;
                            flag1 = false;
                        }
                        if (flag2 && (cid == 20 || cid == 21))
                        {
                            count2++;
                            flag2 = false;
                        }
                        if (flag3 && (cid == 17 || cid == 18))
                        {
                            count2++;
                            flag3 = false;
                        }
                        if (flag4 && (cid == 12 || cid == 13 || cid == 14 || cid == 15 || cid == 23))
                        {
                            count2++;
                            flag4 = false;
                        }
                        count++;
                        if (count == 1) continue;
                        var 所有商品a = db.CategoryDetails
                        .Where(o => o.CategoryId == cid && o.Product.ProductUnitPrice > min && o.Product.ProductUnitPrice < max);
                        所有商品2 = 所有商品2.Union(所有商品a);
                    }
                    var 所有商品3 = 所有商品2.Include(a => a.Product).GroupBy(x =>new { x.ProductId ,x.Product.ProduceDate}).OrderByDescending(a => a.Key.ProduceDate).Select(x => new { pid = x.Key.ProductId, 數量 = x.Count() }).ToList();

                    foreach (var item in 所有商品3)
                    {
                        if (item.數量 >= count2)
                        {
                            var 所有商品4 = (from prod in db.Products
                                         join supp in db.Suppliers
                                        on prod.SupplierId equals supp.SupplierId
                                         orderby prod.ProduceDate descending
                                         where prod.ProductId == item.pid
                                         select new
                                         {
                                             prod.ProductId,
                                             prod.ProductName,
                                             prod.ProductUnitPrice,
                                             prod.ProductSize,
                                             supp.SupplierName,
                                             prod.DefectiveGood,
                                             prod.InShop,
                                             prod.ProductDisCount,
                                             prod.ProductUnitsInStock
                                         }).FirstOrDefault();
                            List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                            var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.pid);
                            var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.pid);
                            相片List.Add(封面相片);
                            所有商品列表.Add(new ShoppingListViewModel()
                            {
                                ProductId = 所有商品4.ProductId,
                                ProductName = 所有商品4.ProductName,
                                ProductUnitPrice = 所有商品4.ProductUnitPrice,
                                ProductSize = 所有商品4.ProductSize,
                                SupplierName = 所有商品4.SupplierName,
                                InShop = 所有商品4.InShop,
                                DefectiveGood = 所有商品4.DefectiveGood,
                                myFavorite = 最愛商品,
                                ProductDisCount = 所有商品4.ProductDisCount,
                                ProductUnitsInStock = 所有商品4.ProductUnitsInStock,
                                photoBank = 相片List
                            });
                        }
                    }
                    return PartialView("ProductSearchPartial", 所有商品列表);
                }
                else
                {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max 
                            orderby prod.ProduceDate descending
                            select new
                            {
                                prod.ProductId,
                                prod.ProductName,
                                prod.ProductUnitPrice,
                                prod.ProductSize,
                                supp.SupplierName,
                                prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
            }
            //價格由高至低
            if (id == 2)
            {
                if (cateSel != null)
                {
                    string[] textid = cateSel.Split(",");
                    int[] cateid = Array.ConvertAll<string, int>(textid, s => int.Parse(s));
                    var 所有商品2 = db.CategoryDetails
                        .Where(o => o.CategoryId == cateid[0] && o.Product.ProductUnitPrice > min && o.Product.ProductUnitPrice < max);
                    int count = 0, count2 = 0; //count2為產品數量
                    bool flag1 = true, flag2 = true, flag3 = true, flag4 = true;
                    foreach (var cid in cateid)
                    {
                        if (flag1 && (cid == 3 || cid == 4 || cid == 5 || cid == 7))
                        {
                            count2++;
                            flag1 = false;
                        }
                        if (flag2 && (cid == 20 || cid == 21))
                        {
                            count2++;
                            flag2 = false;
                        }
                        if (flag3 && (cid == 17 || cid == 18))
                        {
                            count2++;
                            flag3 = false;
                        }
                        if (flag4 && (cid == 12 || cid == 13 || cid == 14 || cid == 15 || cid == 23))
                        {
                            count2++;
                            flag4 = false;
                        }
                        count++;
                        if (count == 1) continue;
                        var 所有商品a = db.CategoryDetails
                        .Where(o => o.CategoryId == cid && o.Product.ProductUnitPrice > min && o.Product.ProductUnitPrice < max);
                        所有商品2 = 所有商品2.Union(所有商品a);
                    }
                    var 所有商品3 = 所有商品2.Include(a => a.Product).GroupBy(x => new { x.ProductId, x.Product.ProductUnitPrice }).OrderByDescending(a => a.Key.ProductUnitPrice).Select(x => new { pid = x.Key.ProductId, 數量 = x.Count() }).ToList();

                    foreach (var item in 所有商品3)
                    {
                        if (item.數量 >= count2)
                        {
                            var 所有商品4 = (from prod in db.Products
                                         join supp in db.Suppliers
                                        on prod.SupplierId equals supp.SupplierId
                                         where prod.ProductId == item.pid
                                         select new
                                         {
                                             prod.ProductId,
                                             prod.ProductName,
                                             prod.ProductUnitPrice,
                                             prod.ProductSize,
                                             supp.SupplierName,
                                             prod.DefectiveGood,
                                             prod.InShop,
                                             prod.ProductDisCount,
                                             prod.ProductUnitsInStock
                                         }).FirstOrDefault();
                            List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                            var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.pid);
                            var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.pid);
                            相片List.Add(封面相片);
                            所有商品列表.Add(new ShoppingListViewModel()
                            {
                                ProductId = 所有商品4.ProductId,
                                ProductName = 所有商品4.ProductName,
                                ProductUnitPrice = 所有商品4.ProductUnitPrice,
                                ProductSize = 所有商品4.ProductSize,
                                SupplierName = 所有商品4.SupplierName,
                                InShop = 所有商品4.InShop,
                                DefectiveGood = 所有商品4.DefectiveGood,
                                myFavorite = 最愛商品,
                                ProductDisCount = 所有商品4.ProductDisCount,
                                ProductUnitsInStock = 所有商品4.ProductUnitsInStock,
                                photoBank = 相片List
                            });
                        }
                    }
                    return PartialView("ProductSearchPartial", 所有商品列表);
                }
                else
                {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max 
                            orderby prod.ProductUnitPrice descending
                            select new
                            {
                                prod.ProductId,
                                prod.ProductName,
                                prod.ProductUnitPrice,
                                prod.ProductSize,
                                supp.SupplierName,
                                prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
            }
            //價格由低至高
            if (id == 3)
            {
                if (cateSel != null)
                {
                    string[] textid = cateSel.Split(",");
                    int[] cateid = Array.ConvertAll<string, int>(textid, s => int.Parse(s));
                    var 所有商品2 = db.CategoryDetails
                        .Where(o => o.CategoryId == cateid[0] && o.Product.ProductUnitPrice > min && o.Product.ProductUnitPrice < max);
                    int count = 0, count2 = 0; //count2為產品數量
                    bool flag1 = true, flag2 = true, flag3 = true, flag4 = true;
                    foreach (var cid in cateid)
                    {
                        if (flag1 && (cid == 3 || cid == 4 || cid == 5 || cid == 7))
                        {
                            count2++;
                            flag1 = false;
                        }
                        if (flag2 && (cid == 20 || cid == 21))
                        {
                            count2++;
                            flag2 = false;
                        }
                        if (flag3 && (cid == 17 || cid == 18))
                        {
                            count2++;
                            flag3 = false;
                        }
                        if (flag4 && (cid == 12 || cid == 13 || cid == 14 || cid == 15 || cid == 23))
                        {
                            count2++;
                            flag4 = false;
                        }
                        count++;
                        if (count == 1) continue;
                        var 所有商品a = db.CategoryDetails
                        .Where(o => o.CategoryId == cid && o.Product.ProductUnitPrice > min && o.Product.ProductUnitPrice < max);
                        所有商品2 = 所有商品2.Union(所有商品a);
                    }
                    var 所有商品3 = 所有商品2.Include(a => a.Product).GroupBy(x => new { x.ProductId, x.Product.ProductUnitPrice }).OrderBy(a => a.Key.ProductUnitPrice).Select(x => new { pid = x.Key.ProductId, 數量 = x.Count() }).ToList();

                    foreach (var item in 所有商品3)
                    {
                        if (item.數量 >= count2)
                        {
                            var 所有商品4 = (from prod in db.Products
                                         join supp in db.Suppliers
                                        on prod.SupplierId equals supp.SupplierId
                                         where prod.ProductId == item.pid
                                         select new
                                         {
                                             prod.ProductId,
                                             prod.ProductName,
                                             prod.ProductUnitPrice,
                                             prod.ProductSize,
                                             supp.SupplierName,
                                             prod.DefectiveGood,
                                             prod.InShop,
                                             prod.ProductDisCount,
                                             prod.ProductUnitsInStock
                                         }).FirstOrDefault();
                            List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                            var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.pid);
                            var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.pid);
                            相片List.Add(封面相片);
                            所有商品列表.Add(new ShoppingListViewModel()
                            {
                                ProductId = 所有商品4.ProductId,
                                ProductName = 所有商品4.ProductName,
                                ProductUnitPrice = 所有商品4.ProductUnitPrice,
                                ProductSize = 所有商品4.ProductSize,
                                SupplierName = 所有商品4.SupplierName,
                                InShop = 所有商品4.InShop,
                                DefectiveGood = 所有商品4.DefectiveGood,
                                myFavorite = 最愛商品,
                                ProductDisCount = 所有商品4.ProductDisCount,
                                ProductUnitsInStock = 所有商品4.ProductUnitsInStock,
                                photoBank = 相片List
                            });
                        }
                    }
                    return PartialView("ProductSearchPartial", 所有商品列表);
                }
                else
                {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max
                            orderby prod.ProductUnitPrice
                            select new
                            {
                                prod.ProductId,
                                prod.ProductName,
                                prod.ProductUnitPrice,
                                prod.ProductSize,
                                supp.SupplierName,
                                prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
            }
            //=============================
            foreach (var item in 所有商品)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.ProductId);
                var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.ProductId);
                相片List.Add(封面相片);
                所有商品列表.Add(new ShoppingListViewModel()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductUnitPrice = item.ProductUnitPrice,
                    ProductSize = item.ProductSize,
                    SupplierName = item.SupplierName,
                    InShop = item.InShop,
                    DefectiveGood = item.DefectiveGood,
                    myFavorite = 最愛商品,
                    ProductDisCount = item.ProductDisCount,
                    ProductUnitsInStock = item.ProductUnitsInStock,
                    photoBank = 相片List
                });
            }
            return PartialView("ProductSearchPartial", 所有商品列表);
        }
        /*
        public IActionResult OrderProductPartial(int id,int min, int max, int categetoryId)
        {
            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();

            var 所有商品 = (from prod in db.Products
                        join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                        orderby prod.ProductId
                        select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName,prod.ProductUnitsInStock,
                            prod.DefectiveGood,
                            prod.ProductDisCount,
                            prod.InShop
                        }).ToList();
            if (id == 0) {
                if (categetoryId == 0)
                {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max
                            orderby prod.ProductId
                            select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
                else
                {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            join c in db.CategoryDetails on prod.ProductId equals c.ProductId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max && c.CategoryId == categetoryId
                            orderby prod.ProductId
                            select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
            }
            //最新商品
            if (id == 1) {
                所有商品 = (from prod in db.Products
                        join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                        orderby prod.ProduceDate descending
                        select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                            prod.DefectiveGood,
                            prod.ProductDisCount,
                            prod.InShop
                        }).ToList();
                if (categetoryId == 0)
                {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max
                            orderby prod.ProduceDate descending
                            select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
                else {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            join c in db.CategoryDetails on prod.ProductId equals c.ProductId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max && c.CategoryId == categetoryId
                            orderby prod.ProduceDate descending
                            select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
            }
            //價格由高至低
            if (id == 2)
            {
                所有商品 = (from prod in db.Products
                        join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                        orderby prod.ProductUnitPrice descending
                        select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                            prod.DefectiveGood,
                            prod.ProductDisCount,
                            prod.InShop
                        }).ToList();
                if (categetoryId == 0)
                {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max
                            orderby prod.ProductUnitPrice descending
                            select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
                else
                {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            join c in db.CategoryDetails on prod.ProductId equals c.ProductId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max && c.CategoryId == categetoryId
                            orderby prod.ProductUnitPrice descending
                            select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
            }
            //價格由低至高
            if (id == 3)
            {
                所有商品 = (from prod in db.Products
                        join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                        orderby prod.ProductUnitPrice
                        select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                            prod.DefectiveGood,
                            prod.ProductDisCount,
                            prod.InShop
                        }).ToList();
                if (categetoryId == 0)
                {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max
                            orderby prod.ProductUnitPrice
                            select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
                else
                {
                    所有商品 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            join c in db.CategoryDetails on prod.ProductId equals c.ProductId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max && c.CategoryId == categetoryId
                            orderby prod.ProductUnitPrice
                            select new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.ProductDisCount,
                                prod.InShop
                            }).ToList();
                }
            }            
            if (id == 4)
            {
                var 銷售排行 = (from prod in db.Products
                        join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                        join od in db.OrderDetails on prod.ProductId equals od.ProductId
                       group new { prod, supp, od } by new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                           prod.DefectiveGood,
                           prod.InShop
                       }  into g
                       select new { g.Key.ProductId, g.Key.ProductName, g.Key.ProductUnitPrice, g.Key.ProductSize, g.Key.SupplierName,g.Key.ProductUnitsInStock,g.Key.InShop,g.Key.DefectiveGood, 銷售量=g.Sum(a=>a.od.UnitsPurchased)}).OrderByDescending(a=>a.銷售量).ToList();
                if (categetoryId == 0)
                {
                    銷售排行 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            join od in db.OrderDetails on prod.ProductId equals od.ProductId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max
                            group new { prod, supp, od } by new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.InShop
                            } into g
                            select new { g.Key.ProductId, g.Key.ProductName, g.Key.ProductUnitPrice, g.Key.ProductSize, g.Key.SupplierName,g.Key.ProductUnitsInStock,g.Key.InShop, g.Key.DefectiveGood, 銷售量 = g.Sum(a => a.od.UnitsPurchased) }).OrderByDescending(a => a.銷售量).ToList();
                }
                else
                {
                    銷售排行 = (from prod in db.Products
                            join supp in db.Suppliers on prod.SupplierId equals supp.SupplierId
                            join od in db.OrderDetails on prod.ProductId equals od.ProductId
                            join c in db.CategoryDetails on prod.ProductId equals c.ProductId
                            where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max && c.CategoryId == categetoryId
                            group new { prod, supp, od } by new { prod.ProductId, prod.ProductName, prod.ProductUnitPrice, prod.ProductSize, supp.SupplierName, prod.ProductUnitsInStock,
                                prod.DefectiveGood,
                                prod.InShop
                            } into g
                            select new { g.Key.ProductId, g.Key.ProductName, g.Key.ProductUnitPrice, g.Key.ProductSize, g.Key.SupplierName,g.Key.ProductUnitsInStock, g.Key.InShop, g.Key.DefectiveGood, 銷售量 = g.Sum(a => a.od.UnitsPurchased) }).OrderByDescending(a => a.銷售量).ToList();
                }
                //=============================
                foreach (var item in 銷售排行)
                {
                    List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                    var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.ProductId);
                    var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.ProductId);
                    相片List.Add(封面相片);
                    所有商品列表.Add(new ShoppingListViewModel()
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductUnitPrice = item.ProductUnitPrice,
                        ProductSize = item.ProductSize,
                        InShop = item.InShop,
                        DefectiveGood = item.DefectiveGood,
                        SupplierName = item.SupplierName,
                        ProductUnitsInStock=item.ProductUnitsInStock,
                        myFavorite = 最愛商品,
                        photoBank = 相片List
                    });
                }
                return PartialView("ProductSearchPartial", 所有商品列表);
               
            }
            //=============================
            foreach (var item in 所有商品)
            {
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
                var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.ProductId);
                var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.ProductId);
                相片List.Add(封面相片);
                所有商品列表.Add(new ShoppingListViewModel()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductUnitPrice = item.ProductUnitPrice,
                    ProductSize = item.ProductSize,
                    SupplierName = item.SupplierName,
                    InShop = item.InShop,
                    DefectiveGood = item.DefectiveGood,
                    myFavorite = 最愛商品,
                    ProductUnitsInStock=item.ProductUnitsInStock,
                    photoBank = 相片List
                });
            }
            return PartialView("ProductSearchPartial", 所有商品列表);
        }
        */
        public IActionResult ListAddMyFavorite(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
                MyFavorite myFavorite = new MyFavorite()
                {
                    MemberId = UserLogin.member.MemberId,
                    ProductId = id
                };
                db.Add(myFavorite);
                db.SaveChanges();
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return Content("0");
            }
            return Content("1");
        }
        public IActionResult ListRemoveMyFavorite(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
                var 移除我的最愛 = db.MyFavorites.Remove(db.MyFavorites.Where(a => a.MemberId == UserLogin.member.MemberId && a.ProductId == id).FirstOrDefault());
                db.SaveChanges();
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return Content("0");
            }
            return Content("1");
        }
        #endregion
        public IActionResult ProductName() {
            var 所有商品 = (from p in db.Products orderby p.ProductName select p.ProductName).Distinct().ToList();
            return Json(所有商品);
        }

        public IActionResult ShopDetail(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
            }
            else if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_SUPPLIER)) //Seesion有找到
            {
                ViewBag.SUPP = UserLogin.supplier.SupplierName;
                ViewBag.userID = UserLogin.supplier.SupplierAccount;
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
            }
            ShoppingListViewModel 單筆商品 = new ShoppingListViewModel();
            var 商品明細 = (from p in db.Products
                       join s in db.Suppliers
                       on p.SupplierId equals s.SupplierId
                       where p.ProductId == id
                       select new { p, s }).FirstOrDefault();
            if (商品明細 == null)
                return RedirectToAction("List");
            //db = new 鮮蔬果季Context();
            var 封面相片 = db.ProductPhotoBanks.Where(p => p.ProductId ==id).ToList();
            var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == id); /*TODO 目前會員ID寫死的*/
            var 商品出售數量 = (from p in db.OrderDetails
                         where p.ProductId == id
                         group p by p.ProductId into g
                         select  g.Sum(p => p.UnitsPurchased)).FirstOrDefault();
            var 列出評論 = (from p in db.Reviews
                       join a in db.OrderDetails
                       on p.OrderDetailId equals a.OrderDetailId
                       join b in db.Orders
                       on a.OrderId equals b.OrderId
                       where a.ProductId == id
                       select new { p, b.MemberId }).ToList();
            var 分類明細 = (from cd in db.CategoryDetails
                        join c in db.Categories
                        on cd.CategoryId equals c.CategoryId
                       where cd.ProductId == id
                       orderby cd.CategoryId
                       select new { cd.CategoryId,c.CategoryName}
                       ).ToList();
            單筆商品.product = 商品明細.p;
            單筆商品.supplier = 商品明細.s;
            foreach (var 照片 in 封面相片)
                單筆商品.photoBank.Add(照片);
            單筆商品.出售量 = 商品出售數量;
            單筆商品.myFavorite = 最愛商品;
            foreach (var 分類 in 分類明細)
                單筆商品.categoryDetails.Add(new CCategoryDetailName { CategoryId=分類.CategoryId,CategoryName=分類.CategoryName});
            foreach (var item in 列出評論)
            {
                var 會員資訊 = (from x in db.Members
                            where x.MemberId == item.MemberId &&
                            x.BlackList == "No"
                            select x).FirstOrDefault();
                if(會員資訊==null)
                {
                    continue;
                }
                單筆商品.review.Add(item.p);
                單筆商品.Member.Add(會員資訊);
            }
            return View(單筆商品);
        }
       
        public IActionResult DetailAddMyFavorite(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
                MyFavorite myFavorite = new MyFavorite()
                {
                    MemberId = UserLogin.member.MemberId,
                    ProductId = id
                };
                db.Add(myFavorite);
                db.SaveChanges();
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
            return RedirectToAction("ShopDetail", new { id });
        }
        public IActionResult Cart()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
                //=============================
                List<ShoppingListViewModel> 購物車商品列表 = new List<ShoppingListViewModel>();

                var 購物車商品 = (from pro in db.Products
                             join item in db.ShoppingCarts
                             on pro.ProductId equals item.ProductId
                             join sup in db.Suppliers
                             on pro.SupplierId equals sup.SupplierId
                             join stat in db.Statuses
                             on item.StatusId equals stat.StatusId
                             where item.MemberId == UserLogin.member.MemberId && stat.StatusId == 1
                             select new { item, pro, stat, sup }).ToList();
                var 總價 = 0;
                foreach (var c in 購物車商品)
                {
                    總價 += c.pro.ProductUnitPrice * c.item.UnitsInCart;
                    var 封面相片 = db.ProductPhotoBanks.Where(p => p.ProductId == c.pro.ProductId).FirstOrDefault();
                    購物車商品列表.Add(new ShoppingListViewModel()
                    {
                        shopCart = c.item,
                        product = c.pro,
                        photoforCart = 封面相片,
                        status = c.stat,
                        supplier = c.sup
                        ////單筆訂單細項總價 = 訂單細項總價
                    });
                }
                 
                var couponsHad = (from p in db.Coupons
                                  join cd in db.CouponDetails
                                  on p.CouponId equals cd.CouponId
                                  where cd.CouponQuantity >= 0 &&
                                  cd.CouponId != 0 &&
                                  cd.MemberId == UserLogin.member.MemberId &&
                                  p.DiscountCondition <= 總價 &&
                                  p.CouponEndDate > DateTime.Now
                                  select new { p, cd }).ToList();

                List<CouponsListViewModel> list = new List<CouponsListViewModel>();
                foreach (var item in couponsHad)
                {
                    list.Add(new CouponsListViewModel()
                    {
                        coupon = item.p,
                        couponDetail = item.cd,
                    });
                }
                ViewBag.Coupons = list;
                return View(購物車商品列表);
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult CartPartial()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
                //=============================
                List<ShoppingListViewModel> 購物車商品列表 = new List<ShoppingListViewModel>();

                var 購物車商品 = (from pro in db.Products
                             join item in db.ShoppingCarts
                             on pro.ProductId equals item.ProductId
                             join sup in db.Suppliers
                             on pro.SupplierId equals sup.SupplierId
                             join stat in db.Statuses
                             on item.StatusId equals stat.StatusId
                             where item.MemberId == UserLogin.member.MemberId && stat.StatusId == 1
                             select new { item, pro, stat, sup }).ToList();
                var 總價 = 0;
                foreach (var c in 購物車商品)
                {
                    總價 += c.pro.ProductUnitPrice * c.item.UnitsInCart;
                    var 封面相片 = db.ProductPhotoBanks.Where(p => p.ProductId == c.pro.ProductId).FirstOrDefault();
                    購物車商品列表.Add(new ShoppingListViewModel()
                    {
                        shopCart = c.item,
                        product = c.pro,
                        photoforCart = 封面相片,
                        status = c.stat,
                        supplier = c.sup
                        ////單筆訂單細項總價 = 訂單細項總價
                    });
                }

                var couponsHad = (from p in db.Coupons
                                  join cd in db.CouponDetails
                                  on p.CouponId equals cd.CouponId
                                  where cd.CouponQuantity >= 0 &&
                                  cd.CouponId != 0 &&
                                  cd.MemberId == UserLogin.member.MemberId &&
                                  p.DiscountCondition <= 總價 &&
                                  p.CouponEndDate > DateTime.Now
                                  select new { p, cd }).ToList();

                List<CouponsListViewModel> list = new List<CouponsListViewModel>();
                foreach (var item in couponsHad)
                {
                    list.Add(new CouponsListViewModel()
                    {
                        coupon = item.p,
                        couponDetail = item.cd,
                    });
                }
                ViewBag.Coupons = list;
                return PartialView(購物車商品列表);
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult ListAddToCart(int id,int count)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                var 購物車內商品 = (from p in db.Products
                              join c in db.ShoppingCarts
                              on p.ProductId equals c.ProductId
                              where p.ProductId == id && c.StatusId == 1 && c.MemberId==UserLogin.member.MemberId
                              select new { p, c }).FirstOrDefault();

                if (購物車內商品 == null)
                {
                    ShoppingCart myCartitem = new ShoppingCart()
                    {
                        MemberId = UserLogin.member.MemberId,
                        ProductId = id,
                        UnitsInCart = count,
                        StatusId = 1
                    };
                    db.Add(myCartitem);
                    db.SaveChanges();
                }
                else
                {
                    ShoppingCart myCartitem = db.ShoppingCarts.FirstOrDefault(i => i.ProductId == id&& i.StatusId==1&& i.MemberId==UserLogin.member.MemberId);
                    //myCartitem.MemberId = UserLogin.member.MemberId;
                    //myCartitem.ProductId = id;
                    myCartitem.UnitsInCart = 購物車內商品.c.UnitsInCart + count;
                    //myCartitem.StatusId = 1;
                    db.SaveChanges();
                }
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return Content("0");
            }
            var 購物車品量 = (from c in db.ShoppingCarts        
                         where c.StatusId ==1&& c.MemberId == UserLogin.member.MemberId
                         select c).Count();
            return Content(購物車品量.ToString());
        }

        public IActionResult CartAddToNavCart(int id, int count)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                var 購物車內商品 = (from p in db.Products
                              join c in db.ShoppingCarts
                              on p.ProductId equals c.ProductId
                              where p.ProductId == id && c.StatusId == 1 && c.MemberId == UserLogin.member.MemberId
                              select new { p, c }).FirstOrDefault();

                if (購物車內商品 == null)
                {
                    ShoppingCart myCartitem = new ShoppingCart()
                    {
                        MemberId = UserLogin.member.MemberId,
                        ProductId = id,
                        UnitsInCart = count,
                        StatusId = 1
                    };
                    db.Add(myCartitem);
                    db.SaveChanges();
                }
                else
                {
                    ShoppingCart myCartitem = db.ShoppingCarts.FirstOrDefault(i => i.ProductId == id && i.StatusId == 1 && i.MemberId == UserLogin.member.MemberId);
                    //myCartitem.MemberId = UserLogin.member.MemberId;
                    //myCartitem.ProductId = id;
                    myCartitem.UnitsInCart = count;
                    //myCartitem.StatusId = 1;
                    db.SaveChanges();
                }
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return Content("0");
            }
            var 購物車品量 = (from c in db.ShoppingCarts
                         where c.StatusId == 1 && c.MemberId == UserLogin.member.MemberId
                         select c).Count();
            return Content(購物車品量.ToString());
        }


        public IActionResult CartNum()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
            }
            var 購物車數量 = (from c in db.ShoppingCarts
                         join stat in db.Statuses
                         on c.StatusId equals stat.StatusId
                         where c.StatusId ==1 &&  c.MemberId == UserLogin.member.MemberId
                         select c).Count();
            return Content(購物車數量.ToString());
        }

        public IActionResult NavCart()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
            }
            List <ShoppingListViewModel> 購物車商品列表 = new List<ShoppingListViewModel>();
            var 購物車商品 = (from pro in db.Products
                         join item in db.ShoppingCarts
                         on pro.ProductId equals item.ProductId
                         //join stat in db.Statuses
                         //on item.StatusId equals stat.StatusId
                         join sup in db.Suppliers
                         on pro.SupplierId equals sup.SupplierId
                         where item.MemberId == UserLogin.member.MemberId && item.StatusId == 1
                         select new { item, pro, /*stat,*/ sup }).ToList();

            foreach (var c in 購物車商品)
            {
                var 封面相片 = db.ProductPhotoBanks.Where(p => p.ProductId == c.pro.ProductId).FirstOrDefault();
                購物車商品列表.Add(new ShoppingListViewModel()
                {
                    shopCart = c.item,
                    product = c.pro,
                    photoforCart = 封面相片,
                    //status = c.stat,
                    supplier = c.sup
                    ////單筆訂單細項總價 = 訂單細項總價
                });
            }          
            return PartialView(購物車商品列表);
        }

        public IActionResult Checkout(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                List<ShoppingListViewModel> 購物車商品列表 = new List<ShoppingListViewModel>();
                var 購物車商品 = (from pro in db.Products
                             join item in db.ShoppingCarts
                             on pro.ProductId equals item.ProductId
                             join stat in db.Statuses
                             on item.StatusId equals stat.StatusId
                             join sup in db.Suppliers
                             on pro.SupplierId equals sup.SupplierId
                             where item.MemberId == UserLogin.member.MemberId && stat.StatusId == 1
                             //where item.MemberId ==19 && stat.StatusId == 1
                             select new { item, pro, stat, sup }).ToList();

                foreach (var i in 購物車商品)
                {
                    購物車商品列表.Add(new ShoppingListViewModel()
                    {
                        shopCart = i.item,
                        product = i.pro,
                        status = i.stat,
                        supplier = i.sup
                    });
                }
                ViewBag.cpd = (from cp in db.Coupons
                               where cp.CouponId == id
                               select cp.CouponDiscount).FirstOrDefault();
                ViewBag.cpid = id;
                CPID.couponid = id;

                return View(購物車商品列表);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        #region -- <方法> SHA256 編碼 --
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        #endregion

        #region -- <方法> 隨機產生四碼英(小寫)數混雜編碼 --
        public static string GetRandomString4()
        {
            var str = "0123456789abcdefghijklmnopqrstuvwxyz";
            var next = new Random();
            var builder = new StringBuilder();
            for (var i = 0; i < 4; i++)
            {
                builder.Append(str[next.Next(0, str.Length)]);
            }
            return builder.ToString();
        }
        #endregion

        public IActionResult Checkout2Partial(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;

                var 購物車商品 = (from pro in db.Products
                             join item in db.ShoppingCarts
                             on pro.ProductId equals item.ProductId
                             join stat in db.Statuses
                             on item.StatusId equals stat.StatusId
                             where item.MemberId == UserLogin.member.MemberId && stat.StatusId == 1
                             //where item.MemberId == 19 && stat.StatusId == 1
                             select new { item, pro, stat }).ToList();
                //抓酷碰折扣
                CPID.couponid = id;
                var discount = (from cp in db.Coupons
                                where cp.CouponId == id
                                select cp.CouponDiscount).FirstOrDefault();
                int totalAmount = 0;
                string itemName = "";

                foreach (var i in 購物車商品)
                {
                    totalAmount += (i.pro.ProductUnitPrice * i.item.UnitsInCart);
                    itemName += i.pro.ProductName + " " + i.item.UnitsInCart + "xNT$ " + i.pro.ProductUnitPrice + "#";
                }
                //itemName = itemName.Substring(0, itemName.Length - 1);
                totalAmount = totalAmount - discount;
                itemName = itemName + "折扣-NT$" + discount;
                DateTime date = DateTime.Now;
                string random4 = GetRandomString4();
                string tradeNo = "DX" + DateTime.Now.ToString("yyyyMMddHHmmss") + random4;
                string backtoUrl = "https://vegetable132web-vegetable132staging.azurewebsites.net/Order/Orders";
                string dateString = date.ToString("yyyy/MM/dd HH:mm:ss");
                string checkMac = "HashKey=5294y06JbISpM5x9&ChoosePayment=Credit&ClientBackURL=" + backtoUrl + "&CreditInstallment=&EncryptType=1&InstallmentAmount=&ItemName=" + itemName + "&MerchantID=2000132&MerchantTradeDate=" + dateString + "&MerchantTradeNo=" + tradeNo + "&PaymentType=aio&Redeem=&ReturnURL=https://developers.opay.tw/AioMock/MerchantReturnUrl" + "&StoreID=&TotalAmount=" + totalAmount + "&TradeDesc=建立信用卡測試訂單&HashIV=v77hoKGq4kWxNNIS";
                string checkMac2 = HttpUtility.UrlEncode(checkMac, System.Text.Encoding.UTF8);
                string checkMac3 = checkMac2.ToLower();
                //string checkMac4 = SHA256Encrypt(checkMac3).ToUpper();
                SHA256 sha256 = SHA256.Create();
                string checkMac4 = GetHash(sha256, checkMac3).ToUpper();
                Cashier 支付資訊 = new Cashier()
                {
                    TotalAmount = totalAmount,
                    ItemName = itemName,
                    TradeNo = tradeNo,
                    BacktoUrl = backtoUrl,
                    Date = dateString,
                    CheckMac = checkMac4
                };
                return PartialView(支付資訊);
            }            
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult CheckAddOrder(string address, int paymentMethod)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
            }
            ViewBag.USER = UserLogin.member.MemberName;

            var 結帳區商品商品 = (from items in db.ShoppingCarts
                          where items.MemberId == UserLogin.member.MemberId && items.StatusId == 1
                          select items).ToList();

            if (結帳區商品商品 != null)
            {
                if(CPID.couponid!=0)
                {
                    Order newOrder = new Order()
                    {
                        MemberId = UserLogin.member.MemberId,
                        OrderDate = DateTime.Now,
                        ShippedTo = address,
                        StatusId = 4,
                        PayMethodId = paymentMethod,
                        CouponId = CPID.couponid
                    };
                    db.Add(newOrder);
                }
                else
                {
                    Order newOrder = new Order()
                    {
                        MemberId = UserLogin.member.MemberId,
                        OrderDate = DateTime.Now,
                        ShippedTo = address,
                        StatusId = 4,
                        PayMethodId = paymentMethod,                       
                    };
                    db.Add(newOrder);
                }
                            
                db.SaveChanges();

                var latestOrder = (from i in db.Orders
                                   orderby i.OrderId descending
                                   select i.OrderId).FirstOrDefault();
                OrderDetail OD = null;
                foreach (var i in 結帳區商品商品)
                {
                    OD = new OrderDetail
                    {
                        OrderId = latestOrder,
                        ProductId = i.ProductId,
                        UnitsPurchased = i.UnitsInCart,
                        HaveReviews = false
                    };
                    db.OrderDetails.Add(OD);
                    var units = (from p in db.Products
                                 where p.ProductId == i.ProductId
                                 select p).FirstOrDefault();
                    units.ProductUnitsInStock -= i.UnitsInCart;
                }
                db.SaveChanges();

                foreach (var j in 結帳區商品商品)
                {
                    j.StatusId = 3;
                }
                db.SaveChanges();               
                
                CouponDetail couponDetail = (from cpd in db.CouponDetails
                                             where cpd.MemberId == UserLogin.member.MemberId &&
                                             cpd.CouponId == CPID.couponid
                                             select cpd).FirstOrDefault();
                if(couponDetail!=null)
                {
                    couponDetail.CouponQuantity = couponDetail.CouponQuantity - 1;
                }                
                db.SaveChanges();
            }

            return Content("1");

        }

        public IActionResult Check2AddOrder(string address, int paymentMethod)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
            }
            ViewBag.USER = UserLogin.member.MemberName;

            var 結帳區商品商品 = (from items in db.ShoppingCarts
                           where items.MemberId == UserLogin.member.MemberId && items.StatusId == 1
                           select items).ToList();

            if (結帳區商品商品 != null)
            {
                Order newOrder = new Order()
                {
                    MemberId = UserLogin.member.MemberId,
                    OrderDate = DateTime.Now,
                    ShippedTo = address,
                    StatusId = 4,
                    PayMethodId = paymentMethod,
                    CouponId = CPID.couponid
                };
                db.Add(newOrder);
                db.SaveChanges();

                var latestOrder = (from i in db.Orders
                                   orderby i.OrderId descending
                                   select i.OrderId).FirstOrDefault();
                OrderDetail OD = null;
                foreach (var i in 結帳區商品商品)
                {
                    OD = new OrderDetail
                    {
                        OrderId = latestOrder,
                        ProductId = i.ProductId,
                        UnitsPurchased = i.UnitsInCart,
                        HaveReviews = false
                    };
                    db.OrderDetails.Add(OD);
                    var units = (from p in db.Products
                                 where p.ProductId == i.ProductId
                                 select p).FirstOrDefault();
                    units.ProductUnitsInStock -= i.UnitsInCart;
                }
                db.SaveChanges();

                foreach (var j in 結帳區商品商品)
                {
                    j.StatusId = 3;
                }
                db.SaveChanges();

                CouponDetail couponDetail = (from cpd in db.CouponDetails
                                             where cpd.MemberId == UserLogin.member.MemberId &&
                                             cpd.CouponId == CPID.couponid
                                             select cpd).FirstOrDefault();
                couponDetail.CouponQuantity = couponDetail.CouponQuantity - 1;
                db.SaveChanges();
            }

            return Content("1");

        }

        public IActionResult CartCpPartial(int ttcart)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                Member user = JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                UserLogin.member = user;
                ViewBag.USER = UserLogin.member.MemberName;
                ViewBag.userID = UserLogin.member.MemberId;
                //=============================
                var couponsHad = (from p in db.Coupons
                                  join cd in db.CouponDetails
                                  on p.CouponId equals cd.CouponId
                                  where cd.CouponQuantity >= 0 &&
                                  cd.CouponId != 0 &&
                                  cd.MemberId == UserLogin.member.MemberId &&
                                  p.DiscountCondition <= ttcart &&
                                  p.CouponEndDate >DateTime.Now
                                  select new { p, cd }).ToList();
                List<CouponsListViewModel> list = new List<CouponsListViewModel>();
                foreach (var item in couponsHad)
                {
                    list.Add(new CouponsListViewModel()
                    {
                        coupon = item.p,
                        couponDetail = item.cd
                    });
                }
                ViewBag.Coupons = list;
                return PartialView();
            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
        }
    }
}

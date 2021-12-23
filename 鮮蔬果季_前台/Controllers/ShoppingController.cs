using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult List(CSelectViewModel select)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
                ViewBag.USER = UserLogin.member.MemberName;
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
                       select new { prod, supp }).ToList();
            //if (select.SelectOrderBy == 1)/*最新商品*/
            //{
            //    所有產品 = from prod in db.Products
            //           join supp in db.Suppliers
            //           on prod.SupplierId equals supp.SupplierId
            //           orderby prod.ProduceDate descending
            //           select new { prod, supp };
            //    if (!string.IsNullOrEmpty(select.txtKeyword))
            //        所有產品 = from prod in db.Products
            //               join supp in db.Suppliers
            //               on prod.SupplierId equals supp.SupplierId
            //               where prod.ProductName.Contains(@select.txtKeyword)
            //               orderby prod.ProduceDate descending
            //               select new { prod, supp };
            //    ViewBag.Select = 1;
            //}
            //else if (select.SelectOrderBy == 2) /*價格高到低*/
            //{
            //    所有產品 = from prod in db.Products
            //           join supp in db.Suppliers
            //           on prod.SupplierId equals supp.SupplierId
            //           orderby prod.ProductUnitPrice descending
            //           select new { prod, supp };
            //    if (!string.IsNullOrEmpty(select.txtKeyword))
            //        所有產品 = from prod in db.Products
            //               join supp in db.Suppliers
            //               on prod.SupplierId equals supp.SupplierId
            //               where prod.ProductName.Contains(@select.txtKeyword)
            //               orderby prod.ProductUnitPrice descending
            //               select new { prod, supp };
            //    ViewBag.Select = 2;
            //}
            //else if (select.SelectOrderBy == 3)/*價格低到高*/
            //{
            //    所有產品 = from prod in db.Products
            //           join supp in db.Suppliers
            //           on prod.SupplierId equals supp.SupplierId
            //           orderby prod.ProductUnitPrice
            //           select new { prod, supp };
            //    if (!string.IsNullOrEmpty(select.txtKeyword))
            //        所有產品 = from prod in db.Products
            //               join supp in db.Suppliers
            //               on prod.SupplierId equals supp.SupplierId
            //               where prod.ProductName.Contains(@select.txtKeyword)
            //               orderby prod.ProductUnitPrice
            //               select new { prod, supp };
            //    ViewBag.Select = 3;
            //}
            //else if (!string.IsNullOrEmpty(select.txtKeyword)) /*沒選排序直接搜尋*/
            //{
            //    所有產品 = from prod in db.Products
            //           join supp in db.Suppliers
            //           on prod.SupplierId equals supp.SupplierId
            //           where prod.ProductName.Contains(@select.txtKeyword)
            //           select new { prod, supp };
            //}
            //db = new 鮮蔬果季Context();
            foreach (var item in 所有產品)
            {            
                List<ProductPhotoBank> 相片List = new List<ProductPhotoBank>();
               var 封面相片 = db.ProductPhotoBanks.FirstOrDefault(p => p.ProductId == item.prod.ProductId);
                相片List.Add(封面相片);
                var 欲加購物車商品 = db.ShoppingCarts.FirstOrDefault(c => c.MemberId == UserLogin.member.MemberId && c.ProductId == item.prod.ProductId);
                var 最愛商品 = db.MyFavorites.FirstOrDefault(f => f.MemberId == UserLogin.member.MemberId && f.ProductId == item.prod.ProductId); /*TODO 目前會員ID寫死的*/
                所有商品列表.Add(new ShoppingListViewModel()
                {
                    product=item.prod,
                    supplier = item.supp,
                    photoBank= 相片List,
                    myFavorite= 最愛商品,
                    shopCart = 欲加購物車商品
                }) ; 
            }

            var 商品主類別 = db.Categories.Where(c => !c.CategoryName.Contains("活動類") && c.FatherCategoryId == null).OrderByDescending(c => c.CategoryId).ToList();
            var 商品次類別 = db.Categories.Where(c => c.FatherCategoryId != 8).ToList();
            var 商品次類別2 = db.Categories.Where(c => c.FatherCategoryId != 8).ToList();
            var 商品分類明細 = (from p in db.CategoryDetails
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


            return View(所有商品列表);
        }

        public IActionResult CategoryPartial(int id)
        {
            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            var 所有商品 = (from prod in db.Products
                        join supp in db.Suppliers
                       on prod.SupplierId equals supp.SupplierId
                        join c in db.CategoryDetails
                        on prod.ProductId equals c.ProductId
                        where c.CategoryId == id
                        select new
                        {
                            c.CategoryId,
                            prod.ProductId,
                            prod.ProductName,
                            prod.ProductUnitPrice,
                            prod.ProductSize,
                            supp.SupplierName
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
                    myFavorite = 最愛商品,
                    photoBank = 相片List,
                    CategoryId=item.CategoryId
                });
            }
            return PartialView(所有商品列表);
        }
        public IActionResult PricePartial(int min,int max,int categetoryId)
        {
            if (categetoryId == 0)
            {
                List<ShoppingListViewModel> 商品列表 = new List<ShoppingListViewModel>();
                var 所有商品2 = (from prod in db.Products
                             join supp in db.Suppliers
                            on prod.SupplierId equals supp.SupplierId
                             where prod.ProductUnitPrice > min && prod.ProductUnitPrice < max
                             select new
                             {
                                 prod.ProductId,
                                 prod.ProductName,
                                 prod.ProductUnitPrice,
                                 prod.ProductSize,
                                 supp.SupplierName
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
                        myFavorite = 最愛商品,
                        photoBank = 相片List
                    });
                }
                return PartialView(商品列表);
            }

            List<ShoppingListViewModel> 所有商品列表 = new List<ShoppingListViewModel>();
            var 所有商品 = (from prod in db.Products
                        join supp in db.Suppliers
                       on prod.SupplierId equals supp.SupplierId
                        join c in db.CategoryDetails
                        on prod.ProductId equals c.ProductId
                        where c.CategoryId == categetoryId && prod.ProductUnitPrice > min && prod.ProductUnitPrice < max
                        select new
                        {
                            c.CategoryId,
                            prod.ProductId,
                            prod.ProductName,
                            prod.ProductUnitPrice,
                            prod.ProductSize,
                            supp.SupplierName
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
                    myFavorite = 最愛商品,
                    photoBank = 相片List,
                    CategoryId = item.CategoryId
                });
            }
            return PartialView(所有商品列表);
        }
        public IActionResult ShopDetail(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
                ViewBag.USER = UserLogin.member.MemberName;
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
            單筆商品.product = 商品明細.p;
            單筆商品.supplier = 商品明細.s;
            foreach (var 照片 in 封面相片)
                單筆商品.photoBank.Add(照片);
            單筆商品.出售量 = 商品出售數量;
            單筆商品.myFavorite = 最愛商品;

            foreach (var item in 列出評論)
            {
                var 會員資訊 = (from x in db.Members
                            where x.MemberId == item.MemberId
                            select x).FirstOrDefault();
                單筆商品.review.Add(item.p);
                單筆商品.Member.Add(會員資訊);
            }
            return View(單筆商品);
        }
        public IActionResult ListAddMyFavorite(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                MyFavorite myFavorite = new MyFavorite()
                {
                    MemberId=UserLogin.member.MemberId,
                    ProductId=id
                };
                db.Add(myFavorite);
                db.SaveChanges();
            }               
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login","Login");
            }
            return RedirectToAction("List");
        }
        public IActionResult DetailAddMyFavorite(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;

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
            //if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            //{
            //    ViewBag.USER = UserLogin.member.MemberName;
                //=============================
                List<ShoppingListViewModel> 購物車商品列表 = new List<ShoppingListViewModel>();
                
                var 購物車商品 = (from pro in db.Products
                              join item in db.ShoppingCarts
                              on pro.ProductId equals item.ProductId
                              join stat in db.Statuses
                              on item.StatusId equals stat.StatusId
                             //where item.MemberId == UserLogin.member.MemberId && stat.StatusId == 1
                             where item.MemberId == 19
                              select new { item, pro, stat }).ToList();

                foreach (var c in 購物車商品)
                {
                    var 封面相片 = db.ProductPhotoBanks.Where(p => p.ProductId == c.pro.ProductId).FirstOrDefault();
                    購物車商品列表.Add(new ShoppingListViewModel()
                    {
                        shopCart = c.item,
                        product = c.pro,
                        photoforCart = 封面相片,
                        status = c.stat
                        ////單筆訂單細項總價 = 訂單細項總價
                    });
                }
                return View(購物車商品列表);
            //}
            //else //Seesion沒找到
            //{
            //    ViewBag.USER = null;
            //    UserLogin.member = null;
            //    return RedirectToAction("Login", "Login");
            //}
        }

        public IActionResult ListAddToCart(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                var 購物車內商品 = (from p in db.Products
                            join c in db.ShoppingCarts
                            on p.ProductId equals c.ProductId
                            where p.ProductId == id
                            select new { p, c }).FirstOrDefault();

                if(購物車內商品 == null)
                {
                    ShoppingCart myCartitem = new ShoppingCart()
                    {
                        MemberId = UserLogin.member.MemberId,
                        ProductId = id,
                        UnitsInCart = 1,
                        StatusId = 1
                    };
                    db.Add(myCartitem);
                    db.SaveChanges();
                }
                else
                {
                    ShoppingCart myCartitem = db.ShoppingCarts.FirstOrDefault(i => i.ProductId == id);                    
                    myCartitem.MemberId = UserLogin.member.MemberId;
                    myCartitem.ProductId = id;
                    myCartitem.UnitsInCart = 購物車內商品.c.UnitsInCart + 1;
                    myCartitem.StatusId = 1;
                    db.SaveChanges();
                }

            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
            return RedirectToAction("List");
        }
        public IActionResult DetailAddToCart(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //Seesion有找到
            {
                ViewBag.USER = UserLogin.member.MemberName;
                var 購物車內商品 = (from p in db.Products
                              join c in db.ShoppingCarts
                              on p.ProductId equals c.ProductId
                              where p.ProductId == id
                              select new { p, c }).FirstOrDefault();

                if (購物車內商品 == null)
                {
                    ShoppingCart myCartitem = new ShoppingCart()
                    {
                        MemberId = UserLogin.member.MemberId,
                        ProductId = id,
                        UnitsInCart = 1,
                        StatusId = 1
                    };
                    db.Add(myCartitem);
                    db.SaveChanges();
                }
                else
                {
                    ShoppingCart myCartitem = db.ShoppingCarts.FirstOrDefault(i => i.ProductId == id);
                    myCartitem.MemberId = UserLogin.member.MemberId;
                    myCartitem.ProductId = id;
                    myCartitem.UnitsInCart = 購物車內商品.c.UnitsInCart + 1;
                    myCartitem.StatusId = 1;
                    db.SaveChanges();
                }

            }
            else //Seesion沒找到
            {
                ViewBag.USER = null;
                UserLogin.member = null;
                return RedirectToAction("Login", "Login");
            }
            return RedirectToAction("ShopDetail", new { id });
        }
        public IActionResult Checkout()
        {
            return View();           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace 鮮蔬果季_前台.Models
{
    /* //使用方式:
                 var 商品分類明細 = (from p in (new 鮮蔬果季Context()).CategoryDetails
                          group p by p.CategoryId into g
                          select new { CategoryId=g.Key, Total = g.Count(p => p.CategoryId == g.Key) });
            List<C商品各類別總數> 分類list = new List<C商品各類別總數>();
            foreach (var 分類 in 商品分類明細) {
                分類list.Add(new C商品各類別總數() { 
                      分類ID=分類.CategoryId,
                      總數=分類.Total
                });
            }
     */
    public class C商品各類別總數
    {
        public  int 分類ID { get; set; }

        public  int 總數 { get; set; }
    }
}

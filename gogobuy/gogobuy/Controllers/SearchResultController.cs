using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gogobuy.Models;

namespace gogobuy.Controllers
{
    public class SearchResultController : Controller
    {
        // GET: SearchResult
        public ActionResult WishSearchResult(string fCategory)
        {
            IEnumerable<tProduct> table = null;
            string keyword = Request.Form["txtKeyword"];
            if (string.IsNullOrEmpty(keyword))
            {
                table = from p in (new gogobuydbEntities()).tProduct
                        select p;
            }
            else
            {
                table = from p in (new gogobuydbEntities()).tProduct
                        where p.fProductName.Contains(keyword)
                        select p;
            }

            if (!string.IsNullOrEmpty(fCategory))
            {
                table = from p in (new gogobuydbEntities()).tProduct
                        where p.fCategory == fCategory
                        select p;
            }

            if (!string.IsNullOrEmpty(fCategory) && !string.IsNullOrEmpty(keyword))
            {
                table = from p in (new gogobuydbEntities()).tProduct
                        where (p.fCategory == fCategory
                        && p.fProductName.Contains(keyword))
                        select p;
            }

            return View(table);
        }
        public ActionResult SellSearchResult(string fCategory)
        {

            IEnumerable<tProduct> table = null;
            string keyword = Request.Form["txtKeyword"];

            //int txtPrice1 = int.Parse(Request.Form["txtPrice1"]);
            //int txtPrice2 = int.Parse(Request.Form["txtPrice2"]);

            //string txtPrice1 = Request.Form["txtPrice1"];
            //string txtPrice2 = Request.Form["txtPrice2"];

            if (string.IsNullOrEmpty(keyword))
            {
                table = from p in (new gogobuydbEntities()).tProduct
                        
                        select p;
            }
            else  
            {
                table = from p in (new gogobuydbEntities()).tProduct
                        where p.fProductName.Contains(keyword)
                        select p;
            }
            if (!string.IsNullOrEmpty(fCategory))
            {
                table = from p in (new gogobuydbEntities()).tProduct
                        where p.fCategory == fCategory
                        select p;
            }

            if (!string.IsNullOrEmpty(fCategory) && !string.IsNullOrEmpty(keyword))
            {
                table = from p in (new gogobuydbEntities()).tProduct
                        where (p.fCategory == fCategory 
                        && p.fProductName.Contains(keyword))
                        select p;
            }




            //if (!string.IsNullOrEmpty(txtPrice1) && !string.IsNullOrEmpty(keyword))

            //{
            //    table = from p in (new gogobuydbEntities()).tProduct
            //            where p.fPrice >= Decimal.Parse(txtPrice1)
            //            select p;
            //}

            //if (!string.IsNullOrEmpty(txtPrice2))
            //{
            //    table = from p in (new gogobuydbEntities()).tProduct
            //            where p.fPrice <= int.Parse(txtPrice2)
            //            select p;
            //}


            //else
            //{
            //    table = from p in (new gogobuydbEntities()).tProduct
            //            where p.fCategory.Contains(keyword)
            //            select p;
            //}

            return View(table);
        }


        //public ActionResult SellSearchResult()
        //{

        //    CProductFactory x = (new CProductFactory()).queryByFCategory(fCategory);
        //    if (x == null)
        //        return RedirectToAction("List");
        //    return View(x);


        //    List<CProductFactory> list = null;
        //    IEnumerable<tProduct> table = null;
            
        //    return View(table);
        //}


        public ActionResult 手機周邊()
        {

            IEnumerable<tProduct> table = null;
            string keyword = Request.Form["txtKeyword"];
            if (string.IsNullOrEmpty(keyword))
            {
                table = from p in (new gogobuydbEntities()).tProduct
                        where p.fCategory.Contains("手機周邊")
                        select p;
            }
            else
            {
                table = from p in (new gogobuydbEntities()).tProduct
                        where p.fProductName.Contains(keyword) &&
                        p.fCategory.Contains("手機周邊")
                        select p;
            }


            return View(table);
        }

        public ActionResult 三C產品()
        {

            IEnumerable<tProduct> table = null;

            table = from p in (new gogobuydbEntities()).tProduct
                    where p.fCategory.Contains("3C產品")
                    select p;


            return View(table);
        }

        public ActionResult 生活居家()
        {

            IEnumerable<tProduct> table = null;

            table = from p in (new gogobuydbEntities()).tProduct
                    where p.fCategory.Contains("生活居家")
                    select p;


            return View(table);
        }



        
        //public ActionResult 時間篩選()
        //{

        //    IEnumerable<tProduct> table = null;

        //    table = from p in (new gogobuydbEntities()).tProduct
        //            where p.fUpdateTime >= choosedate1 && p.fUpdateTime <= choosedate2
        //            select p;


        //    return View(table);
        //}

        //public ActionResult 價格篩選()
        //{

        //    IEnumerable<tProduct> table = null;

        //    table = from p in (new gogobuydbEntities()).tProduct
        //            where p.fPrice >= chooseprice1 && p.fUpdateTime <= chooseprice2
        //            select p;


        //    return View(table);
        //}
    }
}
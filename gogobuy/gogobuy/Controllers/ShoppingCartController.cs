using System.Web.Mvc;
using System.Linq;
using System.Data.SqlClient;
using gogobuy.Models;
using gogobuy.ViewModels;
using System.Collections.Generic;
using System;

namespace gogobuy.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult DeleteAll()
        {
            // 驗證使用者有無登入
            if(Session[CDictionary.SK_LOGINED_USER_ID] == null)
                    return Json("fail");
            try
                {
                gogobuydbEntities db = new gogobuydbEntities();
                // 抓取使用者ID
                int memberID = (int)Session[CDictionary.SK_LOGINED_USER_ID];
                // 清空裡面有使用者ID的購物車
                var shopData = db.tShopping.Where(s => s.fMemberID == memberID).ToList();
                if (shopData.Count > 0)
                {
                    db.tShopping.RemoveRange(shopData);
                    db.SaveChanges();
                    return Json("success");
                }
                return Json("noData");
            }catch
            {
                return Json("fail");
            }
            
        }
        public ActionResult Delete(int id)
        {
            gogobuydbEntities shopitem = new gogobuydbEntities();
            tShopping items = shopitem.tShopping.FirstOrDefault(p => p.fCartID == id);
            if (items != null)
            {
                shopitem.tShopping.Remove(items);
                shopitem.SaveChanges();
            }
            return RedirectToAction("ShoppingCart");
        }

        gogobuydbEntities db = new gogobuydbEntities();

        public ActionResult ShoppingCart()
        {
            if (Session[CDictionary.SK_LOGINED_USER_ID] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int memberId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            //var cartitem = db.tShopping.OrderByDescending(m => m.fCartID).ToList();

            var shoptable = from s in db.tShopping
                            join p in db.tProduct on s.fProductID equals p.fProductID
                            where s.fMemberID == memberId
                            select new { s, p.fProductName, p.fImgPath, p.fPrice, p.fDescription };
            List<CartViewModel> cartItem = new List<CartViewModel>();
            foreach (var item in shoptable)
            {
                cartItem.Add(new CartViewModel
                {
                    fCartID = item.s.fCartID,
                    fMemberID = item.s.fMemberID,
                    fProductID = item.s.fProductID,
                    fDescription = item.fDescription,
                    fQuantity = item.s.fQuantity,
                    fImgPath = item.fImgPath,
                    fProductName = item.fProductName,
                    fPrice = item.fPrice
                });
            }
            return View(cartItem);
        }

        public ActionResult CheckOut()
        {
            if (Session[CDictionary.SK_LOGINED_USER_ID] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int memberId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var user = db.tMembership.Where(m => m.fMemberID == memberId).FirstOrDefault();
            ViewBag.name = user.fFirstName + user.fLastName;
            ViewBag.phone = user.fPhone;
            var shoptable = from s in db.tShopping
                            join p in db.tProduct on s.fProductID equals p.fProductID
                            where s.fMemberID == memberId
                            select new { s, p.fProductName, p.fImgPath, p.fPrice, p.fDescription };
            List<CartViewModel> cartItem = new List<CartViewModel>();
            foreach (var item in shoptable)
            {
                cartItem.Add(new CartViewModel
                {
                    fCartID = item.s.fCartID,
                    fMemberID = item.s.fMemberID,
                    fProductID = item.s.fProductID,
                    fDescription = item.fDescription,
                    fQuantity = item.s.fQuantity,
                    fImgPath = item.fImgPath,
                    fProductName = item.fProductName,
                    fPrice = item.fPrice
                });
            }
            return View(cartItem);
        }
        public ActionResult CheckoutComplete()
        {
            return View();
        }
    }
}
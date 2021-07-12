using gogobuy.Models;
using gogobuy.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gogobuy.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        #region 帳戶修改
        public ActionResult EditProfile()
        {
            if(Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            gogobuydbEntities db = new gogobuydbEntities();
            string email = Session[CDictionary.SK_LOGINED_USER_EMAIL].ToString();
            tMembership member = db.tMembership.SingleOrDefault(m => m.fEmail == email);

            return View(member);
        }

        [HttpPost]
        public ActionResult EditProfile(tMembership member)
        {
            if(Session[CDictionary.SK_LOGINED_USER_ID] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            gogobuydbEntities db = new gogobuydbEntities();
            int memberId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            tMembership sent = db.tMembership.SingleOrDefault(s => s.fMemberID == memberId);
            if (sent != null)
            {
                sent.fFirstName = member.fFirstName;
                sent.fPhone = member.fPhone;
                sent.fDateOfBirth = member.fDateOfBirth;
                sent.fEmail = member.fEmail;
                sent.fAddress = member.fAddress;
                db.SaveChanges();
            }
            return View(member);
        }
        #endregion
        public ActionResult MemberCenter()
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        #region 購買查詢
        public ActionResult OrderlistBuy()
        {
            //--------------------------------------------
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            gogobuydbEntities db = new gogobuydbEntities();

            List<OrderlistBuyViewModel> 這是lstVMOrderlistBuy = new List<OrderlistBuyViewModel>();
            int memberId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var mytOrderList = db.tOrder.Where(m  =>  m.fBuyerID  == memberId).ToList();
            
            //遍歷每張訂單 
            foreach (var everyOrder in mytOrderList)
            {
                OrderlistBuyViewModel temp = new OrderlistBuyViewModel();
                temp.fOrderUUID = everyOrder.fOrderUUID;
                temp.fOrderDate = everyOrder.fOrderDate;
                temp.fOrderStatus = everyOrder.fOrderStatus;
                temp.fPrice = everyOrder.fPrice;
                temp.fOrderPayWay = everyOrder.fOrderPayWay;

                //抓取每張Order裡的Detail集合
                var OrderDetailsList = db.tOrderDetails.Where(m => m.fOrderID == everyOrder.fOrderID).ToList();
                temp.lsttOrderDetailslist = OrderDetailsList;

                List<tProduct> ProductList = new List<tProduct>();
                foreach (var everyOrderdetail in OrderDetailsList)
                {
                    var myProduct = db.tProduct.Where(m => m.fProductID == everyOrderdetail.fProductID).FirstOrDefault();
                    ProductList.Add(myProduct);
                }
                temp.lsttProductlist = ProductList;

                //var mytOrderdetailList = db.tOrderDetails.Where(m => m.fOrderID == everyOrder.fOrderID)
                //    .Join(db.tProduct, c => c.fProductID, s => s.fProductID, (c, s) => new
                //    {
                //        fProductID = s.fProductID
                //    })
                //    .ToList();
                //temp.lsttOrderlist = mytOrderdetailList;
                這是lstVMOrderlistBuy.Add(temp);
            }

            return View(這是lstVMOrderlistBuy);
        }
        #endregion
        public ActionResult OrderlistSell()
        {
            //--------------------------------------------
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            gogobuydbEntities db = new gogobuydbEntities();

            List<OrderlistSellViewModel> 這是lstVMOrderlistSell = new List<OrderlistSellViewModel>();
            int memberId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var mytOrderList = db.tOrder.Where(m => m.fSellerID == memberId).ToList();

            //遍歷每張訂單 
            foreach (var everyOrder in mytOrderList)
            {
                OrderlistSellViewModel temp = new OrderlistSellViewModel();
                temp.fOrderUUID = everyOrder.fOrderUUID;
                temp.fOrderDate = everyOrder.fOrderDate;
                temp.fOrderStatus = everyOrder.fOrderStatus;
                temp.fPrice = everyOrder.fPrice;
                temp.fOrderPayWay = everyOrder.fOrderPayWay;

                //抓取每張Order裡的Detail集合
                var OrderDetailsList = db.tOrderDetails.Where(m => m.fOrderID == everyOrder.fOrderID).ToList();
                temp.lsttOrderDetailslist = OrderDetailsList;

                List<tProduct> ProductList = new List<tProduct>();
                foreach (var everyOrderdetail in OrderDetailsList)
                {
                    var myProduct = db.tProduct.Where(m => m.fProductID == everyOrderdetail.fProductID).FirstOrDefault();
                    ProductList.Add(myProduct);
                }
                temp.lsttProductlist = ProductList;

                //var mytOrderdetailList = db.tOrderDetails.Where(m => m.fOrderID == everyOrder.fOrderID)
                //    .Join(db.tProduct, c => c.fProductID, s => s.fProductID, (c, s) => new
                //    {
                //        fProductID = s.fProductID
                //    })
                //    .ToList();
                //temp.lsttOrderlist = mytOrderdetailList;
                這是lstVMOrderlistSell.Add(temp);
            }

            return View(這是lstVMOrderlistSell);
        }
        public ActionResult SwitchToBuyAndShop()
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        #region 密碼修改
        public ActionResult UpdatePassword()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePassword(tMembership update)
        {
            if (Session[CDictionary.SK_LOGINED_USER_ID] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            gogobuydbEntities db = new gogobuydbEntities();
            string old = Request.Form["old"];
            int memberId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            tMembership sent = db.tMembership.SingleOrDefault(s => s.fMemberID == memberId);
            if (!Account.IsPasswordCorrect(old,sent))
            {
                ViewBag.msg = "舊密碼錯誤";
                return View();
            }
            if (Request.Form["newpassword"]!= Request.Form["check"])
            {
                ViewBag.msg = "兩次密碼輸入不一樣";
                return View();
            }
            sent.fPassword = Account.HashPassword(Request.Form["check"], sent.fSalt);
            db.SaveChanges();
            ViewBag.msg = "密碼修改成功";
            return View();
        }
        #endregion
    }
}
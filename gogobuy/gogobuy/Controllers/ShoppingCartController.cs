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
        public string GetSerialNumber()
        {


            string headDate = DateTime.Now.ToString("yyyyMMdd");
            Random crantom = new Random();
            string lastnum = crantom.Next(0000, 9999).ToString();
            string x = headDate + lastnum;
            return x;

        }
        public ActionResult AddOrder()
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null || Session[CDictionary.SK_LOGINED_USER_ID] == null)
            {

                return RedirectToAction("Login", "Home");
            }
            int memberId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var user = db.tMembership.Where(m => m.fMemberID == memberId).FirstOrDefault();
            ViewBag.name = user.fFirstName + user.fLastName;
            ViewBag.phone = user.fPhone;


            string address = Request.Form["Address"];
            string price = Request.Form["Price"];
            string payway = Request.Form["Payway"];
            string datetime = DateTime.Now.ToString("yyyyMMdd");
            string phone = ViewBag.phone;
            string buyername = ViewBag.name;
            int buyerID = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            string orderstate = "已成立";
            string orderuuID = GetSerialNumber();


            string sql = "insert into tOrder (fOrderAddress,fPrice,fOrderDate,fOrderPayWay,fOrderPhone,fBuyerName,fBuyerID,fOrderStatus,fOrderUUID) values(@K_FADDRESS,@K_FPRICE,@K_FORDERDATE,@K_FPAYWAY,@K_ORDERFPHONE,@K_FBUYERNAME,@K_FBUYERID,@K_FORDERSTATUS,@K_FORDERUUID)";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_FADDRESS", (object)address));
            paras.Add(new SqlParameter("K_FPRICE", (object)price));
            paras.Add(new SqlParameter("K_FORDERDATE", (object)datetime));
            paras.Add(new SqlParameter("K_FPAYWAY", (object)payway));
            paras.Add(new SqlParameter("K_ORDERFPHONE", (object)phone));
            paras.Add(new SqlParameter("K_FBUYERNAME", (object)buyername));
            paras.Add(new SqlParameter("K_FBUYERID", (object)buyerID));
            paras.Add(new SqlParameter("K_FORDERSTATUS", (object)orderstate));
            paras.Add(new SqlParameter("K_FORDERUUID", (object)orderuuID));

            SqlConnection con = new SqlConnection();
            con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

            con.Open();

            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                    cmd.Parameters.Add(p);
            }
            cmd.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("CheckoutComplete", new { orderuuID = orderuuID });
        }
        public ActionResult DeleteAll()
        {
            // 驗證使用者有無登入
            if (Session[CDictionary.SK_LOGINED_USER_ID] == null)
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
            }
            catch
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
        public ActionResult AddToCart(int productID)
        {
            if (Session[CDictionary.SK_LOGINED_USER_ID] == null)
            {
                return Json("login");
            }
            int memberId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var cart = db.tShopping.Where(s => s.fMemberID == memberId && s.fProductID == productID);
            if (cart.Count() > 0)
            {
                cart.FirstOrDefault().fQuantity += 1;
                db.SaveChanges();
                return Json("success");
            }
            if (cart.Count() == 0)
            {
                var newCart = new tShopping()
                {
                    fMemberID = memberId,
                    fProductID = productID,
                    fQuantity = 1
                };

                db.tShopping.Add(newCart);
                db.SaveChanges();
                return Json("success");
            }
            return Json("fail");
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
        public ActionResult CheckoutComplete(string orderuuID)
        {
            if (Session[CDictionary.SK_LOGINED_USER_ID] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            //IEnumerable<tOrder> table = null;
            //string orderuuID = GetSerialNumber();
            //table = from p in (new gogobuydbEntities()).tOrder
            //        where p.fOrderUUID == orderuuID
            //        select p;

            var uuis = db.tOrder.Where(p => p.fOrderUUID == orderuuID).Select(p => p.fOrderUUID).FirstOrDefault();
            ViewBag.uuid = uuis;

            
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
    }
}
using gogobuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gogobuy.Controllers
{
    public class ChatroomController : Controller
    {
        // GET: Chatroom
        public ActionResult Chatroom()
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            // 獲取user資料
            string email = Session[CDictionary.SK_LOGINED_USER_EMAIL].ToString();
            var db = new gogobuydbEntities();
            tMembership user = db.tMembership.SingleOrDefault(m => m.fEmail == email);
            ViewBag.UserName = user.fFirstName + user.fLastName;
            return View();
        }



        public ActionResult Update()
        {
            return View();
        }
    }

    
}
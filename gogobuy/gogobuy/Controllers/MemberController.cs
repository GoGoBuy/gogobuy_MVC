using gogobuy.Models;
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
            return View();
        }
        public ActionResult OrderlistBuy()
        {
            return View();
        }
        public ActionResult OrderlistSell()
        {
            return View();
        }
        public ActionResult SwitchToBuyAndShop()
        {
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
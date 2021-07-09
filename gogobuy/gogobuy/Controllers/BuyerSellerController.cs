using gogobuy.Models;
using System.Web.Mvc;

namespace gogobuy.Controllers
{
    public class BuyerSellerController : Controller
    {
        // GET: BuyerSeller
        public ActionResult BuyerUpload()
        {
            return View();
        }
        public ActionResult SellerUpload()
        {
            return View();
        }
        public ActionResult BuyerManagement()
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult SellerManagement()
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult BucketListDetails()
        {
            return View();
        }
        public ActionResult SellListDetails()
        {
            return View();
        }
    }
}
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
            return View();
        }
        public ActionResult SellerManagement()
        {
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
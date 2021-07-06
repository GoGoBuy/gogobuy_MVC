using System.Web.Mvc;

namespace gogobuy.Controllers
{
    public class DetailsController : Controller
    {
        // GET: Details
        public ActionResult ProductDetails()
        {
            return View();
        }
        public ActionResult BucketListDetails()
        {
            return View();
        }
    }
}
using System.Web.Mvc;

namespace gogobuy.Controllers
{
    public class SearchResultController : Controller
    {
        // GET: SearchResult
        public ActionResult WishSearchResult()
        {
            return View();
        }
        public ActionResult SellSearchResult()
        {
            return View();
        }
    }
}
using System.Web.Mvc;

namespace gogobuy.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult ShoppingCart()
        {
            return View();
        }

        public ActionResult CheckOut()
        {
            return View();
        }
        public ActionResult CheckoutComplete()
        {
            return View();
        }
    }
}
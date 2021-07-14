using gogobuy.Models;
using gogobuy.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace gogobuy.Controllers
{
    public class CollectController : Controller
    {
        // GET: Collect
        public ActionResult Collection()
        {
            if (Session[CDictionary.SK_LOGINED_USER_ID] == null)
                return RedirectToAction("Login", "Home");
            
            var db = new gogobuydbEntities();
            int id = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var queryCollections = db.tCollection.Where(c => c.fMemberID == id)
                .Join(db.tProduct,
                c => c.fProductID,
                p => p.fProductID,
                (c, p) => new 
                {
                    c.fCollectID,
                    c.fProductID,
                    p.fProductName,
                    p.fImgPath,
                    p.fPrice
                })
                .OrderByDescending(c => c.fCollectID);
            List <CollectViewModel> collections = new List<CollectViewModel>();
            foreach (var collection in queryCollections)
            {
                collections.Add(new CollectViewModel { fProductID = collection.fProductID,fProductName = collection.fProductName, 
                    fImgPath = collection.fImgPath, fPrice = collection.fPrice});
            }
            return View(collections);
        }

        public ActionResult Delete(int productID)
        {
            if (Session[CDictionary.SK_LOGINED_USER_ID] == null)
                return Json("login");
            int userID = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var db = new gogobuydbEntities();
            tCollection collection = db.tCollection.FirstOrDefault(c => c.fProductID == productID && c.fMemberID == userID);
            if (collection != null)
            {
                db.tCollection.Remove(collection);
                db.SaveChanges();
                return Json("success");
            }
            return Json("fail");
        }

        public ActionResult Likeitem(int productID) {
            // 抓取使用者有無登入
            if (Session[CDictionary.SK_LOGINED_USER_ID] == null)
                return Json("login");

            int userID = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var db = new gogobuydbEntities();
            var collection = new tCollection
            {
                fMemberID = userID,
                fProductID = productID
            };
            db.tCollection.Add(collection);
            db.SaveChanges();

            return Json("success");
        }
    }
}
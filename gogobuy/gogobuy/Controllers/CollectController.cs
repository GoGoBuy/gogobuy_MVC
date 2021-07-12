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
                    p.fProductName,
                    p.fImgPath,
                    p.fPrice
                })
                .OrderByDescending(c => c.fCollectID);
            List <CollectViewModel> collections = new List<CollectViewModel>();
            foreach (var collection in queryCollections)
            {
                collections.Add(new CollectViewModel { fCollectID = collection.fCollectID, fProductName = collection.fProductName, 
                    fImgPath = collection.fImgPath, fPrice = collection.fPrice});
            }
            return View(collections);
        }

        public ActionResult Delete(int collectID)
        {
            var db = new gogobuydbEntities();
            tCollection collection = db.tCollection.FirstOrDefault(c => c.fCollectID == collectID);
            if (collection != null)
            {
                db.tCollection.Remove(collection);
                db.SaveChanges();
                return Json("success");
            }
            return Json("fail");
        }

        public ActionResult Likeitem(int productID) {
            if (Session[CDictionary.SK_LOGINED_USER_ID] == null)
                return Json("fail");

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
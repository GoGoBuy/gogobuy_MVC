using gogobuy.Models;
using gogobuy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace gogobuy.Controllers
{
    public class BuyerSellerController : Controller
    {
        #region 商品上架
        public ActionResult BuyerUpload()
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }
        [HttpPost]
        public ActionResult BuyerUpload(UploadViewModel product)
        {

            if (!ModelState.IsValid)
                return View(product);


            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            string[] tempPhotoName = new string[product.photo.Count];

            for (int i = 0; i < product.photo.Count; i++)
            {
                string savePath = Server.MapPath("~/Content/img/Product/");
                string phtoName = Guid.NewGuid().ToString() + ".jpg";
                tempPhotoName[i] = phtoName;
                string saveResult = savePath + phtoName;
                product.photo[i].SaveAs(saveResult);
            }

            using (var db = new gogobuydbEntities()) {

                var wishProd = new tProduct()
                {
                    fMemberID = userId,
                    fCategory = product.fCategory,
                    fDescription = product.fDescription,
                    fPrice = product.fPrice,
                    fQuantity = product.fQuantity,
                    fProductName = product.fProductName,
                    fProductLocation = product.fProductLocation,
                    fImgPath = tempPhotoName[0],
                    fIsWish = true,
                    fUpdateTime = DateTime.Now
                };
                db.tProduct.Add(wishProd);
                db.SaveChanges();
                
                int productID = wishProd.fProductID;
                for(int i = 0; i < tempPhotoName.Length; i++) {
                    bool isCover = false;
                    if (i == 0)
                        isCover = true;
                    var wishPhoto = new tProductImage()
                    {
                        fProductID = productID,
                        fCover = isCover,
                        fImgPath = tempPhotoName[i]
                    };
                    db.tProductImage.Add(wishPhoto);
                }
                db.SaveChanges();

            }
            return RedirectToAction("BuyerManagement");
        }
        #endregion
        #region 許願商品上架
        public ActionResult SellerUpload()
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }
        [HttpPost]
        public ActionResult SellerUpload(UploadViewModel product)
        {
            if (!ModelState.IsValid)
                return View(product);


            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            string[] tempPhotoName = new string[product.photo.Count];

            for (int i = 0; i < product.photo.Count; i++)
            {
                string savePath = Server.MapPath("~/Content/img/Product/");
                string phtoName = Guid.NewGuid().ToString() + ".jpg";
                tempPhotoName[i] = phtoName;
                string saveResult = savePath + phtoName;
                product.photo[i].SaveAs(saveResult);
            }

            using (var db = new gogobuydbEntities())
            {

                var wishProd = new tProduct()
                {
                    fMemberID = userId,
                    fCategory = product.fCategory,
                    fDescription = product.fDescription,
                    fPrice = product.fPrice,
                    fQuantity = product.fQuantity,
                    fProductName = product.fProductName,
                    fProductLocation = product.fProductLocation,
                    fImgPath = tempPhotoName[0],
                    fIsWish = false,
                    fUpdateTime = DateTime.Now
                };
                db.tProduct.Add(wishProd);
                db.SaveChanges();

                int productID = wishProd.fProductID;
                for (int i = 0; i < tempPhotoName.Length; i++)
                {
                    bool isCover = false;
                    if (i == 0)
                        isCover = true;
                    var wishPhoto = new tProductImage()
                    {
                        fProductID = productID,
                        fCover = isCover,
                        fImgPath = tempPhotoName[i]
                    };
                    db.tProductImage.Add(wishPhoto);
                }
                db.SaveChanges();

            }
            return RedirectToAction("SellerManagement");
        }
        #endregion
        public ActionResult BuyerManagement()
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var db = new gogobuydbEntities();
            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var wishProd = from w in db.tProduct
                           where w.fIsWish == true && w.fMemberID == userId
                           select w;
            List<ProductViewModel> list = new List<ProductViewModel>();
            foreach(var wishProds in wishProd)
            {
                list.Add(new ProductViewModel { fProductID = wishProds.fProductID, fProductName = wishProds.fProductName,
                fImgPath = wishProds.fImgPath, fPrice = wishProds.fPrice, fQuantity = wishProds.fQuantity, fUpdateTime = wishProds.fUpdateTime});
            }
            return View(list);
        }
        public ActionResult SellerManagement()
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var db = new gogobuydbEntities();
            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var wishProd = from w in db.tProduct
                           where w.fIsWish == false && w.fMemberID == userId
                           select w;
            List<ProductViewModel> list = new List<ProductViewModel>();
            foreach (var wishProds in wishProd)
            {
                list.Add(new ProductViewModel
                {
                    fProductID = wishProds.fProductID,
                    fProductName = wishProds.fProductName,
                    fImgPath = wishProds.fImgPath,
                    fPrice = wishProds.fPrice,
                    fQuantity = wishProds.fQuantity,
                    fUpdateTime = wishProds.fUpdateTime
                });
            }
            return View(list);
        }
        public ActionResult BucketListDetails()
        {
            return View();
        }
        public ActionResult SellListDetails()
        {
            return View();
        }
        public ActionResult ProductList(string category)
        {
            int userId = -1;
            
            if (Session[CDictionary.SK_LOGINED_USER_ID] != null)
                userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            List<ProductViewModel> products = new List<ProductViewModel>();
            var db = new gogobuydbEntities();
            IEnumerable<tProduct> table;

            if (string.IsNullOrEmpty(category))
            {
                table = db.tProduct.Where(p => p.fIsWish == false)
                .Select(p => p);
            } else
            {
                table = db.tProduct.Where(p => p.fIsWish == false && p.fCategory == category)
                .Select(p => p);
            }

            foreach (var p in table)
            {
                bool isLike = false;
                if (userId != -1)
                {
                    var collection = db.tCollection.FirstOrDefault(c => c.fMemberID == userId && c.fProductID == p.fProductID);
                    if (collection != null)
                        isLike = true;
                }
                products.Add(new ProductViewModel
                {
                    fProductID = p.fProductID,
                    fProductName = p.fProductName,
                    fImgPath = p.fImgPath,
                    fPrice = p.fPrice,
                    fProductLocation = p.fProductLocation,
                    isLike = isLike
                });
            }
            return View(products);
        }
        public ActionResult WishProductList(string category)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            var db = new gogobuydbEntities();
            IEnumerable<tProduct> table;
            if (string.IsNullOrEmpty(category))
            {
                table = db.tProduct.Where(p => p.fIsWish == true)
                .Select(p => p);
            }
            else
            {
                table = db.tProduct.Where(p => p.fIsWish == true && p.fCategory == category)
                .Select(p => p);
            }
            foreach (var p in table)
            {
                products.Add(new ProductViewModel
                {
                    fProductID = p.fProductID,
                    fProductName = p.fProductName,
                    fImgPath = p.fImgPath,
                    fPrice = p.fPrice,
                    fProductLocation = p.fProductLocation,
                });
            }
            return View(products);
        }
    }
}
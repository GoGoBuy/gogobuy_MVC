using gogobuy.Models;
using gogobuy.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace gogobuy.Controllers
{
    public class DetailsController : Controller
    {
        // GET: Details
        public ActionResult ProductDetails(int fProductID)
        {
            int userId = -1;
            // 預設使用者未收藏商品
            bool isLike = false;
            // 抓取user是否登入
            if (Session[CDictionary.SK_LOGINED_USER_ID] != null)
                userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            //Create a ViewModel and New it here when you want to use more than one table
            ProductDetailsViewModel pdViewModel = new ProductDetailsViewModel();

            //Use entity framework to import db which could use in C#
            gogobuydbEntities db = new gogobuydbEntities();

            var tP = db.tProduct.Where(t => t.fProductID == fProductID).FirstOrDefault();
            var tPDetails = db.tProductDetails.Where(t => t.fProductID == fProductID).FirstOrDefault();
            var tPImage = db.tProductImage.Where(t => t.fProductID == fProductID);
            var tMemberShip = db.tMembership.Where(t => t.fMemberID == tP.fMemberID).FirstOrDefault();
            // 若是有登入查看使用者是否收藏商品
            if (userId != -1)
            {
                var collection = db.tCollection.FirstOrDefault(c => c.fMemberID == userId && c.fProductID == fProductID);
                if (collection != null)
                    isLike = true;
            }
            //FirstOrDefault: to ensure that obtaining only "one" piece of data,
            //                if there's no data, default would be set

            //Add column of table into ViewModel
            pdViewModel.fCategory = tP.fCategory;
            pdViewModel.fProductName = tP.fProductName;
            pdViewModel.fPrice = tP.fPrice;
            pdViewModel.fDescription = tP.fDescription;
            pdViewModel.fArrivalTime = tP.fArrivalTime;
            pdViewModel.fUpdateTime = tP.fUpdateTime;
            pdViewModel.fProductLocation = tP.fProductLocation;
            pdViewModel.fQuantity = tP.fQuantity;
            pdViewModel.fImgPath = db.tProductImage.Where(p => p.fCover == true && p.fProductID == fProductID).Select(p => p.fImgPath).FirstOrDefault();
            //pdViewModel.fImgPath = db.tProductImage.Where(p => p.fCover == true && p.fProductID==fProductID).Select(p => new { p.fImgPath }).FirstOrDefault().ToString();

            pdViewModel.fProductID = tP.fProductID;
            pdViewModel.fMemberID = tP.fMemberID;

            pdViewModel.fFirstName = tMemberShip.fFirstName;
            pdViewModel.fLastName = tMemberShip.fLastName;
            pdViewModel.isLike = isLike;

            foreach (var f in tPImage)
            {
                pdViewModel.listImgPath.Add(f.fImgPath);
            }

            return View(pdViewModel);
        }


        public ActionResult BucketListDetails(int fProductID)
        {
            ProductDetailsViewModel pdViewModel = new ProductDetailsViewModel();

            gogobuydbEntities db = new gogobuydbEntities();

            var tProduct = db.tProduct.Where(p => p.fProductID == fProductID).FirstOrDefault();
            var tPMemberShip = db.tMembership.Where(p => p.fMemberID == tProduct.fMemberID).FirstOrDefault();
            var tPDetails = db.tProductDetails.Where(p => p.fProductID == fProductID);
            var tPImages = db.tProductImage.Where(p => p.fProductID == fProductID).OrderBy(p => p.fImgPath);

            pdViewModel.fProductID = tProduct.fProductID;
            pdViewModel.fProductName = tProduct.fProductName;
            pdViewModel.fPrice = tProduct.fPrice;
            pdViewModel.fMemberID = tProduct.fMemberID;
            pdViewModel.fProductLocation = tProduct.fProductLocation;
            pdViewModel.fQuantity = tProduct.fQuantity;
            pdViewModel.fUpdateTime = tProduct.fUpdateTime;
            pdViewModel.fArrivalTime = tProduct.fArrivalTime;
            pdViewModel.fCategory = tProduct.fCategory;
            pdViewModel.fDescription = tProduct.fDescription;

            pdViewModel.fFirstName = tPMemberShip.fFirstName;
            pdViewModel.fLastName = tPMemberShip.fLastName;

            //pdViewModel.fPname = tPDetails.fPName;

            pdViewModel.fImgPath = db.tProductImage.Where(p => p.fProductID == fProductID && p.fCover == true).Select(p => p.fImgPath).FirstOrDefault();
            pdViewModel.fPname = db.tProductDetails.Where(p => p.fProductID == fProductID).Select(p => p.fPName).FirstOrDefault();

            foreach (var f in tPImages)
            {
                pdViewModel.listImgPath.Add(f.fImgPath);
            }

            foreach (var f in tPDetails)
            {
                pdViewModel.listStyle.Add(f.fPName);
            }

            return View(pdViewModel);
        }
    }
}
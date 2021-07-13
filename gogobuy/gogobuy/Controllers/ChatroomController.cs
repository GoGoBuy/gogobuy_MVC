using gogobuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using gogobuy.ViewModels;



namespace gogobuy.Controllers
{
    public class ChatroomController : Controller
    {
        // GET: Chatroom
        public ActionResult Chatroom(int? fMemberID)
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            // 獲取user資料


            string email = Session[CDictionary.SK_LOGINED_USER_EMAIL].ToString();
            var db = new gogobuydbEntities();
            tMembership user = db.tMembership.SingleOrDefault(m => m.fEmail == email);
            ViewBag.UserName = user.fFirstName + user.fLastName;




            IEnumerable<tProduct> table = null;

            if(fMemberID > 0)
            { 

                table = from p in (new gogobuydbEntities()).tProduct
                        where p.fMemberID == fMemberID
                        select p;
                return View(table);

            }


            return View();
        }





        public ActionResult Delete(int id)
        {
            IEnumerable<tProduct> table = null;
            

            table = from p in (new gogobuydbEntities()).tProduct
                    where p.fProductID != id
                    select p;        
            return View(table);       




            //gogobuydbEntities db = new gogobuydbEntities();
            //tProduct prod = db.tProduct.FirstOrDefault(p => p.fProductID == id);
            //if (prod != null)
            //{
            //    db.tProduct.Remove(prod);
            //    db.SaveChanges();
            //}

            //return RedirectToAction("Chatroom");
        }









        //[HttpPost]

        //public ActionResult Chatroom(int fMemberID)
        //{


        //    IEnumerable<tProduct> table = null;
        //    table = from p in (new gogobuydbEntities()).tProduct
        //            where p.fMemberID == fMemberID
        //            select p;
        //    return View(table);




        //    //return View();
        //}








        public ActionResult Update()
        {
            return View();
        }



        //public ActionResult AddOrder(int fMemberID)
        //{
        //    IEnumerable<tProduct> table = null;
        //    table = from p in (new gogobuydbEntities()).tProduct
        //            where p.fMemberID == fMemberID
        //            select p;
        //    return View(table);

            

        //}





    }

    
}
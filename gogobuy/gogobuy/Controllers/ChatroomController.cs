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




            //int count = Convert.ToInt32(Request.Form["Count"]);
            //for (int i = 1; i < count; i++)
            //{

            //    int ProductID = Convert.ToInt32(Request.Form["ProductID" + i]);

            //    string ProductQuantity = Request.Form["ProductQuantity" + i];
            //    string BuyerID = Request.Form["MemberID" + i];
            //    string OrderNote = Request.Form["ProductNote" + i];
            //    string ProductPrice = Request.Form["ProductPrice" + i];
            //    string ProductNote = Request.Form["ProductNote" + i];

            //    int SellerID = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            //    gogobuydbEntities gogobuydb = new gogobuydbEntities();
            //    tProduct prod = gogobuydb.tProduct.FirstOrDefault(m => m.fProductID == ProductID);
            //    if (prod != null)
            //    {
            //        prod.fPrice = decimal.Parse(ProductPrice);

            //        db.SaveChanges();
            //    }

            //    string sql =

            //    "insert into tShopping (fProductID,fMemberID,fQuantity,fShoppingNote) values(@K_FPRODUCTID,@K_FMEMBERID,@K_FQUANTITY,@K_SHOPPINGNOTE)";


            //    List<SqlParameter> paras = new List<SqlParameter>();

            //    paras.Add(new SqlParameter("K_FPRODUCTID", (object)ProductID));

            //    paras.Add(new SqlParameter("K_FQUANTITY", (object)ProductQuantity));

            //    paras.Add(new SqlParameter("K_FMEMBERID", (object)SellerID));

            //    paras.Add(new SqlParameter("K_SHOPPINGNOTE", (object)SellerID));


            //    SqlConnection con = new SqlConnection();
            //    con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;
            //    con.Open();

            //    SqlCommand cmd = new SqlCommand(sql, con);
            //    if (paras != null)
            //    {
            //        foreach (SqlParameter p in paras)
            //            cmd.Parameters.Add(p);
            //    }

            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}



            return View();
        }





        public ActionResult Delete(int id)
        {
            IEnumerable<tProduct> table = null;
            

            table = from p in (new gogobuydbEntities()).tProduct
                    where p.fProductID != id
                    select p;        
            return View(table);       




            
        }





        public ActionResult Update()
        {
            return View();
        }





        public ActionResult AddWishOrder()
        {


            int count = Convert.ToInt32(Request.Form["Count"]);
            for (int i = 1; i < count; i++)
            {

                int ProductID = Convert.ToInt32(Request.Form["ProductID" + i]);

                string ProductQuantity = Request.Form["ProductQuantity" + i];
                string BuyerID = Request.Form["MemberID" + i];
                string OrderNote = Request.Form["ProductNote" + i];
                string ProductPrice = Request.Form["ProductPrice" + i];
                string ProductNote = Request.Form["ProductNote" + i];

                int SellerID = (int)Session[CDictionary.SK_LOGINED_USER_ID];

                gogobuydbEntities db = new gogobuydbEntities();
                tProduct prod = db.tProduct.FirstOrDefault(m => m.fProductID == ProductID);
                if (prod != null)
                {
                    prod.fPrice = decimal.Parse(ProductPrice);

                    db.SaveChanges();
                }

                string sql =

                "insert into tShopping (fProductID,fMemberID,fQuantity,fShoppingNote) values(@K_FPRODUCTID,@K_FMEMBERID,@K_FQUANTITY,@K_SHOPPINGNOTE)";


                List<SqlParameter> paras = new List<SqlParameter>();

                paras.Add(new SqlParameter("K_FPRODUCTID", (object)ProductID));

                paras.Add(new SqlParameter("K_FQUANTITY", (object)ProductQuantity));

                paras.Add(new SqlParameter("K_FMEMBERID", (object)SellerID));

                paras.Add(new SqlParameter("K_SHOPPINGNOTE", (object)SellerID));


                SqlConnection con = new SqlConnection();
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;
                con.Open();

                SqlCommand cmd = new SqlCommand(sql, con);
                if (paras != null)
                {
                    foreach (SqlParameter p in paras)
                        cmd.Parameters.Add(p);
                }

                cmd.ExecuteNonQuery();
                con.Close();
            }

            return Redirect("~/ShoppingCart/Checkout");
        }

    }

    
}
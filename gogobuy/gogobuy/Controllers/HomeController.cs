using gogobuy.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gogobuy.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            string sql = "select top (8) * from tProduct where fIsWish=0 order by fProductID desc;";
            List<tProduct> list = SelectProductBySQL(sql, null);

            string sql2 = "select top (8) * from tProduct where fIsWish=1 order by fUpdateTime desc;";
            List<tProduct> list2 = SelectProductBySQL(sql2, null);

            List<tProduct> listall = list.Concat(list2).ToList<tProduct>();
            return View(listall);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tLogin p)
        {
            if (string.IsNullOrWhiteSpace(p.txtAccount) || string.IsNullOrWhiteSpace(p.txtPassword))
            {
                ViewBag.Msg = "帳號或密碼為空白，請確認是否有填寫?";
                return View();
            }
            tMembership user = SelectByEmail(p.txtAccount);
            // 帳號錯誤
            if (user == null)
            {
                ViewBag.Msg = "查無此帳號";
                return View();
            }
            // 驗證密碼
            if (Account.IsPasswordCorrect(p.txtPassword, user))
            {

                Session[CDictionary.SK_LOGINED_USER_EMAIL] = user.fEmail;
                Session[CDictionary.SK_LOGINED_USER_ID] = user.fMemberID;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Msg = "密碼錯誤，請重新確認";
                return View();
            }

        }
        tMembership SelectByEmail(string Email)
        {
            string sql = "SELECT * FROM tMembership WHERE fEmail=@K_FEMAIL";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_FEMAIL", (object)Email));
            List<tMembership> list = SelectMemberBySQL(sql, paras);

            if (list.Count == 0)
                return null;
            return list[0];

        }
        List<tMembership> SelectMemberBySQL(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;
            con.Open();

            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                    cmd.Parameters.Add(p);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            List<tMembership> list = new List<tMembership>();
            while (reader.Read())
            {
                tMembership x = new tMembership()
                {
                    fMemberID = (int)reader["fMemberID"],
                    fFirstName = reader["fFirstName"].ToString(),
                    fLastName = reader["fLastName"].ToString(),
                    fAddress = reader["fAddress"].ToString(),
                    fEmail = reader["fEmail"].ToString(),
                    fPassword = reader["fPassword"].ToString(),
                    fPhone = reader["fPhone"].ToString(),
                    fSalt = reader["fSalt"].ToString()
                    //fDateOfBirth = (DateTime)reader["fDateOfBirth"],
                    //fGender = (bool)reader["fGender"],
                    //fEmailVerified = (bool)reader["fEmailVerified"]
                };
                list.Add(x);
            }
            con.Close();
            return list;
        }
        public ActionResult Register()
        {
            return View(new tMembership());
        }
        [HttpPost]
        public ActionResult Register(tMembership p)
        {
            if (!ModelState.IsValid)
                return View(p);
            gogobuydbEntities db = new gogobuydbEntities();

            // 檢查email是否重複
            if (SelectByEmail(p.fEmail) != null)
            {
                ViewBag.Msg = "此信箱已被註冊";
                return View(p);
            }

            string PasswordFirst = Request.Form["fPasswordFirst"];
            if (string.IsNullOrWhiteSpace(p.fPassword) || p.fPassword != PasswordFirst)
            {
                ViewBag.Msg = "密碼空白，或重複輸入時錯誤，請重新確認";
                return View(p);
            }
            else
            {
                p.fSalt = Account.GetSalt();
                p.fPassword = Account.HashPassword(PasswordFirst, p.fSalt);
                ViewBag.fName = p.fFirstName + p.fLastName;
                db.tMembership.Add(p);
                db.SaveChanges();
                return RedirectToAction("RegisterFinish");
            }
        }
        public ActionResult RegisterFinish()
        {
            return View();
        }
        public ActionResult LoginOutCheck()
        {
            return View();
        }
        public ActionResult LoginOutCheckFinish()
        {
            Session[CDictionary.SK_LOGINED_USER_EMAIL] = null;
            return RedirectToAction("Index");
        }
        public ActionResult SelectKeyWord()
        {
            string keyword = Request.Form["txtKeyWord"];
            if (keyword == "" || keyword == null)
            {
                return RedirectToAction("SellSearchResult", "SearchResult", null);
                //string sql = "select * from tProduct;";
                //List<tProduct> list = SelectProductBySQL(sql,null);
                //return RedirectToAction("SellSearchResult", "SearchResult", list);
            }
            else
            {
                string sql = $"select * from tProduct where fProductName like @K_FPRODUCTNAME";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("K_FPRODUCTNAME", "%"+keyword+"%"));
                List<tProduct> list = SelectProductBySQL(sql, paras);
                return RedirectToAction("SellSearchResult", "SearchResult", list);
            }

        }
        List<tProduct> SelectProductBySQL(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;
            con.Open();

            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                    cmd.Parameters.Add(p);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            List<tProduct> list = new List<tProduct>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    tProduct x = new tProduct()
                    {
                        fProductID = (int)reader["fProductID"],
                        fCategory = reader["fCategory"].ToString(),
                        fProductName = reader["fProductName"].ToString(),
                        fPrice = Math.Truncate((decimal)reader["fPrice"]),
                        fQuantity = (int)reader["fQuantity"],
                        fImgPath = reader["fImgPath"].ToString(),
                        fDescription = reader["fDescription"].ToString(),
                        fMemberID = (int)reader["fMemberID"],
                        fProductLocation = reader["fProductLocation"].ToString(),
                        fArrivalTime =reader["fArrivalTime"].ToString(),
                        fUpdateTime = (DateTime)reader["fUpdateTime"],
                        fIsWish = (bool)reader["fIsWish"]
                    };
                    list.Add(x);
                }
            }
            else
            {
                return null;
            }
            con.Close();
            return list;
        }
        public ActionResult AddCollection()
        {
            if (Session[CDictionary.SK_LOGINED_USER_EMAIL] == null || Session[CDictionary.SK_LOGINED_USER_ID] ==null)
            {
                TempData["message"] = "收藏相關功能請先登入，謝謝~!";
                return RedirectToAction("Index");
            }
            int ProductID = Convert.ToInt32(Request.Form["ProductID"]);
            int MemberID = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            string sql = "insert into tCollection (fProductID,fMemberID) values(@K_FPRODUCTID,@K_FMEMBERID)";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_FPRODUCTID", ProductID));
            paras.Add(new SqlParameter("K_FMEMBERID", MemberID));

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
            TempData["message"] = "已加入收藏~!";
            return RedirectToAction("Index");
        }

        public ActionResult SelectCategory1()
        {
            string sql = $"select * from tProduct where fCategory='手機周邊';";
            List<tProduct> list = SelectProductBySQL(sql, null);
            return RedirectToAction("SellSearchResult", "SearchResult", list);
        }
        public ActionResult SelectCategory2()
        {
            string sql = $"select * from tProduct where fCategory='3C產品';";
            List<tProduct> list = SelectProductBySQL(sql, null);
            return RedirectToAction("SellSearchResult", "SearchResult", list);
        }
        public ActionResult SelectCategory3()
        {
            string sql = $"select * from tProduct where fCategory='生活居家';";
            List<tProduct> list = SelectProductBySQL(sql, null);
            return RedirectToAction("SellSearchResult", "SearchResult", list);
        }
        public ActionResult SelectCategory4()
        {
            string sql = $"select * from tProduct where fCategory='服飾飾品';";
            List<tProduct> list = SelectProductBySQL(sql, null);
            return RedirectToAction("SellSearchResult", "SearchResult", list);
        }
        public ActionResult SelectCategory5()
        {
            string sql = $"select * from tProduct where fCategory='名產食品';";
            List<tProduct> list = SelectProductBySQL(sql, null);
            return RedirectToAction("SellSearchResult", "SearchResult", list);
        }
        public ActionResult SelectCategory6()
        {
            string sql = $"select * from tProduct where fCategory='文創書籍';";
            List<tProduct> list = SelectProductBySQL(sql, null);
            return RedirectToAction("SellSearchResult", "SearchResult", list);
        }
        public ActionResult SelectCategory7()
        {
            string sql = $"select * from tProduct where fCategory='影音周邊';";
            List<tProduct> list = SelectProductBySQL(sql, null);
            return RedirectToAction("SellSearchResult", "SearchResult", list);
        }
        public ActionResult SelectCategory8()
        {
            string sql = $"select * from tProduct where fCategory='古董古玩';";
            List<tProduct> list = SelectProductBySQL(sql, null);
            return RedirectToAction("SellSearchResult", "SearchResult", list);
        }
        public ActionResult SelectCategory9()
        {
            string sql = $"select * from tProduct where fCategory='其他';";
            List<tProduct> list = SelectProductBySQL(sql, null);
            return RedirectToAction("SellSearchResult", "SearchResult", list);
        }
    }
}
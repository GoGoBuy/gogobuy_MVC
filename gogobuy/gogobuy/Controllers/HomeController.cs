using gogobuy.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;

namespace gogobuy.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
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
            List<tMembership> list = SelectBySQL(sql, paras);

            if (list.Count == 0)
                return null;
            return list[0];

        }
        List<tMembership> SelectBySQL(string sql, List<SqlParameter> paras)
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
    }
}
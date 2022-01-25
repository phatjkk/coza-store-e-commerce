using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CozaStore.Models;
namespace CozaStore.Controllers
{
    public class LoginAdminController : Controller
    {
        // GET: LoginAdmin
        // GET: PrivatePages/Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string user, string pass)
        {
            pass = MaHoa.encryptMD5(pass).Replace("-","").ToLower();
            bool checkAccount = Common.getAccount().Where(x => x.taiKhoan1 == user && x.matKhau== pass).ToList().Count>0;
            
            if (checkAccount)
            {
                TaiKhoan tk = Common.getAccount().Where(x => x.taiKhoan1 == user && x.matKhau == pass).First<TaiKhoan>();
                Session["ttAdmin"]= tk;
                Session["GiaoDienAdmin"] = "bg-theme bg-theme4";
                return Redirect("~/Private/Dashboard/Index");
            } 
                
                
            //return RedirectToAction("Index", "Dashboard", new { Areas= "PrivatePages" });
            //return View("~/Areas/PrivatePages/Views/DangBai/Index.cshtml");
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["ttAdmin"] = null;
            return Redirect("~/Private/Dashboard/Index");
        }
    }
}
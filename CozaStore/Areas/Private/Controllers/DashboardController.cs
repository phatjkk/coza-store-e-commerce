using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CozaStore.Areas.Private.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Private/Dashboard
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public string ChangeBackground(string name)
        {
            Session["GiaoDienAdmin"] = name;
            return (string)Session["GiaoDienAdmin"];
        }
    }
}
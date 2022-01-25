using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CozaStore.Controllers
{
    public class ThongTinDonHangController : Controller
    {
        // GET: ThongTinDonHang
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string maDH, string ten)
        {

            return View();
        }
    }
}
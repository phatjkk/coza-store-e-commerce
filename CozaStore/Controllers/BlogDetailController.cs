using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CozaStore.Models;
namespace CozaStore.Controllers
{
    public class BlogDetailController : Controller
    {
        // GET: BlogDetail
        public ActionResult Index(string maBaiViet)
        {
            if (maBaiViet != null)
            {
                ShopOnlineConnect cn = new ShopOnlineConnect();
                BaiViet x = cn.BaiViets.Where(sp => sp.maBV.Equals(maBaiViet)).First<BaiViet>();
                ViewData["bv"] = x;
            }

            return View();
        }
    }
}
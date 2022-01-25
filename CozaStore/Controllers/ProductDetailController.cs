using CozaStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CozaStore.Controllers
{
    public class ProductDetailController : Controller
    {
        // GET: ProductDetail
        [HttpGet]
        public ActionResult Index(string maSP)
        {
            if (maSP!=null)
            {
                string masp = maSP;
                ShopOnlineConnect cn = new ShopOnlineConnect();
                SanPham x = cn.SanPhams.Where(sp => sp.maSP.Equals(masp)).First<SanPham>();
                ViewData["sp"] = x;
            }
            
            return View();
        }
        [HttpGet]
        public ActionResult AddToCart(string maSP,int slThemMoi)
        {
            if (maSP!=null)
            {
                    ShopOnlineConnect cn = new ShopOnlineConnect();
                    SanPham x = cn.SanPhams.Where(sp => sp.maSP.Equals(maSP)).First<SanPham>();
                    ((KhachSession)Session["ttKhach"]).addSanPham(x, slThemMoi);
            }
            return View("Index");
        }
    }
}
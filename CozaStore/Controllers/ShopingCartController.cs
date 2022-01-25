using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CozaStore.Models;
namespace CozaStore.Controllers
{
    public class ShopingCartController : Controller
    {
        // GET: ShopingCart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult XoaSP(string maSP)
        {
            ((KhachSession)Session["ttKhach"]).BotSLSanPham(maSP);
            return View("Index");
        }
        public ActionResult ThemSP(string maSP)
        {
            ((KhachSession)Session["ttKhach"]).ThemSLSanPham(maSP);
            return View("Index");
        }
        public string CheckSLSP()
        {
            return "{\"sl\":"+ ((KhachSession)Session["ttKhach"]).gioHang.Count+"}";
        }
        public string TongTien()
        {
            int tongTien = 0;
            int sl;
            foreach (SanPham k in ((KhachSession)Session["ttKhach"]).gioHang)
            {
                sl = ((KhachSession)Session["ttKhach"]).listSLSP.Where(m => m.masp == k.maSP).ToList().First().sl;
                tongTien += ((int)k.giaBan * sl);
            }
            return String.Format("{0:#,##0 đ}", (tongTien));
        }
        public string TongTienTheoSP(string maSP)
        {
            int tongTien = 0;
            try
            {
                int sl = ((KhachSession)Session["ttKhach"]).listSLSP.Where(m => m.masp == maSP).ToList().First().sl;

                int giaSP = (int)Common.getProducts().Where(m => m.maSP == maSP).ToList().First<SanPham>().giaBan;

                tongTien += ((int)giaSP * sl);
                
            }
            catch
            {
                ///xu ly cho nay di
            }
            return String.Format("{0:#,##0 đ}", (tongTien));

        }
    }
}
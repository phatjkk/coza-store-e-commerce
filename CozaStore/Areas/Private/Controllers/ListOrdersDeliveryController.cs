using CozaStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CozaStore.Areas.Private.Controllers
{
    public class ListOrdersDeliveryController : Controller
    {
        // GET: Private/ListOrdersDelivery
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// XOÁ ĐƠN HÀNG
        /// </summary>
        /// <param name="soDH"></param>
        /// <returns></returns>
        public ActionResult Delete(string soDH)
        {
            try
            {
                using (ShopOnlineConnect context = new ShopOnlineConnect())
                {
                    //--- get information of selected record in student object.
                    DonHang don = context.DonHangs.FirstOrDefault(r => r.soDH == soDH);
                    List<CtDonHang> ctDon = context.CtDonHangs.Where(j => j.soDH == soDH).ToList();
                    //--- Remove record
                    context.CtDonHangs.RemoveRange(ctDon);
                    context.DonHangs.Remove(don);
                    context.SaveChanges();

                }
                ViewBag.Loai = "oke";
            }
            catch
            {
                ViewBag.Loai = "error";
            }


            return Redirect("Index");
        }
        /// <summary>
        /// XÁC NHẬN ĐÃ GIAO HÀNG VÀ CHUYỂN SANG CHẾ ĐỘ ĐƠN HÀNG THÀNH CÔNG
        /// </summary>
        /// <param name="soDH">MÃ ĐƠN HÀNG</param>
        /// <returns></returns>
        public ActionResult Active(string soDH)
        {
            using (var db = new ShopOnlineConnect())
            {
                var result = db.DonHangs.SingleOrDefault(b => b.soDH == soDH);
                if (result != null)
                {
                    result.ghiChu = "3";
                    db.SaveChanges();
                }
            }
            return Redirect("Index");
        }

    }
}
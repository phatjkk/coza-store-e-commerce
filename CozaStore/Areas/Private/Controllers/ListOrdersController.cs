﻿using CozaStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CozaStore.Areas.Private.Controllers
{
    public class ListOrdersController : Controller
    {
        // GET: Private/ListOrders
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// XOÁ ĐƠN HÀNG
        /// </summary>
        /// <param name="soDH">MÃ ĐƠN HÀNG</param>
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
        /// XÁC NHẬN ĐƠN HÀNG VÀ CHUYỂN SANG CHẾ ĐỘ XỬ LÝ ĐƠN HÀNG
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
                    result.ghiChu = "1";
                    db.SaveChanges();
                }
            }
            return Redirect("Index");
        }
        /// <summary>
        /// LẤY THÔNG TIN ĐƠN HÀNG
        /// </summary>
        /// <param name="soDH">MÃ ĐƠN HÀNG</param>
        /// <returns></returns>
        public string GetOrderDetail(string soDH)
        {
            //List<string> listDH = new List<string>();
            string listDH ="";
            foreach (CtDonHang k in Common.getOrderProductsByID(soDH))
            {
                listDH += "{\"maSP\":\""+k.maSP+
                    "\",\"hinhDD\":\"" + k.SanPham.hinhDD + 
                    "\",\"tenSP\":\"" + k.SanPham.tenSP + 
                    "\",\"sl\":\"" + k.soLuong + 
                    "\",\"giaTien\":\"" + String.Format("{0:#,##0 đ}", k.SanPham.giaBan* k.soLuong) + "\"},";
            }
            listDH = listDH.Remove(listDH.Length - 1);
            return "["+ listDH+"]";
        }
    }
}
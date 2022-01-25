using CozaStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CozaStore.Areas.Private.Controllers
{
    public class ListProductsController : Controller
    {
        // GET: Private/ListProducts
        public ActionResult Index()
        {
            ViewBag.Message = "";
            ViewBag.Status = "";
            return View();
        }
        /// <summary>
        /// XOÁ SẢN PHẨM THEO MÃ SP
        /// </summary>
        /// <param name="maSP">MÃ SP</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string maSP,string redirect)
        {
            try
            {
                using (ShopOnlineConnect context = new ShopOnlineConnect())
                {
                    //--- get information of selected record in student object.
                    SanPham obj = context.SanPhams.FirstOrDefault(r => r.maSP == maSP);
                    string path = Server.MapPath( obj.hinhDD);
                    //--- Remove record
                    context.SanPhams.Remove(obj); 
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)//check file exsit or not  
                    {
                        file.Delete();
                    }
                    context.SaveChanges();

                }
                ViewBag.Loai = "oke";
            }
            catch
            {
                ViewBag.Loai = "error";
            }


            return Redirect("~/Private/"+ redirect + "/Index");
        }
        /// <summary>
        /// HÀM DÙNG ĐỂ DUYỆT/KHÔNG DUYỆT SẢN PHẨM LUÂN PHIÊN KHI GỌI
        /// </summary>
        /// <param name="maSP">MÃ SP</param>
        /// <returns></returns>
        public ActionResult Inactive(string maSP,string redirect)
        {
            using (var db = new ShopOnlineConnect())
            {
                var result = db.SanPhams.SingleOrDefault(b => b.maSP == maSP);
                if (result != null)
                {
                    if (result.daDuyet == true)
                    {
                        result.daDuyet = false;
                    }
                    else
                    {
                        result.daDuyet = true;
                    }
                    db.SaveChanges();
                }
            }
            return Redirect("~/Private/" + redirect + "/Index");
        }
        /// <summary>
        /// LƯU LẠI CHỈNH SỬA SẢN PHẨM
        /// </summary>
        /// <param name="maSP"></param>
        /// <param name="tensp"></param>
        /// <param name="noidungsp"></param>
        /// <param name="duyet"></param>
        /// <param name="noidungspct"></param>
        /// <param name="loaisp"></param>
        /// <param name="file"></param>
        /// <param name="km"></param>
        /// <param name="giasp"></param>
        /// <returns></returns>
        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Edit(string maSP,string tensp, string noidungsp, bool duyet, string noidungspct, int loaisp, HttpPostedFileBase file, int km, int giasp,string redirect)
        {
            try { 
                
                    using (var db = new ShopOnlineConnect())
                    {
                        var result = db.SanPhams.SingleOrDefault(b => b.maSP == maSP);
                    // check sản phẩm có tồn tại hay không    
                    if (result != null)
                        {

                            result.tenSP = tensp;
                            result.noiDung = noidungspct;
                        try
                        {
                            if (file!=null)
                            {
                                string _FileName = Path.GetFileName(file.FileName);
                                string _path = Path.Combine(Server.MapPath("~/Asset/Images/sanPham"), _FileName);
                                file.SaveAs(_path);
                                result.hinhDD = "/Asset/Images/sanPham/" + file.FileName;

                            }
                        }
                        catch { }
                        
                        result.ndTomTat = noidungsp;
                            result.maLoai = loaisp;
                            result.daDuyet = duyet;
                            result.giaBan = giasp;
                            result.giamGia = km;
                            db.SaveChanges();
                        }
                    }
                
                ViewBag.Message = "Sửa thành công sản phẩm \"" + tensp + "\"";
                ViewBag.Status = "success";
                return Redirect("~/Private/" + redirect + "/Index");
        }
            catch
            {
                ViewBag.Message = "Không thể sửa được sản phẩm \"" + tensp + "\"\nVui lòng kiểm tra lại thông tin của bạn";
                ViewBag.Status = "danger";
                return Redirect("~/Private/" + redirect + "/Index");
    }

}
    }
}
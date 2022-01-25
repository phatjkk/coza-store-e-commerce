using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CozaStore.Models;
using System.IO;

namespace CozaStore.Areas.Private.Controllers
{
    public class ProductsInformationController : Controller
    {
        // GET: Private/ProductsInformation
        public ActionResult Index()
        {
            ViewBag.Message = "";
            ViewBag.Status = "";
            return View();
        }
        /// <summary>
        /// LƯU SẢN PHẨM VỪA TẠO VÀO DATABASE (mặc định chưa duyệt sản phẩm)
        /// </summary>
        /// <param name="tensp"></param>
        /// <param name="noidungsp"></param>
        /// <param name="ngaydang"></param>
        /// <param name="userdang"></param>
        /// <param name="duyet"></param>
        /// <param name="noidungspct"></param>
        /// <param name="loaisp"></param>
        /// <param name="file"></param>
        /// <param name="km"></param>
        /// <param name="giasp"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Index(string tensp, string noidungsp, string ngaydang, string userdang, bool duyet, string noidungspct, int loaisp, HttpPostedFileBase file, int km, int giasp)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Asset/Images/sanPham"), _FileName);
                    file.SaveAs(_path);
                    using (var db = new DbContext("name=ShopOnlineConnect"))
                    {
                        var customers = db.Set<SanPham>();
                        customers.Add(new SanPham
                        {

                            maSP = Common.RandomString(10),
                            tenSP = tensp,
                            noiDung = noidungspct,
                            hinhDD = "/Asset/Images/sanPham/" + file.FileName,
                            ndTomTat = noidungsp,
                            ngayDang = DateTime.ParseExact(ngaydang, "dd/MM/yyyy", null),
                            maLoai = loaisp,//db.Set<LoaiSP>().Where(x=>x.tenLoai == loaisp).First<LoaiSP>().maLoai,
                            taiKhoan = userdang,
                            daDuyet = duyet,
                            giaBan = giasp,
                            giamGia = km
                        });

                        db.SaveChanges();
                    }
                }
                ViewBag.Message = "Thêm thành công sản phẩm \"" + tensp + "\"";
                ViewBag.Status = "success";
                return View();
            }
            catch
            {
                ViewBag.Message = "Không thể thêm được sản phẩm \"" + tensp + "\"\nVui lòng kiểm tra lại thông tin của bạn";
                ViewBag.Status = "danger";
                return View();
            }

        }
    }
}
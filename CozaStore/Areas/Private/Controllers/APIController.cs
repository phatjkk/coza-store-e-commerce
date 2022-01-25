using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using CozaStore.Models;
using Newtonsoft.Json;

namespace CozaStore.Areas.Private.Controllers
{
    public class APIController : Controller
    {
        // GET: Private/API
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// LẤY TẤT CẢ LOẠI SẢN PHẨM TỪ DB
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllLoai()
        {
            string re = "";
            foreach (LoaiSP a in Common.getCategories())
            {
                re = re + "{\"maLoai\":\"" + a.maLoai + "\",\"tenLoai\":\"" + a.tenLoai + "\",\"SL\":\"" + Common.getProducts().Where(m => m.maLoai == a.maLoai).ToList().Count + "\"},";
            }
            ViewBag.Loai = "[" + re.TrimEnd(',') + "]";
            return View();
        }
        /// <summary>
        /// XOÁ LOẠI SẢN PHẨM THEO ID
        /// </summary>
        /// <param name="maLoai">ID LOẠI</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult XoaLoai(int maLoai)
        {
            try
            {
                using (ShopOnlineConnect context = new ShopOnlineConnect())
                {
                    //--- get information of selected record in student object.
                    LoaiSP obj = context.LoaiSPs.FirstOrDefault(r => r.maLoai == maLoai);

                    //--- Remove record
                    context.LoaiSPs.Remove(obj);
                    context.SaveChanges();

                }
                ViewBag.Loai = "oke";
            }
            catch
            {
                ViewBag.Loai = "error";
            }
           

                return View();
        }
        /// <summary>
        /// LẤY THÔNG TIN VỀ MÃ LOẠI
        /// </summary>
        /// <param name="maLoai">MÃ LOẠI</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLoai(int maLoai)
        {
            LoaiSP loai =Common.getCategories().Where(x => x.maLoai == maLoai).First<LoaiSP>();
            ViewBag.Loai = "{\"maLoai\":\"" + loai.maLoai + "\",\"tenLoai\":\"" + loai.tenLoai + "\",\"moTa\":\"" + loai.ghiChu + "\"}";
            return View();
        }
        /// <summary>
        /// LƯU LOẠI VÀO DATABASE
        /// </summary>
        /// <param name="maLoai"></param>
        /// <param name="tenThem"></param>
        /// <param name="noidung"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LuuLoai(int maLoai,string tenThem, string noidung)
        {
            using (var db = new DbContext("name=ShopOnlineConnect"))
            {
                var customers = db.Set<LoaiSP>();
                customers.Add(new LoaiSP
                {
                    maLoai = maLoai,
                    tenLoai = tenThem,
                    ghiChu = noidung

                });

                db.SaveChanges();
            }

            ViewBag.Loai = "oke";
            return View();
        }
        /// <summary>
        /// CHỈNH SỬA LOẠI HIỆN CÓ
        /// </summary>
        /// <param name="maLoai"></param>
        /// <param name="tenThem"></param>
        /// <param name="noidung"></param>
        /// <returns></returns>
        public ActionResult SuaLoai(int maLoai, string tenThem, string noidung)
        {
            using (var db = new ShopOnlineConnect())
            {
                var result = db.LoaiSPs.SingleOrDefault(b => b.maLoai == maLoai);
                if (result != null)
                {
                    result.tenLoai = tenThem;
                    result.ghiChu = noidung;
                    db.SaveChanges();
                }
            }
            ViewBag.Loai = "oke";
            return View();
        }
        /// <summary>
        /// XOÁ SẢN PHẨM THEO MÃ SP
        /// </summary>
        /// <param name="maSP">MÃ SP</param>
        /// <returns></returns>
        [HttpGet]

        public ActionResult XoaSP(string maSP)
        {
            try
            {
                using (ShopOnlineConnect context = new ShopOnlineConnect())
                {
                    //--- get information of selected record in student object.
                    SanPham obj = context.SanPhams.FirstOrDefault(r => r.maSP == maSP);

                    //--- Remove record
                    context.SanPhams.Remove(obj);
                    context.SaveChanges();

                }
                ViewBag.Loai = "oke";
            }
            catch
            {
                ViewBag.Loai = "error";
            }


            return View();
        }
        public string LaySP(string maSP)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Error = (sender, args) =>
                {
                    args.ErrorContext.Handled = true;
                },
            };
            SanPham sp = Common.getAllProducts().Where(x => x.maSP == maSP).First<SanPham>();
            
            return "{\"maSP\":\""+sp.maSP+ "\"," +
                "\"tenSP\":\"" + sp.tenSP + "\"," +
                "\"ndTomTat\":\"" + sp.ndTomTat + "\"," +
                "\"giaBan\":\"" + sp.giaBan + "\"," +
                "\"giamGia\":\"" + sp.giamGia + "\"," +
                "\"ngayDang\":\"" + sp.ngayDang + "\"," +
                "\"nguoiDang\":\"" + sp.taiKhoan + "\"," +
                "\"daDuyet\":\"" + sp.daDuyet + "\"," +
                "\"maLoai\":\"" + sp.maLoai + "\"," +
                "\"hinhDD\":\"" + sp.hinhDD + "\"," +
                "\"noiDung\":\"" + System.Net.WebUtility.HtmlEncode(sp.noiDung) + "\"}";
            //return new JsonResult { Data = sp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// LẤY THÔNG TIN BÀI VIẾT DỰA THEO MÃ BV
        /// </summary>
        /// <param name="maBV">MÃ BÀI VIẾT</param>
        /// <returns></returns>
        public string LayBV(string maBV)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Error = (sender, args) =>
                {
                    args.ErrorContext.Handled = true;
                },
            };
            BaiViet bv = Common.getAllBlog().Where(x => x.maBV == maBV).First<BaiViet>();


            return "{\"maBV\":\"" + bv.maBV + "\"," +
                "\"tenBV\":\"" + bv.tenBV + "\"," +
                "\"ndTomTat\":\"" + bv.ndTomTat + "\"," +
                "\"hinhDD\":\"" + bv.hinhDD + "\"," +
                "\"loaiTin\":\"" + bv.loaiTin + "\"," +
                "\"noiDung\":\"" + Convert.ToBase64String(Encoding.UTF8.GetBytes(bv.noiDung)) + "\"}";
            //return new JsonResult { Data = sp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
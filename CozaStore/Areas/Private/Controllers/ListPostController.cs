using CozaStore.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CozaStore.Areas.Private.Controllers
{
    public class ListPostController : Controller
    {
        // GET: Private/ListPost
        public ActionResult Index()
        {
            ViewBag.Message = "";
            ViewBag.Status = "";
            return View();
        }
        /// <summary>
        /// XOÁ BÀI VIẾT
        /// </summary>
        /// <param name="maBV">MÃ BÀI VIẾT</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string maBV)
        {
            try
            {
                using (ShopOnlineConnect context = new ShopOnlineConnect())
                {
                    //--- get information of selected record in student object.
                    BaiViet obj = context.BaiViets.FirstOrDefault(r => r.maBV == maBV);

                    string path = Server.MapPath(obj.hinhDD);
                    //--- Remove record
                    context.BaiViets.Remove(obj);
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


            return Redirect("~/Private/ListPost/Index");
        }
        /// <summary>
        /// HÀM NÀY DÙNG ĐỂ DUYỆT/KHÔNG DUYỆT BÀI VIẾT KHI GỌI LUÂN PHIÊN
        /// </summary>
        /// <param name="maBV">MÃ BÀI VIẾT</param>
        /// <returns></returns>
        public ActionResult Inactive(string maBV)
        {
            using (var db = new ShopOnlineConnect())
            {
                var result = db.BaiViets.SingleOrDefault(b => b.maBV == maBV);
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
            return Redirect("~/Private/ListPost/Index");
        }
        /// <summary>
        /// HÀM NÀY DÙNG ĐỂ LƯU LẠI CHỈNH SỬA BÀI VIẾT
        /// </summary>
        /// <param name="maBV"></param>
        /// <param name="tenbv"></param>
        /// <param name="noidungbv"></param>
        /// <param name="noidungbvct"></param>
        /// <param name="loaibv"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Edit(string maBV, string tenbv, string noidungbv, string noidungbvct, string loaibv, HttpPostedFileBase file)
        {
            //try
            //{

                using (var db = new ShopOnlineConnect())
                {
                    var result = db.BaiViets.SingleOrDefault(b => b.maBV == maBV);
                    if (result != null)
                    {

                        result.tenBV = tenbv;
                        result.noiDung = noidungbvct;
                        try
                        {
                            if (file != null)
                            {
                                string _FileName = Path.GetFileName(file.FileName);
                                string _path = Path.Combine(Server.MapPath("~/Asset/Images/sanPham"), _FileName);
                                file.SaveAs(_path);
                                result.hinhDD = "/Asset/Images/sanPham/" + file.FileName;

                            }
                        }
                        catch { }

                        result.ndTomTat = noidungbv;
                        result.loaiTin = loaibv;
                        db.SaveChanges();
                    }
                }

                ViewBag.Message = "Sửa thành công bài viết \"" + tenbv + "\"";
                ViewBag.Status = "success";
                return View("Index");
            //}
            //catch
            //{
            //    ViewBag.Message = "Không thể sửa được bài viết\"" + tenbv + "\"\nVui lòng kiểm tra lại thông tin của bạn";
            //    ViewBag.Status = "danger";
            //    return View("Index");
            //}

        }
 
    }
}
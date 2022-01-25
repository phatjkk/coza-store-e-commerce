using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CozaStore.Models;
using System.IO;
using System.Data.Entity.Validation;
using System.Collections;
using Newtonsoft.Json;

namespace CozaStore.Areas.Private.Controllers
{
    public class PostController : Controller
    {
        // GET: Private/Post
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Message = "";
            ViewBag.Status = "";
            return View();
        }
        /// <summary>
        /// LƯU BÀI VIẾT MỚI TẠO VÀO DATABASE (mặc định chưa duyệt)
        /// </summary>
        /// <param name="tenbv"></param>
        /// <param name="noidungbv"></param>
        /// <param name="ngaydang"></param>
        /// <param name="userdang"></param>
        /// <param name="file"></param>
        /// <param name="duyet"></param>
        /// <param name="noidungbvct"></param>
        /// <param name="loaitin"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string tenbv, string noidungbv,string ngaydang,string userdang, HttpPostedFileBase file, bool duyet=false,string noidungbvct="",string loaitin="")
        {

            if (file!=null)
            {
                try
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Asset/Images/baiViet"), _FileName);
                    file.SaveAs(_path);
                    if (file.ContentLength > 0)
                    {

                        using (var db = new DbContext("name=ShopOnlineConnect"))
                        {
                            var customers = db.Set<BaiViet>();
                            customers.Add(new BaiViet
                            {
                                maBV = Common.RandomString(10),
                                tenBV = tenbv,
                                noiDung = noidungbvct,
                                hinhDD = "/Asset/Images/baiViet/" + file.FileName,
                                ndTomTat = noidungbv,
                                ngayDang = DateTime.ParseExact(ngaydang, "dd/MM/yyyy", null),
                                loaiTin = loaitin,
                                taiKhoan = userdang,
                                daDuyet = duyet
                            });

                            db.SaveChanges();
                        }
                    }
                    ViewBag.Message = "Thêm thành công bài viết \"" + tenbv + "\"";
                    ViewBag.Status = "success";
                    return View();
                }
                catch (DbEntityValidationException dbEx)
                {
                    string errormes = "";
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            errormes += "Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage + "\n";
                        }
                    }
                    ViewBag.Message = "Không thể thêm được bài viết \"" + tenbv + "\"\nVui lòng kiểm tra lại thông tin của bạn\n" + errormes;
                    ViewBag.Status = "danger";
                    return View();
                }
            }
            else
            {
                ViewBag.Message = "Bạn chưa thêm hình ảnh đại diện của bài viết \"" + tenbv + "\"\n Bạn vui lòng thử lại \n";
                ViewBag.Status = "danger";
                return View();
            }


        }
        /// <summary>
        /// LƯU LẠI ẢNH ĐƯỢC CHÈN TRONG BÀI VIẾT
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFiles()
        {
            if (Request.Files["file"] != null)
            {
                HttpPostedFileBase MyFile = Request.Files["file"];

                // Setting location to upload files
                string TargetLocation = Server.MapPath("~/Image/Post");

                try
                {
                    if (MyFile.ContentLength > 0)
                    {
                        // Get File Extension
                        string Extension = Path.GetExtension(MyFile.FileName);

                        // Determining file name. You can format it as you wish.
                        string FileName = Path.GetFileName(MyFile.FileName);
                        FileName = Guid.NewGuid().ToString().Substring(0, 8);

                        // Determining file size.
                        int FileSize = MyFile.ContentLength;

                        // Creating a byte array corresponding to file size.
                        byte[] FileByteArray = new byte[FileSize];

                        // Basic validation for file extension
                        string[] AllowedExtension = { ".gif", ".jpeg", ".jpg", ".png", ".svg", ".blob" };
                        string[] AllowedMimeType = { "image/gif", "image/jpeg", "image/pjpeg", "image/x-png", "image/png", "image/svg+xml" };

                        if (AllowedExtension.Contains(Extension) && AllowedMimeType.Contains(MimeMapping.GetMimeMapping(MyFile.FileName)))
                        {
                            // Posted file is being pushed into byte array.
                            MyFile.InputStream.Read(FileByteArray, 0, FileSize);

                            // Uploading properly formatted file to server.
                            MyFile.SaveAs(TargetLocation + FileName + Extension);
                            string json = "";
                            Hashtable resp = new Hashtable();
                            string urlPath = MapURL(TargetLocation) + FileName + Extension;

                            // Make the response an json object
                            resp.Add("link", urlPath);
                            json = JsonConvert.SerializeObject(resp);

                            // Clear and send the response back to the browser.
                            Response.Clear();
                            Response.ContentType = "application/json; charset=utf-8";
                            Response.Write(json);
                            Response.End();
                        }
                        else
                        {
                            // Handle validation errors
                        }
                    }
                }

                catch (Exception ex)
                {
                    // Handle errors
                }
            }
            return View();
        }

        // Convert file path to url
        // http://stackoverflow.com/questions/16007/how-do-i-convert-a-file-path-to-a-url-in-asp-net
        private string MapURL(string path)
        {
            string appPath = Server.MapPath("/").ToLower();
            return string.Format("/{0}", path.ToLower().Replace(appPath, "").Replace(@"\", "/"));
        }
        
    }
}
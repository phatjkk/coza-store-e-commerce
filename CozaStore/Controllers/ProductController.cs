using CozaStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CozaStore.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            Session["Loc"] = null;
            Session["GiaTu"] = null;
            Session["GiaDen"] = null;
            Session["Sort"] = null;
            return View();
        }
        public ActionResult Sort(string fromPrice,string toPrice,string by)
        {
            var db = new DbContext("name=ShopOnlineConnect").Set<SanPham>().Where(x => (x.daDuyet == true));

            int? from, to;
            //Nếu có giá gửi vô
            if (fromPrice != null && toPrice !=null)
            {
                from = int.Parse(fromPrice);
                to = int.Parse(toPrice);
                //nếu có sắp xếp trước đó
                if (Session["Sort"] != null)
                {
                    if ((string)Session["Sort"] == "LowToHigh")
                    {
                        db = db.Where(x => (x.daDuyet == true) && x.giaBan >= from && x.giaBan <= to).OrderBy(x => x.giaBan);
                        Session["Sort"] = "LowToHigh";
                    }
                    if ((string)Session["Sort"] == "HighToLow")
                    {
                        db = db.Where(x => (x.daDuyet == true && x.giaBan >= from && x.giaBan <= to)).OrderByDescending(x => x.giaBan);
                        Session["Sort"] = "HighToLow";
                    }
                    if ((string)Session["Sort"] == "New")
                    {
                        db = db.Where(x => (x.daDuyet == true && x.giaBan >= from && x.giaBan <= to)).OrderByDescending(x => x.ngayDang);
                        Session["Sort"] = "New";
                    }
                }
                else
                {
                    //nếu không có sắp xếp
                    db = db.Where(x => (x.daDuyet == true) && x.giaBan >= from && x.giaBan <= to);
                }
                
                Session["GiaTu"] = from;
                Session["GiaDen"] = to;
            }
            //nếu không có giá gửi vô nhưng có sắp xếp
            else if(by != null)
            {
                //nếu có giá trước đó
                if (Session["GiaTu"] != null || Convert.ToInt32(Session["GiaTu"])!=0)
                {
                    from = Convert.ToInt32(Session["GiaTu"]);
                    to = Convert.ToInt32(Session["GiaDen"]);
                    if (by == "LowToHigh")
                    {
                        db = db.Where(x => (x.daDuyet == true) && x.giaBan >= from && x.giaBan <= to).OrderBy(x => x.giaBan);
                        Session["Sort"] = "LowToHigh";
                    }
                    if (by == "HighToLow")
                    {
                        db = db.Where(x => (x.daDuyet == true && x.giaBan >= from && x.giaBan <= to)).OrderByDescending(x => x.giaBan);
                        Session["Sort"] = "HighToLow";
                    }
                    if (by == "New")
                    {
                        db = db.Where(x => (x.daDuyet == true && x.giaBan >= from && x.giaBan <= to)).OrderByDescending(x => x.ngayDang);
                        Session["Sort"] = "New";
                    }
                }
                else
                {
                    //nếu không có giá trước đó
                    from = Convert.ToInt32(Session["GiaTu"]);
                    to = Convert.ToInt32(Session["GiaDen"]);
                    if (by == "LowToHigh")
                    {
                        db = db.Where(x => (x.daDuyet == true)).OrderBy(x => x.giaBan);
                        Session["Sort"] = "LowToHigh";
                    }
                    if (by == "HighToLow")
                    {
                        db = db.Where(x => (x.daDuyet == true)).OrderByDescending(x => x.giaBan);
                        Session["Sort"] = "HighToLow";
                    }
                    if (by == "New")
                    {
                        db = db.Where(x => (x.daDuyet == true)).OrderByDescending(x => x.ngayDang);
                        Session["Sort"] = "New";
                    }
                }
            }



            //else if (Session["GiaTu"] != null)
            //{
            //    from = Convert.ToInt32(Session["GiaTu"]);
            //    to = Convert.ToInt32(Session["GiaTu"]);
            //    db = db.Where(x => (x.daDuyet == true && x.giaBan >= from && x.giaBan <= to));
            //}

            //if (by !=null)
            //{
            //    if (by == "LowToHigh")
            //    {
            //        db = db.Where(x => (x.daDuyet == true)).OrderBy(x => x.giaBan);
            //        Session["Sort"] = "LowToHigh";
            //    }
            //    if (by == "HighToLow")
            //    {
            //        db = db.Where(x => (x.daDuyet == true)).OrderByDescending(x => x.giaBan);
            //        Session["Sort"] = "HighToLow";
            //    }
            //    if (by == "New")
            //    {
            //        db = db.Where(x => (x.daDuyet == true)).OrderByDescending(x => x.ngayDang);
            //        Session["Sort"] = "New";
            //    }
            //}
            //else if (Session["Sort"] !=null)
            //{
            //    if ((string)Session["Sort"] == "LowToHigh")
            //    {
            //        db = db.Where(x => (x.daDuyet == true)).OrderBy(x => x.giaBan);
            //        Session["Sort"] = "LowToHigh";
            //    }
            //    if ((string)Session["Sort"] == "HighToLow")
            //    {
            //        db = db.Where(x => (x.daDuyet == true)).OrderByDescending(x => x.giaBan);
            //        Session["Sort"] = "HighToLow";
            //    }
            //    if ((string)Session["Sort"] == "New")
            //    {
            //        db = db.Where(x => (x.daDuyet == true)).OrderByDescending(x => x.ngayDang);
            //        Session["Sort"] = "New";
            //    }
            //}
            Session["Loc"] = db.ToList<SanPham>();
            return View("Index");
        }

    }
}
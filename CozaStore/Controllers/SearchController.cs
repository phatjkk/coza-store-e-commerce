using CozaStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CozaStore.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Find(string q)
        {
            ViewBag.search = q;
            return View("Index");
        }
        public ActionResult FindByCategories(string type)
        {
            int maLoai = int.Parse(type);
            ViewBag.TypeName = Common.getCategories().Where(m => m.maLoai == maLoai).ToList<LoaiSP>().First().tenLoai;
            ViewBag.Type = maLoai;
            return View("Index");
        }
    }
}
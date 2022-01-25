using CozaStore.Models;
using CozaStore.MoMo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CozaStore.Controllers
{
    public class CheckOutController : Controller
    {
        // GET: CheckOut
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MaDH = "";
            return View();
        }
        /// <summary>
        /// LƯU ĐƠN ĐẶT HÀNG VÀO DATABASE
        /// </summary>
        /// <param name="tenkh">TÊN KHÁCH HÀNG</param>
        /// <param name="email">EMAIL</param>
        /// <param name="phone">ĐIỆN THOẠI</param>
        /// <param name="diachi">ĐỊA CHỈ</param>
        /// <param name="city">TỈNH, TP</param>
        /// <param name="district">QUẬN,HUYỆN</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckOut(string tenkh, string email, string phone, string diachi, string city, string district, string tongtien,string payment)
        {
            if (payment == "momo")
            {
                // Thanh Toan MoMo
                string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                string partnerCode = "MOMO7K7G20211228";
                string accessKey = "SVbtyysg2FxvUfvz";
                string serectkey = "BSmAXqo05I8q2OsR6nQY6KRkNIIy2nT1";
                string orderInfo = "Thanh Toán Mua Hàng CozaStore";
                string host = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "");
                string returnUrl = host + "/CheckOut/MomoReturn";
                string notifyurl = host + "/NotifyMoMo";
                string amount = tongtien;
                string orderid = Guid.NewGuid().ToString();
                string requestId = Guid.NewGuid().ToString();
                string extraData = "";

                //Before sign HMAC SHA256 signature
                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderid + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyurl + "&extraData=" +
                    extraData;
                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);

                //build body json request
                JObject message = new JObject{
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
                };
                // Lưu Gia Trị Vào Session
                Session["tenDatHang"] = tenkh;
                Session["sdtDatHang"] = phone;
                Session["emailDatHang"] = email;

                Session["diachiDatHang"] = diachi + ", " + district + ", " + city;
                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                JObject jmessage = JObject.Parse(responseFromMomo);
                string linkurl = jmessage.GetValue("payUrl").ToString();
                return Redirect(linkurl);
            }
            else
            {
                string makh = "";
                string sodh = Common.RandomString(10);
                int lanmua = 0;
                //check khach hang ton tai
                try
                {
                    KhachHang kh = Common.getCustomer().Where(m => m.soDT == phone).ToList().First<KhachHang>();
                    makh = kh.maKH;
                    lanmua = ((int)kh.lanMua) + 1;
                }
                catch
                {
                    makh = Common.RandomString(10);
                    lanmua = 1;
                }
                using (var db = new DbContext("name=ShopOnlineConnect"))
                {
                    //Dùng Transaction
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var donHangdb = db.Set<DonHang>();
                            var ctdonHangdb = db.Set<CtDonHang>();
                            if (lanmua == 1)
                            {
                                var khachHangdb = db.Set<KhachHang>();

                                khachHangdb.Add(new KhachHang
                                {
                                    maKH = makh,
                                    tenKH = tenkh,
                                    soDT = phone,
                                    email = email,
                                    diaChi = diachi + ", " + district + ", " + city,
                                    ghiChu = "0",
                                    lanMua = lanmua

                                });
                            }

                            donHangdb.Add(new DonHang
                            {
                                soDH = sodh,
                                maKH = makh,
                                ngayDat = DateTime.Now,
                                daKichHoat = false,
                                ngayGH = DateTime.Now.AddDays(2),
                                diaChiGH = diachi + ", " + district + ", " + city,
                                ghiChu = "0"

                            });
                            var listGioHang = ((KhachSession)Session["ttKhach"]).gioHang;
                            var listSLSP = ((KhachSession)Session["ttKhach"]).listSLSP;
                            int sl_tam = 0;
                            foreach (SanPham sp in listGioHang)
                            {
                                sl_tam = listSLSP.Where(m => m.masp == sp.maSP).ToList().First<SLSanPham>().sl;
                                ctdonHangdb.Add(new CtDonHang
                                {
                                    soDH = sodh,
                                    maSP = sp.maSP,
                                    soLuong = sl_tam,
                                    giaBan = (sp.giaBan * sl_tam) * (1 - 1 / 100 * sp.giamGia),
                                    giamGia = sp.giamGia

                                });
                            }

                            db.SaveChanges();
                            dbContextTransaction.Commit();
                            ((KhachSession)Session["ttKhach"]).gioHang.Clear();
                            ((KhachSession)Session["ttKhach"]).listSLSP.Clear();

                        }
                        catch(Exception ex)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }

                }

                ViewBag.MaDH = sodh;
                return View("Index");
            }

        }
        public ActionResult MomoReturn()
        {
            string url = Request.Url.PathAndQuery;
            if (!url.Contains("message=Success"))
                return RedirectToAction("Index", "ShopingCart");

            if (Session["tenDatHang"] == null)
                return RedirectToAction("Index", "ShopingCart");
            string makh = "";
            string sodh = Common.RandomString(10);
            int lanmua = 0;
            //check khach hang ton tai
            try
            {
                KhachHang kh = Common.getCustomer().Where(m => m.soDT == (string)Session["sdtDatHang"]).ToList().First<KhachHang>();
                makh = kh.maKH;
                lanmua = ((int)kh.lanMua) + 1;
            }
            catch
            {
                makh = Common.RandomString(10);
                lanmua = 1;
            }
            using (var db = new DbContext("name=ShopOnlineConnect"))
            {
                //Dùng Transaction
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var donHangdb = db.Set<DonHang>();
                        var ctdonHangdb = db.Set<CtDonHang>();
                        if (lanmua == 1)
                        {
                            var khachHangdb = db.Set<KhachHang>();

                            khachHangdb.Add(new KhachHang
                            {
                                maKH = makh,
                                tenKH = (string)Session["tenDatHang"],
                                soDT = (string)Session["sdtDatHang"],
                                email = (string)Session["emailDatHang"],
                                diaChi = (string)Session["diachiDatHang"],
                                ghiChu = "0",
                                lanMua = lanmua

                            });
                        }

                        donHangdb.Add(new DonHang
                        {
                            soDH = sodh,
                            maKH = makh,
                            ngayDat = DateTime.Now,
                            daKichHoat = false,
                            ngayGH = DateTime.Now.AddDays(2),
                            diaChiGH = (string)Session["diachiDatHang"],
                            ghiChu = "0"

                        });
                        var listGioHang = ((KhachSession)Session["ttKhach"]).gioHang;
                        var listSLSP = ((KhachSession)Session["ttKhach"]).listSLSP;
                        int sl_tam = 0;
                        foreach (SanPham sp in listGioHang)
                        {
                            sl_tam = listSLSP.Where(m => m.masp == sp.maSP).ToList().First<SLSanPham>().sl;
                            ctdonHangdb.Add(new CtDonHang
                            {
                                soDH = sodh,
                                maSP = sp.maSP,
                                soLuong = sl_tam,
                                giaBan = (sp.giaBan * sl_tam) * (1 - 1 / 100 * sp.giamGia),
                                giamGia = sp.giamGia

                            });
                        }

                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        ((KhachSession)Session["ttKhach"]).gioHang.Clear();
                        ((KhachSession)Session["ttKhach"]).listSLSP.Clear();
                        ViewBag.MaDH = sodh;
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }

            }
            return View("Index");
        }
    }
}
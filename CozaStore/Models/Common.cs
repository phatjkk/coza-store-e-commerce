using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CozaStore.Models
{
    public class Common
    {
        static Random rnd = new Random();
        /// <summary>
        /// LẤY DANH SÁCH ĐỐI TƯỢNG SẢN PHẨM THEO MÃ LOẠI
        /// </summary>
        /// <param name="maloai"></param>
        /// <returns></returns>
        public static List<SanPham> getProducts(int maloai) {
            List<SanPham> l = new List<SanPham>();
            DbContext cn = new DbContext("name=ShopOnlineConnect");
            l = cn.Set<SanPham>().Where(x => x.maLoai == maloai && x.daDuyet == true).ToList<SanPham>();
            return l;
        }
        /// <summary>
        /// LẤY TẤT CẢ SẢN PHẨM ĐÃ DUYỆT
        /// </summary>
        /// <returns></returns>
        public static List<SanPham> getProducts()
        {
            List<SanPham> l = new List<SanPham>();
            DbContext cn = new DbContext("name=ShopOnlineConnect");
            l = cn.Set<SanPham>().Where(x=>x.daDuyet==true).ToList<SanPham>();
            return l;
        }
        /// <summary>
        /// LẤY TẤT CẢ SẢN PHẨM CHƯA DUYỆT
        /// </summary>
        /// <returns></returns>
        public static List<SanPham> getInactiveProducts()
        {
            List<SanPham> l = new List<SanPham>();
            DbContext cn = new DbContext("name=ShopOnlineConnect");
            l = cn.Set<SanPham>().Where(x => x.daDuyet == false).ToList<SanPham>();
            return l;
        }
        /// <summary>
        /// LẤY TẤT CẢ SẢN PHẨM (ĐÃ DUYỆT VÀ CHƯA DUYỆT)
        /// </summary>
        /// <returns></returns>
        public static List<SanPham> getAllProducts()
        {
            List<SanPham> l = new List<SanPham>();
            DbContext cn = new DbContext("name=ShopOnlineConnect");
            l = cn.Set<SanPham>().ToList<SanPham>();
            return l;
        }
        /// <summary>
        /// LẤY DANH SÁCH ĐỐI TƯỢNG LOẠI SẢN PHẨM
        /// </summary>
        /// <returns></returns>
        public static List<LoaiSP> getCategories()
        {
            return new DbContext("name=ShopOnlineConnect").Set<LoaiSP>().ToList<LoaiSP>();
        }
        /// <summary>
        /// LẤY DANH SÁCH ĐỐI TƯỢNG BÀI VIẾT ĐÃ DUYỆT
        /// </summary>
        /// <returns></returns>
        public static List<BaiViet> getBlog()
        {
            List<BaiViet> l = new List<BaiViet>();
            DbContext cn = new DbContext("name=ShopOnlineConnect");
            l = cn.Set<BaiViet>().Where(m=>m.daDuyet==true).ToList<BaiViet>();
            return l;
        }
        /// <summary>
        /// LẤY TẤT CẢ ĐỐI TƯỢNG BÀI VIẾT (ĐÃ DUYỆT VÀ CHƯA DUYỆT)
        /// </summary>
        /// <returns></returns>
        public static List<BaiViet> getAllBlog()
        {
            List<BaiViet> l = new List<BaiViet>();
            DbContext cn = new DbContext("name=ShopOnlineConnect");
            l = cn.Set<BaiViet>().ToList<BaiViet>();
            return l;
        }
        /// <summary>
        /// LẤY DANH SÁCH TÀI KHOẢN QUẢN TRỊ
        /// </summary>
        /// <returns></returns>
        public static List<TaiKhoan> getAccount()
        {
            return new DbContext("name=ShopOnlineConnect").Set<TaiKhoan>().ToList<TaiKhoan>();
        }
        /// <summary>
        /// LẤY DANH SÁCH ĐƠN HÀNG THEO TRẠNG THÁI ("0"=Tiếp nhận,"1"=Đang xử lý,"2"=Đang giao hàng,"4"=Xong đơn hàng)
        /// </summary>
        /// <param name="trangThai">TRẠNG THÁI ĐƠN HÀNG</param>
        /// <returns></returns>
        public static List<DonHang> getOrders(string trangThai=null)
        {
            if (trangThai == null)
            {
                return new DbContext("name=ShopOnlineConnect").Set<DonHang>().ToList<DonHang>();

            }
            else
            {
                return new DbContext("name=ShopOnlineConnect").Set<DonHang>().ToList<DonHang>().Where(m => m.ghiChu == trangThai).ToList<DonHang>();

            }
        }
        /// <summary>
        /// LẤY DANH SÁCH ĐỐI TƯỢNG CÁC SẢN PHẨM TRONG 1 ĐƠN HÀNG BẤT KỲ
        /// </summary>
        /// <param name="soDH">MÃ ĐƠN HÀNG</param>
        /// <returns></returns>
        public static List<CtDonHang> getOrderProductsByID(string soDH)
        {
            return new DbContext("name=ShopOnlineConnect").Set<CtDonHang>().ToList<CtDonHang>().Where(m => m.soDH == soDH).ToList<CtDonHang>();
        }
        /// <summary>
        /// LẤY DANH SÁCH ĐỐI TƯỢNG KHÁCH HÀNG ĐÃ MUA HÀNG
        /// </summary>
        /// <returns></returns>
        public static List<KhachHang> getCustomer()
        {
            return new DbContext("name=ShopOnlineConnect").Set<KhachHang>().ToList<KhachHang>();
        }
        /// <summary>
        /// LẤY CHI TIẾT ĐƠN HÀNG CỦA TẤT CẢ ĐƠN HÀNG
        /// </summary>
        /// <returns></returns>
        public static List<CtDonHang> getOrdersDetail()
        {
            return new DbContext("name=ShopOnlineConnect").Set<CtDonHang>().ToList<CtDonHang>();
        }

        private static Random random = new Random();
        /// <summary>
        /// TẠO CHUỐI NGẪU NHIÊN THEO ĐỘ DÀI
        /// </summary>
        /// <param name="length">ĐỘ DÀI CHUỖI MUỐN TẠO</param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        /// <summary>
        /// LẤY NGẪU NHIÊN SẢN PHẨM THEO SỐ LƯỢNG
        /// </summary>
        /// <param name="maloai"></param>
        /// <returns></returns>
        public static List<SanPham> getProductsRandom(int sl)
        {
            //Lay list ma sp
            int r;
            SanPham spTemp;
            List<SanPham> listRandom = new List<SanPham>();
            List<SanPham> listMaSP = new List<SanPham>();
            DbContext cn = new DbContext("name=ShopOnlineConnect");
            foreach (SanPham item in cn.Set<SanPham>().Where(x => x.daDuyet == true).ToList<SanPham>())
            {
                listMaSP.Add(item);
            }
            // Chon ngau nhien
            for(int i = 0; i < sl; i++)
            {
                r = rnd.Next(listMaSP.Count);
                spTemp = listMaSP.ElementAt(r);
                listRandom.Add(spTemp);
                listMaSP.Remove(spTemp);
            }
            return listRandom;
        }
        /// <summary>
        /// TÌM SẢN PHẨM THEO TÊN
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<SanPham> SearchProduct (string search)
        {
            List<SanPham> l = new List<SanPham>();
            DbContext cn = new DbContext("name=ShopOnlineConnect");
            l = cn.Set<SanPham>().Where(x => x.daDuyet == true && x.tenSP.Contains(search)).ToList<SanPham>();
            return l;
        }
    

    }
}
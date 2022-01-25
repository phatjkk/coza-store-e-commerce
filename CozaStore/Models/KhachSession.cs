using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CozaStore.Models
{
    /// <summary>
    /// ĐỐI TƯỢNG KHÁCH HÀNG
    /// </summary>
    public class KhachSession
    {
        public List<SanPham> gioHang = null;
        public string hoTen = "";
        public string sdt = "";
        public string diachi = "";
        public List<SLSanPham> listSLSP = null;
        public KhachSession()
        {
            gioHang = new List<SanPham>();
            listSLSP = new List<SLSanPham>();
        }
        /// <summary>
        /// THÊM 1 SẢN PHẨM VÀO GIỎ HÀNG
        /// </summary>
        /// <param name="sp">ĐỐI TƯỢNG SẢN PHẨM</param>
        /// <param name="slThemMoi">SỐ LƯỢNG MUỐN THÊM</param>
        public void addSanPham(SanPham sp,int slThemMoi=1)
        {
            if (!(gioHang.Where(m => m.maSP == sp.maSP).ToList().Count > 0))
            {
                gioHang.Add(sp);
            }
            ThemSLSanPham(sp.maSP, slThemMoi);
        }
        /// <summary>
        /// XOÁ HẲN SẢN PHẨM ĐÓ RA KHỎI GIỎ HÀNG (bất kể số lượng)
        /// </summary>
        /// <param name="sp"></param>
        public void removeSanPham(SanPham sp)
        {
            if ((gioHang.Where(m => m.maSP == sp.maSP).ToList().Count > 0))
            {
                gioHang.Remove(sp);
                var itemToRemove = listSLSP.SingleOrDefault(m => m.masp == sp.maSP);
                listSLSP.Remove(itemToRemove);
            }
        }
        /// <summary>
        /// TĂNG THÊM SỐ LƯỢNG CỦA 1 SẢN PHẨM ĐANG CÓ TRONG GIỎ 1 ĐƠN VỊ
        /// </summary>
        /// <param name="maSP"></param>
        /// <param name="slThemMoi"></param>
        public void ThemSLSanPham(string maSP,int slThemMoi=1)
        {
            if (listSLSP.Where(m => m.masp == maSP).ToList().Count>0)
            {
                var itemToRemove = listSLSP.SingleOrDefault(m => m.masp == maSP);
                int sltam = itemToRemove.sl;
                listSLSP.Remove(itemToRemove);
                SLSanPham sldp = new SLSanPham(maSP, sltam+1);
                listSLSP.Add(sldp);
                //SLSanPham banSL = listSLSP.Where(m => m.masp == maSP).FirstOrDefault();
            }
            else
            {
                SLSanPham sldp = new SLSanPham(maSP, slThemMoi);
                listSLSP.Add(sldp);
            }
        }
        /// <summary>
        /// XOÁ BỚT SỐ LƯỢNG 1 SẢN PHẨM TRONG GIỎ 1 ĐƠN VỊ (xoá luôn sản phẩm nếu sl = 0)
        /// </summary>
        /// <param name="maSP"></param>
        public void BotSLSanPham(string maSP)
        {
            // Check co ton tai
            if (listSLSP.Where(m => m.masp == maSP).ToList().Count > 0)
            {
                var itemToRemove = listSLSP.SingleOrDefault(m => m.masp == maSP);
                int sltam = itemToRemove.sl-1;
                listSLSP.Remove(itemToRemove);
                if (sltam != 0)
                {
                    SLSanPham sldp = new SLSanPham(maSP, sltam);
                    listSLSP.Add(sldp);
                }
                else
                {
                    removeSanPham(gioHang.Where(m => m.maSP == maSP).ToList().First());
                }
            }

        }
    }
}
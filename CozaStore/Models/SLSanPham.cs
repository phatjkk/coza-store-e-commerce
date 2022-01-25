using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CozaStore.Models
{
    /// <summary>
    /// ĐỐI TƯỢNG NÀY DÙNG ĐỂ LƯU SỐ LƯỢNG CỦA 1 SẢN PHẨM TRONG GIỎ
    /// </summary>
    public class SLSanPham
    {
        public string masp;
        public int sl;
        public SLSanPham()
        {
           
        }
        public SLSanPham(string masp, int sl)
        {
            this.masp = masp;
            this.sl = sl;
        }
    }
}
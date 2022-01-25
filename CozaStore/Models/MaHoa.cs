using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace CozaStore.Models
{
    public class MaHoa
    {
        /// <summary>
        /// HÀM MÃ HOÁ MẬT KHẨU
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string encryptMD5(string text)
        {
            string result = "";
            using (MD5 bb= MD5.Create())
            {
                byte[] sourceData = Encoding.UTF8.GetBytes(text);
                byte[] hashResult = bb.ComputeHash(sourceData);
                result = BitConverter.ToString(hashResult);
            }
            return result;
        }
    }
}
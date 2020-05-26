using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ao.Core
{
    public enum Md5LengthType
    {
        U32,
        U16
    }
    public static class Md5Helper
    {
        public static string GetMd5(string str, Md5LengthType lengthType)
        {
            if (lengthType == Md5LengthType.U32)
            {
                byte[] targetData = null;
                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    var fromData = System.Text.Encoding.Unicode.GetBytes(str);
                    targetData = md5.ComputeHash(fromData);
                }
                str = string.Empty;
                for (int i = 0; i < targetData.Length; i++)
                {
                    str += targetData[i].ToString("x");
                }
            }
            else if (lengthType == Md5LengthType.U16)
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    str = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
                }
                str = str.Replace("-", "");

            }
            return str;
        }
    }
}

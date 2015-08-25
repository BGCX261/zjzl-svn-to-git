using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace zjzl
{
    public static class PswdHelper
    {
        private static SymmetricAlgorithm mCSP;
        private static string txtKey = "tkGGRmBErvc=";//设置加密Key
        private static string txtIV = "Kl7ZgtM1dvQ=";//设置加密IV

        static PswdHelper()
        {
            mCSP = new DESCryptoServiceProvider();
            mCSP.Key = Convert.FromBase64String(txtKey);
            mCSP.IV = Convert.FromBase64String(txtIV);
        }

        /// <summary>
        ///  加密
        /// </summary>
        /// <param name="Value">如果为null或者string.Empty，按照空格处理</param>
        /// <returns></returns>
        /// <exception cref="System.Exceptiion"></exception>
        public static string EncryptString(string Value)
        {
            if(string.IsNullOrEmpty(Value))
            {
                Value = " ";
            }
            ICryptoTransform ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);

            byte[] byt = Encoding.UTF8.GetBytes(Value);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.Exception"></exception>
        public static string DecryptString(string Value)
        {
            ICryptoTransform ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);

            byte[] byt = Convert.FromBase64String(Value);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);

            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

    }
}

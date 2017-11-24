using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Manage.Framework
{
    /// <summary>
    /// 安全助手
    /// </summary>
    internal class SecurityHelper
    {
        private static byte[] _Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary>
        /// DES加密字符串(来自DiscuzNT)
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string ToDES(string encryptString, string encryptKey)
        {
            //encryptKey = TextKit.Substring(encryptKey, 8, "");
            encryptKey = StringHelper.GetSubString(encryptKey, 0, 8, "");
            encryptKey = encryptKey.PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = _Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// DES解密字符串(来自DiscuzNT)
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string FromDES(string decryptString, string decryptKey)
        {
            try
            {
                //decryptKey = TextKit.Substring(decryptKey, 8, "");
                decryptKey = StringHelper.GetSubString(decryptKey, 0, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = _Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">待编码的内容</param>
        /// <returns>SHA256结果</returns>
        public static string ToSHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">待加密的内容</param>
        /// <returns>MD5结果</returns>
        public static string ToMD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }
        /// <summary>
        /// 对字符串进行 Base64 编码，utf8
        /// </summary>
        /// <param name="str">待编码的内容</param>
        /// <returns>Base64 编码</returns>
        public static string ToBase64(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }
        /// <summary>
        /// 进行字符串 Base64 解码，utf8
        /// </summary>
        /// <param name="str">待解码的内容</param>
        /// <returns>解码后的字符串内容</returns>
        public static string FromBase64(string str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.Dynamic;
using System.Drawing;
//using ThoughtWorks.QRCode.Codec;
using Business.Common;
namespace Business.Controller.Common.Helpers
{
    public static class QRCodeHelper
    {
        #region 生成二维码
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="enCodeString"></param>
        //public static void Create(string QRCodeUrl,string enCodeString)
        //{
        //    QRCodeUrl = QRCodeUrl.Replace("#*#", "&");
        //    string QRCode = string.Format(QRCodeUrl, enCodeString);
        //    Bitmap bt;
        //    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        //    bt = qrCodeEncoder.Encode(QRCode, Encoding.UTF8);
        //    string filename = enCodeString + ".jpg";
        //    bt.Save(System.Web.HttpContext.Current.Server.MapPath("/uploadfiles/image/QRCode/") + filename);
        //}

        /// <summary>
        /// 判断二维码是否存在，不存在则生成
        /// </summary>
        /// <param name="enCodeString"></param>
        //public static void IsExist(string QRCodeUrl, string enCodeString)
        //{
        //    QRCodeUrl = QRCodeUrl.Replace("#*#", "&");
        //    string filename = enCodeString + ".jpg";
        //    var path = System.Web.HttpContext.Current.Server.MapPath("/uploadfiles/image/QRCode/") + filename;
        //    if (!File.Exists(path))
        //        Create(QRCodeUrl,enCodeString);
        //}
        #endregion
    }
}

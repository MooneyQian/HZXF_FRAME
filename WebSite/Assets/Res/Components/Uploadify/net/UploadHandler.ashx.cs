using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebSite.Assets.Res.Components.Uploadify.net
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            context.Response.Charset = "utf-8";
            string strFileType = context.Request.Form["fileType"];

            HttpPostedFile file = context.Request.Files["Filedata"];
            string uploadPath =
                HttpContext.Current.Server.MapPath("/UploadFiles/" + strFileType + "/" + DateTime.Now.ToString("yyyyMMdd")) + "/";

            if (file != null)
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string strExtension = Path.GetExtension(file.FileName);

                Random random = new Random();
                string strFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(999999).ToString() + strExtension;

                //if (File.Exists(uploadPath + strFileName))
                //{
                //    string strFileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                //    strFileName = strFileNameWithoutExtension + DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(999999).ToString() + strExtension;
                //}
                //else
                //{
                //    strFileName = file.FileName;
                //}
                file.SaveAs(uploadPath + strFileName);

                //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                //context.Response.Write("/uploadfiles/" + strFileType + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + strFileName);
              
                context.Response.Write("{\"status\":\"success\",\"url\":\"/uploadfiles/" + strFileType + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + strFileName + "\"}");
            }
            else
            {
                context.Response.Write("0");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
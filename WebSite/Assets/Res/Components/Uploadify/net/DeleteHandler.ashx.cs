using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebSite.Assets.Res.Components.Uploadify.net
{
    /// <summary>
    /// DeleteHandler 的摘要说明
    /// </summary>
    public class DeleteHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";

            string strFilePath = context.Request.Form["filePath"];
            if (File.Exists(strFilePath))
            {
                File.Delete(strFilePath);
                context.Response.Write("111");
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
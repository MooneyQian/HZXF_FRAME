using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Collections;

namespace Business.Controller.Common.Helpers
{
    public class HttpHelper
    {
        /// <summary>
        /// 向服务器发送get请求，获取返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetWebReq(string url)
        {
            string ret = "";
            try
            {
                Random random = new Random();
                url += "&randnum=" + DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next();//多加个参数避免缓存
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webReq.Method = "GET";
                //webReq.ContentType = "application/x-www-form-urlencoded";
                System.Net.WebResponse wResp = webReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();

                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.UTF8))
                {
                    ret = reader.ReadToEnd();
                }
                //lastGetTime = DateTime.Now;
                respStream.Close();
                wResp.Close();
            }
            catch (Exception e)
            {
            }
            return ret;
        }

        /// <summary>
        /// 向服务器发送post请求，获取返回数据
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="paramData"></param>
        /// <returns></returns>
        public static string PostWebReq(string postUrl, string paramData)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(paramData);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteArray.Length;

                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();

                HttpWebResponse reponse = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(reponse.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();

                newStream.Close();
                sr.Close();
                reponse.Close();

            }
            catch (Exception ex)
            {

            }
            return ret;
        }

        

    }
}
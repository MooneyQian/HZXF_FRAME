using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace Manage.Framework
{
    /// <summary>
    /// 请求助手
    /// </summary>
    public  class RequestHelper
    {
        #region 泛型获QueryString取值
        /// <summary>
        /// 获取地址栏传递参数值 (T为string,char,bool,DateTime,double,decimal,float,ulong,uint,ushort,byte,long,int,short,sbyte)
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="keyname">键名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>地址栏参数值</returns>
        public static T GetQueryStringValue<T>(string keyname, T defaultValue)
        {
            var obj=HttpContext.Current.Request.QueryString[keyname];
            if(obj!=null) return obj.Convert(defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 获取地址栏传递参数值(可为null) (T只能为bool,DateTime,double,decimal,float,ulong,uint,ushort,byte,long,int,short,sbyte)
        /// (string,char不需要使用Nullable类型)
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="keyname">键名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>地址栏参数值</returns>
        public static T? GetQueryStringValue<T>(string keyname, T? defaultValue) where T : struct
        {
            var obj = HttpContext.Current.Request.QueryString[keyname];
            if (obj != null) return obj.Convert(defaultValue);
            return null;
        }
        #endregion
        #region 泛型获Form取值
        /// <summary>
        /// 获取表单传递参数(Post方式) (T为string,char,bool,DateTime,double,decimal,float,ulong,uint,ushort,byte,long,int,short,sbyte)
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="keyname">键名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>表单值</returns>
        public static T GetFormValue<T>(string keyname, T defaultValue)
        {
            var obj = HttpContext.Current.Request.Form[keyname];
            if (obj != null) return obj.Convert(defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 获取表单传递参数(Post方式,可为null) (T只能为bool,DateTime,double,decimal,float,ulong,uint,ushort,byte,long,int,short,sbyte)
        /// (string,char不需要使用Nullable类型)
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="keyname">键名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>表单值</returns>
        public static T? GetFormValue<T>(string keyname, T? defaultValue) where T : struct
        {
            var obj = HttpContext.Current.Request.Form[keyname];
            if (obj != null) return obj.Convert(defaultValue);
            return null;
        }
        #endregion
        #region  泛型获Params取值
        /// <summary>
        /// 获取表单、地址栏等传递参数 (T为string,char,bool,DateTime,double,decimal,float,ulong,uint,ushort,byte,long,int,short,sbyte)
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="keyname">键名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>表单、地址栏参数值</returns>
        public static T GetParamsValue<T>(string keyname, T defaultValue)
        {
            var obj = HttpContext.Current.Request.Params[keyname];
            if (obj != null) return obj.Convert(defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 获取表单、地址栏等传递参数(可为null) (T只能为bool,DateTime,double,decimal,float,ulong,uint,ushort,byte,long,int,short,sbyte)
        /// (string,char不需要使用Nullable类型)
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="keyname">键名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>表单、地址栏参数值</returns>
        public static T? GetParamsValue<T>(string keyname, T? defaultValue) where T : struct
        {
            var obj = HttpContext.Current.Request.Params[keyname];
            if (obj != null) return obj.Convert(defaultValue);
            return null;
        }
        #endregion
        #region Request操作

        /// <summary>
        /// 获取根域名(例: user.china.com或123456.user.china.com, 返回china.com; www.china.com.cn返回china.com.cn)
        /// </summary>
        /// <param name="host">主机域名(如: user.china.com)</param>
        /// <returns>根域名</returns>
        public static string GetDomain(string host)
        {
            string domain = string.Empty;
            string[] domains = host.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            int len = domains.Length;
            if (len >= 2)
            {
                if (len <= 3)
                {
                    domain = String.Format("{0}.{1}", domains[len - 2], domains[len - 1]);
                }
                else
                {
                    List<string> roots = new List<string>(5);
                    roots.AddRange(new string[5] { "com", "net", "org", "gov", "biz" });
                    //  是否为 com.cn、net.cn、org.cn、com.tw等
                    if (roots.Contains(domains[len - 2].ToLower()))
                    {
                        domain = String.Format("{0}.{1}.{2}", domains[len - 3], domains[len - 2], domains[len - 1]);
                    }
                    else
                    {
                        domain = String.Format("{0}.{1}", domains[len - 2], domains[len - 1]);
                    }
                }
            }
            return domain;
        }
        /// <summary>
        /// 获取用户IP
        /// </summary>
        /// <returns>用户IP</returns>
        public static string GetIP()
        {
            string user_IP = null;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                user_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                user_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return user_IP;

        }
        /// <summary>
        /// 获取用户操作系统
        /// </summary>
        /// <returns>用户操作系统</returns>
        public static string GetOS()
        {
            return HttpContext.Current.Request.ServerVariables["OS"].ToString();
        }
        /// <summary>
        /// 返回代理代码Mozilla/4.0(compatible;MSIE6.0;WindowsNT5.1;SV1)
        /// </summary>
        /// <returns>代理代码</returns>
        public static string GetAgent()
        {
            return HttpContext.Current.Request.ServerVariables["Http_User_Agent"].ToString();
        }
        /// <summary>
        /// 客户发送的form內容或HTTPPUT的数据类型Content_Type
        /// </summary>
        /// <returns>Content_Type</returns>
        public static string GetContentType()
        {
            return HttpContext.Current.Request.ServerVariables["Content_Type"].ToString();
        }
        #endregion
        #region Https
        /// <summary>
        /// 安全套接字层连接关键字的位数，如128
        /// </summary>
        /// <returns>安全套接字层连接关键字的位数</returns>
        public static string GetHttpsKeysize()
        {
            return HttpContext.Current.Request.ServerVariables["Https_Keysize"].ToString();
        }
        /// <summary>
        /// 服务器验证私人关键字的位数如1024
        /// </summary>
        /// <returns>服务器验证私人关键字的位数如</returns>
        public static string GetHttpsSecretkeysize()
        {
            return HttpContext.Current.Request.ServerVariables["Https_Secretkeysize"].ToString();
        }
        /// <summary>
        /// 服务器证书的发行者字段
        /// </summary>
        /// <returns>服务器证书的发行者字段</returns>
       public static string GetHttpsServerIssuer()
        {
            return HttpContext.Current.Request.ServerVariables["Https_Server_Issuer"].ToString();
        }
        /// <summary>
        /// 服务器证书的主题字段
        /// </summary>
       /// <returns>服务器证书的主题字段</returns>
        public static string GetHttpsServerSubject()
        {
            return HttpContext.Current.Request.ServerVariables["Https_Server_Subject"].ToString();
        }
        /// <summary>
        /// 当使用基本验证模式时，客户在密码对话框中输入的密码
        /// </summary>
        /// <returns>客户在密码对话框中输入的密码</returns>
        public static string GetAuthPassword()
        {
            return HttpContext.Current.Request.ServerVariables["Auth_Password"].ToString();
        }
        /// <summary>
        /// 是用户访问受保护的脚本时，服务器用於检验用户的验证方法
        /// </summary>
        /// <returns>是用户访问受保护的脚本时，服务器用於检验用户的验证方法</returns>
        public static string GetAuthType()
        {
            return HttpContext.Current.Request.ServerVariables["Auth_Type"].ToString();
        }
        /// <summary>
        /// 代证的用户名
        /// </summary>
        /// <returns>代证的用户名</returns>
        public static string GetAuthUser()
        {
            return HttpContext.Current.Request.ServerVariables["Auth_User"].ToString();
        }
        /// <summary>
        /// 唯一的客户证书ID号
        /// </summary>
        /// <returns>唯一的客户证书ID号</returns>
        public static string GetCertCookie()
        {
            return HttpContext.Current.Request.ServerVariables["Cert_Cookie"].ToString();
        }
        /// <summary>
        /// 客户证书标誌，如有客户端证书，则bit0为0如果客户端证书验证无效，bit1被设置为1
        /// </summary>
        /// <returns>客户证书标誌</returns>
        public static string GetCertFlag()
        {
            return HttpContext.Current.Request.ServerVariables["Cert_Flag"].ToString();
        }
        /// <summary>
        /// 用户证书中的发行者字段
        /// </summary>
        /// <returns>用户证书中的发行者字段</returns>
        public static string GetCertIssuer()
        {
            return HttpContext.Current.Request.ServerVariables["Cert_Issuer"].ToString();
        }
        /// <summary>
        /// 安全套接字层连接关键字的位数，如128
        /// </summary>
        /// <returns>安全套接字层连接关键字的位数</returns>
        public static string GetCertKeysize()
        {
            return HttpContext.Current.Request.ServerVariables["Cert_Keysize"].ToString();
        }
        /// <summary>
        /// 服务器验证私人关键字的位数如1024
        /// </summary>
        /// <returns>服务器验证私人关键字的位数</returns>
        public static string GetCertSecretkeysize()
        {
            return HttpContext.Current.Request.ServerVariables["Cert_Secretkeysize"].ToString();
        }
        /// <summary>
        /// 客户证书的序列号字段
        /// </summary>
        /// <returns>客户证书的序列号字段</returns>
        public static string GetCertSerialnumber()
        {
            return HttpContext.Current.Request.ServerVariables["Cert_Serialnumber"].ToString();
        }
        /// <summary>
        /// 服务器证书的发行者字段
        /// </summary>
        /// <returns>服务器证书的发行者字段</returns>
        public static string GetCertServerIssuer()
        {
            return HttpContext.Current.Request.ServerVariables["Cert_Server_Issuer"].ToString();
        }
        /// <summary>
        /// 服务器证书的主题字段
        /// </summary>
        /// <returns>服务器证书的主题字段</returns>
        public static string GetCertServerSubject()
        {
            return HttpContext.Current.Request.ServerVariables["Cert_Server_Subject"].ToString();
        }
        /// <summary>
        /// 客户端证书的主题字段
        /// </summary>
        /// <returns>客户端证书的主题字段</returns>
        public static string GetCertSubject()
        {
            return HttpContext.Current.Request.ServerVariables["Cert_Subject"].ToString();
        }

        #endregion
        #region Cert
        #endregion
        /// <summary>
        /// 文件物理地址
        /// </summary>
        /// <param name="url">地址或者路径</param>
        /// <returns></returns>
        public static string MapPath(string url)
        {
            if (HttpContext.Current == null)
            {
                return Path.Combine( AppDomain.CurrentDomain.BaseDirectory,url.TrimStart('\\').TrimStart('/'));
            }
            else
            {
                return HttpContext.Current.Server.MapPath(url);
            }
        }
    }
}
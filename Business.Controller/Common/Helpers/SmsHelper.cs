using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Business.Model.Entitys.Entity;
using Business.Model.Models;
using Manage.Core.SSO;
using Manage.Framework;
using System.Web;
//using HlSmsApi;

namespace Business.Controller.Common.Helpers
{
    public static class SmsHelper
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public const string SERVICE_URL = "http://sms.10690221.com:9011/hy/?uid={0}&auth={1}&mobile={2}&msg={3}&expid=0";
        /// <summary>
        /// 服务器地址
        /// </summary>
        public const string SERVICE_URL_TIME = "http://sms.10690221.com:9011/hy/?uid={0}&auth={1}&mobile={2}&msg={3}&expid=0&time={4}";

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="uid">账号</param>
        /// <param name="pass">密码</param>
        /// <param name="sjh">手机号</param>
        /// <param name="memo">内容</param>
        /// <returns></returns>
        //public static string SendSms(SmsInfo smsInfo)
        //{
        //    try
        //    {
        //        string auth = string.Empty;
        //        string memo = string.Empty;
        //        if (string.IsNullOrWhiteSpace(smsInfo.uid))
        //        {
        //            var appConfig = ConfigManager.Instance.Single<AppConfig>();
        //            smsInfo.company = appConfig.SmsCompanyCode;
        //            smsInfo.uid = appConfig.SmsUid;
        //            smsInfo.pass = appConfig.SmsPassword;
        //            auth = (smsInfo.company + smsInfo.pass).ToMD5();

        //            //先转换成 GBK 编码数据
        //            var bf = Encoding.GetEncoding("GBK").GetBytes(smsInfo.memo);
        //            //把 GBK 数据 转换为 URL 编码的字符串
        //            memo = HttpUtility.UrlEncode(bf);
        //        }
        //        string url = string.Format(SERVICE_URL, smsInfo.uid, auth, smsInfo.sjh, memo);
        //        string ret = HttpHelper.GetWebReq(url);

        //        var errorCode = ret.Convert<int>(0);

        //        if (errorCode == 0)
        //            return ret;
        //        else
        //            return Enum.GetName(typeof(EMSMSERROR), errorCode);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 定时发送短信 当不定时时，为空;定时的时间格式为：2004-01-01 12:22:22
        /// </summary>
        /// <param name="uid">账号</param>
        /// <param name="pass">密码</param>
        /// <param name="sjh">手机号</param>
        /// <param name="memo">内容</param>
        /// <param name="sendtime">发送时间</param>
        /// <returns></returns>
        //public static string SendFixTimeSms(SmsInfo smsInfo)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(smsInfo.uid))
        //        {
        //            var appConfig = ConfigManager.Instance.Single<AppConfig>();
        //            smsInfo.uid = appConfig.SmsUid;
        //            smsInfo.pass = appConfig.SmsPassword;
        //        }
        //        string url = string.Format(SERVICE_URL_TIME, smsInfo.uid, smsInfo.pass, smsInfo.sjh, smsInfo.memo, smsInfo.sendtime);
        //        string ret = HttpHelper.GetWebReq(url);

        //        var errorCode = ret.Convert<int>(0);

        //        if (errorCode == 0)
        //            return ret;
        //        else
        //            return Enum.GetName(typeof(EMSMSERROR), errorCode);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 获取余额
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //public static string GetBalances(SmsInfo smsInfo)
        //{
        //    try
        //    {
        //        SmsApi sms = new SmsApi(SERVICE_URL, smsInfo.uid, smsInfo.pass);
        //        string ret = sms.GetBalance();
        //        return ret;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}

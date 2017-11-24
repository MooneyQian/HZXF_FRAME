using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Business.Model.Entitys.Entity;
using Business.Model.Models;
using Manage.Core.SSO;
using Manage.Framework;

namespace Business.Controller.Common.Helpers
{
//    public static class WeixinUserHelper
//    {
//        public static string GET_ACCESS_TOKEN_URL = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=wxa9a73e3d7c7f86ec&corpsecret=GFMirlMvZDxS5W9l9vV9-OuWl7X6eAMgIaC-uH3p61AKWLb6iX4OhlCBS6rYhcYR";

//        public static string GET_LOGIN_INFO_URL = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}";

//        public const string SEND_MSG_URL = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}";
//        //获取部门列表 id:部门id
//        public const string GET_DEPARTMENT_URL = "https://qyapi.weixin.qq.com/cgi-bin/department/list?access_token={0}&id={1}";

//        public const string UPDATE_DEPARTMENT_URL = "https://qyapi.weixin.qq.com/cgi-bin/department/create?access_token={0}";

//        public const string UPDATE_DEPARTMENT_MEMBER_URL = "https://qyapi.weixin.qq.com/cgi-bin/user/create?access_token={0}";

//        /// <summary>
//        /// 移动端用户模拟登录
//        /// </summary>
//        /// <param name="code">微信端code</param>
//        /// <param name="CurrentUserContext">当前用户信息</param>
//        /// <param name="isOnWXClient">是否移动端true：移动端 false：测试PC端</param>
//        /// <returns></returns>
//        public static ErrorInfo MobileLogOn(string code, LoginUserContext CurrentUserContext, bool isOnWXClient)
//        {
//            var user = new AccessTokenHelper.OAuthUserInfo();
//            if (!string.IsNullOrWhiteSpace(code))
//                user = WeixinUserHelper.GetUserByCode(code, isOnWXClient);
//            if (!string.IsNullOrWhiteSpace(user.UserId))
//                SSOAuthorization.LoginWithoutPassword(user.UserId, ref CurrentUserContext);
//            if (CurrentUserContext == null || CurrentUserContext.UserInfo == null || string.IsNullOrWhiteSpace(user.UserId) || (CurrentUserContext.UserInfo.UserLoginName != user.UserId && !string.IsNullOrWhiteSpace(user.UserId)))
//            {
//                ErrorInfo errorinfo = new ErrorInfo();
//                errorinfo.ErrorMessage = JsonHelper.SerializeObject(user);
//                return errorinfo;
//            }
//            else
//                return null;
//        }

//        /// <summary>
//        /// 根据code获取微信号
//        /// </summary>
//        /// <param name="code"></param>
//        /// <returns></returns>
//        public static string GetUserByCode(string code)
//        {
//            try
//            {
//                var access_token = GetAccessTokenByExpires();

//                string ret_user = HttpHelper.GetWebReq(string.Format(GET_LOGIN_INFO_URL, access_token, code));

//                AccessTokenHelper.OAuthUserInfo uinfo = JsonHelper.DeserializeJsonToObject<AccessTokenHelper.OAuthUserInfo>(ret_user); //返回的json对象转换成c#对象

//                return uinfo.UserId;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        /// <summary>
//        /// 发送信息到微信
//        /// </summary>
//        /// <param name="Users">'|'隔开的多个user</param>
//        /// <param name="Message">需要发送的信息</param>
//        /// <returns></returns>
//        public static string PostMessage(string Users, string Message)
//        {
//            try
//            {
//                var access_token = GetAccessTokenByExpires();
//                string sendmsg = "{" +
//                  "\"touser\": \"" + Users + "\"," +
//                  "\"toparty\": \"\"," +
//                  "\"totag\": \"\"," +
//                  "\"msgtype\": \"text\"," +
//                  "\"agentid\": 12," +
//                  "\"text\": {" +
//                  "\"content\": \"" + Message + "\"" +
//                  "}," +
//                  "\"safe\":0" +
//               "}";
//                return HttpHelper.PostWebReq(string.Format(SEND_MSG_URL, access_token), sendmsg);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        /// <summary>
//        /// 根据code获取用户微信号
//        /// </summary>
//        /// <param name="code"></param>
//        /// <param name="b">false:直接将code做为微信号返回（测试用）</param>
//        /// <returns></returns>
//        public static AccessTokenHelper.OAuthUserInfo GetUserByCode(string code, bool b)
//        {
//            string ret_user;
//            try
//            {
//                if (b)
//                {
//                    var access_token = GetAccessTokenByExpires();

//                    ret_user = HttpHelper.GetWebReq(string.Format(GET_LOGIN_INFO_URL, access_token, code));

//                    AccessTokenHelper.OAuthUserInfo uinfo = JsonHelper.DeserializeJsonToObject<AccessTokenHelper.OAuthUserInfo>(ret_user); //返回的json对象转换成c#对象

//                    return uinfo;
//                }
//                else
//                {
//                    return new AccessTokenHelper.OAuthUserInfo { UserId = code };
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        /// <summary>
//        /// 判断access_token是否过期，若未过期则返回access_token
//        /// </summary>
//        /// <param name="strAccessToken"></param>
//        /// <returns></returns>
//        public static string GetAccessTokenByExpires()
//        {
//            using (var factory = new BaseAccess())
//            {
//                try
//                {
//                    var accessToken = factory.GetSingle<AccessTokenEntity>(Specification<AccessTokenEntity>.Create(c => true));
//                    if (accessToken != null)
//                    {
//                        var appConfig = ConfigManager.Instance.Single<AppConfig>();
//                        var dtNow = DateTime.Now;
//                        var days = (dtNow - (DateTime)accessToken.DT_TIME).Days;
//                        var hours = (dtNow - (DateTime)accessToken.DT_TIME).Hours;
//                        var minutes = (dtNow - (DateTime)accessToken.DT_TIME).Minutes;
//                        var seconds = (dtNow - (DateTime)accessToken.DT_TIME).Seconds;
//                        var totalSeconds = days * 24 * 60 * 60 + hours * 60 * 60 + minutes * 60 + seconds;
//                        if (totalSeconds < appConfig.AccessTokenExpires)
//                            return accessToken.VC_ACCESS_TOKEN;//若未过期则直接返回
//                    }


//                    //若过期则重新获取
//                    string ret = HttpHelper.GetWebReq(GET_ACCESS_TOKEN_URL);
//                    // 反序列化JSON字符串到对象
//                    AccessTokenHelper.AccessTokenInfo ainfo = JsonHelper.DeserializeJsonToObject<AccessTokenHelper.AccessTokenInfo>(ret); //返回的json对象转换成c#对象
//                    if (accessToken == null)
//                    {
//                        accessToken = new AccessTokenEntity();
//                        accessToken.VC_ACCESS_TOKEN = ainfo.access_token;
//                        accessToken.DT_TIME = DateTime.Now;
//                        factory.Insert<AccessTokenEntity>(accessToken);
//                    }
//                    else
//                    {
//                        accessToken.VC_ACCESS_TOKEN = ainfo.access_token;
//                        accessToken.DT_TIME = DateTime.Now;
//                        factory.Update<AccessTokenEntity>(accessToken);
//                    }
//                    return ainfo.access_token;
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//            }
//        }

//        /// <summary>
//        /// 获取微信部门信息
//        /// </summary>
//        /// <returns></returns>
//        public static AccessTokenHelper.DepartMentResultInfo GetWeiXinDepartMentList()
//        {
//            var access_token = GetAccessTokenByExpires();
//            string _url = string.Format(GET_DEPARTMENT_URL, access_token,"5");

//            string _strJson = HttpHelper.GetWebReq(_url);
//            AccessTokenHelper.DepartMentResultInfo deptInfo = JsonHelper.DeserializeJsonToObject<AccessTokenHelper.DepartMentResultInfo>(_strJson); //返回的json对象转换成c#对象

//            return deptInfo;

//        }
//        /// <summary>
//        /// 更新微信部门信息
//        /// </summary>
//        /// <param name="pid"></param>
//        /// <param name="deptname"></param>
//        /// <returns></returns>
//        public static AccessTokenHelper.UpdateDepartMentResultInfo UpdateWeiXinDepartMentInfo(string pid, string deptname)
//        {
//             var access_token = GetAccessTokenByExpires();
//            string _url = string.Format(UPDATE_DEPARTMENT_URL, access_token);

//            string deptMsgInfo = "{" +
//                          "\"name\": \"" + deptname + "\"," +
//                          "\"parentid\": " + pid +
//                            "}";
//            string _strJson= HttpHelper.PostWebReq(_url, deptMsgInfo);
//            AccessTokenHelper.UpdateDepartMentResultInfo deptResultInfo = JsonHelper.DeserializeJsonToObject<AccessTokenHelper.UpdateDepartMentResultInfo>(_strJson); //返回的json对象转换成c#对象
//            return deptResultInfo;
           
//        }
//        /// <summary>
//        /// 更新部门下人员信息
//        /// </summary>
//        /// <param name="deptid"></param>
//        /// <param name="weixinid"></param>
//        /// <param name="uname"></param>
//        /// <returns></returns>
//        public static AccessTokenHelper.ErrorInfo UpdateWeiXinDepartMentMemberInfo(string deptid, string weixinid, string uname)
//        {
//            var access_token = GetAccessTokenByExpires();
//            string _url = string.Format(UPDATE_DEPARTMENT_MEMBER_URL, access_token);

//            string deptMsgInfo = "{" +
//                         "\"userid\": \"" + weixinid + "\"," +
//                       "\"name\": \"" + uname + "\"," +
//                       "\"department\": " + deptid + "," +
//                       "\"weixinid\": \"" + weixinid + "\"," +
//                            "}";
//            string _strJson = HttpHelper.PostWebReq(_url, deptMsgInfo);
//            AccessTokenHelper.ErrorInfo errInfo = JsonHelper.DeserializeJsonToObject<AccessTokenHelper.ErrorInfo>(_strJson); //返回的json对象转换成c#对象
//            return errInfo;
//        }

//    }
}

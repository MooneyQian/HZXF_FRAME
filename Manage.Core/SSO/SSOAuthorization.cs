using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Manage.Core.Cache;
using Manage.Framework;
using Manage.SSO.Entity;
using Manage.Open;
using Manage.Core.Common;
using Manage.Core.Facades;

namespace Manage.Core.SSO
{
    /// <summary>
    /// 登录用户上下文
    /// </summary>
    public class SSOAuthorization
    {
        private static AppConfig appConfig = ConfigManager.Instance.Single<AppConfig>();

        /// <summary>
        /// 获取当前登录用户上下文
        /// </summary>
        public static LoginUserContext GetCurrentUserContext
        {
            get
            {
                if (IsLogin)
                {
                    string id = HttpContext.Current.Session["userContext_UserID"].ToString();


                    //ConfigurationFacade.

                    var context = LoginUserCacheStorage.Current.Get(id);
                    if (context == null)
                    {
                        //重新从数据库获取
                        IUserFacade facade = SSOFacadeAdapter.UserInstance();
                        var user = facade.GetUserByID(id);
                        if (user != null)
                        {
                            //转登录用户信息对象
                            UserInfo userInfo = user.Adapter<UserInfo>(new UserInfo());
                            //生成用户上下文对象
                            context = context = CreateUserContext(userInfo);
                            //用户上下文存缓存
                            LoginUserCacheStorage.Current.Set(context.UserID, context);
                        }
                    }
                    return context;
                }
                else
                {
                    return null;
                }
            }
        }

        #region 登录方案

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static LoginReturnInfo Login(string loginName, string Password)
        {
            LoginReturnInfo loginRes = new LoginReturnInfo();
            var user = Manage.Open.MembershipFactory.Instance.GetUserByLoginName(loginName);
            if (user == null || string.IsNullOrEmpty(user.ID))
            {
                loginRes.Status = LoginingStatus.NotExits;
                loginRes.Message = "登录失败，用户名不存在或已被禁用！";
                return loginRes;
            }
            if (user.UserPassword != (loginName + Define._PASSWORDSPLIT + Password).ToMD5())
            {
                loginRes.Status = LoginingStatus.NotMatch;
                loginRes.Message = "登录失败，密码错误！";
                return loginRes;
            }
            //转登录用户信息对象
            UserInfo userInfo = user.Adapter<UserInfo>(new UserInfo());
            //生成用户上下文对象
            LoginUserContext context = CreateUserContext(userInfo);
            //登录信息存session
            HttpContext.Current.Session["userContext_UserID"] = context.UserID;
            HttpContext.Current.Session["userContext_LoginName"] = context.UserInfo.UserLoginName;
            HttpContext.Current.Session["userContext_md5"] = (context.UserID + Define._USERCACHEKEY + context.UserInfo.UserLoginName).ToMD5();
            //用户上下文存缓存
            LoginUserCacheStorage.Current.Set(context.UserID, context);

            loginRes.LoginUserContext = context;
            loginRes.Status = LoginingStatus.Success;
            loginRes.Message = "登录成功！";
            return loginRes;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static LoginReturnInfo LoginWithoutPassword(string loginName, ref  LoginUserContext context)
        {
            LoginReturnInfo loginRes = new LoginReturnInfo();
            var user = Manage.Open.MembershipFactory.Instance.GetUserByLoginName(loginName);
            if (user == null || string.IsNullOrEmpty(user.ID))
            {
                loginRes.Status = LoginingStatus.NotExits;
                loginRes.Message = "登录失败，用户名不存在或已被禁用！";
                return loginRes;
            }
            //转登录用户信息对象
            UserInfo userInfo = user.Adapter<UserInfo>(new UserInfo());
            //生成用户上下文对象
            context = CreateUserContext(userInfo);
            //登录信息存session
            HttpContext.Current.Session["userContext_UserID"] = context.UserID;
            HttpContext.Current.Session["userContext_LoginName"] = context.UserInfo.UserLoginName;
            HttpContext.Current.Session["userContext_md5"] = (context.UserID + Define._USERCACHEKEY + context.UserInfo.UserLoginName).ToMD5();
            //用户上下文存缓存
            LoginUserCacheStorage.Current.Set(context.UserID, context);

            loginRes.LoginUserContext = context;
            loginRes.Status = LoginingStatus.Success;
            loginRes.Message = "登录成功！";
            return loginRes;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static LoginReturnInfo LoginWithPassword(string loginName, string password, ref  LoginUserContext context)
        {
            LoginReturnInfo loginRes = new LoginReturnInfo();
            var user = Manage.Open.MembershipFactory.Instance.GetUserByLoginName(loginName);
            if (user == null || string.IsNullOrEmpty(user.ID))
            {
                loginRes.Status = LoginingStatus.NotExits;
                loginRes.Message = "登录失败，用户名不存在或已被禁用！";
                return loginRes;
            }
            if (user.UserPassword != (loginName + Define._PASSWORDSPLIT + password).ToMD5())
            {
                loginRes.Status = LoginingStatus.NotMatch;
                loginRes.Message = "登录失败，密码错误！";
                return loginRes;
            }
            //转登录用户信息对象
            UserInfo userInfo = user.Adapter<UserInfo>(new UserInfo());
            //生成用户上下文对象
            context = CreateUserContext(userInfo);
            //登录信息存session
            HttpContext.Current.Session["userContext_UserID"] = context.UserID;
            HttpContext.Current.Session["userContext_LoginName"] = context.UserInfo.UserLoginName;
            HttpContext.Current.Session["userContext_md5"] = (context.UserID + Define._USERCACHEKEY + context.UserInfo.UserLoginName).ToMD5();
            //用户上下文存缓存
            LoginUserCacheStorage.Current.Set(context.UserID, context);

            loginRes.LoginUserContext = context;
            loginRes.Status = LoginingStatus.Success;
            loginRes.Message = "登录成功！";
            return loginRes;
        }

        /// <summary>
        /// 单点登录验证
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static LoginReturnInfo SSOLogin(string ticket)
        {
            LoginReturnInfo loginRes = new LoginReturnInfo();
            if (string.IsNullOrEmpty(ticket))
            {
                loginRes.Status = LoginingStatus.ParameterNeed;
                loginRes.Message = "缺少票据信息";
                return loginRes;
            }
            ticket = System.Net.WebUtility.HtmlDecode(ticket);

            string userTicket = Extends.RSADecryptString(ticket, Define._PRIVATEKEY);//解密票据

            var tickets = userTicket.Split(';');
            if (tickets.Count() != 5)//票据格式由验证中心规定
            {
                loginRes.Status = LoginingStatus.ErrorTicket;
                loginRes.Message = "无效票据信息！";
                return loginRes;
            }
            string ticket_guid = tickets[0].ToString();//票据guid
            string UserID = tickets[1].ToString();//用户编号
            DateTime startTime = tickets[2].ToString().Convert<DateTime>(DateTime.Now.AddDays(-1));//生成时间
            DateTime endTime = tickets[3].ToString().Convert<DateTime>(DateTime.Now.AddDays(-1));//有效期
            string ticket_md5 = tickets[4].ToString();//票据有效性验证码

            //票据有效性验证
            if (string.Format("{0}{1}{2}{3}{4}{5}{6}", tickets[0], Define._SSOTICKETKEY, tickets[1], Define._SSOTICKETKEY, tickets[2], Define._SSOTICKETKEY, tickets[3]).ToMD5() != tickets[4])
            {
                loginRes.Status = LoginingStatus.ErrorTicket;
                loginRes.Message = "非法票据信息！";
                return loginRes;
            }
            if (DateTime.Now < startTime || DateTime.Now > endTime)
            {
                loginRes.Status = LoginingStatus.TimeOut;
                loginRes.Message = "票据超时！请重新登录。";
                return loginRes;
            }
            //本系统登录操作
            var user = Manage.Open.MembershipFactory.Instance.GetUserByID(UserID);
            if (user == null || string.IsNullOrEmpty(user.ID))
            {
                loginRes.Status = LoginingStatus.NotExits;
                loginRes.Message = "登录失败，用户名不存在或已被禁用！";
                return loginRes;
            }
            user.UserType = (int)UserType.Users;
            //转登录用户信息对象
            UserInfo userInfo = user.Adapter<UserInfo>(new UserInfo());
            //生成用户上下文对象
            LoginUserContext context = CreateUserContext(userInfo);
            //登录信息存session
            HttpContext.Current.Session["userContext_UserID"] = context.UserID;
            HttpContext.Current.Session["userContext_LoginName"] = context.UserInfo.UserLoginName;
            HttpContext.Current.Session["userContext_md5"] = (context.UserID + Define._USERCACHEKEY + context.UserInfo.UserLoginName).ToMD5();
            //用户上下文存缓存
            LoginUserCacheStorage.Current.Set(context.UserID, context);

            loginRes.LoginUserContext = context;
            loginRes.Status = LoginingStatus.Success;
            loginRes.Message = "登录成功！";
            return loginRes;
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static LoginReturnInfo AdminLogin(string loginName, string Password)
        {
            AdminFacade facade = new AdminFacade();
            LoginReturnInfo loginRes = new LoginReturnInfo();
            var user = facade.GetUserByLoginName(loginName);
            if (user == null || string.IsNullOrEmpty(user.ID))
            {
                loginRes.Status = LoginingStatus.NotExits;
                loginRes.Message = "登录失败，管理员不存在或已被禁用！";
                return loginRes;
            }
            if (user.UserPassword != (loginName + Define._PASSWORDSPLIT + Password).ToMD5())
            {
                loginRes.Status = LoginingStatus.NotMatch;
                loginRes.Message = "登录失败，密码错误！";
                return loginRes;
            }
            //转登录用户信息对象
            UserInfo userInfo = user.Adapter<UserInfo>(new UserInfo());
            //生成用户上下文对象
            LoginUserContext context = CreateUserContext(userInfo);
            //登录信息存session
            HttpContext.Current.Session["userContext_UserID"] = context.UserID;
            HttpContext.Current.Session["userContext_LoginName"] = context.UserInfo.UserLoginName;
            HttpContext.Current.Session["userContext_md5"] = (context.UserID + Define._USERCACHEKEY + context.UserInfo.UserLoginName).ToMD5();
            //用户上下文存缓存
            LoginUserCacheStorage.Current.Set(context.UserID, context);

            loginRes.LoginUserContext = context;
            loginRes.Status = LoginingStatus.Success;
            loginRes.Message = "登录成功！";
            return loginRes;
        }

        #endregion

        /// <summary>
        /// 退出登录
        /// </summary>
        public static string Logout()
        {
            string msg = string.Empty;
            HttpContext.Current.Session.Clear();
            return msg;
        }

        /// <summary>
        /// 判断是否登陆
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                if (HttpContext.Current.Session["userContext_UserID"] != null && HttpContext.Current.Session["userContext_LoginName"] != null &&
                    HttpContext.Current.Session["userContext_md5"] != null)
                {
                    if (HttpContext.Current.Session["userContext_md5"].ToString() == (HttpContext.Current.Session["userContext_UserID"].ToString() + Define._USERCACHEKEY + HttpContext.Current.Session["userContext_LoginName"].ToString()).ToMD5())
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private static LoginUserContext CreateUserContext(UserInfo userInfo)
        {
            LoginUserContext context = new LoginUserContext();
            context.UserID = userInfo.ID;
            context.UserInfo = userInfo;
            context.LoginTime = DateTime.Now;
            context.DefaultOrganization = userInfo.DefaultOrganization.Value;
            context.Organizations = userInfo.Organizations.Value;
            context.UserRoles = userInfo.UserRoles.Value;
            context.UserMenus = userInfo.UserAllMenus.Value.Where(w => w.MenuType == (int)MenuType.Menu).ToList();
            context.UserFuns = userInfo.UserAllMenus.Value.Where(w => w.MenuType == (int)MenuType.Function).ToList();
            context.UserDatas = userInfo.UserAllMenus.Value.Where(w => w.MenuType == (int)MenuType.Data).ToList();
            context.UserType = (UserType)userInfo.UserType;
            if (context.UserType == UserType.Administrators)
            {
                //超级管理员增加特殊菜单
                context.UserMenus.AddRange(MembershipFactory.Instance.GetAdminMenu());
                context.UserMenus = context.UserMenus.OrderBy(o => o.MenuLevel).ThenBy(t => t.MenuOrder).ToList();
            }
            return context;
        }
    }


    /// <summary>
    /// 登录返回信息
    /// </summary>
    public class LoginReturnInfo
    {
        /// <summary>
        /// 登录的用户上下文
        /// </summary>
        public LoginUserContext LoginUserContext { get; set; }

        /// <summary>
        /// 登录状态
        /// </summary>
        public LoginingStatus Status { get; set; }

        /// <summary>
        /// 登录返回信息
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 登录状态信息枚举
    /// </summary>
    public enum LoginingStatus
    {
        /// <summary>
        /// 系统错误
        /// </summary>
        SystemError = 0,

        /// <summary>
        /// 缺少必要参数
        /// </summary>
        ParameterNeed = 1,

        /// <summary>
        /// 参数格式错误
        /// </summary>
        FormateError = 2,

        /// <summary>
        /// 不存在的用户
        /// </summary>
        NotExits = 3,

        /// <summary>
        /// 账户密码不匹配
        /// </summary>
        NotMatch = 4,

        /// <summary>
        /// 登录成功
        /// </summary>
        Success = 9,

        /// <summary>
        /// 用户未设置角色或角色已停用
        /// </summary>
        NotInRole = 11,

        /// <summary>
        /// 动态口令错误
        /// </summary>
        DynamicError = 12,

        /// <summary>
        /// 超时
        /// </summary>
        TimeOut = 13,
        /// <summary>
        /// 错误票据信息
        /// </summary>
        ErrorTicket = 14
    }
}

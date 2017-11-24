using System;
using Manage.Framework;

namespace Manage.Framework
{
    public class AppConfig
    {
        [Config("系统名称")]
        public string AppName
        {
            get;
            set;
        }

        [Config("底部文字")]
        public string FootTitle
        {
            get;
            set;
        }

        [Config("登录界面logo文字")]
        public string LoginTitle
        {
            get;
            set;
        }

        [Config("登录界面logo文字")]
        public string LoginTitle1
        {
            get;
            set;
        }
        [Config("底部版权文字")]
        public string Copyright
        {
            get;
            set;
        }
        [Config("公司名称")]
        public string CompanyName
        {
            get;
            set;
        }

        [Config("1")]
        public int HomeStyle
        {
            get;
            set;
        }

        [Config("1.0")]
        public string Version
        {
            get;
            set;
        }

        [Config("/Content/HomePage/style/logo.gif")]
        public string LogoUrl
        {
            get;
            set;
        }

        [Config("112233")]
        public string DefaultPassword
        {
            get;
            set;
        }

        [Config("/Account/LogOn")]
        public string LoginAction
        {
            get;
            set;
        }

        [Config("/Views/Account/LogOn.cshtml")]
        public string LoginPage
        {
            get;
            set;
        }

        [Config("/Home/Index")]
        public string IndexAction
        {
            get;
            set;
        }

        [Config("/Views/Home/IndexLiger.cshtml")]
        public string IndexPage
        {
            get;
            set;
        }

        [Config("/Shared/Error")]
        public string ErrorPageAction
        {
            get;
            set;
        }

        [Config("/Shared/Unauthorized")]
        public string UnauthorizedPageAction
        {
            get;
            set;
        }

        [Config("false")]
        public bool UseValidateCode
        {
            get;
            set;
        }

        [Config("/Content/Login/Images/logo.png")]
        public string LoginLogoUrl
        {
            get;
            set;
        }
        [Config("/Content/Login/Images/logon1.png")]
        public string LoginLogoUrl1
        {
            get;
            set;
        }
        [Config("/Content/Login/Images/login_bg_01.jpg")]
        public string LoginBackImgUrl
        {
            get;
            set;
        }

        public string LoginBackNew1
        {
            get;
            set;
        }
        [Config("false")]
        public bool EnableCookie
        {
            get;
            set;
        }

        [Config("6")]
        public int CookieExpiresTime
        {
            get;
            set;
        }

        [Config("false")]
        public bool LogEnable
        {
            get;
            set;
        }

        [Config("0")]
        public int LogType
        {
            get;
            set;
        }

        [Config("false")]
        public bool SSOEnable
        {
            get;
            set;
        }

        [Config("2")]
        public int SSOType
        {
            get;
            set;
        }

        [Config("http://localhost/Login.aspx")]
        public string SSOServerUrl
        {
            get;
            set;
        }

        [Config("http://localhost/WS_SSO/WS_SSODataFactorySoap.asmx")]
        public string SSOFactoryUrl
        {
            get;
            set;
        }

        [Config("/DBConfig/SSOdbConfig.cfg.xml")]
        public string SSOFactoryConfig
        {
            get;
            set;
        }

        [Config("ODMS")]
        public string SSORegisterID
        {
            get;
            set;
        }

        [Config("7200")]
        public int AccessTokenExpires
        {
            get;
            set;
        }

        [Config("false")]
        public bool IsOnWXClient
        {
            get;
            set;
        }

        [Config("")]
        public string MobileManager
        {
            get;
            set;
        }

        [Config("水印文字")]
        public string Watermark
        {
            get;
            set;
        }

        [Config("二维码地址")]
        public string QRCodeUrl
        {
            get;
            set;
        }

        [Config("shkj")]
        public string SmsCompanyCode
        {
            get;
            set;
        }

        [Config("81094")]
        public string SmsUid
        {
            get;
            set;
        }

        [Config("P39obl7")]
        public string SmsPassword
        {
            get;
            set;
        }
    }
}

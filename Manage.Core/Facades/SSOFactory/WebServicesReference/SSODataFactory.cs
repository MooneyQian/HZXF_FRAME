using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Facades.SSOFactory
{
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Web.Services;
    using System.Xml;
    using Manage.Framework;


    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "WS_SSODataFactorySoap", Namespace = "http://tempuri.org/")]
    public class SSODataFactory : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        /// <summary>
        /// 通用配置
        /// </summary>
        public AppConfig AppConfig
        {
            get
            {
                return ConfigManager.Instance.Single<AppConfig>();
            }
        }

        /// <remarks/>
        public SSODataFactory()
        {
            this.Url = AppConfig.SSOFactoryUrl;
        }

        #region User

        /// <summary>
        /// 说明：获取用户信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetUserByID", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SSO_Operator GetUserByID(string UserID, string SiteName = "")
        {
            if (string.IsNullOrEmpty(SiteName)) SiteName = AppConfig.SSORegisterID;
            object[] results = this.Invoke("GetUserByID", new object[] { UserID, SiteName});
            return (SSO_Operator)(results[0]);
        }

        /// <summary>
        /// 说明：获取用户分页数据
        /// </summary>
        /// <param name="LoginName">登录帐户</param>
        /// <param name="DisplayName">用户姓名</param>
        /// <param name="pageSize">行数</param>
        /// <param name="pageIndex">页数</param>
        /// <returns></returns>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetUserPaged", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public List<SSO_Operator> GetUserPaged(string LoginName, string DisplayName, int pageSize, int pageIndex, string SiteName = "")
        {
            if (string.IsNullOrEmpty(SiteName)) SiteName = AppConfig.SSORegisterID;
            object[] results = this.Invoke("GetUserPaged", new object[] { LoginName, DisplayName, pageSize, pageIndex, SiteName });
            return (List<SSO_Operator>)(results[0]);
        }


        /// <summary>
        /// 说明：获取用户信息
        /// </summary>
        /// <param name="LoginName">登录账号</param>
        /// <returns></returns>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetUserByLoginName", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SSO_Operator GetUserByLoginName(string LoginName, string SiteName = "")
        {
            if (string.IsNullOrEmpty(SiteName)) SiteName = AppConfig.SSORegisterID;
            object[] results = this.Invoke("GetUserByLoginName", new object[] { LoginName, SiteName });
            return (SSO_Operator)(results[0]);
        }


        /// <summary>
        /// 说明：获取所有用户信息
        /// </summary>
        /// <param name="LoginName">登录账号</param>
        /// <returns></returns>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetAllUsers", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public List<SSO_Operator> GetAllUsers(string LoginName, string DisplayName, string SiteName = "")
        {
            if (string.IsNullOrEmpty(SiteName)) SiteName = AppConfig.SSORegisterID;
            object[] results = this.Invoke("GetAllUsers", new object[] { LoginName, DisplayName, SiteName });
            return (List<SSO_Operator>)(results[0]);
        }

        /// <summary>
        /// 说明：修改用户密码
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ChangePwd", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool ChangePwd(string UserID, string Password, string SiteName = "")
        {
            if (string.IsNullOrEmpty(SiteName)) SiteName = AppConfig.SSORegisterID;
            object[] results = this.Invoke("ChangePwd", new object[] { UserID, Password, SiteName });
            return (bool)(results[0]);
        }

        /// <summary>
        /// 说明：对比密码是否正确
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ComparePassword", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool ComparePassword(string UserID, string Password, string SiteName = "")
        {
            if (string.IsNullOrEmpty(SiteName)) SiteName = AppConfig.SSORegisterID;
            object[] results = this.Invoke("ComparePassword", new object[] { UserID, Password, SiteName });
            return (bool)(results[0]);
        }

        #endregion

        #region App

        /// <summary>
        /// 说明：获取用户有权限系统
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetAppRegisterByUserID", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public List<SSO_AppRegister> GetAppRegisterByUserID(string UserID, string SiteName = "")
        {
            if (string.IsNullOrEmpty(SiteName)) SiteName = AppConfig.SSORegisterID;
            object[] results = this.Invoke("GetAppRegisterByUserID", new object[] { UserID, SiteName });
            return (List<SSO_AppRegister>)(results[0]);
        }

        #endregion
    }
}

using Manage.Core.Entitys;
using Manage.SSO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Facades.SSOFactory
{
    /// <summary>
    /// 验证中心系统与本系统实体转换
    /// </summary>
    public static class ModelAdapter
    {
        #region User
        public static UserEntity SSOAdapter(this SSO_Operator sso_operator)
        {
            UserEntity model = new UserEntity();
            if (sso_operator != null)
            {
                model.ID = sso_operator.USER_ID.ToString();
                model.UserLoginName = sso_operator.VC_OP_NAME;
                model.UserDisplayName = sso_operator.VC_COMPANY_NAME;
                model.UserPassword = sso_operator.VC_OP_PASSWORD;
                model.UserPhone = sso_operator.VC_MOBILE;
                model.RecordStatus = sso_operator.C_OP_STATUS.Convert<int>(1);
                model.UserType = (int)UserType.Users;
                model.Extend2 = sso_operator.VC_EMAIL;
                model.Extend4 = sso_operator.VC_TEL;
            }
            return model;
        }

        public static List<UserEntity> SSOAdapter(this List<SSO_Operator> sso_operators)
        {
            List<UserEntity> list = new List<UserEntity>();
            foreach (var model in sso_operators)
            {
                list.Add(model.SSOAdapter());
            }
            return list;
        }
        #endregion

        #region App
        public static SYS_AppRegisterEntity SSOAdapter(this SSO_AppRegister ssoAppRegister)
        {
            SYS_AppRegisterEntity model = new SYS_AppRegisterEntity();
            if (ssoAppRegister != null)
            {
                model.ID = ssoAppRegister.ID;
                model.AppRegisterID = ssoAppRegister.AppRegisterID;
                model.AppName = ssoAppRegister.AppName;
                model.LoginVerifiedUrl = ssoAppRegister.LoginVerifiedUrl;
                model.HomePageUrl = ssoAppRegister.HomePageUrl;
                model.OrderNum = ssoAppRegister.OrderNum;
                model.RecordStatus = ssoAppRegister.RecordStatus;
            }
            return model;
        }

        public static List<SYS_AppRegisterEntity> SSOAdapter(this List<SSO_AppRegister> ssoAppRegister)
        {
            List<SYS_AppRegisterEntity> list = new List<SYS_AppRegisterEntity>();
            foreach (var model in ssoAppRegister)
            {
                list.Add(model.SSOAdapter());
            }
            return list;
        }
        #endregion
    }
}

using Manage.Core.Common;
using Manage.Core.Models;
using Manage.Core.SSO;
using Manage.Framework;
using Manage.SSO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Facades
{
    /// <summary>
    /// 管理员登录业务处理器
    /// </summary>
    public class AdminFacade : IAdminFacade
    {

        /// <summary>
        /// 根据用户登录帐号获取用户对象
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public UserInfo GetUserByLoginName(string LoginName)
        {
            using (var factory = new BaseAccess())
            {
                var model = factory.Single<UserEntity>(Specification<UserEntity>.Create(c => c.UserLoginName == LoginName && c.RecordStatus != (int)RecordStatus.UnEnable
                    && c.UserType == (int)UserType.Administrators));
                if (model != null)
                    return model.Adapter<UserInfo>(new UserInfo());
                else
                    return new UserInfo();
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ChangePwd(string UserID, string password)
        {
            using (var factory = new BaseAccess())
            {
                var user = factory.GetSingle<UserEntity>(Specification<UserEntity>.Create(c => c.ID == UserID));
                string newPassword = (user.UserLoginName + Define._PASSWORDSPLIT + password).ToMD5();
                if (user != null)
                {
                    user.UserPassword = newPassword;
                    factory.Update<UserEntity>(user);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 对比密码是否正确
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public bool ComparePassword(string UserID, string Password)
        {
            using (var factory = new BaseAccess())
            {
                var user = factory.GetSingle<UserEntity>(Specification<UserEntity>.Create(c => c.ID == UserID));
                if (user != null)
                {
                    //旧密码
                    string oldpwd = (user.UserLoginName + Define._PASSWORDSPLIT + Password).ToMD5();
                    return user.UserPassword == oldpwd;
                }
                return false;
            }
        }
    }
}

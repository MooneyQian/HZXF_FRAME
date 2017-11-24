using Manage.Core.Models;
using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Facades
{
    public interface IAppRegisterFacade : IBaseFacade
    {
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="Role"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        List<AppRegister_S> GetRolePaged(AppRegister_S app, PageInfo pi);

        /// <summary>
        /// 获取用户有权限系统
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        List<AppRegister_S> GetAppRegisterByUserID(string UserID);

        /// <summary>
        /// 设置角色系统
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleID"></param>
        /// <param name="IsHas">ture设置 false移除</param>
        void SetAppRole(string AppID, string RoleID, bool IsHas);

        /// <summary>
        /// 系统是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        bool _IsExists(string ID, string AppRegisterID);
    }
}

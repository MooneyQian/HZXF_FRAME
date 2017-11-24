using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;
using Manage.Core.Models;
using Manage.SSO.Entity;
using Manage.Core.DAL;
using Manage.Open;
using Manage.Core.Entitys;
using Manage.Core.Facades.SSOFactory;

namespace Manage.Core.Facades
{
    public class SSOAppRegisterFacade : SSOBaseFacade, IAppRegisterFacade
    {
        #region 不实现
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="Role"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public List<AppRegister_S> GetRolePaged(AppRegister_S app, PageInfo pi)
        {
            //不实现
            return null;
        }
        /// <summary>
        /// 设置角色系统
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleID"></param>
        /// <param name="IsHas">ture设置 false移除</param>
        public void SetAppRole(string AppID, string RoleID, bool IsHas)
        {
            //不实现
        }

        /// <summary>
        /// 系统是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public bool _IsExists(string ID, string AppRegisterID)
        {
            //不实现
            return false;
        }
        #endregion

        /// <summary>
        /// 获取用户有权限系统
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public List<AppRegister_S> GetAppRegisterByUserID(string UserID)
        {
            try
            {
                SSODataFactory factory = new SSODataFactory();
                var obj = factory.GetAppRegisterByUserID(UserID);
                var model = obj.SSOAdapter();
                return model.Adapter<SYS_AppRegisterEntity, AppRegister_S>(new List<AppRegister_S>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

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

namespace Manage.Core.Facades
{
    public class AppRegisterFacade : BaseFacade<SYS_AppRegisterEntity>, IAppRegisterFacade
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppRegisterFacade()
        {
        }
        /// <summary>
        /// 构造函数，传数据库配置文件
        /// </summary>
        /// <param name="dbConfigPath"></param>
        public AppRegisterFacade(string dbConfigPath)
        {
            base._DBConfigPath = dbConfigPath;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="Role"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public List<AppRegister_S> GetRolePaged(AppRegister_S app, PageInfo pi)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                var spec = Specification<SYS_AppRegisterEntity>.Create(c => true);
                if (!string.IsNullOrEmpty(app.AppName))
                    spec &= Specification<SYS_AppRegisterEntity>.Create(c => c.AppName.Contains(app.AppName));
                if (!string.IsNullOrEmpty(app.AppRegisterID))
                    spec &= Specification<SYS_AppRegisterEntity>.Create(c => c.AppRegisterID.Contains(app.AppRegisterID));

                var list = factory.GetPage<SYS_AppRegisterEntity>(pi, spec, c => c.OrderNum, SortOrder.Ascending);
                return (list ?? new List<SYS_AppRegisterEntity>()).Adapter<SYS_AppRegisterEntity, AppRegister_S>(new List<AppRegister_S>());
            }
        }

        /// <summary>
        /// 获取用户有权限系统
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public List<AppRegister_S> GetAppRegisterByUserID(string UserID)
        {
            using (AppRegisterAccess access = new AppRegisterAccess(base._DBConfigPath))
            {
                var list = access.GetAppRegistersByUserID(UserID) ?? new List<SYS_AppRegisterEntity>();
                return list.Adapter<SYS_AppRegisterEntity, AppRegister_S>(new List<AppRegister_S>());
            }
        }

        /// <summary>
        /// 设置角色系统
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleID"></param>
        /// <param name="IsHas">ture设置 false移除</param>
        public void SetAppRole(string AppID, string RoleID, bool IsHas)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                var appRole = factory.GetSingle<SYS_AppRoleEntity>(Specification<SYS_AppRoleEntity>.Create(c => c.AppID == AppID && c.RoleID == RoleID));
                if (IsHas)
                {
                    //增加
                    if (appRole == null || string.IsNullOrEmpty(appRole.ID))
                    {
                        appRole = new SYS_AppRoleEntity() { RoleID = RoleID, AppID = AppID };
                        factory.Insert<SYS_AppRoleEntity>(appRole);
                    }
                }
                else
                {
                    //移除
                    if (appRole != null && !string.IsNullOrEmpty(appRole.ID))
                    {
                        factory.Delete<SYS_AppRoleEntity>(appRole);
                    }
                }

            }
        }

        /// <summary>
        /// 系统是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public bool _IsExists(string ID, string AppRegisterID)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                var spec = Specification<SYS_AppRegisterEntity>.Create(c => c.AppRegisterID == AppRegisterID);
                if (!string.IsNullOrEmpty(ID))
                    spec &= Specification<SYS_AppRegisterEntity>.Create(c => c.ID != ID);
                return factory.IsExists<SYS_AppRegisterEntity>(spec);
            }
        }

    }
}

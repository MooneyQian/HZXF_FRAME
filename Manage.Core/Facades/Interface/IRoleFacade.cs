using System;
using Manage.Core.Models;
using System.Collections.Generic;
using Manage.Framework;
using Manage.Core.SSO;
using Manage.SSO.Entity;

namespace Manage.Core.Facades
{
    public interface IRoleFacade : IBaseFacade
    {

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="Role"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        List<Role_S> GetRolePaged(Role_S Role, PageInfo pi);


        /// <summary>
        /// 获取角色分配页面数据
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        List<Role_S> GetAllRoleWithUser(string UserID, string RoleName);

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleID"></param>
        /// <param name="IsHas">ture设置 false移除</param>
        void SetUserRole(string UserID, string RoleID);
        
        /// <summary>
        /// 角色是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        bool _IsExists(string ID, string RoleName);
        
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        void Add(Role_I model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        void Edit(Role_U model);

        /// <summary>
        /// 删除多个对象
        /// </summary>
        /// <param name="IDs">需要删除数据的ID，使用“,”分隔</param>
        void Del(string IDs);

        #region MembershipFactory

        /// <summary>
        /// 获取所有角色，从数据库取
        /// </summary>
        /// <returns></returns>
        List<RoleInfo> GetAllRoles();

        /// <summary>
        /// 通过角色ID获取角色，从数据库中取
        /// </summary>
        /// <returns></returns>
        RoleInfo GetRoleByID(string ID);

        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        List<UserRoleEntity> GetRolesByUser(string UserID);

        #endregion
    }
}

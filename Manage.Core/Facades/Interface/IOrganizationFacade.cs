using System;
using Manage.Core.Models;
using System.Collections.Generic;
using Manage.Framework;
using Manage.SSO.Entity;
using Manage.Core.SSO;
namespace Manage.Core.Facades
{
    public interface IOrganizationFacade : IBaseFacade
    {
        /// <summary>
        /// 判断分组编号是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="OrganNO"></param>
        /// <returns></returns>
        bool IsExists(string ID, string OrganNO);

        
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        void Add(Organization_I model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        void Edit(Organization_U model);

        /// <summary>
        /// 删除多个对象
        /// </summary>
        /// <param name="IDs">需要删除数据的ID，使用“,”分隔</param>
        void Del(string IDs);


        #region MembershipFactory

        /// <summary>
        /// 获取所有分组，从数据库取
        /// </summary>
        /// <returns></returns>
        List<OrganizationInfo> GetAllOrgans();
        /// <summary>
        /// 获取所有分组，从数据库取
        /// </summary>
        /// <returns></returns>
        List<OrganizationInfo> GetAllOrgans(Organization_S model);

        /// <summary>
        /// 通过分组ID获取分组，从数据库中取
        /// </summary>
        /// <returns></returns>
        OrganizationInfo GetOrganByID(string ID);
        /// <summary>
        /// 通过分组名称获取分组，从数据库中取
        /// </summary>
        /// <param name="OrgainName"></param>
        /// <returns></returns>
        OrganizationInfo GetOrganByName(string OrgainName);

        /// <summary>
        /// 获取用户的默认分组
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        UserOrganizationEntity GetDefaultOrganByUser(string UserID);
        /// <summary>
        /// 获取用户的所在的分组
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        List<UserOrganizationEntity> GetOrgansByUser(string UserID);

        /// <summary>
        /// 获取分组用户列表
        /// </summary>
        /// <param name="OrganizationID"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        List<OrganUser_S> GetUserWithOrgan(string OrganizationID, string LoginName, string DisplayName);
        /// <summary>
        /// 获取分组用户列表
        /// </summary>
        /// <param name="OrganizationID"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        List<UserOrganizationEntity> GetUserWithOrgan(string OrganizationID, string UserID);

        
        /// <summary>
        /// 设置分组用户
        /// </summary>
        /// <param name="OrganID"></param>
        /// <param name="UserID"></param>
        /// <param name="IsHas">ture设置 false移除</param>
        void SetUserOrgan(string OrganID, string UserID, bool IsHas);

        /// <summary>
        /// 设置为主分组
        /// </summary>
        /// <param name="OrganID"></param>
        /// <param name="UserID"></param>
        void SetDefaultOrgan(string OrganID, string UserID);

        #endregion
    }
}

using Manage.Framework;
using Manage.Open;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Manage.Core.SSO
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class UserInfo : BaseModel
    {
        public UserInfo()
        {
            //设置数据获取方式
            DefaultOrganization = new Lazy<OrganizationInfo>(() =>
            {
                return MembershipFactory.Instance.GetDefaultOrganByUser(ID);
            }, true);
            Organizations = new Lazy<List<OrganizationInfo>>(() =>
            {
                return MembershipFactory.Instance.GetOrgansByUser(ID);
            }, true);
            UserRoles = new Lazy<List<RoleInfo>>(() =>
            {
                return MembershipFactory.Instance.GetRolesByUser(ID);
            }, true);
            UserAllMenus = new Lazy<List<MenuInfo>>(() =>
            {
                return MembershipFactory.Instance.GetMenusByUserID(ID);
            }, true);
        }
        //public string ID { get; set; }//继承

        /// <summary>
        /// 登录名
        /// </summary>
        public string UserLoginName { set; get; }

        /// <summary>
        /// 用户显示名称
        /// </summary>
        public string UserDisplayName { set; get; }

        /// <summary>
        /// 用户登录密码
        /// </summary>
        public string UserPassword { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string UserPhone { set; get; }

        /// <summary>
        /// 状态:1启用 0停用
        /// </summary>		
        public int RecordStatus { get; set; }

        /// <summary>
        /// 用户类型:-1超级管理员 1用户 2普通管理员 99匿名用户
        /// </summary>		
        public int UserType { get; set; }

        /// <summary>
        /// 默认组织机构
        /// </summary>
        public Lazy<OrganizationInfo> DefaultOrganization { get; set; }

        /// <summary>
        /// 用户所有组织机构
        /// </summary>
        public Lazy<List<OrganizationInfo>> Organizations { get; set; }

        /// <summary>
        /// 用户角色关系视图
        /// </summary>
        public Lazy<List<RoleInfo>> UserRoles { get; set; }

        /// <summary>
        /// 用户角色关系视图(包括菜单和功能)
        /// </summary>
        public Lazy<List<MenuInfo>> UserAllMenus { get; set; }


        #region 冗余字段

        /// <summary>
        /// 冗余1
        /// </summary>
        public string Extend1 { set; get; }
        /// <summary>
        /// 冗余2
        /// </summary>
        public string Extend2 { set; get; }
        /// <summary>
        /// 冗余3
        /// </summary>
        public string Extend3 { set; get; }
        /// <summary>
        /// 冗余4
        /// </summary>
        public string Extend4 { set; get; }
        /// <summary>
        /// 冗余5
        /// </summary>
        public string Extend5 { set; get; }

        #endregion
    }
}

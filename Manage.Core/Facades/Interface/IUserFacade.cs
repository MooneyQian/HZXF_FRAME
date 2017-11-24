using System;
using Manage.Core.Models;
using System.Collections.Generic;
using Manage.Framework;
using Manage.Core.SSO;
namespace Manage.Core.Facades
{
    public interface IUserFacade
    {
        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        User_S GetUserByID(string ID);
        
        /// <summary>
        /// 获取用户分页数据
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        List<User_S> GetUserPaged(User_S user, PageInfo pi);
        
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        List<User_S> GetAllUsers(User_S user);
        
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool AddUser(User_I user);

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool EditUser(User_U user);
        
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="IDs">用户ID，逗号“,”隔开</param>
        /// <returns></returns>
        bool DelUsers(string IDs);
        
        /// <summary>
        /// 判断登录名是否被占用
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        bool IsExists(string LoginName);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ChangePwd(string UserID, string password);
        /// <summary>
        /// 对比密码是否正确
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        bool ComparePassword(string UserID, string Password);


        #region MembershipFactory

        /// <summary>
        /// 根据用户ID获取用户对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        UserInfo GetUserInfoByID(string ID);
        /// <summary>
        /// 根据用户登录帐号获取用户对象
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        UserInfo GetUserByLoginName(string LoginName);
        
        /// <summary>
        /// 获取所有用户数据
        /// </summary>
        /// <param name="LoginNameOrDisplayName"></param>
        /// <returns></returns>
        List<UserInfo> GetAllUsers(string LoginName, string DisplayName);

        /// <summary>
        /// 获取用户分页数据
        /// </summary>
        /// <param name="LoginNameOrDisplayName"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        List<UserInfo> GetUserPaged(string LoginName, string DisplayName, PageInfo pi);

        #endregion
    }
}

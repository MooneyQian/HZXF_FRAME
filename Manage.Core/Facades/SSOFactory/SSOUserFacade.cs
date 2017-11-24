using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;
using Manage.Core.Models;
using Manage.SSO.Entity;
using Manage.Core.Common;
using Manage.Core.SSO;
using Manage.Core.Facades.SSOFactory;

namespace Manage.Core.Facades.SSOFactory
{
    public class SSOUserFacade : SSOBaseFacade, IUserFacade
    {
        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public User_S GetUserByID(string ID)
        {
            try
            {
                SSODataFactory factory = new SSODataFactory();
                var obj = factory.GetUserByID(ID);
                var user = obj.SSOAdapter();
                return user.Adapter<User_S>(new User_S());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取用户分页数据
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public List<User_S> GetUserPaged(User_S user, PageInfo pi)
        {
            try
            {
                SSODataFactory factory = new SSODataFactory();
                var obj = factory.GetUserPaged(user.UserLoginName, user.UserDisplayName, pi.PageSize, pi.PageIndex);
                var model = obj.SSOAdapter();
                return model.Adapter<UserEntity, User_S>(new List<User_S>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public List<User_S> GetAllUsers(User_S user)
        {
            try
            {
                SSODataFactory factory = new SSODataFactory();
                var obj = factory.GetAllUsers(user.UserLoginName, user.UserDisplayName);
                var model = obj.SSOAdapter();
                return model.Adapter<UserEntity, User_S>(new List<User_S>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断登录名是否被占用
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public bool IsExists(string LoginName)
        {
            try
            {
                SSODataFactory factory = new SSODataFactory();
                var user = factory.GetUserByLoginName(LoginName);
                return user != null && user.USER_ID != null;
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                SSODataFactory factory = new SSODataFactory();
                return factory.ChangePwd(UserID, password);
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                SSODataFactory factory = new SSODataFactory();
                return factory.ComparePassword(UserID, Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region MembershipFactory

        /// <summary>
        /// 根据用户ID获取用户对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByID(string ID)
        {
            try
            {
                SSODataFactory factory = new SSODataFactory();
                var obj = factory.GetUserByID(ID);
                var user = obj.SSOAdapter();
                return user.Adapter<UserInfo>(new UserInfo());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据用户登录帐号获取用户对象
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public UserInfo GetUserByLoginName(string LoginName)
        {
            try
            {
                SSODataFactory factory = new SSODataFactory();
                var obj = factory.GetUserByLoginName(LoginName);
                var user = obj.SSOAdapter();
                return user.Adapter<UserInfo>(new UserInfo());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取用户分页数据
        /// </summary>
        /// <param name="LoginNameOrDisplayName"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserPaged(string LoginName, string DisplayName, PageInfo pi)
        {
            try
            {
                SSODataFactory factory = new SSODataFactory();
                var obj = factory.GetUserPaged(LoginName, DisplayName, pi.PageSize, pi.PageIndex);
                var model = obj.SSOAdapter();
                return model.Adapter<UserEntity, UserInfo>(new List<UserInfo>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有用户数据
        /// </summary>
        /// <param name="LoginNameOrDisplayName"></param>
        /// <returns></returns>
        public List<UserInfo> GetAllUsers(string LoginName, string DisplayName)
        {
            try
            {
                SSODataFactory factory = new SSODataFactory();
                var obj = factory.GetAllUsers(LoginName, DisplayName);
                var model = obj.SSOAdapter();
                return model.Adapter<UserEntity, UserInfo>(new List<UserInfo>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 不实现
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(User_I user)
        {
            //不实现
            return false;
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool EditUser(User_U user)
        {
            //不实现
            return false;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="IDs">用户ID，逗号“,”隔开</param>
        /// <returns></returns>
        public bool DelUsers(string IDs)
        {
            //不实现
            return false;
        }

        #endregion

    }
}

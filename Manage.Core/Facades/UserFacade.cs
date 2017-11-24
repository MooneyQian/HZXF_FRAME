using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;
using Manage.Core.Models;
using Manage.SSO.Entity;
using Manage.Core.Common;
using Manage.Core.SSO;

namespace Manage.Core.Facades
{
    public class UserFacade : BaseFacade<UserEntity>, IUserFacade
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserFacade()
        {
        }
        /// <summary>
        /// 构造函数，传数据库配置文件
        /// </summary>
        /// <param name="dbConfigPath"></param>
        public UserFacade(string dbConfigPath)
        {
            base._DBConfigPath = dbConfigPath;
        }

        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public User_S GetUserByID(string ID)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                var user = factory.GetSingle<UserEntity>(Specification<UserEntity>.Create(c => c.ID == ID)).Adapter<User_S>(new User_S()) ?? new User_S();
                user.UserDept = Manage.Open.MembershipFactory.Instance.GetDefaultOrganByUser(user.ID).Adapter<Organization_S>(new Organization_S());
                return user;
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
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                var spec = Specification<UserEntity>.Create(c => c.UserType != (int)UserType.Administrators);
                if (!string.IsNullOrEmpty(user.UserDisplayName))
                    spec &= Specification<UserEntity>.Create(c => c.UserDisplayName.Contains(user.UserDisplayName));
                if (!string.IsNullOrEmpty(user.UserLoginName))
                    spec &= Specification<UserEntity>.Create(c => c.UserLoginName.Contains(user.UserLoginName));

                var list = factory.GetPage<UserEntity>(pi, spec, c => c.UserDisplayName, SortOrder.Ascending);
                return (list ?? new List<UserEntity>()).Adapter<UserEntity, User_S>(new List<User_S>());
            }
        }

        /// <summary>
        /// 获取所有用户数据
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public List<User_S> GetAllUsers(User_S user)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                var spec = Specification<UserEntity>.Create(c => c.UserType != (int)UserType.Administrators);
                if (!string.IsNullOrEmpty(user.UserDisplayName))
                    spec &= Specification<UserEntity>.Create(c => c.UserDisplayName.Contains(user.UserDisplayName));
                if (!string.IsNullOrEmpty(user.UserLoginName))
                    spec &= Specification<UserEntity>.Create(c => c.UserLoginName.Contains(user.UserLoginName));

                var list = factory.GetAll<UserEntity>(spec, c => c.UserDisplayName, SortOrder.Ascending);
                return (list ?? new List<UserEntity>()).Adapter<UserEntity, User_S>(new List<User_S>());
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(User_I user)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                try
                {
                    user.UserPassword = (user.UserLoginName + Define._PASSWORDSPLIT + appConfig.DefaultPassword).ToMD5();
                    user.ID = Guid.NewGuid().ToString();
                    var userDep = new UserOrganizationEntity()
                    {
                        ID = Guid.NewGuid().ToString(),
                        UserID = user.ID,
                        OrganizationID = user.UserDeptID,
                        IsDefault = 1
                    };
                    var model = user.Adapter<UserEntity>(new UserEntity());
                    factory.Insert<UserEntity>(model, false);
                    factory.Insert<UserOrganizationEntity>(userDep, false);
                    if (!string.IsNullOrWhiteSpace(user.Extend5))
                    {
                        var userRole = new UserRoleEntity();
                        userRole.UserID = user.ID;
                        userRole.RoleID = user.Extend5;
                        factory.Insert<UserRoleEntity>(userRole, false);
                    }
                    factory.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    factory.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool EditUser(User_U user)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                try
                {
                    var model = factory.Single<UserEntity>(user.ID);
                    model = user.Adapter<UserEntity>(model);//将页面对象user的属性转换到数据库对象modle中
                    factory.Update<UserEntity>(model, false);
                    //处理用户分组
                    var userDep = factory.Single<UserOrganizationEntity>(Specification<UserOrganizationEntity>
                        .Create(c => c.UserID == user.ID && c.IsDefault == 1));
                    if (userDep.OrganizationID != user.UserDeptID)
                    {
                        userDep.OrganizationID = user.UserDeptID;
                        factory.Update<UserOrganizationEntity>(userDep, false);
                    }
                    factory.Commit();
                    //清理缓存
                    Manage.Open.CacheshipFactory.Instance.ClearSSOUserCache(user.ID);
                    return true;
                }
                catch (Exception ex)
                {
                    factory.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="IDs">用户ID，逗号“,”隔开</param>
        /// <returns></returns>
        public bool DelUsers(string IDs)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                try
                {
                    string[] userIDs = IDs.Split(',');
                    factory.Delete<UserEntity>(userIDs, false);
                    //循环删除用户分组
                    var userOrgans = factory.GetAll<UserOrganizationEntity>(Specification<UserOrganizationEntity>.Create(c => userIDs.Contains(c.UserID)));
                    foreach (var userOrgan in userOrgans)
                    {
                        factory.Delete<UserOrganizationEntity>(userOrgan, false);
                    }
                    //循环删除用户角色
                    var userRoles = factory.GetAll<UserRoleEntity>(Specification<UserRoleEntity>.Create(c => userIDs.Contains(c.UserID)));
                    foreach (var userRole in userRoles)
                    {
                        factory.Delete<UserRoleEntity>(userRole, false);
                    }
                    factory.Commit();

                    //清理缓存
                    foreach (var id in userIDs)
                    {
                        Manage.Open.CacheshipFactory.Instance.ClearSSOUserCache(id);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    factory.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断登录名是否被占用
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public bool IsExists(string LoginName)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                return factory.IsExists<UserEntity>(Specification<UserEntity>.Create(c => c.UserLoginName == LoginName));
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

        #region MembershipFactory

        /// <summary>
        /// 根据用户ID获取用户对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByID(string ID)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                var model = factory.Single<UserEntity>(Specification<UserEntity>.Create(c => c.ID == ID && c.RecordStatus != (int)RecordStatus.UnEnable));
                if (model != null)
                    return model.Adapter<UserInfo>(new UserInfo());
                else
                    return new UserInfo();
            }
        }
        /// <summary>
        /// 根据用户登录帐号获取用户对象
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public UserInfo GetUserByLoginName(string LoginName)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                var model = factory.GetSingle<UserEntity>(Specification<UserEntity>.Create(c => c.UserLoginName == LoginName && c.RecordStatus != (int)RecordStatus.UnEnable));
                if (model != null)
                    return model.Adapter<UserInfo>(new UserInfo());
                else
                    return new UserInfo();
            }
        }

        /// <summary>
        /// 获取所有用户数据
        /// </summary>
        /// <param name="LoginNameOrDisplayName"></param>
        /// <returns></returns>
        public List<UserInfo> GetAllUsers(string LoginName, string DisplayName)
        {
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                var spec = Specification<UserEntity>.Create(c => c.UserType != (int)UserType.Administrators);
                if (!string.IsNullOrEmpty(DisplayName))
                    spec &= Specification<UserEntity>.Create(c => c.UserDisplayName.Contains(DisplayName));
                if (!string.IsNullOrEmpty(LoginName))
                    spec &= Specification<UserEntity>.Create(c => c.UserLoginName.Contains(LoginName));

                var list = factory.GetAll<UserEntity>(spec, c => c.UserDisplayName, SortOrder.Ascending);
                return (list ?? new List<UserEntity>()).Adapter<UserEntity, UserInfo>(new List<UserInfo>());
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
            using (var factory = new BaseAccess(base._DBConfigPath))
            {
                var spec = Specification<UserEntity>.Create(c => c.UserType != (int)UserType.Administrators);
                if (!string.IsNullOrEmpty(DisplayName))
                    spec &= Specification<UserEntity>.Create(c => c.UserDisplayName.Contains(DisplayName));
                if (!string.IsNullOrEmpty(LoginName))
                    spec &= Specification<UserEntity>.Create(c => c.UserLoginName.Contains(LoginName));

                var list = factory.GetPage<UserEntity>(pi, spec, c => c.UserDisplayName, SortOrder.Ascending);
                return (list ?? new List<UserEntity>()).Adapter<UserEntity, UserInfo>(new List<UserInfo>());
            }
        }
        #endregion

    }
}

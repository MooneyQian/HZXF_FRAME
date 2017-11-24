using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;
using Manage.Core.Models;
using Manage.SSO.Entity;
using Manage.Core.DAL;
using Manage.Open;
using Manage.Core.SSO;

namespace Manage.Core.Facades
{
    public class RoleFacade : BaseFacade<RoleEntity>, IRoleFacade
    {
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="Role"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public List<Role_S> GetRolePaged(Role_S Role, PageInfo pi)
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<RoleEntity>.Create(c => true);
                if (!string.IsNullOrEmpty(Role.RoleName))
                    spec &= Specification<RoleEntity>.Create(c => c.RoleName.Contains(Role.RoleName));

                var list = factory.GetPage<RoleEntity>(pi, spec, c => c.RoleName, SortOrder.Ascending);
                return (list ?? new List<RoleEntity>()).Adapter<RoleEntity, Role_S>(new List<Role_S>());
            }
        }

        /// <summary>
        /// 获取角色分配页面数据
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public List<Role_S> GetAllRoleWithUser(string UserID, string RoleName)
        {
            using (RoleAccess access = new RoleAccess())
            {
                var UserRoleIDs = access.GetUserRoles(UserID).Select(s => s.ID);
                var spec = Specification<RoleEntity>.Create(c => c.RecordStatus != (int)RecordStatus.UnEnable);
                if (!string.IsNullOrWhiteSpace(RoleName))
                    spec &= Specification<RoleEntity>.Create(c => c.RoleName.Contains(RoleName));
                var AllRoles = access.GetAll<RoleEntity>(spec);
                var list = AllRoles.Adapter<RoleEntity, Role_S>(new List<Role_S>());
                list.ForEach(f => f.IsHas = UserRoleIDs.Contains(f.ID));
                list = list.OrderBy(o => !o.IsHas).ThenBy(t => t.RoleName).ToList();
                return list;
            }
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleID"></param>
        /// <param name="IsHas">ture设置 false移除</param>
        public void SetUserRole(string UserID, string RoleID)
        {
            using (var factory = new BaseAccess())
            {
                //如果传入的roleid为空的话移除改用户对对应的所有角色
                if (string.IsNullOrWhiteSpace(RoleID))
                {
                    var entity = factory.GetAll<UserRoleEntity>(Specification<UserRoleEntity>.Create(c => c.UserID == UserID));
                    factory.Delete<UserRoleEntity>(entity);
                }
                else
                {
                    var entity = factory.GetAll<UserRoleEntity>(Specification<UserRoleEntity>.Create(c => c.UserID == UserID));
                    factory.Delete<UserRoleEntity>(entity);
                    var Rids = RoleID.Split(',');
                    foreach (var item in Rids)
                    {
                        var spec = Specification<UserRoleEntity>.Create(c => c.UserID == UserID && c.RoleID == item);
                        bool bl = factory.IsExists<UserRoleEntity>(spec);
                        if (!bl)
                        {
                            var userRole = new UserRoleEntity() { RoleID = item, UserID = UserID };
                            factory.Insert<UserRoleEntity>(userRole);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 角色是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public bool _IsExists(string ID, string RoleName)
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<RoleEntity>.Create(c => c.RoleName == RoleName);
                if (!string.IsNullOrEmpty(ID))
                    spec &= Specification<RoleEntity>.Create(c => c.ID != ID);
                return factory.IsExists<RoleEntity>(spec);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        public void Add(Role_I model)
        {
            using (var factory = new BaseAccess())
            {
                if (string.IsNullOrEmpty(model.ID))
                    model.ID = Guid.NewGuid().ToString();
                factory.Insert<RoleEntity>(model.Adapter<RoleEntity>(new RoleEntity()));

                //清理缓存
                CacheshipFactory.Instance.ClearRoleCache();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        public void Edit(Role_U model)
        {
            using (var factory = new BaseAccess())
            {
                if (!string.IsNullOrEmpty(model.ID))
                {
                    var model_old = factory.GetSingle<RoleEntity>(model.ID);
                    model_old = model.Adapter<RoleEntity>(model_old);//将页面对象的属性转换到数据库对象modle中
                    factory.Update<RoleEntity>(model_old);

                    //清理缓存
                    CacheshipFactory.Instance.ClearRoleCache();
                }
            }
        }

        /// <summary>
        /// 删除多个对象
        /// </summary>
        /// <param name="IDs">需要删除数据的ID，使用“,”分隔</param>
        public void Del(string IDs)
        {
            using (var factory = new BaseAccess())
            {
                factory.Delete<RoleEntity>(IDs);

                //清理缓存
                CacheshipFactory.Instance.ClearRoleCache();
            }
        }

        #region MembershipFactory

        /// <summary>
        /// 获取所有角色，从数据库取
        /// </summary>
        /// <returns></returns>
        public List<RoleInfo> GetAllRoles()
        {
            using (var factory = new BaseAccess())
            {
                return factory.GetAll<RoleEntity>(Specification<RoleEntity>.Create(c => c.RecordStatus != (int)RecordStatus.UnEnable)).Adapter<RoleEntity, RoleInfo>(new List<RoleInfo>());
            }
        }

        /// <summary>
        /// 通过角色ID获取角色，从数据库中取
        /// </summary>
        /// <returns></returns>
        public RoleInfo GetRoleByID(string ID)
        {
            using (var factory = new BaseAccess())
            {
                var model = factory.GetSingle<RoleEntity>(Specification<RoleEntity>.Create(c => c.ID == ID && c.RecordStatus != (int)RecordStatus.UnEnable));
                if (model != null)
                    return factory.GetSingle<RoleEntity>(Specification<RoleEntity>.Create(c => c.ID == ID && c.RecordStatus != (int)RecordStatus.UnEnable)).Adapter<RoleInfo>(new RoleInfo());
                else
                    return new RoleInfo();
            }
        }

        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<UserRoleEntity> GetRolesByUser(string UserID)
        {
            using (var factory = new BaseAccess())
            {
                return factory.GetAll<UserRoleEntity>(Specification<UserRoleEntity>.Create(c => c.UserID == UserID));
            }
        }

        #endregion
    }
}

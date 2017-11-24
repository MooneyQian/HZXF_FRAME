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
using Manage.Core.Common;

namespace Manage.Core.Facades
{
    public class MenuFacade : BaseFacade<MenuEntity>, IMenuFacade
    {

        /// <summary>
        /// 获取所有菜单（除管理员菜单）
        /// </summary>
        /// <returns></returns>
        public List<Menu_S> GetAllMenus()
        {
            using (var factory = new BaseAccess())
            {
                List<Orderby<MenuEntity>> orders = new List<Orderby<MenuEntity>>()
                {
                    new Orderby<MenuEntity>(c => c.MenuType, SortOrder.Ascending),
                    new Orderby<MenuEntity>(c => c.MenuOrder, SortOrder.Ascending)
                };
                //return factory.GetAll<MenuEntity>(Specification<MenuEntity>.Create(c => c.MenuType != (int)MenuType.Admin), orders)
                return factory.GetAll<MenuEntity>(Specification<MenuEntity>.Create(c => true), orders)
     .Adapter<MenuEntity, Menu_S>(new List<Menu_S>());
            }
        }

        /// <summary>
        /// 判断按钮菜单编号是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="MenuCode"></param>
        /// <param name="PerMenuID"></param>
        /// <returns></returns>
        public bool IsFuncExists(string ID, string MenuCode, string PerMenuID)
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<MenuEntity>.Create(c => c.PerMenuID == PerMenuID && c.MenuType == (int)MenuType.Function && c.MenuCode == MenuCode);
                if (!string.IsNullOrEmpty(ID))
                    spec &= Specification<MenuEntity>.Create(c => c.ID != ID);
                return factory.IsExists<MenuEntity>(spec);
            }
        }

        /// <summary>
        /// 判断数据权限编号是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="MenuCode"></param>
        /// <returns></returns>
        public bool IsDataExists(string ID, string MenuCode)
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<MenuEntity>.Create(c => c.MenuType == (int)MenuType.Data && c.MenuCode == MenuCode);
                if (!string.IsNullOrEmpty(ID))
                    spec &= Specification<MenuEntity>.Create(c => c.ID != ID);
                return factory.IsExists<MenuEntity>(spec);
            }
        }


        /// <summary>
        /// 获取角色菜单分配页面数据
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="MenuName"></param>
        /// <returns></returns>
        public List<Menu_S> GetAllMenuWithRole(string RoleID, string MenuName)
        {
            using (MenuAccess access = new MenuAccess())
            {
                var RoleMenuIDs = access.GetRoleMenus(RoleID).Select(s => s.ID);
                var spec = Specification<MenuEntity>.Create(c => c.MenuType != (int)MenuType.Admin && c.RecordStatus != (int)RecordStatus.UnEnable);
                if (!string.IsNullOrWhiteSpace(MenuName))
                    spec &= Specification<MenuEntity>.Create(c => c.MenuName.Contains(MenuName));
                var AllMenus = access.GetAll<MenuEntity>(spec);
                var list = AllMenus.Adapter<MenuEntity, Menu_S>(new List<Menu_S>());
                list.ForEach(f => f.IsHas = RoleMenuIDs.Contains(f.ID));
                list = list.OrderBy(o => !o.IsHas).ThenBy(t => t.MenuOrder).ToList();
                return list;
            }
        }

        /// <summary>
        /// 设置角色菜单
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="MenuIDs"></param>
        public void SetRoleMenus(string RoleID, List<string> MenuIDs)
        {
            using (var factory = new BaseAccess())
            {
                try
                {
                    if (!string.IsNullOrEmpty(RoleID))
                    {
                        var menus_old = factory.GetAll<MenuRoleEntity>(Specification<MenuRoleEntity>.Create(c => c.RoleID == RoleID));
                        factory.Delete<MenuRoleEntity>(menus_old, false);
                        if (MenuIDs != null)
                        {
                            foreach (string id in MenuIDs)
                            {
                                var entity = new MenuRoleEntity()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    RoleID = RoleID,
                                    MenuID = id
                                };
                                factory.Insert<MenuRoleEntity>(entity, false);
                            }
                        }
                        factory.Commit();
                    }
                }
                catch (Exception ex)
                {
                    factory.Rollback();
                    throw ex;
                }
            }
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        public void Add(Menu_I model)
        {
            using (var factory = new BaseAccess())
            {
                if (string.IsNullOrEmpty(model.ID))
                    model.ID = Guid.NewGuid().ToString();
                factory.Insert<MenuEntity>(model.Adapter<MenuEntity>(new MenuEntity()));

                //清理缓存
                if (model.MenuType == (int)MenuType.Function)
                    CacheshipFactory.Instance.ClearFunMenuCache();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        public void Edit(Menu_U model)
        {
            using (var factory = new BaseAccess())
            {
                if (!string.IsNullOrEmpty(model.ID))
                {
                    var model_old = factory.GetSingle<MenuEntity>(model.ID);
                    model_old = model.Adapter<MenuEntity>(model_old);//将页面对象的属性转换到数据库对象modle中
                    factory.Update<MenuEntity>(model_old);

                    //清理缓存
                    if (model_old.MenuType == (int)MenuType.Function)
                        CacheshipFactory.Instance.ClearFunMenuCache();
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
                try
                {
                    foreach (var id in IDs.Split(','))
                    {
                        var model = factory.GetSingle<MenuEntity>(id);
                        if (model != null)
                        {
                            //获取所有子节点
                            List<MenuEntity> childrens = new List<MenuEntity>() { model };
                            childrens = GetChilds(factory, model.ID, childrens);
                            foreach (var c in childrens)
                            {
                                factory.Delete<MenuEntity>(c, false);
                            }
                        }
                    }
                    factory.Commit();
                }
                catch (Exception ex)
                {
                    factory.Rollback();
                    throw ex;
                }
            }
        }
        private List<MenuEntity> GetChilds(BaseAccess factory, string PerMenuID, List<MenuEntity> entitys)
        {
            if (entitys == null)
                entitys = new List<MenuEntity>();
            var list = factory.GetAll<MenuEntity>(Specification<MenuEntity>.Create(c => c.PerMenuID == PerMenuID));
            entitys.AddRange(list);
            foreach (var m in list)
            {
                entitys.AddRange(GetChilds(factory, m.ID, entitys));
            }
            return entitys;
        }


        #region MembershipFactory

        /// <summary>
        /// 获取用户有权限的菜单
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<MenuInfo> GetMenusByUserID(string UserID)
        {
            using (var factory = new BaseAccess())
            {
                var list = factory.DbContext.AsQueryable<MenuEntity>().Where(w => w.RecordStatus != (int)RecordStatus.UnEnable)
                    .Join(factory.DbContext.AsQueryable<MenuRoleEntity>(), o => o.ID, i => i.MenuID,
                        (o, i) => new { Menu = o, MenuRole = i })
                    .Join(factory.DbContext.AsQueryable<UserRoleEntity>(), o2 => o2.MenuRole.RoleID, i2 => i2.RoleID,
                        (o2, i2) => new { Menu = o2.Menu, MenuRole = o2.MenuRole, UserRole = i2 })
                    .Join(factory.DbContext.AsQueryable<RoleEntity>(), o3 => o3.UserRole.RoleID, i3 => i3.ID,
                        (o3, i3) => new { Menu = o3.Menu, MenuRole = o3.MenuRole, UserRole = o3.UserRole, Role = i3 })
                    .Where(w => w.UserRole.UserID == UserID && w.Role.RecordStatus != (int)RecordStatus.UnEnable).Select(s => s.Menu)
                    .OrderBy(o => o.MenuLevel).ThenBy(t => t.MenuOrder).ToList() ?? new List<MenuEntity>();
                var listModel = list.Adapter<MenuEntity, MenuInfo>(new List<MenuInfo>());
                listModel = listModel.Distinct(new Compare<MenuInfo>(
                (x, y) => (null != x && null != y) && (x.ID == y.ID))).ToList();
                return listModel;
            }
        }

        /// <summary>
        /// 获取用户有权限的菜单(根据菜单类型)
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<MenuInfo> GetMenusByUserID(string UserID, MenuType menuType)
        {
            using (var factory = new BaseAccess())
            {
                var list = factory.DbContext.AsQueryable<MenuEntity>().Where(w => w.MenuType == (int)menuType && w.RecordStatus != (int)RecordStatus.UnEnable)
                    .Join(factory.DbContext.AsQueryable<MenuRoleEntity>(), o => o.ID, i => i.MenuID,
                        (o, i) => new { Menu = o, MenuRole = i })
                    .Join(factory.DbContext.AsQueryable<UserRoleEntity>(), o2 => o2.MenuRole.RoleID, i2 => i2.RoleID,
                        (o2, i2) => new { Menu = o2.Menu, MenuRole = o2.MenuRole, UserRole = i2 })
                    .Join(factory.DbContext.AsQueryable<RoleEntity>(), o3 => o3.UserRole.RoleID, i3 => i3.ID,
                        (o3, i3) => new { Menu = o3.Menu, MenuRole = o3.MenuRole, UserRole = o3.UserRole, Role = i3 })
                    .Where(w => w.UserRole.UserID == UserID && w.Role.RecordStatus != (int)RecordStatus.UnEnable).Select(s => s.Menu)
                    .OrderBy(o => o.MenuLevel).ThenBy(t => t.MenuOrder).ToList() ?? new List<MenuEntity>();
                return list.Adapter<MenuEntity, MenuInfo>(new List<MenuInfo>());
            }
        }

        /// <summary>
        /// 获取所有或页面内的所有需要验证的功能按钮
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public List<MenuInfo> GetFunMenuByMenuID(string MenuID = "")
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<MenuEntity>.Create(c => c.RecordStatus != (int)RecordStatus.UnEnable && c.MenuType == (int)MenuType.Function);
                if (!string.IsNullOrEmpty(MenuID))
                    spec &= Specification<MenuEntity>.Create(c => c.PerMenuID == MenuID);
                return factory.GetAll<MenuEntity>(spec).Adapter<MenuEntity, MenuInfo>(new List<MenuInfo>());
            }
        }

        /// <summary>
        /// 获取所有菜单（MenuType为Menu的菜单）
        /// </summary>
        /// <returns></returns>
        public List<MenuInfo> GetAllMenuInfos()
        {
            using (var factory = new BaseAccess())
            {
                return factory.GetAll<MenuEntity>(Specification<MenuEntity>.Create(c => c.MenuType == (int)MenuType.Menu && c.RecordStatus != (int)RecordStatus.UnEnable))
                    .Adapter<MenuEntity, MenuInfo>(new List<MenuInfo>());
            }
        }

        /// <summary>
        /// 获取超级管理员菜单
        /// </summary>
        /// <returns></returns>
        public List<MenuInfo> GetAdminMenu()
        {
            using (var factory = new BaseAccess())
            {
                var list = factory.GetAll<MenuEntity>(Specification<MenuEntity>.Create(c => c.MenuType == (int)MenuType.Admin && c.RecordStatus != (int)RecordStatus.UnEnable)) ?? new List<MenuEntity>();
                return list.Adapter<MenuEntity, MenuInfo>(new List<MenuInfo>());
            }
        }

        #endregion
    }
}

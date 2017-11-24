using Manage.Framework;
using Manage.SSO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.DAL
{
    public class MenuAccess : BaseAccess
    {
        /// <summary>
        /// 获取角色菜单
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public List<MenuEntity> GetRoleMenus(string RoleID)
        {
            return DbContext.AsQueryable<MenuEntity>().Join(DbContext.AsQueryable<MenuRoleEntity>(), o => o.ID, i => i.MenuID,
                (o, i) => new { Menu = o, MenuRole = i }).Where(w => w.MenuRole.RoleID == RoleID).Select(s => s.Menu).ToList();
        }
        
    }
}

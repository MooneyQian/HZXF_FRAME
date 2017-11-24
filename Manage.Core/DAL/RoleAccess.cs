using Manage.Framework;
using Manage.SSO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.DAL
{
    public class RoleAccess : BaseAccess
    {
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<RoleEntity> GetUserRoles(string UserID)
        {
            return DbContext.AsQueryable<RoleEntity>().Join(DbContext.AsQueryable<UserRoleEntity>(), o => o.ID, i => i.RoleID,
                (o, i) => new { Role = o, UserRole = i }).Where(w => w.UserRole.UserID == UserID).Select(s => s.Role).ToList();
        }
    }
}

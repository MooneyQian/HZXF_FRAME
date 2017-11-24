using Manage.Core.Entitys;
using Manage.Framework;
using Manage.SSO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.DAL
{
    public class AppRegisterAccess : BaseAccess
    {
        public AppRegisterAccess()
        {
        }

        public AppRegisterAccess(string dbConfigPath)
            : base(dbConfigPath)
        {
        }
        /// <summary>
        /// 获取用户系统
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<SYS_AppRegisterEntity> GetAppRegistersByUserID(string UserID)
        {
            return DbContext.AsQueryable<SYS_AppRegisterEntity>().Where(c => c.RecordStatus != (int)RecordStatus.UnEnable)
                .Join(DbContext.AsQueryable<SYS_AppRoleEntity>(), o => o.ID, i => i.AppID, (o, i) => new { App = o, AppRole = i })
                .Join(DbContext.AsQueryable<UserRoleEntity>(), oo => oo.AppRole.RoleID, ii => ii.RoleID, (oo, ii) => new { App = oo.App, AppRole = oo.AppRole, UserRole = ii })
                .Where(w => w.UserRole.UserID == UserID).Select(s => s.App).OrderBy(b => b.OrderNum).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.SSO.Entity
{
    /// <summary>
    /// 菜单角色关系表
    /// </summary>
    [Serializable]
    public class MenuRoleEntity : AggregateRoot
    {
        //public override string ID { get; set; } //父类继承

        /// <summary>
        /// 菜单ID
        /// </summary>
        public virtual string MenuID { set; get; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual string RoleID { set; get; }
    }
}

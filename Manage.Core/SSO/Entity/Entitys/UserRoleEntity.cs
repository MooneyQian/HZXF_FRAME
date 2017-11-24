using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.SSO.Entity
{
    /// <summary>
    /// 用户角色关系表
    /// </summary>
    [Serializable]
    public class UserRoleEntity : AggregateRoot
    {
        //public override string ID { get; set; } //父类继承

        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual string UserID { set; get; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual string RoleID { set; get; }

        public virtual RoleEntity Role { get; set; }
    }
}

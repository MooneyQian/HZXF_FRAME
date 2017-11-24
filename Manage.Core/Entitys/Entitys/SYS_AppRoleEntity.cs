using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Entitys
{
    /// <summary>
    /// 系统角色
    /// </summary>
    [Serializable]
    [System.ComponentModel.Description("系统角色")]
    public class SYS_AppRoleEntity : AggregateRoot
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual string RoleID { get; set; }

        /// <summary>
        /// 系统ID
        /// </summary>
        public virtual string AppID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.SSO.Entity
{
    /// <summary>
    /// 用户组织机构关系表
    /// </summary>
    [Serializable]
    public class UserOrganizationEntity : AggregateRoot
    {
        //public override string ID { get; set; } //父类继承

        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual string UserID { set; get; }

        /// <summary>
        /// 组织机构ID
        /// </summary>
        public virtual string OrganizationID { set; get; }

        /// <summary>
        /// 是否是主分组
        /// </summary>
        public virtual int IsDefault { get; set; }

        public virtual OrganizationEntity Organization { get; set; }
        public virtual UserEntity User { get; set; }
    }
}

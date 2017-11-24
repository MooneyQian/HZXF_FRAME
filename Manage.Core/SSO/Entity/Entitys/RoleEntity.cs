using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.SSO.Entity
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Serializable]
    public class RoleEntity :  AggregateRoot
    {
        //public override string ID { get; set; } //父类继承

        /// <summary>
        /// 角色名称
        /// </summary>		
        public virtual string RoleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>		
        public virtual string RoleDesc { get; set; }


        /// <summary>
        /// 状态:1启用 0停用
        /// </summary>		
        public virtual int RecordStatus { get; set; }


        #region 冗余字段

        /// <summary>
        /// 冗余1
        /// </summary>
        public virtual string Extend1 { set; get; }
        /// <summary>
        /// 冗余2
        /// </summary>
        public virtual string Extend2 { set; get; }
        /// <summary>
        /// 冗余3
        /// </summary>
        public virtual string Extend3 { set; get; }
        /// <summary>
        /// 冗余4
        /// </summary>
        public virtual string Extend4 { set; get; }
        /// <summary>
        /// 冗余5
        /// </summary>
        public virtual string Extend5 { set; get; }

        #endregion
    }
}

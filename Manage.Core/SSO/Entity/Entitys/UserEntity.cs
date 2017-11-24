using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.SSO.Entity
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Serializable]
    public class UserEntity : AggregateRoot
    {
        //public override string ID { get; set; } //父类继承

        /// <summary>
        /// 登录名
        /// </summary>
        public virtual string UserLoginName { set; get; }

        /// <summary>
        /// 用户显示名称
        /// </summary>
        public virtual string UserDisplayName { set; get; }

        /// <summary>
        /// 用户登录密码
        /// </summary>
        public virtual string UserPassword { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        public virtual string UserPhone { set; get; }

        /// <summary>
        /// 状态:1启用 0停用
        /// </summary>		
        public virtual int RecordStatus { get; set; }

        /// <summary>
        /// 用户类型:-1超级管理员 1用户 2普通管理员 99匿名用户
        /// </summary>		
        public virtual int UserType { get; set; }


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

        #region 扩展字段
        #endregion
    }
}

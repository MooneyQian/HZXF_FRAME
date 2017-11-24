using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Manage.Core.Models
{
    /// <summary>
    /// 用户表-Select
    /// </summary>
    [DataContract]
    public class User_S : BaseModel
    {
        public User_S() { }

        //public string ID { get; set; } //父类继承

        /// <summary>
        /// 登录名
        /// </summary>
        public string UserLoginName { set; get; }

        /// <summary>
        /// 用户显示名称
        /// </summary>
        public string UserDisplayName { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string UserPhone { set; get; }

        /// <summary>
        /// 状态:1启用 0停用
        /// </summary>		
        public int RecordStatus { get; set; }

        /// <summary>
        /// 用户类型:-1超级管理员 1用户 2普通管理员 99匿名用户
        /// </summary>		
        public int UserType { get; set; }

        /// <summary>
        /// 用户分组
        /// </summary>
        public Organization_S UserDept { set; get; }
        
        #region 冗余字段

        /// <summary>
        /// 冗余1
        /// </summary>
        public string Extend1 { set; get; }
        /// <summary>
        /// 冗余2
        /// </summary>
        public string Extend2 { set; get; }
        /// <summary>
        /// 冗余3
        /// </summary>
        public string Extend3 { set; get; }
        /// <summary>
        /// 冗余4
        /// </summary>
        public string Extend4 { set; get; }
        /// <summary>
        /// 冗余5
        /// </summary>
        public string Extend5 { set; get; }

        #endregion
    }
}

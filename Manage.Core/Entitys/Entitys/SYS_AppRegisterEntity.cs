using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Entitys
{
    /// <summary>
    /// 系统注册表
    /// </summary>
    [Serializable]
    [System.ComponentModel.Description("系统注册表")]
    public class SYS_AppRegisterEntity : AggregateRoot
    {
        /// <summary>
        /// 注册系统号
        /// </summary>
        public virtual string AppRegisterID { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public virtual string AppName { get; set; }

        /// <summary>
        /// 系统登录校验地址
        /// </summary>
        public virtual string LoginVerifiedUrl { get; set; }

        /// <summary>
        /// 首页地址
        /// </summary>
        public virtual string HomePageUrl { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int? OrderNum { get; set; }

        /// <summary>
        /// 状态:1启用 0停用
        /// </summary>
        public virtual int? RecordStatus { get; set; }
    }
}

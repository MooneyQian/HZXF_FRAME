using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Models
{
    /// <summary>
    /// 系统注册表
    /// </summary>
    [Serializable]
    public class AppRegister_U : BaseModel
    {
        /// <summary>
        /// 注册系统号
        /// </summary>
        public string AppRegisterID { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 系统登录校验地址
        /// </summary>
        public string LoginVerifiedUrl { get; set; }

        /// <summary>
        /// 首页地址
        /// </summary>
        public string HomePageUrl { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderNum { get; set; }

        /// <summary>
        /// 状态:1启用 0停用
        /// </summary>
        public int? RecordStatus { get; set; }
    }
}

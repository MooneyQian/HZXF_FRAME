using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Models
{
    /// <summary>
    /// 登录类
    /// </summary>
    [Serializable]
    public class LogOnModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidateCode { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 登录后跳转地址
        /// </summary>
        public string ReUrl { get; set; }
    }
}

using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Entitys
{
    /// <summary>
    /// 系统日志
    /// </summary>
    [Serializable]
    [System.ComponentModel.Description("系统日志")]
    public class SYS_Log : AggregateRoot
    {
        /// <summary>
        /// 日志种类(Trace-痕迹,Data-数据日志,Error-错误,Warning-警告,Info-信息,Debug-调试,Fatal-致命)
        /// </summary>
        public virtual string LogType { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public virtual DateTime? LogTime { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public virtual string Module { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public virtual string ClassName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public virtual string MethodName { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public virtual string OperaterId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public virtual string OperaterName { get; set; }

        /// <summary>
        /// 异常错误栈信息
        /// </summary>
        public virtual string Exception { get; set; }

        /// <summary>
        /// 业务数据（实体对象，数据日志使用,格式为JSON)
        /// </summary>
        public virtual string DataString { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public virtual string IPAddress { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    /// <summary>
    /// 归档日志
    /// </summary>
    public class LogArchiveModel
    {
        /// <summary>
        /// 键
        /// </summary>
        public string ID { set; get; }

        /// <summary>
        /// 日志种类(Trace-痕迹,Data-数据日志,Error-错误,Warning-警告,Info-信息,Debug-调试,Fatal-致命)
        /// </summary>
        public virtual string ArchiveLogType { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public virtual DateTime? ArchiveLogTime { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public virtual string ArchiveModule { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public virtual string ArchiveClassName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public virtual string ArchiveMethodName { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public virtual string ArchiveOperaterId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public virtual string ArchiveOperaterName { get; set; }

        /// <summary>
        /// 异常错误栈信息
        /// </summary>
        public virtual string ArchiveException { get; set; }

        /// <summary>
        /// 业务数据（实体对象，数据日志使用,格式为JSON)
        /// </summary>
        public virtual string ArchiveDataString { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public virtual string ArchiveMessage { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public virtual string ArchiveIPAddress { get; set; }

        /// <summary>
        /// 归档日期
        /// </summary>
        public virtual DateTime? ArchiveDate { get; set; }
    }
}

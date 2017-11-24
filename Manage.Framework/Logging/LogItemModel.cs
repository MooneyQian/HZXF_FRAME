//*********************************************
// 公司名称：杭州方欣计算机工程有限公司              
// 部    门：研发中心
// 创 建 者：彭天自
// 创建日期：2013/1/10 9:42:52
// 修 改 人：
// 修改日期：
// 修改说明：
// 文件说明：
//**********************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    /// <summary>
    /// 日志项目模型
    /// </summary>
    public class LogItemModel
    {
        /// <summary>
        /// 键
        /// </summary>
        public string ID { set; get; }

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
        public virtual Exception Exception { get; set; }

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

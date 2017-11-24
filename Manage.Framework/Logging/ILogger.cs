using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    /// <summary>
    /// 日志容器接口
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 痕迹日志，用户操作以操作引起的流转
        /// </summary>
        /// <param name="log"></param>
        void Track(LogItemModel log);
        /// <summary>
        /// 信息日志，记录方法调用信息完成情况信息
        /// </summary>
        /// <param name="log"></param>
        void Info(LogItemModel log);
        /// <summary>
        /// 错误日志，记录系统异常与业务异常信息
        /// </summary>
        /// <param name="log"></param>
        void Error(LogItemModel log);
        /// <summary>
        /// 数据日志，需要记录业务数据修改过程的数据
        /// </summary>
        /// <param name="log"></param>
        void Data(LogItemModel log);
        /// <summary>
        /// 警告日志，不影响系统与业务完成的不正常信息
        /// </summary>
        /// <param name="log"></param>
        void Warning(LogItemModel log);
        /// <summary>
        /// 调试日志，协助开发人员调试检查程序的日志信息
        /// </summary>
        /// <param name="log"></param>
        void Debug(LogItemModel log);
        /// <summary>
        /// 致命日志，系统发生致命错误时的日志信息，如内存溢出。
        /// </summary>
        /// <param name="log"></param>
        void Fatal(LogItemModel log);
 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    /// <summary>
    /// 日志容器工厂接口
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Create a new ILog
        /// </summary>
        /// <returns>The ILog created</returns>
        ILogger Create();

        /// <summary>
        /// 根据名称获得日志容器
        /// </summary>
        /// <param name="name">日志容器名称</param>
        /// <returns></returns>
        ILogger Create(string name);
    }
}

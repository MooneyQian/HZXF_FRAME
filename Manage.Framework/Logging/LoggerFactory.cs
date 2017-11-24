using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.Framework
{
    /// <summary>
    /// 日志容器工厂
    /// </summary>
    public static class LoggerFactory
    {
        #region Members

        static Dictionary<LoggingType, ILoggerFactory> _factories = new Dictionary<LoggingType, ILoggerFactory>();

        #endregion

        #region Public Methods
        /// <summary>
        /// 装载工厂列表
        /// </summary>
        /// <param name="logFactory">日志工厂列表</param>
        public static void Init(Dictionary<LoggingType, ILoggerFactory> factories)
        {
            _factories = factories;
        }
 

        /// <summary>
        /// 创造一个新的日志容器 <paramref name="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILog"/>
        /// </summary>
        /// <param name="loggingType">日志类型</param>
        /// <returns>返回日志容器</returns>
        public static ILogger CreateLog(LoggingType loggingType)
        {
            if (_factories.ContainsKey(loggingType))
            {
               return _factories[loggingType].Create();
            }
            return null;
        }

        /// <summary>
        ///创造一个新的日志容器  <paramref name="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILog"/>
        /// </summary>
        /// <param name="loggingType">日志类型</param>
        /// <param name="name">日志容器名称</param>
        /// <returns>返回日志容器</returns>
        public static ILogger CreateLog(LoggingType loggingType,string name)
        {
            if (_factories.ContainsKey(loggingType))
            {
                return _factories[loggingType].Create(name);
            }
            return null;
        }

        #endregion
    }
}

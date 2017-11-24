using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using log4net;
using Manage.Framework;
using System.Threading;

namespace Manage.Core.Logging
{
    /// <summary>
    /// 请写类说明
    /// </summary>
    public class Log4netLog : ILogger
    {
        #region Members

        private readonly ILog source;

        #endregion

        #region  Constructor

        /// <summary>
        /// Create a new instance of this trace manager
        /// </summary>
        public Log4netLog()
        {
            // Create default source
            source = LogManager.GetLogger(this.GetType());
        }

        public Log4netLog(string name)
        {
            source = LogManager.GetLogger(name);
        }

        #endregion


        public void Track(LogItemModel log)
        {
            if (log != null)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    if (source != null)
                    {
                        try { source.Info(log.Message); }
                        catch { }
                    }
                });
            }
        }

        public void Info(LogItemModel log)
        {
            if (log != null)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    if (source != null)
                    {
                        try { source.Info(log.Message); }
                        catch { }
                    }
                });
            }
        }

        public void Error(LogItemModel log)
        {
            if (log != null)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    if (source != null)
                    {
                        try { source.Error(log.Message, log.Exception); }
                        catch { }
                    }
                });
            }
        }

        public void Data(LogItemModel log)
        {
            if (log != null)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    if (source != null)
                    {
                        try { source.Info(log.Message); }
                        catch { }
                    }
                });
            }
        }

        public void Warning(LogItemModel log)
        {
            if (log != null)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    if (source != null)
                    {
                        try { source.Info(log.Message); }
                        catch { }
                    }
                });
            }
        }

        public void Debug(LogItemModel log)
        {
            if (log != null)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    if (source != null)
                    {
                        try { source.Debug(log.Message, log.Exception); }
                        catch { }
                    }
                });
            }
        }

        public void Fatal(LogItemModel log)
        {
            if (log != null)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    if (source != null)
                    {
                        try { source.Fatal(log.Message, log.Exception); }
                        catch { }
                    }
                });
            }
        }
    }
}

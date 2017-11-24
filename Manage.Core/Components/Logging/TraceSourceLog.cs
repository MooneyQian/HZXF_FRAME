using System;
using System.Diagnostics;
using System.Globalization;
using System.Security;
using Manage.Framework;
using System.Threading;
using System.Text;

namespace Manage.Core.Logging
{
    /// <summary>
    /// Implementation of contract <see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/>
    /// using System.Diagnostics API.
    /// </summary>
    public sealed class TraceSourceLog
        : ILogger
    {
        #region Members

        TraceSource source;

        #endregion

        #region  Constructor

        /// <summary>
        /// Create a new instance of this trace manager
        /// </summary>
        public TraceSourceLog()
        {
            // Create default source
            source = new TraceSource("HS.Mvc");
        }
        public TraceSourceLog(string name)
        {
            source = new TraceSource(name);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Trace internal message in configured listeners
        /// </summary>
        /// <param name="eventType">Event type to trace</param>
        /// <param name="message">Message of event</param>
        void TraceInternal(TraceEventType eventType, string message)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                if (source != null)
                {
                    try
                    {
                        source.TraceEvent(eventType, (int)eventType, message);
                    }
                    catch
                    {
                    }
                }
            });
        }
        #endregion

        #region ILogger Members


        public void Track(LogItemModel log)
        {
            if (log != null)
            {
                var messageToTrace = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", log.LogTime.ToString(), log.Message);

                TraceInternal(TraceEventType.Information, messageToTrace);
            }
        }

        public void Info(LogItemModel log)
        {
            if (log != null)
            {
                var messageToTrace = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", log.LogTime.ToString(), log.Message);

                TraceInternal(TraceEventType.Information, messageToTrace);
            }
        }

        public void Error(LogItemModel log)
        {
            if (log != null)
            {
                var exceptionStrBuilder = new StringBuilder();
                if (log.Exception != null)
                {
                    exceptionStrBuilder.AppendLine(string.Format("\n\t Exception:{0},StackTrace:{1}", log.Exception.Message, log.Exception.StackTrace));

                    if (log.Exception.InnerException != null)
                    {
                        exceptionStrBuilder.AppendLine(string.Format("InnerException:{0},StackTrace:{1}", log.Exception.InnerException.Message, log.Exception.InnerException.StackTrace));
                    }
                }
                var messageToTrace = string.Format(CultureInfo.InvariantCulture, "{0}:{1}{2}", log.LogTime.ToString(), log.Message, exceptionStrBuilder.ToString());

                TraceInternal(TraceEventType.Information, messageToTrace);
            }
        }

        public void Data(LogItemModel log)
        {
            if (log != null)
            {
                var messageToTrace = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", log.LogTime.ToString(), log.Message);

                TraceInternal(TraceEventType.Information, messageToTrace);
            }
        }

        public void Warning(LogItemModel log)
        {
            if (log != null)
            {
                var messageToTrace = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", log.LogTime.ToString(), log.Message);

                TraceInternal(TraceEventType.Information, messageToTrace);
            }
        }

        public void Debug(LogItemModel log)
        {
            if (log != null)
            {
                var exceptionStrBuilder = new StringBuilder();
                if (log.Exception != null)
                {
                    exceptionStrBuilder.AppendLine(string.Format("\n\t Exception:{0},StackTrace:{1}", log.Exception.Message, log.Exception.StackTrace));

                    if (log.Exception.InnerException != null)
                    {
                        exceptionStrBuilder.AppendLine(string.Format("InnerException:{0},StackTrace:{1}", log.Exception.InnerException.Message, log.Exception.InnerException.StackTrace));
                    }
                }
                var messageToTrace = string.Format(CultureInfo.InvariantCulture, "{0}:{1}{2}", log.LogTime.ToString(), log.Message, exceptionStrBuilder.ToString());

                TraceInternal(TraceEventType.Information, messageToTrace);
            }
        }

        public void Fatal(LogItemModel log)
        {
            if (log != null)
            {
                var exceptionStrBuilder = new StringBuilder();
                if (log.Exception != null)
                {
                    exceptionStrBuilder.AppendLine(string.Format("\n\t Exception:{0},StackTrace:{1}", log.Exception.Message, log.Exception.StackTrace));

                    if (log.Exception.InnerException != null)
                    {
                        exceptionStrBuilder.AppendLine(string.Format("InnerException:{0},StackTrace:{1}", log.Exception.InnerException.Message, log.Exception.InnerException.StackTrace));
                    }
                }
                var messageToTrace = string.Format(CultureInfo.InvariantCulture, "{0}:{1}{2}", log.LogTime.ToString(), log.Message, exceptionStrBuilder.ToString());

                TraceInternal(TraceEventType.Information, messageToTrace);
            }
        }

        #endregion
    }
}

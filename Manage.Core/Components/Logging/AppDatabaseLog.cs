using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Manage.Framework;
using Manage.Core.Facades;

namespace Manage.Core.Logging
{
    public class AppDatabaseLog : ILogger
    {

        private void LogWrite(LogItemModel log)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((object state)=>{
                var logmodel = state as LogItemModel;
                ILogFacade _logFacade = new LogFacade();
                if (_logFacade != null)
                {
                    try
                    {
                        _logFacade.Insert(logmodel);
                    }
                    catch
                    {
                    }
                }
            }), log);
        }

        public void Track(LogItemModel log)
        {
            log.LogType = LogType.Trace.ToString();
            LogWrite(log);
        }

        public void Info(LogItemModel log)
        {
            log.LogType = LogType.Info.ToString();
            LogWrite(log);
        }

        public void Error(LogItemModel log)
        {
            log.LogType = LogType.Error.ToString();
            LogWrite(log);
        }

        public void Data(LogItemModel log)
        {
            log.LogType = LogType.Data.ToString();
            LogWrite(log);
        }

        public void Warning(LogItemModel log)
        {
            log.LogType = LogType.Warning.ToString();
            LogWrite(log);
        }

        public void Debug(LogItemModel log)
        {
            log.LogType = LogType.Debug.ToString();
            LogWrite(log);
        }

        public void Fatal(LogItemModel log)
        {
            log.LogType = LogType.Fatal.ToString();
            LogWrite(log);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.Core.Logging
{
    /// <summary>
    /// 请写类说明
    /// </summary>
    public class Log4netLogFactory : ILoggerFactory
    {
        public ILogger Create()
        {
            return new Log4netLog();
        }

        public ILogger Create(string name)
        {
            return new Log4netLog(name);
        }
    }
}

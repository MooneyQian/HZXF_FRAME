using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.Core.Logging
{
    /// <summary>
    /// A Trace Source base, log factory
    /// </summary>
    public class TraceSourceLogFactory
        : ILoggerFactory
    {
        /// <summary>
        /// Create the trace source log
        /// </summary>
        /// <returns>New ILog based on Trace Source infrastructure</returns>
        public ILogger Create()
        {
            return new TraceSourceLog();
        }

        public ILogger Create(string name)
        {
            return new TraceSourceLog(name);
        }
    }
}

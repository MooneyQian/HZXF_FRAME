using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.Core.Logging
{
    public class AppDatabaseLogFactory : ILoggerFactory
    {
        #region ILoggerFactory 成员

        public ILogger Create()
        {
            return new AppDatabaseLog();
        }

        public ILogger Create(string name)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

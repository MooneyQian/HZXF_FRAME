using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    public class ConfigAttribute : Attribute
    {
        public string Default
        {
            get;
            set;
        }

        public ConfigAttribute(string _default)
        {
            this.Default = _default;
        }
    }
}

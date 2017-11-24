using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    /// <summary>
    ///排序方式
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// 指示未指定排序样式。
        /// </summary>
        Unspecified = -1,
        /// <summary>
        /// 指示按升序排序。
        /// </summary>
        Ascending = 0,
        /// <summary>
        /// 指示按降序排序。
        /// </summary>
        Descending = 1
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 新增
        /// </summary>
        Insert=0,
        /// <summary>
        ///修改
        /// </summary>
        Update=1,

        /// <summary>
        /// 删除
        /// </summary>
        Delete=2,

        /// <summary>
        /// 无任何操作
        /// </summary>
        None=99
    }
}

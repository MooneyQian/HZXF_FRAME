using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    /// <summary>
    /// 表示已实现的类是聚合的根。
    /// </summary>
    public interface IAggregateRoot
    {
        /// <summary>
        /// Gets or sets the identifier of the entity.
        /// </summary>
        string ID { get; set; }
        /// <summary>
        /// 获取或设置云存储标识符。
        /// </summary>
        string GlobalID { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        OperationType OperationType { get; set; }

        /// <summary>
        /// Sql脚本代码块
        /// </summary>
        Dictionary<string, Dictionary<string, object>> SqlBlocks { get; set; }
    }
}

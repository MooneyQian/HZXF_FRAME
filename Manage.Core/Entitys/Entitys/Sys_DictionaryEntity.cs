using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Entitys
{
    /// <summary>
    /// 系统字典表
    /// </summary>
    [Serializable]
    [System.ComponentModel.Description("系统字典表")]
    public class Sys_DictionaryEntity : AggregateRoot
    {
        /// <summary>
        /// 父节点ID，0为顶层
        /// </summary>
        public virtual string ParDictID { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        public virtual string DictType { get; set; }
        /// <summary>
        /// 字典名
        /// </summary>
        public virtual string DictName { get; set; }
        /// <summary>
        /// 字典编号
        /// </summary>
        public virtual string DictCode { get; set; }
        /// <summary>
        /// 字典描述
        /// </summary>
        public virtual string DictDesc { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int? DictOrder { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public virtual int? LevelNO { get; set; }
        /// <summary>
        /// 是否缓存 1：是 0否
        /// </summary>
        public virtual int? IsCache { get; set; }
    }
}

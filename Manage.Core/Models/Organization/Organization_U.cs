using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.Core.Models
{
    /// <summary>
    /// 组织机构表
    /// </summary>
    [Serializable]
    public class Organization_U : BaseModel
    {
        //public string ID { get; set; } // 继承

        /// <summary>
        /// 组织机构名称
        /// </summary>		
        public string OrganName { get; set; }

        /// <summary>
        /// 组织机构描述
        /// </summary>		
        public string OrganDesc { get; set; }

        /// <summary>
        /// 组织机构父节点ID。0表示顶层
        /// </summary>		
        //public string OrganParentID { get; set; }

        /// <summary>
        /// 状态:1启用 0停用
        /// </summary>		
        public int RecordStatus { get; set; }

        /// <summary>
        /// 组织机构编码
        /// </summary>		
        public string OrganNO { get; set; }

        /// <summary>
        /// 层次
        /// </summary>		
        //public int LevelNO { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>		
        public int OrganOrder { get; set; }

        #region 冗余字段

        /// <summary>
        /// 冗余1
        /// </summary>
        public string Extend1 { set; get; }
        /// <summary>
        /// 冗余2
        /// </summary>
        public string Extend2 { set; get; }
        /// <summary>
        /// 冗余3
        /// </summary>
        public string Extend3 { set; get; }
        /// <summary>
        /// 冗余4
        /// </summary>
        public string Extend4 { set; get; }
        /// <summary>
        /// 冗余5
        /// </summary>
        public string Extend5 { set; get; }

        #endregion

    }
}

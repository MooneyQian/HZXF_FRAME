using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.SSO
{
    /// <summary>
    /// 角色信息
    /// </summary>
    [Serializable]
    public class RoleInfo : BaseModel
    {

        /// <summary>
        /// 主键
        /// </summary>
        //public string ID { get; set; }//继承

        /// <summary>
        /// 角色名称
        /// </summary>		
        public string RoleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>		
        public string RoleDesc { get; set; }

        /// <summary>
        /// 状态:1启用 0停用
        /// </summary>		
        public int RecordStatus { get; set; }


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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.Core.Models
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Serializable]
    public class Menu_I : BaseModel
    {
        //public string ID { get; set; }//继承

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { set; get; }

        /// <summary>
        /// 菜单地址
        /// </summary>
        public string MenuPath { set; get; }

        /// <summary>
        /// 菜单类型。-1：超级管理员菜单 1：菜单 2：功能
        /// </summary>
        public int MenuType { set; get; }

        /// <summary>
        /// 父菜单ID。0表示顶层
        /// </summary>
        public string PerMenuID { set; get; }

        /// <summary>
        /// 菜单状态
        /// </summary>
        public int RecordStatus { set; get; }

        /// <summary>
        /// 菜单编号
        /// </summary>
        public string MenuCode { set; get; }

        /// <summary>
        /// 菜单层级
        /// </summary>
        public int MenuLevel { set; get; }

        /// <summary>
        /// 菜单排序
        /// </summary>
        public int MenuOrder { set; get; }

        /// <summary>
        /// 菜单描述
        /// </summary>
        public string MenuDesc { set; get; }

        /// <summary>
        /// 权限控制-Controller
        /// </summary>
        public string Controller { set; get; }

        /// <summary>
        /// 权限控制-Action
        /// </summary>
        public string Action { set; get; }

        /// <summary>
        /// 在导航栏显示 1.是 0.否
        /// </summary>
        public string IsNav { set; get; }
        /// <summary>
        /// 是否post提交1.是 0.否
        /// </summary>
        public string IsPost { set; get; }
        /// <summary>
        /// 是否需要确认 1.是 0.否
        /// </summary>
        public string NeedConfirm { set; get; }
        /// <summary>
        /// 确认信息
        /// </summary>
        public string ConfirmInfo { set; get; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { set; get; }
        /// <summary>
        /// 是否在工具栏显示 1：是 0：否
        /// </summary>
        public string IsTool { set; get; }
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

        #region 扩展
        
        #endregion
    }
}

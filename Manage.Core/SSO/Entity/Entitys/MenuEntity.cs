using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;

namespace Manage.SSO.Entity
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Serializable]
    public class MenuEntity : AggregateRoot
    {
        //public override string ID { get; set; } //父类继承

        /// <summary>
        /// 菜单名称
        /// </summary>
        public virtual string MenuName { set; get; }

        /// <summary>
        /// 菜单地址
        /// </summary>
        public virtual string MenuPath { set; get; }

        /// <summary>
        /// 菜单类型。-1：超级管理员菜单 1：菜单 2：功能
        /// </summary>
        public virtual int MenuType { set; get; }

        /// <summary>
        /// 父菜单ID。0表示顶层
        /// </summary>
        public virtual string PerMenuID { set; get; }

        /// <summary>
        /// 菜单状态
        /// </summary>
        public virtual int RecordStatus { set; get; }

        /// <summary>
        /// 菜单编号
        /// </summary>
        public virtual string MenuCode { set; get; }

        /// <summary>
        /// 菜单层级
        /// </summary>
        public virtual int MenuLevel { set; get; }

        /// <summary>
        /// 菜单排序
        /// </summary>
        public virtual int MenuOrder { set; get; }

        /// <summary>
        /// 菜单描述
        /// </summary>
        public virtual string MenuDesc { set; get; }

        /// <summary>
        /// 权限控制-Controller
        /// </summary>
        public virtual string Controller { set; get; }

        /// <summary>
        /// 权限控制-Action
        /// </summary>
        public virtual string Action { set; get; }


        /// <summary>
        /// 在导航栏显示 1.是 0.否
        /// </summary>
        public virtual string IsNav { set; get; }
        /// <summary>
        /// 是否post提交1.是 0.否
        /// </summary>
        public virtual string IsPost { set; get; }
        /// <summary>
        /// 是否需要确认 1.是 0.否
        /// </summary>
        public virtual string NeedConfirm { set; get; }
        /// <summary>
        /// 确认信息
        /// </summary>
        public virtual string ConfirmInfo { set; get; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public virtual string MenuIcon { set; get; }
        /// <summary>
        /// 是否在工具栏显示 1：是 0：否
        /// </summary>
        public virtual string IsTool { set; get; }

        #region 冗余字段

        /// <summary>
        /// 冗余1
        /// </summary>
        public virtual string Extend1 { set; get; }
        /// <summary>
        /// 冗余2
        /// </summary>
        public virtual string Extend2 { set; get; }
        /// <summary>
        /// 冗余3
        /// </summary>
        public virtual string Extend3 { set; get; }
        /// <summary>
        /// 冗余4
        /// </summary>
        public virtual string Extend4 { set; get; }
        /// <summary>
        /// 冗余5
        /// </summary>
        public virtual string Extend5 { set; get; }


        #endregion
    }
}

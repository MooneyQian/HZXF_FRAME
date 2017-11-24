using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 登录用户类型
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        Administrators = 0,

        /// <summary>
        /// 普通用户
        /// </summary>
        Users = 1,

        /// <summary>
        /// 普通管理员
        /// </summary>
        GeneralManagers = 2,

        /// <summary>
        /// 匿名用户
        /// </summary>
        Guest = 99
    }
    /// <summary>
    /// 登录用户类型(中文)
    /// </summary>
    public enum UserType_ZH
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        超级管理员 = 0,

        /// <summary>
        /// 普通用户
        /// </summary>
        用户 = 1,

        /// <summary>
        /// 普通管理员
        /// </summary>
        管理员 = 2,

        /// <summary>
        /// 匿名用户
        /// </summary>
        匿名用户 = 99
    }

    /// <summary>
    /// 是否
    /// </summary>
    public enum YesNo
    {
        No = 0,
        Yes = 1
    }

    /// <summary>
    /// 状态
    /// </summary>
    public enum RecordStatus
    {
        UnEnable = 0,
        Enable = 1
    }

    /// <summary>
    /// 配送状态
    /// </summary>
    public enum DistributionStatus
    {
        编制中 = 1,
        正在配送 = 2,
        配送完成 = 3
    }
    /// <summary>
    /// 课程状态
    /// </summary>
    public enum Subject_STATUS
    {
        UnEnable = 0,
        Enable = 1
    }
    /// <summary>
    /// 状态(中文)
    /// </summary>
    public enum RecordStatus_ZH
    {
        禁用 = 0,
        启用 = 1
    }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuType
    {
        Admin = -1,
        Menu = 1,
        Function = 2,
        Data = 3
    }
    /// <summary>
    /// 菜单类型(中文)
    /// </summary>
    public enum MenuType_ZH
    {
        管理员菜单 = -1,
        菜单 = 1,
        功能按钮 = 2,
        数据权限 = 3
    }

    /// <summary>
    /// 页面风格
    /// </summary>
    public enum HomeStyle
    {
        桌面 = 1,
        传统 = 2
    }

    /// <summary>
    /// 目录类型
    /// </summary>
    public enum CatalogType
    {
        部分 = 0,
        单元 = 1,
        章节 = 2,
        小节 = 3
    }

}

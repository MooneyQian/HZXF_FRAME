//*********************************************
// 公司名称：杭州方欣计算机工程有限公司              
// 部    门：研发中心
// 创 建 者：彭天自
// 创建日期：2013/1/10 9:25:32
// 修 改 人：
// 修改日期：
// 修改说明：
// 文件说明：
//**********************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Manage.Framework
{
    /// <summary>
    /// 日志种类
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 操作痕迹
        /// </summary>
        [Description("跟踪")]
        Trace,
        /// <summary>
        /// 信息
        /// </summary>
        [Description("信息")]
        Info,
        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        Warning,
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error,
        /// <summary>
        /// 调试
        /// </summary>
        [Description("调试")]
        Debug,
        /// <summary>
        /// 数据
        /// </summary>
        [Description("数据")]
        Data,
        /// <summary>
        /// 致命
        /// </summary>
        [Description("致命")]
        Fatal
    }
}

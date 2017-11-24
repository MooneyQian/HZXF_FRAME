//*********************************************
// 公司名称：杭州方欣计算机工程有限公司              
// 部    门：研发中心
// 创 建 者：邵卫平
// 创建日期：2012-11-20 19:45:22
// 修 改 人：
// 修改日期：
// 修改说明：
// 文件说明：
//**********************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LoggingType
    {
        /// <summary>
        /// 文本记录
        /// </summary>
        Text=0,
        /// <summary>
        /// 系统跟踪日志
        /// </summary>
        TraceSource=1,
        /// <summary>
        /// sqlite存储
        /// </summary>
        Sqlite=2,
        /// <summary>
        /// 应用数据库
        /// </summary>
        AppDatabase=3,
        /// <summary>
        /// nosql存储
        /// </summary>
        Nosql=4
    }
}

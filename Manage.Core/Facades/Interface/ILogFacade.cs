using System;
using System.Collections.Generic;
using Manage.Framework;

namespace Manage.Core.Facades
{
    /// <summary>
    /// 日志业务访问逻辑
    /// </summary>
    public interface ILogFacade
    {
        /// <summary>
        /// 增加日志
        /// </summary>
        /// <param name="LogModel">日志对象</param>
        void Insert(LogItemModel log);

        ///// <summary>
        ///// 根据日志ID获取日志信息
        ///// </summary>
        ///// <param name="logID">日志ID</param>
        ///// <returns></returns>
        //LogItemModel SingleLogModel(string logID);

        ///// <summary>
        ///// 获得日志列表（包含分页及检索条件）
        ///// </summary>
        ///// <param name="pi">分页及检索条件封装对象</param>
        ///// <returns>检索到日志分页列表</returns>
        //List<LogItemModel> GetLogsByPage(PageInfo pi);
        ///// <summary>
        ///// 根据类型获取日志列表
        ///// </summary>
        ///// <param name="logtype"></param>
        ///// <returns></returns>
        //List<LogItemModel> GetLogListByLogType(string logtype);

        ///// <summary>
        ///// 获取归档日志列表
        ///// </summary>
        ///// <param name="pi"></param>
        ///// <returns></returns>
        //List<LogArchiveModel> GetArchiveLogs(PageInfo pi, string message, string periods);
        ///// <summary>
        ///// 分组获取归档日期
        ///// </summary>
        ///// <returns></returns>        
        //List<string> GetAllArchiveDate();
    }
}

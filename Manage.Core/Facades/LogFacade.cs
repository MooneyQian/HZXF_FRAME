using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;
using Manage.Core.Entitys;

namespace Manage.Core.Facades
{
    public class LogFacade : ILogFacade
    {
        public LogFacade()
        {
        }
        /// <summary>
        /// 增加日志
        /// </summary>
        /// <param name="logType">日志类型</param>
        /// <param name="source">日志来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="createTime">创建时间</param>
        /// <param name="createUserId">创建用户编号</param>
        /// <param name="createUserName">创建用户名称</param>
        public void Insert(LogItemModel log)
        {
            using (var factory = new BaseAccess())
            {
                var entity = log.Adapter<SYS_Log>(new SYS_Log());
                factory.Insert<SYS_Log>(entity);
            }
        }

        ///// <summary>
        ///// 获得日志列表（包含分页及检索条件）
        ///// </summary>
        ///// <param name="pi">分页及检索条件封装对象</param>
        ///// <returns>检索到日志分页列表</returns>
        //public List<LogItemModel> GetLogsByPage(PageInfo pi)
        //{
        //    using (var factory = new BaseAccess())
        //    {
        //        List<LogItemModel> models = new List<LogItemModel>();
        //        LogAccess logAccess = new LogAccess(factory.Context);
        //        var entities = logAccess.GetPageUseTranslator(pi);
        //        if (entities != null)
        //        {
        //            //models = entities.Adapter<SYS_Log, LogItemModel>(models, null);
        //            //update by zhaoxiaohui 20131210 SYS_Log和LogItemModel中的Exception字段类型不同 不能直接Adapter
        //            #region entity转换为model
        //            foreach (var entity in entities)
        //            {
        //                LogItemModel model = new LogItemModel();
        //                model.ID = entity.ID;
        //                model.ClassName = entity.ClassName;
        //                model.DataString = entity.DataString;
        //                model.Exception = new Exception(entity.Exception);
        //                model.IPAddress = entity.IPAddress;
        //                model.LogTime = entity.LogTime;
        //                model.LogType = entity.LogType;
        //                model.Message = entity.Message;
        //                model.MethodName = entity.MethodName;
        //                model.Module = entity.Module;
        //                model.OperaterId = entity.OperaterId;
        //                model.OperaterName = entity.OperaterName;
        //                models.Add(model);
        //            }
        //            #endregion
        //        }
        //        return models;
        //    }
        //}

        ///// <summary>
        ///// 根据类型获取日志列表
        ///// </summary>
        ///// <param name="logtype"></param>
        ///// <returns></returns>
        //public List<LogItemModel> GetLogListByLogType(string logtype)
        //{
        //    using (var factory = new DBContextFactory())
        //    {
        //        List<LogItemModel> result = null;
        //        LogAccess logAccess = new LogAccess(factory.Context);
        //        var list = logAccess.GetAll();
        //        if (list == null)
        //        {
        //            result = new List<LogItemModel>();
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(logtype))
        //            {
        //                list = list.Where(u => u.LogType == logtype).ToList();
        //            }
        //            result = list.Adapter<SYS_Log, LogItemModel>(result, null);
        //        }
        //        return result;
        //    }
        //}

        ///// <summary>
        ///// 根据日志ID获取日志信息
        ///// </summary>
        ///// <param name="logID">日志ID</param>
        ///// <returns></returns>
        //public LogItemModel SingleLogModel(string logID)
        //{
        //    using (var factory = new DBContextFactory())
        //    {
        //        LogAccess logAccess = new LogAccess(factory.Context);
        //        var entity = logAccess.Single(logID);
        //        if (entity != null)
        //        {
        //            return entity.Adapter<LogItemModel>(null);
        //        }
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 获取归档日志列表
        ///// </summary>
        ///// <param name="pi"></param>
        ///// <param name="periods"></param>
        ///// <returns></returns>
        //public List<LogArchiveModel> GetArchiveLogs(PageInfo pi, string message, string periods)
        //{
        //    using (var factory = new DBContextFactory())
        //    {
        //        List<LogArchiveModel> models = null;
        //        LogArchiveAccess logarchiveaccess = new LogArchiveAccess(factory.Context);
        //        var entities = logarchiveaccess.GetAllLogArchive(pi, message, periods);
        //        if (entities == null)
        //        {
        //            models = new List<LogArchiveModel>();
        //        }
        //        else
        //        {
        //            models = entities.Adapter<SYS_LogArchive, LogArchiveModel>(models, null);
        //        }
        //        return models;
        //    }
        //}
        ///// <summary>
        ///// 分组获取归档日期
        ///// </summary>
        ///// <returns></returns>        
        //public List<string> GetAllArchiveDate()
        //{
        //    using (var factory = new DBContextFactory())
        //    {
        //        LogArchiveAccess logarchiveaccess = new LogArchiveAccess(factory.Context);
        //        return logarchiveaccess.GetAllArchiveDate();
        //    }
        //}
    }
}
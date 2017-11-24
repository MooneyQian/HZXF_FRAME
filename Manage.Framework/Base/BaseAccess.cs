using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;
using System.Linq.Expressions;
using System.Data;
using System.Reflection;
using NHibernate;

namespace Manage.Framework
{
    /// <summary>
    /// 数据表访问的基类
    /// </summary>
    public class BaseAccess : IDisposable //where T : AggregateRoot, new()
    {
        /// <summary>
        /// 数据访问日志记录器
        /// </summary>
        public ILogger dataAccessLogger = null;

        private readonly IRepository _dbContext;
        private bool disposeflag = false;
        /// <summary>
        /// 定义委托,在数据发生存储之前的委托
        /// </summary>
        /// <param name="changeType">变化类型：1新增 2修改 3删除</param>
        /// <param name="dbContext">数据访问上下文</param>
        /// <param name="entity">实体内容</param>
        public delegate void BeforeChanged(byte changeType, IRepository dbContext, AggregateRoot entity);

        public delegate void DBContextCreatedHandle(ISessionFactory sessionFactory);

        /// <summary>
        /// 定义修改事件
        /// </summary>
        public event BeforeChanged OnBeforeChanged;

        /// <summary>
        /// 定义委托,在数据发生存储之后的委托
        /// </summary>
        /// <param name="changeType">变化类型:1新增 2修改 3删除</param>
        /// <param name="dbContext">数据访问上下文</param>
        /// <param name="entity">实体内容</param>
        public delegate void AfterChanged(byte changeType, IRepository dbContext, AggregateRoot entity);

        /// <summary>
        /// 定义修改事件
        /// </summary>
        public event AfterChanged OnAfterChanged;

        /// <summary>
        /// 定义数据库组件
        /// </summary>
        public IRepository DbContext
        {
            get { return _dbContext; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseAccess()
        {
            _dbContext = new NHContextAdapter();
            dataAccessLogger = LoggerFactory.CreateLog(LoggingType.Text, "DataAccessLogger");
        }


        /// <summary>
        /// 释放数据访问连接,销毁资源
        /// </summary>
        public void Dispose()
        {
            if (!disposeflag)
            {
                disposeflag = true;
                if (this._dbContext != null) this._dbContext.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="configFileName"></param>
        public BaseAccess(string configFileName)
        {
            if (!string.IsNullOrEmpty(configFileName))
                _dbContext = new NHContextAdapter(configFileName);
            else
                _dbContext = new NHContextAdapter();

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">数据仓储操作上下文</param>
        public BaseAccess(IRepository context)
        {
            _dbContext = context;

        }

        #region 增删改查
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoCommit">是否自动执行提交</param>
        /// <param name="tranActions">事务执行委托方法</param>
        /// <returns>实体标识</returns>
        public virtual object Insert<T>(T entity, bool autoCommit = true, Func<IRepository, object, bool> tranActions = null) where T : AggregateRoot, new()
        {
            if (this.OnBeforeChanged != null) this.OnBeforeChanged(1, this._dbContext, entity);
            var obj = _dbContext.Add(entity);
            if (tranActions != null) tranActions(_dbContext, obj);
            if (this.OnAfterChanged != null) this.OnAfterChanged(1, this._dbContext, entity);
            if (autoCommit) Commit();
            return obj;
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoCommit">是否自动执行提交</param>
        /// <param name="tranActions">事务执行委托方法</param>
        public virtual void Update<T>(T entity, bool autoCommit = true, Func<IRepository, bool> tranActions = null) where T : AggregateRoot, new()
        {
            if (this.OnBeforeChanged != null)
            {
                this.OnBeforeChanged(2, this._dbContext, entity);
            }
            this._dbContext.Update<T>(entity);
            if (tranActions != null)
            {
                tranActions(this._dbContext);
            }
            if (this.OnAfterChanged != null)
            {
                this.OnAfterChanged(2, this._dbContext, entity);
            }
            if (autoCommit)
            {
                this.Commit();
            }
        }
        /// <summary>
        /// 部分更新实体
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="entity">部分实体,建议采用new { 参数1=值1 ..} 动态对象构造</param>
        /// <param name="autoCommit">是否自动执行提交</param>
        /// <param name="tranActions">事务执行委托方法</param>
        public virtual void Update<T>(string id, object entity, bool autoCommit = true, Func<IRepository, bool> tranActions = null) where T : AggregateRoot, new()
        {
            T t = this.Single<T>(id);
            if (t != null)
            {
                T t2 = entity.Adapter(t, new string[0]);
                if (this.OnBeforeChanged != null)
                {
                    this.OnBeforeChanged(2, this._dbContext, t2);
                }
                this._dbContext.Update<T>(t2);
                if (tranActions != null)
                {
                    tranActions(this._dbContext);
                }
                if (this.OnAfterChanged != null)
                {
                    this.OnAfterChanged(2, this._dbContext, t2);
                }
                if (autoCommit)
                {
                    this.Commit();
                }
            }
        }

        /// <summary>
        /// 【谨慎使用】根据条件批量更新表记录
        /// </summary>
        /// <param name="entity">实体类型</param>
        /// <param name="updateValues">待更新的键/值对</param>
        /// <param name="specification">【谨慎】更新条件</param>
        /// <param name="autoCommit">是否自动执行提交</param>
        /// <param name="tranActions">事务执行委托方法</param>
        public virtual string Update<T>(T entity, string[] updateFields, ISpecification<T> specification, bool autoCommit = true, Func<IRepository, bool> tranActions = null) where T : AggregateRoot, new()
        {
            string result;
            if (entity != null)
            {
                if (this.OnBeforeChanged != null)
                {
                    this.OnBeforeChanged(2, this._dbContext, entity);
                }
                string text = this._dbContext.Update<T>(entity, updateFields, specification);
                if (tranActions != null)
                {
                    tranActions(this._dbContext);
                }
                if (this.OnAfterChanged != null)
                {
                    this.OnAfterChanged(2, this._dbContext, entity);
                }
                if (autoCommit)
                {
                    this.Commit();
                }
                result = text;
            }
            else
            {
                result = "";
            }
            return result;
        }


        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoCommit">是否自动执行提交</param>
        /// <param name="tranActions">事务执行委托方法</param>
        public virtual void Delete<T>(T entity, bool autoCommit = true, Func<IRepository, bool> tranActions = null) where T : AggregateRoot, new()
        {
            if (this.OnBeforeChanged != null) this.OnBeforeChanged(3, this._dbContext, entity);
            if (tranActions != null) tranActions(_dbContext);
            _dbContext.Delete(entity);
            if (this.OnAfterChanged != null) this.OnAfterChanged(3, this._dbContext, entity);
            if (autoCommit) Commit();

        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="ids">实体编号列表</param>
        /// <param name="autoCommit">是否自动执行提交</param>
        /// <param name="tranActions">事务执行委托方法</param>
        public virtual void Delete<T>(string[] ids, bool autoCommit = true, Func<IRepository, bool> tranActions = null) where T : AggregateRoot, new()
        {

            if (tranActions != null) tranActions(_dbContext);
            foreach (var id in ids)
            {
                var entity = _dbContext.Get<T>(id);
                if (entity != null)
                {
                    if (this.OnBeforeChanged != null) this.OnBeforeChanged(2, this._dbContext, entity);
                    _dbContext.Delete(entity);
                    if (this.OnAfterChanged != null) this.OnAfterChanged(2, this._dbContext, entity);
                }
            }
            if (autoCommit) Commit();
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="ids">实体编号列表</param>
        /// <param name="autoCommit">是否自动执行提交</param>
        /// <param name="tranActions">事务执行委托方法</param>
        public virtual void Delete<T>(string ids, bool autoCommit = true, Func<IRepository, bool> tranActions = null) where T : AggregateRoot, new()
        {
            Delete<T>(ids.Split(','), autoCommit, tranActions);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entitys">数据库查询出的实体</param>
        /// <param name="autoCommit">是否自动执行提交</param>
        /// <param name="tranActions">事务执行委托方法</param>
        public virtual void Delete<T>(List<T> entitys, bool autoCommit = true, Func<IRepository, bool> tranActions = null) where T : AggregateRoot, new()
        {

            if (tranActions != null) tranActions(_dbContext);
            foreach (var entity in entitys)
            {
                if (entity != null)
                {
                    if (this.OnBeforeChanged != null) this.OnBeforeChanged(2, this._dbContext, entity);
                    _dbContext.Delete(entity);
                    if (this.OnAfterChanged != null) this.OnAfterChanged(2, this._dbContext, entity);
                }
            }
            if (autoCommit) Commit();
        }

        /// <summary>
        /// 获取单条记录。
        /// </summary>
        /// <param name="id">实体编号</param>
        /// <returns>符合编号的实体</returns>
        public virtual T Single<T>(string id) where T : AggregateRoot, new()
        {
            //判断c是否属于数据底层基类，如果属于则直接访问数据库，如果不属于则访问缓存
            return _dbContext.Get<T>(id);
        }

        /// <summary>
        /// 根据条件获取单条记录。
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <returns>如果未找到则返回null,如果多条返回第一条</returns>
        public virtual T Single<T>(ISpecification<T> specification) where T : AggregateRoot, new()
        {
            return _dbContext.Get<T>(specification);
        }
        /// <summary>
        /// 根据条件(排序)获取单条记录。
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <param name="sortPredicate">排序字段</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns>如果未找到则返回null,如果多条返回第一条</returns>
        public virtual T Single<T>(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder) where T : AggregateRoot, new()
        {
            return _dbContext.Get<T>(specification, sortPredicate, sortOrder);
        }

        /// <summary>
        /// 根据连接字符串获取当前所有表
        /// </summary>
        /// <param name="listConfigFileName"></param>
        /// <returns></returns>
        public virtual Dictionary<string, DataSet> GetDBSchema<T>(List<string> listConfigFileName)
        {
            Dictionary<string, DataSet> dictDataSet = new Dictionary<string, DataSet>();
            foreach (var configFileName in listConfigFileName)
            {
                var context = new NHContextAdapter(configFileName);
                DataSet ds = new DataSet();
                //ds.Tables.Add( context.Connection.GetSchema("Databases"));
                ds.Tables.Add(context.Connection.GetSchema("Tables"));
                ds.Tables.Add(context.Connection.GetSchema("Views"));
                ds.Tables.Add(context.Connection.GetSchema("Columns"));
                dictDataSet.Add(configFileName, ds);
            }
            return dictDataSet;
        }


        #endregion

        #region GetPage
        /// <summary>
        /// 分页获取实体列表
        /// </summary>
        /// <param name="pi">分页器</param>
        /// <returns>实体列表</returns>
        public virtual List<T> GetPage<T>(PageInfo pi) where T : AggregateRoot, new()
        {
            return GetPage(pi, Specification<T>.Create(p => true));
        }

        /// <summary>
        /// 分页获取实体列表
        /// </summary>
        /// <param name="pi">分页器</param>
        /// <param name="specification">查询条件</param>
        /// <returns>实体列表</returns>
        public virtual List<T> GetPage<T>(PageInfo pi, ISpecification<T> specification) where T : AggregateRoot, new()
        {
            return GetPage(pi, specification, null, SortOrder.Unspecified);
        }
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pi">分页列表</param>
        /// <param name="specification">查询条件</param>
        /// <param name="sortPredicate">排序字段</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns>实体列表</returns>
        public virtual List<T> GetPage<T>(PageInfo pi, ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder) where T : AggregateRoot, new()
        {
            pi.Total = _dbContext.GetAll(specification).Count();
            return _dbContext.GetPaged<T>(pi.PageIndex, pi.PageSize, specification, sortPredicate, sortOrder).ToList();
        }
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pi">分页列表</param>
        /// <param name="specification">查询条件</param>
        /// <param name="sortOrders">排序组合列表</param>
        /// <returns>实体列表</returns>
        public virtual List<T> GetPage<T>(PageInfo pi, ISpecification<T> specification, List<Orderby<T>> sortOrders) where T : AggregateRoot, new()
        {
            pi.Total = _dbContext.GetAll(specification).Count();
            return _dbContext.GetPaged<T>(pi.PageIndex, pi.PageSize, specification, sortOrders).ToList();
        }


        #endregion

        #region GetAll

        /// <summary>
        /// 获取适合条件的所有记录列表
        /// </summary>
        /// <returns>实体列表</returns>
        public virtual List<T> GetAll<T>() where T : AggregateRoot, new()
        {
            return _dbContext.GetAll<T>().ToList();
        }

        /// <summary>
        /// 根据主键数组获取对应记录列表
        /// </summary>
        /// <param name="idArray">主键数组</param>
        /// <returns>实体列表</returns>
        public virtual List<T> GetAll<T>(string[] idArray) where T : AggregateRoot, new()
        {
            var spec = Specification<T>.Create(p => idArray.Contains(p.ID));
            return GetAll(spec);
        }

        /// <summary>
        /// 根据主键数组获取对应记录列表
        /// </summary>
        /// <param name="idArray">主键数组</param>
        /// <returns>实体列表</returns>
        public virtual List<T> GetAll<T>(List<string> idArray) where T : AggregateRoot, new()
        {
            var spec = Specification<T>.Create(p => idArray.Contains(p.ID));
            return GetAll(spec);
        }

        /// <summary>
        /// 获取适合条件的所有记录列表
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <returns>实体列表</returns>
        public virtual List<T> GetAll<T>(ISpecification<T> specification) where T : AggregateRoot, new()
        {
            return GetAll(specification, null, SortOrder.Unspecified);
        }

        /// <summary>
        /// 获取适合条件的所有记录列表，可以排序
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <param name="sortPredicate">排序字段</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns>实体列表</returns>
        public virtual List<T> GetAll<T>(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder) where T : AggregateRoot, new()
        {
            var query = _dbContext.GetAll<T>(specification, sortPredicate, sortOrder);
            //AddDataColumnPrivileage<T>(query);
           return query.ToList();
        }

        /// <summary>
        /// 获取适合条件的所有记录列表，可以排序
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <param name="sortOrders">排序对象</param>
        /// <returns>实体列表</returns>
        public virtual List<T> GetAll<T>(ISpecification<T> specification, List<Orderby<T>> sortOrders) where T : AggregateRoot, new()
        {
            var query = _dbContext.GetAll<T>(specification, sortOrders);
            //AddDataColumnPrivileage<T>(query);
            return query.ToList();
        }

        #endregion

        #region getSingle
        /// <summary>
        /// 获取单个或第一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetSingle<T>(string id) where T : AggregateRoot, new()
        {
            var spec = Specification<T>.Create(p => p.ID == id);
            return GetAll(spec).FirstOrDefault();
        }

        /// <summary>
        /// 获取单个或第一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        public virtual T GetSingle<T>(ISpecification<T> specification) where T : AggregateRoot, new()
        {
            return GetAll(specification).FirstOrDefault();
        }
        #endregion

        /// <summary>
        /// 获取数据条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        public virtual int GetCount<T>(ISpecification<T> specification) where T : AggregateRoot, new()
        {
            return _dbContext.GetCount<T>(specification);
        }

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        public virtual bool IsExists<T>(ISpecification<T> specification) where T : AggregateRoot, new()
        {
            return _dbContext.Exists<T>(specification);
        }

        /// <summary>
        /// 返回未执行前的全部列表操作
        /// </summary>
        /// <returns>所有结果集枚举迭代器</returns>
        public IQueryable<T> GetContext<T>() where T : AggregateRoot, new()
        {
            return _dbContext.GetAll<T>(null);
        }

        /// <summary>
        /// 返回未执行前的获取操作
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <returns>符合条件的所有结果集枚举迭代器</returns>
        public IQueryable<T> GetContext<T>(ISpecification<T> specification) where T : AggregateRoot, new()
        {
            if (specification != null)
                return _dbContext.GetAll<T>(specification);

            return _dbContext.GetAll<T>();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            _dbContext.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            _dbContext.Rollback();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志信息</param>
        public void WriteLog(string message, string method)
        {
            if (dataAccessLogger != null)
            {
                var log = new LogItemModel()
                {
                    ClassName = "BaseAccess",
                    MethodName = method,
                    LogTime = DateTime.Now,
                    IPAddress = "",
                    Message = message
                };

                dataAccessLogger.Track(log);
            }
        }

    }
}

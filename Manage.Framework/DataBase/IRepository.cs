using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;
using System.Data.Common;
using System.Data;
using Manage.Framework;
using NHibernate;
using System.Collections.Concurrent;

namespace Manage.Framework
{
    public delegate void Commited();
    public delegate ConcurrentQueue<IAggregateRoot> Commiting();
    /// <summary>
    /// 提供数据仓储操作的接口
    /// </summary>
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// 配置文件名
        /// </summary>
        string CfgName { get; }

        event Commited OnCommited;

        event Commiting OnCommiting;

        /// <summary>
        /// 事务独占模式开启标识
        /// </summary>
        bool SingletonMode { get; }

        /// <summary>
        /// 数据库的连接
        /// </summary>
        DbConnection Connection { get; }

        /// <summary>
        /// 数据持久化操作,可以包括新增，修改，删除
        /// </summary>
        /// <param name="entities"></param>
        void Persist(List<IAggregateRoot> entities);

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体泛型:必须继承IAggregateRoot</typeparam>
        /// <param name="aggregateRoot">聚合根实体：必须继承IAggregateRoot</param>
        /// <returns>返回聚合根实体主键</returns>
        object Add<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns>返回聚合根实体主键</returns>
        object Add(AggregateRoot obj);

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体泛型：必须继承IAggregateRoot</typeparam>
        /// <param name="aggregateRoot">聚合根实体：必须继承IAggregateRoot</param>
        void Update<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="obj">聚合根实体：必须继承IAggregateRoot</param>
        void Update(AggregateRoot obj);

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="obj">数据实体</param>
        /// <param name="isUpdate">是否需要更新；若为true，则Parameters为需要更新的字段列表。若为false，则Parameters为不需要更新的字段列表</param>
        /// <param name="Parameters">字段参数列表</param>
        /// <param name="PrimarykeyValue">当前数据实体的主键键/值对</param>
        void Update(AggregateRoot obj, bool isUpdate, List<string> Parameters, Dictionary<string, string> PrimarykeyValues);


        /// <summary>
        /// 根据条件批量更新表记录
        /// </summary>
        /// <typeparam name="TAggregateRoot">实体类型</typeparam>
        /// <param name="updateValues">待更新的键/值对</param>
        /// <param name="specification">更新条件</param>
        
        string Update<TAggregateRoot>(AggregateRoot obj, string[] updateFields, ISpecification<TAggregateRoot> specification) where TAggregateRoot : class, IAggregateRoot, new();


        /// <summary>
        /// 根据聚合根实体(的标识)删除
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体泛型：必须继承IAggregateRoot</typeparam>
        /// <param name="aggregateRoot">聚合根实体</param>
        void Delete<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 根据实体对象(的标识)删除
        /// </summary>
        /// <param name="obj">聚合根实体</param>
        void Delete(AggregateRoot obj);

        /// <summary>
        /// 提供对数据类型TAggregateRoot已知的特定数据源的查询进行计算的功能
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体泛型</typeparam>
        /// <returns>提供对聚合根的查询操作</returns>
        IQueryable<TAggregateRoot> AsQueryable<TAggregateRoot>() where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        ///  提供对数据类型TAggregateRoot已知的特定数据源的查询进行计算的功能,只能在NHibernate中使用
        /// </summary>
        /// <typeparam name="TAggregateRoot"></typeparam>
        /// <returns></returns>
        IQueryOver<TAggregateRoot, TAggregateRoot> AsQueryOver<TAggregateRoot>(Expression<Func<TAggregateRoot>> alias) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        ///  提供对数据类型TAggregateRoot已知的特定数据源的查询进行计算的功能,只能在NHibernate中使用
        /// </summary>
        /// <typeparam name="TAggregateRoot"></typeparam>
        /// <returns></returns>
        IQueryOver<TAggregateRoot, TAggregateRoot> AsQueryOver<TAggregateRoot>() where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 通过主键返回对象
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns>聚合根实体</returns>
        TAggregateRoot Get<TAggregateRoot>(object key) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 查询聚合根实体
        /// </summary>
        /// <param name="classzz">聚合根实体类型</param>
        /// <param name="key">主键值</param>
        /// <returns>聚合根实体</returns>
        object Get(Type clazz, object key);

        /// <summary>
        /// 通过使用给定的规范,从存储库中获取单个聚合根实例。
        /// </summary>
        /// <param name="specification">与聚合根匹配的规范。</param>
        /// <returns>聚合根的实例。</returns>
        TAggregateRoot Get<TAggregateRoot>(ISpecification<TAggregateRoot> specification) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 通过使用给定的规范,从存储库中获取单个聚合根实例(取得条件排序第一条)。
        /// </summary>
        /// <param name="specification">与聚合根匹配的规范。</param>
        /// <returns>聚合根的实例。</returns>
        TAggregateRoot Get<TAggregateRoot>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 根据类型获得表数据
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        IList GetAll(Type entityType);

        /// <summary>
        /// 条件查询对象列表
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="specification">查询条件字典</param>
        /// <returns></returns>
        IList GetAll(Type entityType, Dictionary<string, string> specification);

        /// <summary>
        /// 条件查询对象列表
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="specification">查询条件字典</param>
        /// <returns></returns>
        IList GetAll(Type entityType, Dictionary<string, string> specification, List<string> sortPredicate, SortOrder sortOrder);

        /// <summary>
        /// 条件查询对象列表
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="specification">查询条件字典</param>
        /// <returns></returns>
        IList GetPaged(Type entityType, Dictionary<string, string> specification, int pageIndex, int pageSize);


        /// <summary>
        /// 条件查询对象列表
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="specification">查询条件字典</param>
        /// <returns></returns>
        IList GetPaged(Type entityType, Dictionary<string, string> specification, List<string> sortPredicate, SortOrder sortOrder, int pageIndex, int pageSize);


        /// <summary>
        /// 从存储库中获取所有聚合的根。
        /// </summary>
        /// <returns>获取的所有聚合根</returns>
        IQueryable<TAggregateRoot> GetAll<TAggregateRoot>() where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        ///通过使用提供的排序谓词和指定的排序顺序进行排序,从存储库中获取所有聚合根。
        /// </summary>
        /// <param name="sortPredicate">排序字段。</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns>获取的所有聚合根</returns>
        IQueryable<TAggregateRoot> GetAll<TAggregateRoot>(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 通过使用给定的规范,从存储库中获取聚合根实例集合。
        /// </summary>
        /// <param name="specification">与聚合根匹配的规范</param>
        /// <returns>聚合根的实例集合.</returns>
        IQueryable<TAggregateRoot> GetAll<TAggregateRoot>(ISpecification<TAggregateRoot> specification) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 通过使用提供的排序谓词和指定的排序顺序进行排序,从存储库中获取所有聚合根集合。
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <param name="sortPredicate">排序字段</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns>获取的所有聚合根集合</returns>
        IQueryable<TAggregateRoot> GetAll<TAggregateRoot>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 通过使用提供的排序谓词和指定的排序顺序进行排序,从存储库中获取所有聚合根集合。
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <param name="sortOrders">排序对象</param>
        /// <returns>获取的所有聚合根集合</returns>
        IQueryable<TAggregateRoot> GetAll<TAggregateRoot>(ISpecification<TAggregateRoot> specification, List<Orderby<TAggregateRoot>> sortOrders) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        ///通过使用提供的排序谓词、指定的排序顺序和分页参数,从存储库中获取分页聚合根集合。
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">页记录</param>
        /// <returns>获取的分页聚合根集合</returns>
        IQueryable<TAggregateRoot> GetPaged<TAggregateRoot>(int pageIndex, int pageCount) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        ///通过使用提供的排序谓词、指定的排序顺序和分页参数,从存储库中获取分页聚合根集合。
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">页记录</param>
        /// <param name="specification">查询条件</param>
        /// <returns>获取的分页聚合根集合</returns>
        IQueryable<TAggregateRoot> GetPaged<TAggregateRoot>(int pageIndex, int pageCount, ISpecification<TAggregateRoot> specification) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        ///通过使用提供的排序谓词、指定的排序顺序和分页参数,从存储库中获取分页聚合根集合。
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">页记录</param>
        /// <param name="orderByExpression">排序字段</param>
        /// <param name="ascending">排序方式</param>
        /// <returns>获取的分页聚合根集合</returns>
        IQueryable<TAggregateRoot> GetPaged<TAggregateRoot>(int pageIndex, int pageCount, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        ///通过使用提供的排序谓词、指定的排序顺序和分页参数,从存储库中获取分页聚合根集合。
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">页记录</param>
        /// <param name="specification">查询条件</param>
        /// <param name="orderByExpression">排序字段</param>
        /// <param name="ascending">排序方式</param>
        /// <returns>获取的分页聚合根集合</returns>
        IQueryable<TAggregateRoot> GetPaged<TAggregateRoot>(int pageIndex, int pageCount, ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) where TAggregateRoot : class,IAggregateRoot, new();

        
        /// <summary>
        ///通过使用提供的排序谓词、指定的排序顺序和分页参数,从存储库中获取分页聚合根集合。
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">页记录</param>
        /// <param name="specification">查询条件</param>
        /// <param name="sortOrders">排序对象</param>
        /// <returns>获取的分页聚合根集合</returns>
        IQueryable<TAggregateRoot> GetPaged<TAggregateRoot>(int pageIndex, int pageCount, ISpecification<TAggregateRoot> specification, List<Orderby<TAggregateRoot>> sortOrders) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 通过使用给定的规范,统计存储库中记录数。
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <returns>返回符合的记录数。</returns>
        int GetCount<TAggregateRoot>(ISpecification<TAggregateRoot> specification) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 通过使用给定的规范,对比聚合根是否存在存储库中。
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <returns>如果聚合根存在，否则为 false，则为 true。</returns>
        bool Exists<TAggregateRoot>(ISpecification<TAggregateRoot> specification) where TAggregateRoot : class,IAggregateRoot, new();

        /// <summary>
        /// 执行SQL，返回结果集。
        /// </summary>
        /// <typeparam name="TElement">结果集泛型</typeparam>
        /// <param name="sql">查询语句SQL</param>
        /// <param name="param">参数字典</param>
        /// <returns>返回执行SQL语句的结果集</returns>
        IQueryable<TElement> ExecuteString<TElement>(string sql, List<DbParameter> param)
            where TElement : class,IAggregateRoot, new();

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">查询语句SQL</param>
        /// <param name="param">参数字典</param>
        /// <returns>提供DataReader查询数据</returns>
        IDataReader ExecuteString(string sql, List<DbParameter> param);


        /// <summary>
        /// 执行SQL语句，像UPDATE ,DELETE 有执行行数的操作
        /// </summary>
        /// <param name="sql">
        /// <example>update table set field=@field0 where pkid=@id </example>
        /// </param>
        /// <param name="dict">
        /// <code>
        /// EF: new SqlParameter { ParameterName = "field0", Value = 1 }
        /// NH: key:字段名; value:数值
        /// </code>
        /// </param>
        /// <returns>返回受影响的行数</returns>
        int ExecuteSqlCommand(string queryString, Dictionary<string, object> dict);

        /// <summary>
        /// 从数据库中检索单个值(例如一个聚合值)
        /// </summary>
        /// <param name="queryString">查询语句SQL</param>
        /// <param name="dict">参数字典</param>
        /// <returns>返回结果集的第一行第一列</returns>
        object ExecuteScalar(string queryString, Dictionary<string, object> dict);


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="TElement">实体</typeparam>
        /// <param name="storedProcedureName">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <returns>提供对存储过程的执行结果查询器</returns>
        IDataReader ExecuteProcToDateReader(string procedureName, string[] paramNames, object[] paramValues);
        IQueryable<TElement> ExecuteProc<TElement>(string procedureName, object[] param) where TElement : class, IAggregateRoot, new();

        IQueryable<TElement> ExecuteProc<TElement>(string procedureName, string[] paramNames, object[] paramValues) where TElement : class, IAggregateRoot, new();

        IQueryable<TElement> ExecuteProc<TElement>(string procedureName, string[] paramNames, object[] paramValues, string[] outParamNames, DbType[] outParamTypes, out object[] outParamResults) where TElement : class, IAggregateRoot, new();

        List<TElement> ExecuteProcToList<TElement>(string procedureName, string[] paramNames, object[] paramValues, string[] outParamNames, DbType[] outParamTypes, out object[] outParamResults) where TElement : class, IAggregateRoot, new();

        /// <summary>
        /// 通过SQL返回指定实体类型的数据集
        /// </summary>
        /// <typeparam name="TElement">实体类型</typeparam>
        /// <param name="sql">
        /// <example>
        /// ◆ context.SqlQuery<Product>("select * from Products where pid = {0}", 1);
        /// ◆ context.SqlQuery<Product>("select * from Products where pid = @p0", new SqlParameter { ParameterName = "p0", Value = 1 });
        /// </example>
        /// </param>
        /// </param>
        /// <param name="dict">
        /// <code>
        /// EF: new SqlParameter { ParameterName = "field0", Value = 1 }
        /// NH: key:字段名; value:数值
        /// </code>
        /// </param>
        /// <returns>提供对sql语句的执行结果查询器</returns>
        IQueryable<TElement> SqlQuery<TElement>(string queryString, Dictionary<string, object> dict) where TElement : class,IAggregateRoot, new();

        /// <summary>
        /// 通过SQL返回指定实体类型的数据集
        /// </summary>
        /// <typeparam name="TElement">实体类型</typeparam>
        /// <param name="sql">
        /// <example>
        /// ◆ context.SqlQuery<Product>("select * from Products where pid = {0}", 1);
        /// ◆ context.SqlQuery<Product>("select * from Products where pid = @p0", new SqlParameter { ParameterName = "p0", Value = 1 });
        /// </example>
        /// </param>
        /// </param>
        /// <param name="dict">
        /// <code>
        /// EF: new SqlParameter { ParameterName = "field0", Value = 1 }
        /// NH: key:字段名; value:数值
        /// </code>
        /// </param>
        /// <returns>提供对sql语句的执行结果查询器</returns>
        IQueryable SqlQuery(Type elementType, string queryString, Dictionary<string, object> dict);

        /// <summary>
        /// 指示是否已提交
        /// </summary>
        bool Committed { get; }

        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using System.Collections.Concurrent;
using System.ComponentModel;
using Manage.Framework;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;


namespace Manage.Framework
{
    public delegate void DBContextCreatedHandle(ISessionFactory sessionFactory);

    public class NHContextAdapter : IRepository
    {
        public event DBContextCreatedHandle DBContextCreated;
        public event Commited OnCommited;
        public event Commiting OnCommiting;

        private string _SingletoModeId = string.Empty;
        /// <summary>
        /// 事务独占模式开启标识
        /// </summary>
        public bool SingletonMode
        {
            get { return !string.IsNullOrEmpty(_SingletoModeId); }
        }
        private string _cfgname = string.Empty;
        public string CfgName
        {
            get
            {
                return _cfgname;
            }
        }

        #region Private Fields
        [ThreadStatic]
        private bool committed = true;
        [ThreadStatic]
        private readonly DatabaseSessionFactory databaseSessionFactory;
        [ThreadStatic]
        private readonly ISession session = null;
        private readonly ConcurrentQueue<IAggregateRoot> _objects = new ConcurrentQueue<IAggregateRoot>();
        #endregion


        #region Ctor

        public NHContextAdapter()
            : this("")
        {
        }

        public NHContextAdapter(string configFileName)
        {
            databaseSessionFactory = new DatabaseSessionFactory(configFileName);
            databaseSessionFactory.BuildedSessionFactory += new BuildSessionFactoryHandle(databaseSessionFactory_BuildedSessionFactory);
            _cfgname = databaseSessionFactory.FactoryName;
            session = databaseSessionFactory.Session;
        }

        public NHContextAdapter(NHibernate.Cfg.Configuration nhibernateConfig)
        {
            databaseSessionFactory = new DatabaseSessionFactory(nhibernateConfig);

            databaseSessionFactory.BuildedSessionFactory += new BuildSessionFactoryHandle(databaseSessionFactory_BuildedSessionFactory);
            _cfgname = databaseSessionFactory.FactoryName;
            session = databaseSessionFactory.Session;
        }

        #endregion

        void databaseSessionFactory_BuildedSessionFactory(ISessionFactory sessionFactory)
        {
            if (DBContextCreated != null)
            {
                DBContextCreated(sessionFactory);
            }
        }

        /// <summary>
        /// NHibernate数据库访问会话
        /// </summary>
        public ISession Session { get { return session; } }

        #region IRepositoryNew接口

        public DbConnection Connection
        {
            get
            {
                DbConnection conn = Session.Connection as DbConnection;
                if (conn == null)
                {
                    throw new Exception("不支持本数据库类型获取数据库连接对象");
                }
                return conn;
            }
        }

        public void Persist(List<IAggregateRoot> entities)
        {
            if (entities == null || entities.Count == 0) return;
            foreach (var entity in entities)
            {
                if (entity.OperationType == OperationType.None) continue;
                this._objects.Enqueue(entity);
            }
            committed = false;
        }


        /// <summary>
        /// 新增记录
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体泛型:必须继承IAggregateRoot</typeparam>
        /// <param name="aggregateRoot">聚合根实体：必须继承IAggregateRoot</param>
        /// <returns>返回聚合根实体主键</returns>
        public object Add<TAggregateRoot>(TAggregateRoot obj)
           where TAggregateRoot : class,IAggregateRoot, new()
        {
            if (string.IsNullOrWhiteSpace(obj.ID))
            {
                obj.ID = Guid.NewGuid().ToString();
            }
            obj.OperationType = OperationType.Insert;
            this._objects.Enqueue(obj);
            committed = false;
            return obj.ID;
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns>返回聚合根实体主键</returns>
        public object Add(AggregateRoot obj)
        {
            if (string.IsNullOrWhiteSpace(obj.ID))
            {
                obj.ID = Guid.NewGuid().ToString();
            }
            obj.OperationType = OperationType.Insert;
            this._objects.Enqueue(obj);
            committed = false;
            return obj.ID;
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体泛型：必须继承IAggregateRoot</typeparam>
        /// <param name="aggregateRoot">聚合根实体：必须继承IAggregateRoot</param>
        public void Update<TAggregateRoot>(TAggregateRoot obj)
             where TAggregateRoot : class,IAggregateRoot, new()
        {
            obj.OperationType = OperationType.Update;
            this._objects.Enqueue(obj);
            committed = false;
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="obj">聚合根实体：必须继承IAggregateRoot</param>
        public void Update(AggregateRoot obj)
        {
            obj.OperationType = OperationType.Update;
            this._objects.Enqueue(obj);
            committed = false;
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="obj">数据实体</param>
        /// <param name="isUpdate">是否需要更新；若为true，则Parameters为需要更新的字段列表。若为false，则Parameters为不需要更新的字段列表</param>
        /// <param name="Parameters">字段参数列表</param>
        /// <param name="PrimarykeyValue">当前数据实体的主键键/值对</param>
        public void Update(AggregateRoot obj, bool isUpdate, List<string> Parameters, Dictionary<string, string> PrimarykeyValues)
        {
            if (Parameters == null || Parameters.Count == 0 || PrimarykeyValues == null || PrimarykeyValues.Count == 0)
                return;

            Dictionary<string, object> dicValues = new Dictionary<string, object>();
            StringBuilder sbSetValue = new StringBuilder();
            StringBuilder sbKeyValue = new StringBuilder();
            string fxparm = "fxParm";
            int index = 0;
            //组装set语句
            if (isUpdate)
            {
                foreach (string str in Parameters)
                {
                    PropertyInfo pi = obj.GetType().GetProperty(str);
                    if (pi == null) { throw new ArgumentException(str + "不是" + obj.GetType().Name + "里的属性!", str); }
                    sbSetValue.Append(string.Format(" {0}= :{1} ,", str, fxparm + index));
                    dicValues.Add(fxparm + index, obj.GetValue(pi));
                    index++;
                }
            }
            else
            {
                foreach (PropertyInfo info in obj.GetType().GetProperties())
                {
                    if (info != null && !Parameters.Contains(info.Name) && !PrimarykeyValues.Keys.Contains(info.Name))
                    {
                        sbSetValue.Append(string.Format(" {0}= :{1} ,", info.Name, fxparm + index));
                        dicValues.Add(fxparm + index, obj.GetValue(info));
                        index++;
                    }
                }
            }
            //组装where语句
            foreach (KeyValuePair<string, string> keyValuePair in PrimarykeyValues)
            {
                sbKeyValue.Append(string.Format(" {0} = '{1}' and", keyValuePair.Key, keyValuePair.Value));
            }
            string sqlStr = string.Format(" update {0} set {1} where {2} ",
                            obj.GetType().Name,
                            sbSetValue.ToString().Substring(0, sbSetValue.ToString().LastIndexOf(",")),
                            sbKeyValue.ToString().Substring(0, sbKeyValue.ToString().LastIndexOf("and")));

            obj.OperationType = OperationType.None;
            if (obj.SqlBlocks == null) obj.SqlBlocks = new Dictionary<string, Dictionary<string, object>>();
            obj.SqlBlocks.Add(sqlStr, dicValues);
            this._objects.Enqueue(obj);
            committed = false;
        }

        /// <summary>
        /// 【谨慎】根据条件批量更新表记录
        /// </summary>
        /// <typeparam name="TAggregateRoot">实体类型</typeparam>
        /// <param name="updateValues">待更新的键/值对</param>
        /// <param name="specification">【谨慎】更新条件</param>
        public string Update<TAggregateRoot>(AggregateRoot obj, string[] updateFields, ISpecification<TAggregateRoot> specification) where TAggregateRoot : class, IAggregateRoot, new()
        {
            string result;
            if (updateFields == null || updateFields.Length == 0)
            {
                result = "";
            }
            else
            {
                ExpressionToSql expressionToSql = new ExpressionToSql();
                IClassMetadata classMetadata = this.session.SessionFactory.GetClassMetadata(typeof(TAggregateRoot));
                string tableName = ((SingleTableEntityPersister)classMetadata).TableName;
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                StringBuilder stringBuilder = new StringBuilder();
                StringBuilder stringBuilder2 = new StringBuilder();
                string arg = "vParm";
                int num = 0;
                for (int i = 0; i < updateFields.Length; i++)
                {
                    string text = updateFields[i];
                    stringBuilder.Append(string.Format(" {0}= :{1} ,", text, arg + num));
                    dictionary.Add(arg + num, obj.GetValue(text));
                    num++;
                }
                string text2 = string.Format(" update {0} set {1} where {2} ", tableName, stringBuilder.ToString().Substring(0, stringBuilder.ToString().LastIndexOf(",")), expressionToSql.Where<TAggregateRoot>(specification.SatisfiedBy()));
                obj.OperationType = OperationType.None;
                if (obj.SqlBlocks == null)
                {
                    obj.SqlBlocks = new Dictionary<string, Dictionary<string, object>>();
                }
                obj.SqlBlocks.Add(text2, dictionary);
                this._objects.Enqueue(obj);
                this.committed = false;
                result = text2;
            }
            return result;
        }
        /// <summary>
        /// 根据聚合根实体(的标识)删除
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体泛型：必须继承IAggregateRoot</typeparam>
        /// <param name="aggregateRoot">聚合根实体</param>
        public void Delete<TAggregateRoot>(TAggregateRoot obj)
             where TAggregateRoot : class,IAggregateRoot, new()
        {
            obj.OperationType = OperationType.Delete;
            this._objects.Enqueue(obj);
            committed = false;
        }

        /// <summary>
        /// 根据实体对象(的标识)删除
        /// </summary>
        /// <param name="obj">聚合根实体</param>
        public void Delete(AggregateRoot obj)
        {
            obj.OperationType = OperationType.Delete;
            this._objects.Enqueue(obj);
            committed = false;
        }

        /// <summary>
        /// 通过主键返回对象
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns>聚合根实体</returns>
        public TAggregateRoot Get<TAggregateRoot>(object key)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            if (key == null)
                return null;
            return session.Get<TAggregateRoot>(key);
        }

        /// <summary>
        /// 查询聚合根实体
        /// </summary>
        /// <param name="classzz">聚合根实体类型</param>
        /// <param name="key">主键值</param>
        /// <returns>聚合根实体</returns>
        public object Get(Type clazz, object key)
        {
            if (key == null)
                return null;

            return session.Get(clazz, key);
        }

        /// <summary>
        /// 通过使用给定的规范,从存储库中获取单个聚合根实例。
        /// </summary>
        /// <param name="specification">与聚合根匹配的规范。</param>
        /// <returns>聚合根的实例。</returns>
        public TAggregateRoot Get<TAggregateRoot>(ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            return session.Query<TAggregateRoot>().FirstOrDefault(specification.SatisfiedBy());
        }

        /// <summary>
        /// 通过使用给定的规范,从存储库中获取单个聚合根实例(取得条件排序第一条)。
        /// </summary>
        /// <param name="specification">与聚合根匹配的规范。</param>
        /// <returns>聚合根的实例。</returns>
        public TAggregateRoot Get<TAggregateRoot>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
    where TAggregateRoot : class,IAggregateRoot, new()
        {
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return session.Query<TAggregateRoot>().OrderBy(sortPredicate).FirstOrDefault(specification.SatisfiedBy());
                case SortOrder.Descending:
                    return session.Query<TAggregateRoot>().OrderByDescending(sortPredicate).FirstOrDefault(specification.SatisfiedBy());
                case SortOrder.Unspecified:
                    return session.Query<TAggregateRoot>().FirstOrDefault(specification.SatisfiedBy());
                default:
                    return session.Query<TAggregateRoot>().FirstOrDefault(specification.SatisfiedBy());

            }
        }

        public IList GetAll(Type entityType)
        {
            return session.CreateCriteria(entityType).List();
        }

        /// <summary>
        /// 条件查询对象列表
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="specification">查询条件字典</param>
        /// <returns></returns>
        public IList GetAll(Type entityType, Dictionary<string, string> specification)
        {
            var query = session.CreateCriteria(entityType);
            if (specification != null)
            {
                foreach (var kvp in specification)
                {
                    query.Add(NHibernate.Criterion.Restrictions.GeProperty(kvp.Key, kvp.Value));
                }
                return query.List();
            }
            else
            {
                return session.CreateCriteria(entityType).List();
            }
        }

        /// <summary>
        /// 条件查询对象列表
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="specification">查询条件字典</param>
        /// <returns></returns>
        public IList GetAll(Type entityType, Dictionary<string, string> specification, List<string> sortPredicate, SortOrder sortOrder)
        {
            var query = session.CreateCriteria(entityType);
            if (specification != null)
            {
                foreach (var kvp in specification)
                {
                    query.Add(NHibernate.Criterion.Restrictions.GeProperty(kvp.Key, kvp.Value));
                }

                foreach (var sortid in sortPredicate)
                {
                    switch (sortOrder)
                    {
                        case SortOrder.Ascending:
                            query.AddOrder(NHibernate.Criterion.Order.Asc(sortid));
                            break;
                        case SortOrder.Descending:
                            query.AddOrder(NHibernate.Criterion.Order.Desc(sortid));
                            break;
                        case SortOrder.Unspecified:
                            query.AddOrder(NHibernate.Criterion.Order.Asc(sortid));
                            break;
                    }

                }
                return query.List();
            }
            else
            {
                return session.CreateCriteria(entityType).List();
            }
        }

        /// <summary>
        /// 条件查询对象列表
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="specification">查询条件字典</param>
        /// <returns></returns>
        public IList GetPaged(Type entityType, Dictionary<string, string> specification, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;
            var query = session.CreateCriteria(entityType);
            if (specification != null)
            {
                foreach (var kvp in specification)
                {
                    query.Add(NHibernate.Criterion.Restrictions.GeProperty(kvp.Key, kvp.Value));
                }
                int firstIndex = (pageIndex - 1) * pageSize;
                query.SetFirstResult((firstIndex == 0 ? 1 : firstIndex));
                query.SetMaxResults(pageSize);
                return query.List();
            }
            else
            {
                return session.CreateCriteria(entityType).List();
            }
        }

        /// <summary>
        /// 条件查询对象列表
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="specification">查询条件字典</param>
        /// <returns></returns>
        public IList GetPaged(Type entityType, Dictionary<string, string> specification, List<string> sortPredicate, SortOrder sortOrder, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;
            var query = session.CreateCriteria(entityType);
            if (specification != null)
            {
                foreach (var kvp in specification)
                {
                    query.Add(NHibernate.Criterion.Restrictions.GeProperty(kvp.Key, kvp.Value));
                }

                int firstIndex = (pageIndex - 1) * pageSize;
                query.SetFirstResult((firstIndex == 0 ? 1 : firstIndex));
                query.SetMaxResults(pageSize);

                foreach (var sortid in sortPredicate)
                {
                    switch (sortOrder)
                    {
                        case SortOrder.Ascending:
                            query.AddOrder(NHibernate.Criterion.Order.Asc(sortid));
                            break;
                        case SortOrder.Descending:
                            query.AddOrder(NHibernate.Criterion.Order.Desc(sortid));
                            break;
                        case SortOrder.Unspecified:
                            query.AddOrder(NHibernate.Criterion.Order.Asc(sortid));
                            break;
                    }

                }
                return query.List();
            }
            else
            {
                return session.CreateCriteria(entityType).List();
            }
        }


        /// <summary>
        /// 从存储库中获取所有聚合的根。
        /// </summary>
        /// <returns>获取的所有聚合根</returns>
        public IQueryable<TAggregateRoot> GetAll<TAggregateRoot>()
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            return GetAll(new TrueSpecification<TAggregateRoot>(), null, SortOrder.Unspecified);
        }

        /// <summary>
        ///通过使用提供的排序谓词和指定的排序顺序进行排序,从存储库中获取所有聚合根。
        /// </summary>
        /// <param name="sortPredicate">排序字段。</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns>获取的所有聚合根</returns>
        public IQueryable<TAggregateRoot> GetAll<TAggregateRoot>(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            return GetAll(new TrueSpecification<TAggregateRoot>(), sortPredicate, sortOrder);
        }

        /// <summary>
        /// 通过使用给定的规范,从存储库中获取聚合根实例集合。
        /// </summary>
        /// <param name="specification">与聚合根匹配的规范</param>
        /// <returns>聚合根的实例集合.</returns>
        public IQueryable<TAggregateRoot> GetAll<TAggregateRoot>(ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            return GetAll(specification, null, SortOrder.Unspecified);
        }

        /// <summary>
        /// 通过使用提供的排序谓词和指定的排序顺序进行排序,从存储库中获取所有聚合根集合。
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <param name="sortPredicate">排序字段</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns>获取的所有聚合根集合</returns>
        public IQueryable<TAggregateRoot> GetAll<TAggregateRoot>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            IQueryable<TAggregateRoot> query = this.session.Query<TAggregateRoot>();

            if (specification != null)
            {
                query = this.session.Query<TAggregateRoot>().Where(specification.SatisfiedBy());
            }

            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    if (sortPredicate != null)
                        return query.OrderBy(sortPredicate).AsQueryable();
                    break;
                case SortOrder.Descending:
                    if (sortPredicate != null)
                        return query.OrderByDescending(sortPredicate).AsQueryable();
                    break;
                default:
                    break;
            }

            return query.AsQueryable();
        }

        /// <summary>
        /// 通过使用提供的排序谓词和指定的排序顺序进行排序,从存储库中获取所有聚合根集合。
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <param name="sortOrders">排序对象</param>
        /// <returns>获取的所有聚合根集合</returns>
        public IQueryable<TAggregateRoot> GetAll<TAggregateRoot>(ISpecification<TAggregateRoot> specification, List<Orderby<TAggregateRoot>> sortOrders)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            IQueryable<TAggregateRoot> query = this.session.Query<TAggregateRoot>();

            if (specification != null)
            {
                query = this.session.Query<TAggregateRoot>().Where(specification.SatisfiedBy());
            }
            for (int i = 0; i < sortOrders.Count; i++)
            {
                if (i == 0)
                {
                    switch (sortOrders[i].OrderBy)
                    {
                        case SortOrder.Ascending:
                            if (sortOrders[i].SortPredicate != null)
                                query = query.OrderBy(sortOrders[i].SortPredicate);
                            break;
                        case SortOrder.Descending:
                            if (sortOrders[i].SortPredicate != null)
                                query = query.OrderByDescending(sortOrders[i].SortPredicate);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (sortOrders[i].OrderBy)
                    {
                        case SortOrder.Ascending:
                            if (sortOrders[i].SortPredicate != null)
                                query = ((IOrderedQueryable<TAggregateRoot>)query).ThenBy(sortOrders[i].SortPredicate);
                            break;
                        case SortOrder.Descending:
                            if (sortOrders[i].SortPredicate != null)
                                query = ((IOrderedQueryable<TAggregateRoot>)query).ThenByDescending(sortOrders[i].SortPredicate);
                            break;
                        default:
                            break;
                    }
                }
            }

            return query.AsQueryable();
        }

        /// <summary>
        ///通过使用提供的排序谓词、指定的排序顺序和分页参数,从存储库中获取分页聚合根集合。
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">页记录</param>
        /// <returns>获取的分页聚合根集合</returns>
        public IQueryable<TAggregateRoot> GetPaged<TAggregateRoot>(int pageIndex, int pageCount)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            return this.GetPaged(pageIndex, pageCount, new TrueSpecification<TAggregateRoot>(), null, SortOrder.Unspecified);
        }

        /// <summary>
        ///通过使用提供的排序谓词、指定的排序顺序和分页参数,从存储库中获取分页聚合根集合。
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">页记录</param>
        /// <param name="specification">查询条件</param>
        /// <returns>获取的分页聚合根集合</returns>
        public IQueryable<TAggregateRoot> GetPaged<TAggregateRoot>(int pageIndex, int pageCount, ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            return this.GetPaged(pageIndex, pageCount, specification, null, SortOrder.Unspecified);
        }

        /// <summary>
        ///通过使用提供的排序谓词、指定的排序顺序和分页参数,从存储库中获取分页聚合根集合。
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">页记录</param>
        /// <param name="orderByExpression">排序字段</param>
        /// <param name="ascending">排序方式</param>
        /// <returns>获取的分页聚合根集合</returns>
        public IQueryable<TAggregateRoot> GetPaged<TAggregateRoot>(int pageIndex, int pageCount, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            return this.GetPaged(pageIndex, pageCount, new TrueSpecification<TAggregateRoot>(), sortPredicate, sortOrder);
        }

        /// <summary>
        ///通过使用提供的排序谓词、指定的排序顺序和分页参数,从存储库中获取分页聚合根集合。
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">页记录</param>
        /// <param name="specification">查询条件</param>
        /// <param name="orderByExpression">排序字段</param>
        /// <param name="ascending">排序方式</param>
        /// <returns>获取的分页聚合根集合</returns>
        public IQueryable<TAggregateRoot> GetPaged<TAggregateRoot>(int pageIndex, int pageCount, ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            var query = session.Query<TAggregateRoot>();

            if (specification != null)
            {
                query = query.Where(specification.SatisfiedBy());
            }
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    if (sortPredicate != null)
                        query = query.OrderBy(sortPredicate);
                    break;
                case SortOrder.Descending:
                    if (sortPredicate != null)
                        query = query.OrderByDescending(sortPredicate);
                    break;
                default:
                    break;
            }
            query = query.Skip(pageCount * (pageIndex - 1)).Take(pageCount);

            return query.AsQueryable();
        }

        /// <summary>
        ///通过使用提供的排序谓词、指定的排序顺序和分页参数,从存储库中获取分页聚合根集合。
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">页记录</param>
        /// <param name="specification">查询条件</param>
        /// <param name="sortOrders">排序对象</param>
        /// <returns>获取的分页聚合根集合</returns>
        public IQueryable<TAggregateRoot> GetPaged<TAggregateRoot>(int pageIndex, int pageCount, ISpecification<TAggregateRoot> specification, List<Orderby<TAggregateRoot>> sortOrders)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            var query = session.Query<TAggregateRoot>();

            if (specification != null)
            {
                query = query.Where(specification.SatisfiedBy());
            }
            for (int i = 0; i < sortOrders.Count; i++)
            {
                if (i == 0)
                {
                    switch (sortOrders[i].OrderBy)
                    {
                        case SortOrder.Ascending:
                            if (sortOrders[i].SortPredicate != null)
                                query = query.OrderBy(sortOrders[i].SortPredicate);
                            break;
                        case SortOrder.Descending:
                            if (sortOrders[i].SortPredicate != null)
                                query = query.OrderByDescending(sortOrders[i].SortPredicate);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (sortOrders[i].OrderBy)
                    {
                        case SortOrder.Ascending:
                            if (sortOrders[i].SortPredicate != null)
                                query = ((IOrderedQueryable<TAggregateRoot>)query).ThenBy(sortOrders[i].SortPredicate);
                            break;
                        case SortOrder.Descending:
                            if (sortOrders[i].SortPredicate != null)
                                query = ((IOrderedQueryable<TAggregateRoot>)query).ThenByDescending(sortOrders[i].SortPredicate);
                            break;
                        default:
                            break;
                    }
                }
            }
            query = query.Skip(pageCount * (pageIndex - 1)).Take(pageCount);

            return query.AsQueryable();
        }

        /// <summary>
        /// 通过使用给定的规范,统计存储库中记录数。
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <returns>返回符合的记录数。</returns>
        public int GetCount<TAggregateRoot>(ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            return session.Query<TAggregateRoot>().Count(specification.SatisfiedBy());
        }

        /// <summary>
        /// 通过使用给定的规范,对比聚合根是否存在存储库中。
        /// </summary>
        /// <param name="specification">查询条件</param>
        /// <returns>如果聚合根存在，否则为 false，则为 true。</returns>
        public bool Exists<TAggregateRoot>(ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class,IAggregateRoot, new()
        {
            return this.GetCount<TAggregateRoot>(specification) > 0;
        }

        /// <summary>
        /// 执行SQL，返回结果集。
        /// </summary>
        /// <typeparam name="TElement">结果集泛型</typeparam>
        /// <param name="sql">查询语句SQL</param>
        /// <param name="param">参数字典</param>
        /// <returns>返回执行SQL语句的结果集</returns>
        public IQueryable<TElement> ExecuteString<TElement>(string sql, List<DbParameter> param)
            where TElement : class,IAggregateRoot, new()
        {
            var cmd = session.Connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            if (param != null)
            {
                foreach (var o in param)
                {
                    cmd.Parameters.Add(o);
                }
            }
            try
            {
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.ToList<TElement>().AsQueryable();
                    //return ((DbDataReader)reader).ToObjects<TElement>("reader1").ToList();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.session.Close();
            }
        }

        /// <summary>
        /// 根据SQL查询，返回Reader对象（记得使用后要关闭Reader.例如：using(var reader = db.ExecuteString()) )
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataReader ExecuteString(string sql, List<DbParameter> param)
        {
            var cmd = session.Connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            if (param != null)
            {
                foreach (var o in param)
                {
                    cmd.Parameters.Add(o);
                }
            }
            try
            {
                return cmd.ExecuteReader();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

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
        public int ExecuteSqlCommand(string queryString, Dictionary<string, object> dict)
        {
            IQuery query = session.CreateSQLQuery(queryString);
            if (query != null)
            {
                if (dict != null)
                {
                    foreach (string each in dict.Keys)
                    {
                        query.SetParameter(each, dict[each]);
                    }
                }
                return query.ExecuteUpdate();
            }
            return 0;
        }

        /// <summary>
        /// 执行存储过程 返回datereader
        /// </summary>
        /// <typeparam name="TElement">实体</typeparam>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <returns>提供对存储过程的执行结果查询器</returns>
        public IDataReader ExecuteProcToDateReader(string procedureName, string[] paramNames, object[] paramValues)
        {
               IDbCommand dbCommand = this.session.Connection.CreateCommand();
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = procedureName;
            if (paramNames != null && paramValues != null)
            {
                for (int i = 0; i < paramNames.Length; i++)
                {
                    IDbDataParameter dbDataParameter = dbCommand.CreateParameter();
                    dbDataParameter.ParameterName = paramNames[i];
                    dbDataParameter.DbType = DbType.Object;
                    dbDataParameter.Value = paramValues[i];
                    dbCommand.Parameters.Add(dbDataParameter);
                }
            }
           
            return dbCommand.ExecuteReader();
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="TElement">实体</typeparam>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <returns>提供对存储过程的执行结果查询器</returns>
        public IQueryable<TElement> ExecuteProc<TElement>(string procedureName, object[] param) where TElement : class, IAggregateRoot, new()
        {
            IDbCommand dbCommand = this.session.Connection.CreateCommand();
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = procedureName;
            for (int i = 0; i < param.Length; i++)
            {
                object value = param[i];
                dbCommand.Parameters.Add(value);
            }
            IQueryable<TElement> result;
            try
            {
                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    result = dataReader.ToList<TElement>().AsQueryable<TElement>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.session.Close();
            }
            return result;
        }

        public IQueryable<TElement> ExecuteProc<TElement>(string procedureName, string[] paramNames, object[] paramValues) where TElement : class, IAggregateRoot, new()
        {
            object[] array;
            return this.ExecuteProc<TElement>(procedureName, paramNames, paramValues, null, null, out array);
        }

        public IQueryable<TElement> ExecuteProc<TElement>(string procedureName, string[] paramNames, object[] paramValues, string[] outParamNames, DbType[] outParamTypes, out object[] outParamResults) where TElement : class, IAggregateRoot, new()
        {
            IDbCommand dbCommand = this.session.Connection.CreateCommand();
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = procedureName;
            if (paramNames != null && paramValues != null)
            {
                for (int i = 0; i < paramNames.Length; i++)
                {
                    IDbDataParameter dbDataParameter = dbCommand.CreateParameter();
                    dbDataParameter.ParameterName = paramNames[i];
                    dbDataParameter.DbType = DbType.Object;
                    dbDataParameter.Value = paramValues[i];
                    dbCommand.Parameters.Add(dbDataParameter);
                }
            }
            if (outParamNames != null && outParamNames.Count<string>() > 0)
            {
                for (int i = 0; i < outParamNames.Length; i++)
                {
                    IDbDataParameter dbDataParameter2 = dbCommand.CreateParameter();
                    dbDataParameter2.ParameterName = outParamNames[i];
                    if (outParamTypes != null && outParamNames.Count<string>() == outParamTypes.Count<DbType>())
                    {
                        dbDataParameter2.DbType = outParamTypes[i];
                    }
                    else
                    {
                        dbDataParameter2.DbType = DbType.Object;
                    }
                    dbDataParameter2.Size = 4000;
                    dbDataParameter2.Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add(dbDataParameter2);
                }
            }
            foreach (DbParameter dbParameter in dbCommand.Parameters)
            {
                dbParameter.ParameterName = dbParameter.ParameterName.Replace(":", "").Replace("@", "").Replace("?", "");
            }
            IQueryable<TElement> result;
            try
            {
                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    //DataTable dt = DataReaderHelper.DataTableToIDataReader(dataReader);
                    IQueryable<TElement> queryable = dataReader.ToList<TElement>().AsQueryable<TElement>();
                    if (outParamNames != null && outParamNames.Count<string>() > 0)
                    {
                        outParamResults = new object[outParamNames.Count<string>()];
                        for (int i = 0; i < outParamNames.Count<string>(); i++)
                        {
                            outParamResults[i] = ((DbParameter)dbCommand.Parameters[outParamNames[i]]).Value;
                        }
                    }
                    else
                    {
                        outParamResults = new object[0];
                    }
                    result = queryable;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.session.Close();
            }
            return result;
        }

        public List<TElement> ExecuteProcToList<TElement>(string procedureName, string[] paramNames, object[] paramValues, string[] outParamNames, DbType[] outParamTypes, out object[] outParamResults) where TElement : class, IAggregateRoot, new()
        {
            IDbCommand dbCommand = this.session.Connection.CreateCommand();
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = procedureName;
            if (paramNames != null && paramValues != null)
            {
                for (int i = 0; i < paramNames.Length; i++)
                {
                    IDbDataParameter dbDataParameter = dbCommand.CreateParameter();
                    dbDataParameter.ParameterName = paramNames[i];
                    dbDataParameter.DbType = DbType.Object;
                    dbDataParameter.Value = paramValues[i];
                    dbCommand.Parameters.Add(dbDataParameter);
                }
            }
            if (outParamNames != null && outParamNames.Count<string>() > 0)
            {
                for (int i = 0; i < outParamNames.Length; i++)
                {
                    IDbDataParameter dbDataParameter2 = dbCommand.CreateParameter();
                    dbDataParameter2.ParameterName = outParamNames[i];
                    if (outParamTypes != null && outParamNames.Count<string>() == outParamTypes.Count<DbType>())
                    {
                        dbDataParameter2.DbType = outParamTypes[i];
                    }
                    else
                    {
                        dbDataParameter2.DbType = DbType.Object;
                    }
                    dbDataParameter2.Size = 4000;
                    dbDataParameter2.Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add(dbDataParameter2);
                }
            }
            foreach (DbParameter dbParameter in dbCommand.Parameters)
            {
                dbParameter.ParameterName = dbParameter.ParameterName.Replace(":", "").Replace("@", "").Replace("?", "");
            }
            List<TElement> result;
            try
            {
                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    DataTable dt = DataReaderHelper.DataTableToIDataReader(dataReader);
                    List<TElement> queryable = dt.ToList<TElement>();
                    if (outParamNames != null && outParamNames.Count<string>() > 0)
                    {
                        outParamResults = new object[outParamNames.Count<string>()];
                        for (int i = 0; i < outParamNames.Count<string>(); i++)
                        {
                            outParamResults[i] = ((DbParameter)dbCommand.Parameters[outParamNames[i]]).Value;
                        }
                    }
                    else
                    {
                        outParamResults = new object[0];
                    }
                    result = queryable;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.session.Close();
            }
            return result;
        }


        /// <summary>
        /// 从数据库中检索单个值(例如一个聚合值)
        /// </summary>
        /// <param name="queryString">查询语句SQL</param>
        /// <param name="dict">参数字典</param>
        /// <returns>返回结果集的第一行第一列</returns>
        public object ExecuteScalar(string queryString, Dictionary<string, object> dict)
        {
            IQuery query = session.CreateSQLQuery(queryString);
            if (query != null)
            {
                if (dict != null)
                {
                    foreach (string each in dict.Keys)
                    {
                        query.SetParameter(each, dict[each]);
                    }
                }
                return query.UniqueResult();
            }
            return null;
        }


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
        public IQueryable<TElement> SqlQuery<TElement>(string queryString, Dictionary<string, object> dict) where TElement : class,IAggregateRoot, new()
        {
            IQuery query = session.CreateQuery(queryString);
            if (query != null)
            {
                if (dict != null)
                {
                    foreach (string each in dict.Keys)
                    {
                        query.SetParameter(each, dict[each]);
                    }
                }
                return query.List<TElement>().AsQueryable();
            }
            return null;
        }

        /// <summary>
        /// 通过SQL返回指定实体类型的数据集
        /// </summary>
        /// <param name="elementType">实体类型</param>
        /// <param name="queryString">查询的SQL语句
        /// <example>
        /// ◆ context.SqlQuery<Product>("select * from Products where pid = {0}", 1);
        /// ◆ context.SqlQuery<Product>("select * from Products where pid = @p0", new SqlParameter { ParameterName = "p0", Value = 1 });
        /// </example>
        /// </param>
        /// <param name="dict">
        /// <code>
        /// EF: new SqlParameter { ParameterName = "field0", Value = 1 }
        /// NH: key:字段名; value:数值
        /// </code>
        /// </param>
        /// <returns>返回sql语句查询的结果集</returns>
        public System.Linq.IQueryable SqlQuery(Type elementType, string queryString, Dictionary<string, object> dict)
        {
            NHibernate.Impl.SqlQueryImpl query = session.CreateSQLQuery(queryString) as NHibernate.Impl.SqlQueryImpl;
            if (query != null)
            {
                if (dict != null)
                {
                    foreach (string each in dict.Keys)
                    {
                        query.SetParameter(each, dict[each]);
                    }
                }
                return query.List().AsQueryable();
            }
            return null;
        }


        ///// <summary>
        ///// 根据query语句返回查询接口
        ///// </summary>
        ///// <param name="hql">hql查询语句</param>
        ///// <returns>查询接口</returns>
        //private IQuery GetQuery(string queryString)
        //{
        //    IQuery query = null;
        //    if (queryString.Trim().ToLower().StartsWith("select"))
        //    {
        //        query = session.CreateSQLQuery(queryString);
        //    }
        //    else
        //    {
        //        query = session.CreateQuery(queryString);
        //    }
        //    return query;
        //}

        /// <summary>
        /// 提供对数据类型TAggregateRoot已知的特定数据源的查询进行计算的功能
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体泛型</typeparam>
        /// <returns>提供对聚合根的查询操作</returns>
        public IQueryable<TAggregateRoot> AsQueryable<TAggregateRoot>() where TAggregateRoot : class,IAggregateRoot, new()
        {
            return session.Query<TAggregateRoot>();
        }

        /// <summary>
        /// 提供对数据类型TAggregateRoot已知的特定数据源的查询进行计算的功能,只能在NHibernate中使用
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体泛型</typeparam>
        /// <returns>提供对聚合根的查询操作</returns>
        public IQueryOver<TAggregateRoot, TAggregateRoot> AsQueryOver<TAggregateRoot>(Expression<Func<TAggregateRoot>> alias) where TAggregateRoot : class,IAggregateRoot, new()
        {
            return session.QueryOver<TAggregateRoot>(alias);
        }

        /// <summary>
        /// 提供对数据类型TAggregateRoot已知的特定数据源的查询进行计算的功能,只能在NHibernate中使用
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体泛型</typeparam>
        /// <returns>提供对聚合根的查询操作</returns>
        public IQueryOver<TAggregateRoot, TAggregateRoot> AsQueryOver<TAggregateRoot>() where TAggregateRoot : class,IAggregateRoot, new()
        {
            return session.QueryOver<TAggregateRoot>();
        }

        /// <summary>
        /// 指示事务是否提交
        /// </summary>
        public bool Committed
        {
            get { return committed; }
        }

        /// <summary>
        /// 提交事务内上下文所有内容
        /// </summary>
        public void Commit()
        {
            //增加数据提交前事件
            if (this.OnCommiting != null)
            {
                var otherQueue = this.OnCommiting();
                if (otherQueue != null)
                {
                    foreach (var item in otherQueue)
                    {
                        this._objects.Enqueue(item);
                    }
                }
            }
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    if (this._objects.Count > 0)
                    {
                        while (this._objects.Count > 0)
                        {
                            IAggregateRoot entity;
                            bool flag = this._objects.TryDequeue(out entity);
                            if (flag)
                            {
                                if (entity.SqlBlocks != null && entity.SqlBlocks.Count > 0)
                                {
                                    //执行已组装好的sql语句
                                    foreach (KeyValuePair<string, Dictionary<string, object>> sqlKP in entity.SqlBlocks)
                                    {
                                        this.ExecuteSqlCommand(sqlKP.Key, sqlKP.Value);
                                    }
                                }
                                else
                                {
                                    switch (entity.OperationType)
                                    {
                                        case OperationType.Insert:
                                            session.Save(entity);
                                            break;
                                        case OperationType.Update:
                                            if (!string.IsNullOrEmpty(entity.ID))
                                            {
                                                session.Update(entity, entity.ID);
                                            }
                                            else
                                            {
                                                session.Merge(entity);
                                            }
                                            break;
                                        case OperationType.Delete:
                                            session.Delete(entity);
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    transaction.Commit();
                    committed = true;
                    if (this.OnCommited != null)
                    {
                        this.OnCommited();
                        this.OnCommited = null;
                        //var list = this.OnCommited.GetInvocationList();
                        //foreach (var m in list)
                        //{
                        //    System.Delegate.RemoveAll(this.OnCommited,m);
                        //}
                    }
                }
                catch (Exception ex)
                {
                    if (transaction.IsActive)
                        transaction.Rollback();
                    this.session.Clear();
                    IAggregateRoot entity;
                    while (this._objects.Count > 0)
                    {
                        this._objects.TryDequeue(out entity);
                    }
                    throw ex;
                }
            }
        }

        public void Rollback()
        {
            committed = false;
            this.session.Clear();
            IAggregateRoot entity;
            while (this._objects.Count > 0)
            {
                this._objects.TryDequeue(out entity);
            }
        }
        #endregion

        #region Private


        /// <summary>
        /// 动态构建OrderBy的Lambda表达式
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="query">查询功能</param>
        /// <param name="od">排序类型</param>
        //private IQueryable<T> BuildOrderBy<T>(IQueryable<T> query, Orderby od)
        //{
        //    var entityType = typeof(T);
        //    foreach (var each in od)
        //    {
        //        var propertyInfo = entityType.GetProperty(each.Key);
        //        if (propertyInfo != null)
        //        {
        //            var parameter = Expression.Parameter(entityType, "p");
        //            var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
        //            var orderByExp = Expression.Lambda(propertyAccess, parameter);
        //            var sourceExpression = query.Expression;
        //            Type sourcePropertyType = propertyInfo.PropertyType;
        //            var lambda = Expression.Call(typeof(Queryable), (each.Value == OrderByType.Asc ? "OrderBy" : "OrderByDescending"), new Type[] { entityType, sourcePropertyType },
        //                sourceExpression, Expression.Quote(orderByExp));
        //            query = query.Provider.CreateQuery<T>(lambda);
        //        }
        //    }
        //    return query;
        //}

        #endregion

        #region Dispose
        /// <summary>
        /// 实例释放的同时，会将会话Session关闭。
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!committed)
                    Commit();
                ISession dbSession = session;
                if (dbSession != null && dbSession.IsOpen)
                {
                    dbSession.Close();
                }
                dbSession.Dispose();
            }
        }

        #region Protected Methods
        /// <summary>
        /// 强制销毁，并执行GC回收
        /// </summary>
        protected void ExplicitDispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            this.ExplicitDispose();
        }
        #endregion
        #endregion
    }
}

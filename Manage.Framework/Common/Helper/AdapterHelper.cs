using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Manage.Framework
{
    /// <summary>
    /// 实体适配器：实体即一个实列化的类。转换目标的相关实体类型中的属性必须为可以Set的属性
    /// </summary>
    internal static class AdapterHelper
    {
        /// <summary>
        /// 实体适配器:把一种实体转换为另一种实体
        /// </summary>
        /// <typeparam name="T">转换后实体泛型</typeparam>
        /// <param name="obj">转换前的实体</param>
        /// <param name="defaultObjectEntity">默认值：如果转换失败在返回该默认值</param>
        /// <param name="ignoreProperties">列外属性：这些属性将不被转换</param>
        /// <returns>转换后的实体</returns>
        public static T Adapter<T>(this object obj, T defaultObjectEntity, params string[] ignoreProperties) where T : class,new()
        {
            T entity;
            if (defaultObjectEntity == null)
            {
                entity = new T();
            }
            else
            {
                entity = defaultObjectEntity;
            }
            if (obj != null)
            {
                Type s = obj.GetType();
                PropertyInfo[] spis = s.GetProperties();

                Type t = typeof(T);
                PropertyInfo[] tpis = t.GetProperties();

                List<string> ignoreList = new List<string>();
                if (ignoreProperties != null)
                {
                    ignoreList = ignoreProperties.AsParallel().ToList();
                }
                foreach (PropertyInfo pi in spis)
                {
                    //注:使用Single,若未查询到数据,则会抛出异常
                    //var tpi = tpis.Single(o => o.Name.ToLower() == pi.Name.ToLower());
                    var tpi = tpis.FirstOrDefault(o => string.Compare(o.Name, pi.Name, true) == 0);

                    if (tpi != null && pi.GetCustomAttributes(typeof(AdapterMapperIgnore), false).Length == 0
                        && ignoreList.Count(o => string.Compare(o, tpi.Name, true) == 0) == 0)
                    {
                        //判断属性是否允许写入数据,即判断属性是否有set功能
                        if (pi.CanWrite == true)
                        {
                            entity.SetValue(tpi, obj.GetValue(pi));
                        }
                    }
                }
            }
            else
            {
                return defaultObjectEntity;
            }
            return entity;
        }
        #region 映射地图的实体适配器
        /// <summary>
        /// 实体适配器:把一种实体转换为另一种实体
        /// </summary>
        /// <typeparam name="T">转换后实体泛型</typeparam>
        /// <param name="obj">转换前的实体</param>
        /// <param name="defaultObjectEntity">默认值：如果转换失败在返回该默认值</param>
        /// <param name="mapping">字段名映射方式</param>
        /// <param name="ignoreProperties">列外属性：这些属性将不被转换</param>
        /// <returns>转换后的实体</returns>
        public static T Adapter<T>(this object obj, T defaultObjectEntity, Dictionary<string, string> mapping, params string[] ignoreProperties) where T : class,new()
        {
            T entity;
            if (defaultObjectEntity == null)
            {
                entity = new T();
            }
            else
            {
                entity = defaultObjectEntity;
            }
            if (obj != null)
            {
                Type s = obj.GetType();
                PropertyInfo[] spis = s.GetProperties();

                Type t = typeof(T);
                PropertyInfo[] tpis = t.GetProperties();

                List<string> ignoreList = new List<string>();
                if (ignoreProperties != null)
                {
                    ignoreList = ignoreProperties.AsParallel().ToList();
                }
                foreach (PropertyInfo pi in spis)
                {
                    //注:使用Single,若未查询到数据,则会抛出异常
                    //var tpi = tpis.Single(o => o.Name.ToLower() == pi.Name.ToLower());
                    string piname = pi.Name;
                    if (mapping != null)
                    {
                        piname = mapping[pi.Name];
                    }
                    var tpi = tpis.FirstOrDefault(o => string.Compare(o.Name, piname, true) == 0);

                    if (tpi != null && pi.GetCustomAttributes(typeof(AdapterMapperIgnore), false).Length == 0
                        && ignoreList.Count(o => string.Compare(o, tpi.Name, true) == 0) == 0)
                    {
                        //判断属性是否允许写入数据,即判断属性是否有set功能 
                        if (pi.CanWrite == true)
                        {
                            entity.SetValue(tpi, obj.GetValue(pi));
                        }
                    }
                }
            }
            else
            {
                return defaultObjectEntity;
            }
            return entity;
        }

        /// <summary>
        /// 实体列表适配器
        /// </summary>
        /// <typeparam name="C">转换前列表中实体泛型</typeparam>
        /// <typeparam name="T">转换后列表中实体泛型</typeparam>
        /// <param name="objList">转换前实体列表</param>
        /// <param name="defaultObjectEntitys">默认值：如果转换失败在返回该默认值</param>
        /// <param name="ignoreProperties">列外属性：这些属性将不被转换</param>
        /// <returns>转换后的实体列表</returns>
        public static List<T> Adapter<C, T>(this List<C> objList, List<T> defaultObjectEntitys, Dictionary<string, string> mapping, params string[] ignoreProperties)
            where T : class,new()
            where C : class,new()
        {
            List<T> entityList = new List<T>();
            if (objList != null)
            {
                Type s = typeof(C);
                PropertyInfo[] spis = s.GetProperties();

                Type t = typeof(T);
                PropertyInfo[] tpis = t.GetProperties();

                List<string> excepts = new List<string>();
                if (ignoreProperties != null)
                {
                    excepts = ignoreProperties.AsParallel().ToList();
                }

                foreach (var obj in objList)
                {
                    T entity = new T();
                    foreach (PropertyInfo pi in spis)
                    {
                        //注:使用Single,若未查询到数据,则会抛出异常 modify by 齐树宾 12.11.26
                        //var tpi = tpis.Single(o => o.Name.ToLower() == pi.Name.ToLower());
                        string piname = pi.Name;
                        if (mapping != null)
                        {
                            piname = mapping[pi.Name];
                        }
                        var tpi = tpis.SingleOrDefault(o => string.Compare(o.Name, piname, true) == 0);
                        if (tpi != null && pi.GetCustomAttributes(typeof(AdapterMapperIgnore), false).Length == 0
                            && excepts.Count(o => string.Compare(o, tpi.Name, true) == 0) == 0)
                        {
                            if (pi.CanWrite == true)
                            {
                                entity.SetValue(tpi, obj.GetValue(pi));
                            }
                        }
                    }
                    entityList.Add(entity);
                }
            }
            else
            {
                return defaultObjectEntitys;
            }
            return entityList;
        }
        #endregion

        /// <summary>
        /// 实体列表适配器
        /// </summary>
        /// <typeparam name="C">转换前列表中实体泛型</typeparam>
        /// <typeparam name="T">转换后列表中实体泛型</typeparam>
        /// <param name="objList">转换前实体列表</param>
        /// <param name="defaultObjectEntitys">默认值：如果转换失败在返回该默认值</param>
        /// <param name="ignoreProperties">列外属性：这些属性将不被转换</param>
        /// <returns>转换后的实体列表</returns>
        public static List<T> Adapter<C, T>(this List<C> objList, List<T> defaultObjectEntitys, params string[] ignoreProperties)
            where T : class,new()
            where C : class,new()
        {
            List<T> entityList = new List<T>();
            if (objList != null)
            {
                Type s = typeof(C);
                PropertyInfo[] spis = s.GetProperties();

                Type t = typeof(T);
                PropertyInfo[] tpis = t.GetProperties();

                List<string> excepts = new List<string>();
                if (ignoreProperties != null)
                {
                    excepts = ignoreProperties.AsParallel().ToList();
                }

                foreach (var obj in objList)
                {
                    T entity = new T();
                    foreach (PropertyInfo pi in spis)
                    {
                        //注:使用Single,若未查询到数据,则会抛出异常
                        //var tpi = tpis.Single(o => o.Name.ToLower() == pi.Name.ToLower());
                        var tpi = tpis.SingleOrDefault(o => string.Compare(o.Name, pi.Name, true) == 0);
                        if (tpi != null && pi.GetCustomAttributes(typeof(AdapterMapperIgnore), false).Length == 0
                            && excepts.Count(o => string.Compare(o, tpi.Name, true) == 0) == 0)
                        {
                            if (pi.CanWrite == true)
                            {
                                entity.SetValue(tpi, obj.GetValue(pi));
                            }
                        }
                    }
                    entityList.Add(entity);
                }
            }
            else
            {
                return defaultObjectEntitys;
            }
            return entityList;
        }
    }
}

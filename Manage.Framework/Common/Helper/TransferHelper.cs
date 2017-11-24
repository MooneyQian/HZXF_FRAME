using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Linq.Expressions;

namespace Manage.Framework
{
    /// <summary>
    /// 转换助手
    /// </summary>
    public class TransferHelper
    {
        /// <summary>
        /// 将DataReader转换成实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> DataReaderToList<T>(IDataReader reader) where T : class,new()
        {
            List<T> result = new List<T>();
            Type type = typeof(T);
            var pis = type.GetProperties();
            while (reader.Read())
            {
                T item = new T();
                foreach (PropertyInfo pi in pis)
                {
                    string propertyName = pi.Name;
                    if (!reader.GetSchemaTable().Columns.Contains(propertyName)) continue;
                    Type propertyDataType = pi.PropertyType;
                    
                    var objVal = reader[propertyName];
                    if (objVal == DBNull.Value ) continue;
                    item.SetValue(pi, objVal);
                }
                result.Add(item);
            }
            return result;
        }



        /// <summary>
        /// 将实体对象列表转换成datatable，并将实体数据存储在表相应列
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="modelList">实体列表</param>
        /// <param name="tableName">表名</param>
        /// <returns>转换产生的表名</returns>
        public static System.Data.DataTable ToDataTable<T>(List<T> modelList,string tableName)
        {
            System.Data.DataTable dt = new System.Data.DataTable(typeof(T).Name);
          //  dt.TableName;
            dt.TableName = tableName.ToLower();
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add((tableName + "." + info.Name).ToLower());               
            }

            foreach (T model in modelList)
            {
                try
                {
                    DataRow dr = dt.NewRow();

                    foreach (PropertyInfo info in typeof(T).GetProperties())
                    {
                        dr[(tableName + "." + info.Name).ToLower()] = model.GetValue(info);// info.GetValue(model, null);
                    }
                    dt.Rows.Add(dr);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }

        public static DataTable ToDataTable<T>(List<T> modelList, string tableName, Dictionary<string, string> columnMapping = null, bool columnNameContainTableName = true)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            dataTable.TableName = tableName.ToLower();
            if (columnMapping != null)
            {
                foreach (string current in columnMapping.Keys)
                {
                    PropertyInfo propertyInfo = typeof(T).GetProperty(current);
                    if (propertyInfo != null)
                    {
                        dataTable.Columns.Add(columnMapping[current], propertyInfo.PropertyType.GetUnNullableType());
                    }
                    else
                    {
                        dataTable.Columns.Add(columnMapping[current]);
                    }
                }
            }
            else
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo propertyInfo = properties[i];
                    string text = propertyInfo.Name;
                    if (columnNameContainTableName)
                    {
                        text = tableName + "." + text;
                    }
                    dataTable.Columns.Add(text, propertyInfo.PropertyType.GetUnNullableType());
                }
            }
            foreach (T current2 in modelList)
            {
                try
                {
                    DataRow dataRow = dataTable.NewRow();
                    PropertyInfo[] properties = typeof(T).GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        PropertyInfo propertyInfo = properties[i];
                        string text = propertyInfo.Name;
                        if (columnMapping != null)
                        {
                            if (columnMapping.ContainsKey(text))
                            {
                                text = columnMapping[text];
                            }
                            else
                            {
                                text = string.Empty;
                            }
                        }
                        if (!string.IsNullOrEmpty(text))
                        {
                            if (columnNameContainTableName)
                            {
                                text = tableName + "." + text;
                            }
                            object value = current2.GetValue(propertyInfo);
                            if (value != null)
                            {
                                dataRow[text] = value;
                            }
                        }
                    }
                    dataTable.Rows.Add(dataRow);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dataTable;
        }

        /// <summary>
        /// 将datatable转成实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dataTable) where T : class,new()
        {
            List<T> list = null;

            //参数检测
            if (dataTable == null || dataTable.Rows.Count <= 0) return list;

            DataTable dtTemp = dataTable.Copy();
            if (dtTemp == null || dtTemp.Rows.Count <= 0) return list;

            //获取属性列表
            //T template = Activator.CreateInstance<T>();
            Type type = typeof(T);

            PropertyInfo[] pis = type.GetProperties();
            if (pis == null || pis.Length <= 0) return list;

            list = new List<T>();
            foreach (DataRow drTemp in dtTemp.Rows)
            {
                T item = new T();
                foreach (PropertyInfo pi in pis)
                {
                    string propertyName = pi.Name;
                    Type propertyDataType = pi.PropertyType;

                    bool isFind = false;
                    isFind = dtTemp.Columns.Contains(propertyName);
                    if (isFind == false) continue;

                    object propertyValue = drTemp[propertyName];
                    if (StringHelper.ObjectIsNullOrEmpty(propertyValue)) continue;
                    item.SetValue(pi, propertyValue);
                    //pi.SetValue(item, propertyValue, null);
                }
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 将实体对象转换成Hashtable
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="model">实体列表</param>
        /// <param name="tableName">表名</param>
        /// <returns>转换产生的表名</returns>
        public static Hashtable ToHashtable<T>(T model, string tableName)
        {
            Hashtable ht =new  Hashtable();

            try
            {
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {                             
                    ht.Add((tableName +"."+ info.Name).ToLower(), info.GetValue(model, null));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           return ht;
        }

        /// <summary>
        /// 把字符串数组转化为以分割符号(默认为",")分割的字符串
        /// </summary>
        /// <param name="arr">字符串数组</param>
        /// <param name="spliter">分割符号，默认为","</param>
        /// <returns>以分割符号的字符串</returns>
        public static string TransArrayToString(string[] arr,string spliter=",")
        {
            StringBuilder result = new StringBuilder();
            int count = 0;
            foreach (string str in arr)
            {
                if (count > 0)
                {
                    result.Append(",");
                }
                result.Append(str);
                count++;
            }

            return result.ToString();
        }
    }
}
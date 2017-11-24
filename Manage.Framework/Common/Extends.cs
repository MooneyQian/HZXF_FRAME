using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Manage.Framework;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.IO;
using System.Xml;

namespace System
{
    public static class Extends
    {
        #region Type
        /// <summary>
        /// 获取可空类型的实际类型
        /// </summary>
        /// <param name="conversionType">可空类型</param>
        /// <returns>可同类型的基类</returns>
        public static Type GetUnNullableType(this Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                //如果是泛型方法，且泛型类型为Nullable<>则视为可空类型
                //并使用NullableConverter转换器进行转换
                var nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return conversionType;
        }
        #endregion

        #region 类型转换
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj">待转换的数据</param>
        /// <param name="defaultValue">转换默认值</param>
        /// <returns>转换后的数据</returns>
        public static T Convert<T>(this object obj, T defaultValue)
        {
            return ParseHelper.ConvertValue(obj, defaultValue);
        }
        #endregion

        #region 实体适配器
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

            return AdapterHelper.Adapter(obj, defaultObjectEntity, ignoreProperties);
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
        public static List<T> Adapter<C, T>(this List<C> objList, List<T> defaultEntityList, params string[] ignoreProperties)
            where T : class,new()
            where C : class,new()
        {
            return AdapterHelper.Adapter(objList, defaultEntityList, ignoreProperties);
        }
        /// <summary>
        /// 实体适配器:把一种实体转换为另一种实体
        /// </summary>
        /// <typeparam name="T">转换后实体泛型</typeparam>
        /// <param name="obj">转换前的实体</param>
        /// <param name="defaultObjectEntity">默认值：如果转换失败在返回该默认值</param>
        /// <param name="mapping">属性映射配置</param>
        /// <param name="ignoreProperties">列外属性：这些属性将不被转换</param>
        /// <returns>转换后的实体</returns>
        public static T MappingAdapter<T>(this object obj, T defaultObjectEntity, Dictionary<string, string> mapping, params string[] ignoreProperties) where T : class,new()
        {

            return AdapterHelper.Adapter(obj, defaultObjectEntity, mapping, ignoreProperties);
        }
        /// <summary>
        /// 实体列表适配器
        /// </summary>
        /// <typeparam name="C">转换前列表中实体泛型</typeparam>
        /// <typeparam name="T">转换后列表中实体泛型</typeparam>
        /// <param name="objList">转换前实体列表</param>
        /// <param name="defaultEntityList">默认值：如果转换失败在返回该默认值</param>
        /// <param name="mapping">属性映射配置</param>
        /// <param name="ignoreProperties">列外属性：这些属性将不被转换</param>
        /// <returns>转换后的实体列表</returns>
        public static List<T> MappingAdapter<C, T>(this List<C> objList, List<T> defaultEntityList, Dictionary<string, string> mapping, params string[] ignoreProperties)
            where T : class,new()
            where C : class,new()
        {
            return AdapterHelper.Adapter(objList, defaultEntityList, mapping, ignoreProperties);
        }
        #endregion

        #region 反射赋值取值

        /// <summary>
        /// 对象属性赋值
        /// </summary>
        /// <param name="obj">待赋值的对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">值</param>
        public static void SetValue(this object obj, string propertyName, object value)
        {
            PropertyInfo pi = obj.GetType().GetProperty(propertyName);
            if (pi != null)
            {
                if (pi.PropertyType.IsValueType)
                {
                    if (pi.PropertyType == typeof(bool))
                    {
                        if (value == null || value == DBNull.Value)
                        {
                            PropertyHelper.FastSetValue(pi, obj, new object[] { false });
                        }
                        else
                        {
                            bool flag = value.Convert(false);
                            if (value.Convert(0) > 0) flag = true;
                            PropertyHelper.FastSetValue(pi, obj, new object[] { flag });
                        }
                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        PropertyHelper.FastSetValue(pi, obj, new object[] { value == null || value == DBNull.Value ? new DateTime(1, 1, 1) : value.Convert(new DateTime(1, 1, 1)) });
                    }
                    else
                    {
                        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
                            {
                                PropertyHelper.FastSetValue(pi, obj, new object[] { null });
                            }
                            else
                            {
                                Type[] typeArray = pi.PropertyType.GetGenericArguments();
                                Type baseType = typeArray[0];
                                PropertyHelper.FastSetValue(pi, obj, new object[] { System.Convert.ChangeType(value, baseType) });
                            }
                        }
                        else
                        {
                            PropertyHelper.FastSetValue(pi, obj, new object[] { value == null || value == DBNull.Value || value.ToString() == string.Empty ? System.Convert.ChangeType(0, pi.PropertyType) : System.Convert.ChangeType(value, pi.PropertyType) });
                        }

                    }
                }
                else
                {
                    PropertyHelper.FastSetValue(pi, obj, new object[] { value == DBNull.Value || value.ToString() == string.Empty ? null : value });
                }

            }
        }

        /// <summary>
        /// 对象属性赋值
        /// </summary>
        /// <param name="obj">待赋值的对象</param>
        /// <param name="pi">属性</param>
        /// <param name="value">值</param>
        public static void SetValue(this object obj, PropertyInfo pi, object value)
        {
            if (pi.PropertyType.IsValueType)
            {
                if (pi.PropertyType == typeof(bool))
                {
                    if (value == null || value == DBNull.Value)
                    {
                        PropertyHelper.FastSetValue(pi, obj, new object[] { false });
                    }
                    else
                    {
                        bool flag = value.Convert(false);
                        if (value.Convert(0) > 0) flag = true;
                        PropertyHelper.FastSetValue(pi, obj, new object[] { flag });
                    }

                }
                else if (pi.PropertyType == typeof(DateTime))
                {
                    PropertyHelper.FastSetValue(pi, obj, new object[] { value == null || value == DBNull.Value ? new DateTime(1, 1, 1) : value.Convert(new DateTime(1, 1, 1)) });
                }
                else
                {
                    if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {

                        if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
                        {
                            PropertyHelper.FastSetValue(pi, obj, new object[] { null });
                        }
                        else
                        {
                            Type[] typeArray = pi.PropertyType.GetGenericArguments();
                            Type baseType = typeArray[0];
                            PropertyHelper.FastSetValue(pi, obj, new object[] { System.Convert.ChangeType(value, baseType) });
                        }
                    }
                    else
                    {
                        PropertyHelper.FastSetValue(pi, obj, new object[] { value == null || value == DBNull.Value || value.ToString() == string.Empty ? System.Convert.ChangeType(0, pi.PropertyType) : System.Convert.ChangeType(value, pi.PropertyType) });
                    }

                }
            }
            else
            {
                if (pi.PropertyType == typeof(string))
                {
                    PropertyHelper.FastSetValue(pi, obj, new object[] { value == DBNull.Value || value == null ? string.Empty : value });
                }
                else
                {
                    PropertyHelper.FastSetValue(pi, obj, new object[] { value == DBNull.Value ? null : value });
                }

            }

        }
        /// <summary>
        /// 获取对象的属性值
        /// </summary>
        /// <param name="obj">待获取值的对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性的值</returns>
        public static object GetValue(this object obj, string propertyName)
        {
            PropertyInfo pi = obj.GetType().GetProperty(propertyName);
            if (pi != null)
            {
                return PropertyHelper.FastGetValue(pi, obj);
            }
            return null;
        }
        /// <summary>
        /// 获取对象的属性值
        /// </summary>
        /// <param name="obj">待获取值的对象</param>
        /// <param name="pi">属性</param>
        /// <returns>属性的值</returns>
        public static object GetValue(this object obj, PropertyInfo pi)
        {
            return PropertyHelper.FastGetValue(pi, obj);
        }
        #endregion

        #region DataTable和List转化
        /// <summary>
        /// 将实体对象列表转换成datatable，并将实体数据存储在表相应列
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="modelList">实体列表</param>
        /// <param name="tableName">表名</param>
        /// <returns>转换产生的表名</returns>
        public static DataTable ToDataTable<T>(this List<T> modelList, string tableName, Dictionary<string, string> columnMapping = null, bool columnNameContainTableName = true)
        {
            return TransferHelper.ToDataTable<T>(modelList, tableName, columnMapping, columnNameContainTableName);
        }
        /// <summary>
        /// 将实体对象转换成Hashtable
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="model">实体列表</param>
        /// <param name="tableName">表名</param>
        /// <returns>转换产生的表名</returns>
        public static Hashtable ToHashtable<T>(this T model, string tableName)
        {
            return TransferHelper.ToHashtable<T>(model, tableName);
        }
        /// <summary>
        /// 将datatable转换成实体对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dataTable) where T : class,new()
        {
            return TransferHelper.DataTableToList<T>(dataTable);
        }
        /// <summary>
        /// 将datareader转换成实体对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IDataReader dataReader) where T : class,new()
        {
            return TransferHelper.DataReaderToList<T>(dataReader);
        }
        #endregion

        #region 安全
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <returns>加密后的md5值</returns>
        public static string ToMD5(this string str)
        {
            return SecurityHelper.ToMD5(str);
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="encryptKey">加密密钥</param>
        /// <returns>返回DES加密结果</returns>
        public static string ToDES(this string str, string encryptKey)
        {
            return SecurityHelper.ToDES(str, encryptKey);
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="str">待解密字符串</param>
        /// <param name="decryptKey">解密密钥</param>
        /// <returns>返回使用DES解密后的结果</returns>
        public static string FromDES(this string str, string decryptKey)
        {
            return SecurityHelper.FromDES(str, decryptKey);
        }
        #endregion

        #region RSA加密
        public static void CreateRSAKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string PrivateKey = rsa.ToXmlString(true);
            string PublicKey = rsa.ToXmlString(false);
        }
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(string publicKey, byte[] data)
        {
            RSACryptoServiceProvider.UseMachineKeyStore = true;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(publicKey);

            int keySize = rsa.KeySize / 8;

            int bufferSize = keySize - 11;

            byte[] buffer = new byte[bufferSize];

            MemoryStream msInput = new MemoryStream(data);

            MemoryStream msOutput = new MemoryStream();

            int readLen = msInput.Read(buffer, 0, bufferSize);

            while (readLen > 0)
            {

                byte[] dataToEnc = new byte[readLen];

                Array.Copy(buffer, 0, dataToEnc, 0, readLen);

                byte[] encData = rsa.Encrypt(dataToEnc, false);

                msOutput.Write(encData, 0, encData.Length);

                readLen = msInput.Read(buffer, 0, bufferSize);

            }

            msInput.Close();

            byte[] result = msOutput.ToArray();    //得到加密结果

            msOutput.Close();

            rsa.Clear();

            return result;
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="rsaKeyString"></param>
        /// <param name="btEncryptedSecret"></param>
        /// <returns></returns>
        public static byte[] RSADecrypt(string privateKey, byte[] btEncryptedSecret)
        {
            RSACryptoServiceProvider.UseMachineKeyStore = true;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(privateKey);

            int keySize = rsa.KeySize / 8;

            byte[] buffer = new byte[keySize];

            MemoryStream msInput = new MemoryStream(btEncryptedSecret);

            MemoryStream msOutput = new MemoryStream();

            int readLen = msInput.Read(buffer, 0, keySize);

            while (readLen > 0)
            {

                byte[] dataToDec = new byte[readLen];

                Array.Copy(buffer, 0, dataToDec, 0, readLen);

                byte[] decData = rsa.Decrypt(dataToDec, false);

                msOutput.Write(decData, 0, decData.Length);

                readLen = msInput.Read(buffer, 0, keySize);

            }

            msInput.Close();

            byte[] result = msOutput.ToArray();    //得到解密结果

            msOutput.Close();

            rsa.Clear();

            return result;
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string RSAEncryptString(String sSource, string publicKey)
        {
            try
            {
                RSACryptoServiceProvider.UseMachineKeyStore = true;
                byte[] plaintbytes = RSAEncrypt(publicKey, Encoding.ASCII.GetBytes(sSource));
                return System.Convert.ToBase64String(plaintbytes);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string RSADecryptString(String sSource, string privateKey)
        {
            try
            {
                RSACryptoServiceProvider.UseMachineKeyStore = true;
                byte[] plaintbytes = RSADecrypt(privateKey, System.Convert.FromBase64String(sSource));
                return Encoding.UTF8.GetString(plaintbytes);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 枚举转换
        /// <summary>
        /// Enum转SelectList
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="except">需要排除的类型</param>
        /// <returns></returns>
        public static List<SelectListItem> EnumToSelectList(this Type enumType, string[] except = null)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (int n in Enum.GetValues(enumType))
            {
                var text = Enum.GetName(enumType, n);
                if (except != null && except.Contains(text))
                    continue;
                list.Add(new SelectListItem() { Text = text, Value = n.ToString() });
            }
            return list;
        }

        /// <summary>
        /// 获取枚举成员与值键值对字典
        /// </summary>
        /// <param name="enumType">typeof(枚举名)</param>
        /// <returns>Dictionary(值，枚举名)对</returns>
        public static Dictionary<int, string> GetEnumDictionary(this Type enumType)
        {
            return EnumHelper.GetEnumDictionary(enumType);
        }

        /// <summary>
        /// 根据值转化为对应枚举类型(不在枚举内返回默认值)
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="value">值或者名称</param>
        /// <returns>枚举值(不在枚举内返回默认值)</returns>
        public static T ConvertToEnum<T>(this string value)
        {
            return EnumHelper.ConvertToEnum<T>(value);
        }

        /// <summary>
        /// 根据值转换为对应枚举类型名称（存在枚举中返回空）
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="value">值</param>
        /// <returns>值对应的名称（存在枚举中返回空）</returns>
        public static string ToEnumName<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            return EnumHelper.ConvertToEnumName<T>(value.Convert<int>(0));
        }
        /// <summary>
        /// 根据值转换为对应枚举类型名称（存在枚举中返回空）
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="value">值</param>
        /// <returns>值对应的名称（存在枚举中返回空）</returns>
        public static string ToEnumName<T>(this int? value)
        {
            if (value == null)
                return string.Empty;
            return EnumHelper.ConvertToEnumName<T>(value ?? 0);
        }
        #endregion

        #region XML TO Model

        /// <summary>
        /// XMLNode数组转对象实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmls"></param>
        /// <param name="defaultObjectEntity"></param>
        /// <returns></returns>
        public static T ToModel<T>(this XmlNode[] xmls, T defaultObjectEntity = null) where T : class,new()
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
            if (xmls != null)
            {
                Type t = typeof(T);
                PropertyInfo[] tpis = t.GetProperties();

                foreach (XmlNode node in xmls)
                {
                    var tpi = tpis.FirstOrDefault(o => string.Compare(o.Name, node.Name, true) == 0);

                    if (tpi != null)
                    {
                        //判断属性是否允许写入数据,即判断属性是否有set功能
                        if (tpi.CanWrite == true)
                        {
                            entity.SetValue(tpi, node.InnerText);
                        }
                    }
                }
            }
            return entity;
        }

        #endregion

        #region 时间转换
        /// <summary>
        /// 字符串格式化时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string time, string format)
        {
            try
            {
                return DateTime.ParseExact(time, format, System.Globalization.CultureInfo.CurrentCulture);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        #endregion

        #region 匿名变量        
        /// <summary>
        /// 将dynamic类型的对象传递到view页面
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object ToDynamic(this object entity)
        {
            return DynamicFactory.ToDynamic(entity);
        }
        #endregion
    }
}

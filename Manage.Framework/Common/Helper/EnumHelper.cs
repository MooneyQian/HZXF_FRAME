using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    /// <summary>
    /// 枚举转换助手
    /// </summary>
    public class EnumHelper
    {
        #region 枚举转换
        /// <summary>
        /// 根据值转化为对应枚举类型
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="value">值或者名称</param>
        /// <returns>枚举值</returns>
        public static T ConvertToEnum<T>(string value) 
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 根据值转化为对应枚举类型
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="value">值或者名称</param>
        /// <returns>枚举值</returns>
        public static string ConvertToEnumName<T>(int value)
        {
            try
            {
                if (Enum.IsDefined(typeof(T), value))
                    return Enum.GetName(typeof(T), value);
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region 获取枚举成员与值Dictionary
        /// <summary>
        /// 获取枚举成员与值键值对字典
        /// </summary>
        /// <param name="enumType">typeof(枚举名)</param>
        /// <returns>Dictionary(值，枚举名)对</returns>
        public static Dictionary<int, string> GetEnumDictionary(Type enumType)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            try
            {
               
                string[] names = Enum.GetNames(enumType);
                foreach (string n in names)
                {   
                    int key=(int)(byte)Enum.Parse(enumType, n);
                    if (dict.ContainsKey(key))
                    {
                        dict.Add(key, n);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dict;
        }
        #endregion
    }
}

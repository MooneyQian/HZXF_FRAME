using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Manage.Framework
{
    /// <summary>
    /// 类型转换助手:对类型实现强制转换
    /// </summary>
    internal class ParseHelper
    {
        #region ConvertValue
        /// <summary>
        /// 值类型转换 (T为string,char,bool,DateTime,double,decimal,float,ulong,uint,ushort,byte,long,int,short,sbyte)
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="sourceValue">源值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回转换后的值</returns>
        public static T ConvertValue<T>(object sourceValue, T defaultValue)
        {
            object getValue = defaultValue;
            Type t = typeof(T);
            if (sourceValue != null)
            {
                getValue = sourceValue;
                if (t == typeof(string) || t == typeof(char))
                {
                    if (getValue.ToString() == string.Empty)
                    {
                        getValue = defaultValue;
                    }
                }
                if (t == typeof(bool))
                {
                    bool temp;
                    if (bool.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                if (t == typeof(DateTime))
                {
                    DateTime temp;
                    if (DateTime.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  小数(可为负数)
                if (t == typeof(double) || t == typeof(decimal) || t == typeof(float))
                {
                    double temp;
                    if (double.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  无符号整数(不可为负数)
                if (t == typeof(ulong) || t == typeof(uint) || t == typeof(ushort) || t == typeof(byte))
                {
                    ulong temp;
                    if (ulong.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  有符号整数(可为负数)
                if (t == typeof(long) || t == typeof(int) || t == typeof(short) || t == typeof(sbyte))
                {
                    long temp;
                    if (long.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
            }
            return (T)Convert.ChangeType(getValue, t);
        }

        /// <summary>
        /// 可为空的值类型转换 (T只能为bool,DateTime,double,decimal,float,ulong,uint,ushort,byte,long,int,short,sbyte)
        /// (string,char不需要使用Nullable类型)
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="sourceValue">原值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回转换后的值</returns>
        public static T? ConvertValue<T>(object sourceValue, T? defaultValue) where T : struct
        {
            object getValue = defaultValue.HasValue ? defaultValue.Value : sourceValue;
            Type t = typeof(T);
            if (sourceValue != null)
            {
                getValue = sourceValue;
                if (t == typeof(bool))
                {
                    bool temp;
                    if (bool.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                if (t == typeof(DateTime))
                {
                    DateTime temp;
                    if (DateTime.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  小数(可为负数)
                if (t == typeof(double) || t == typeof(decimal) || t == typeof(float))
                {
                    double temp;
                    if (double.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  无符号整数(不可为负数)
                if (t == typeof(ulong) || t == typeof(uint) || t == typeof(ushort) || t == typeof(byte))
                {
                    ulong temp;
                    if (ulong.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  无符号整数(可为负数)
                if (t == typeof(long) || t == typeof(int) || t == typeof(short) || t == typeof(sbyte))
                {
                    long temp;
                    if (long.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
            }
            if (getValue == null)
            {
                return (T?)getValue;
            }
            return (T?)Convert.ChangeType(getValue, t);
        }
         
        #endregion

        #region 判断类型
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="expression">待判断的内容</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
            {
                return IsNumeric(expression.ToString());
            }
            return false;

        }
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="expression">待判断的内容</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }
        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="expression">待判断的内容</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsSignlessInt(string expression)
        {
            return Regex.IsMatch(expression, @"^[0-9]*$");
        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression">待判断的内容</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
            {
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }
        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="expression">待判断的内容</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] expression)
        {
            if (expression == null)
            {
                return false;
            }
            if (expression.Length < 1)
            {
                return false;
            }
            foreach (string id in expression)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;

        }
        #endregion
    }
}

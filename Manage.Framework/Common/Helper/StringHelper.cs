using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace Manage.Framework
{
    /// <summary>
    /// 字符串助手
    /// </summary>
    public class StringHelper
    {
        private static Regex RegexBr = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);
        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <param name="str">内容字符串</param>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringarray"></param>
        /// <param name="strsplit"></param>
        /// <returns></returns>
        public static bool IsCompriseStr(string str, string stringarray, string strsplit)
        {
            if (stringarray == "" || stringarray == null)
            {
                return false;
            }
            str = str.ToLower();
            string[] stringArray = SplitString(stringarray.ToLower(), strsplit);
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (str.IndexOf(stringArray[i]) > -1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }

            }
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strlength"></param>
        /// <returns></returns>
        public static object[] CreateRegionCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素 
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
            Random rnd = new Random();
            //定义一个object数组用来 
            object[] bytes = new object[strlength];
            /**/
            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中 
                每个汉字有四个区位码组成 
                区位码第1位和区位码第2位作为字节数组第一个元素 
                区位码第3位和区位码第4位作为字节数组第二个元素 
             */
            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位 
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

                //区位码第2位 
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的 

                //种子避免产生重复值 
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();
                //区位码第3位 
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();
                //区位码第4位 
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();
                //定义两个字节变量存储产生的随机汉字区位码 
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中 
                byte[] str_r = new byte[] { byte1, byte2 };
                //将产生的一个汉字的字节数组放入object数组中 
                bytes.SetValue(str_r, i);
            }
            return bytes;
        }
        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray)
        {
            return InArray(str, SplitString(stringarray, ","), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit)
        {
            return InArray(str, SplitString(stringarray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
        {
            return InArray(str, SplitString(stringarray, strsplit), caseInsensetive);
        }


        /// <summary>
        /// 删除字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(string str)
        {
            for (int i = str.Length; i >= 0; i--)
            {
                if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
                {
                    str.Remove(i, 1);
                }
            }
            return str;
        }


        /// <summary>
        /// 清除给定字符串中的回车及换行符
        /// </summary>
        /// <param name="str">要清除的字符串</param>
        /// <returns>清除后返回的字符串</returns>
        public static string ClearBR(string str)
        {
            Match m = null;
            for (m = RegexBr.Match(str); m.Success; m = m.NextMatch())
            {
                str = str.Replace(m.Groups[0].ToString(), "");
            }
            return str;
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }
                if (startIndex > str.Length)
                {
                    return "";
                }
            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }
            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }
        /// <summary>
        /// 清理字符串(清楚不是字母，不是.@-的符号)
        /// </summary>
        /// <param name="strIn">待处理的字符串</param>
        /// <returns>处理后的结果</returns>
        public static string CleanInput(string strIn)
        {
            return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", "");
        }
        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                    {
                        return "";
                    }
                    else
                    {
                        return p_SrcString.Substring(p_StartIndex,((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                    }
                }
            }
            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;
                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾
                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }
                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }
        /// <summary>
        /// 获取Unicode的子字符串
        /// </summary>
        /// <param name="str">待处理的字符串</param>
        /// <param name="len">子字符串长度</param>
        /// <param name="p_TailString">尾部要追加的字符串</param>
        /// <returns>获取Unicode的子字符串</returns>
        public static string GetUnicodeSubString(string str, int len, string p_TailString)
        {
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度

            int charLen = str.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        byteCount += 2;
                    else// 按英文字符计算加1
                        byteCount += 1;
                    if (byteCount > len)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                    result = str.Substring(0, pos) + p_TailString;
            }
            else
                result = str;

            return result;
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">待分割的字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <returns>分割后的结果</returns>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!StringHelper.StrIsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                {
                    string[] tmp = { strContent };
                    return tmp;
                }
                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
            {
                return new string[0] { };
            }
        }

       /// <summary>
        /// 分割字符串
       /// </summary>
        /// <param name="strContent">待分割的字符串</param>
        /// <param name="strSplit">分割符</param>
       /// <param name="count">限定结果的总数</param>
        /// <returns>分割后的结果</returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];

            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">待分割的字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <param name="ignoreRepeatItem">忽略重复项</param>
        /// <param name="maxElementLength">单个元素最大长度</param>
        /// <returns>分割后的结果</returns>
        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem, int maxElementLength)
        {
            string[] result = SplitString(strContent, strSplit);

            return ignoreRepeatItem ? DistinctStringArray(result, maxElementLength) : result;
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">待分割的字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <param name="ignoreRepeatItem">忽略重复项</param>
        /// <param name="minElementLength">单个元素最大长度</param>
        /// <param name="maxElementLength">单个元素最小长度</param>
        /// <returns>分割后的结果</returns>
        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem, int minElementLength, int maxElementLength)
        {
            string[] result = SplitString(strContent, strSplit);

            if (ignoreRepeatItem)
            {
                result = DistinctStringArray(result);
            }
            return PadStringArray(result, minElementLength, maxElementLength);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">待分割的字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <param name="ignoreRepeatItem">忽略重复项</param>
        /// <returns>分割后的结果</returns>
        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem)
        {
            return SplitString(strContent, strSplit, ignoreRepeatItem, 0);
        }

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <param name="maxElementLength">字符串数组中单个元素的最大长度</param>
        /// <returns>过滤后的结果</returns>
        public static string[] DistinctStringArray(string[] strArray, int maxElementLength)
        {
            Hashtable h = new Hashtable();

            foreach (string s in strArray)
            {
                string k = s;
                if (maxElementLength > 0 && k.Length > maxElementLength)
                {
                    k = k.Substring(0, maxElementLength);
                }
                h[k.Trim()] = s;
            }

            string[] result = new string[h.Count];

            h.Keys.CopyTo(result, 0);

            return result;
        }

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <returns>清楚后的结果</returns>
        public static string[] DistinctStringArray(string[] strArray)
        {
            return DistinctStringArray(strArray, 0);
        }
        /// <summary>
        /// 进行指定的替换(脏字过滤)
        /// </summary>
        /// <param name="str">待处理的字符串</param>
        /// <param name="bantext">过滤的规则【习近平=*】</param>
        /// <returns>过滤后的结果</returns>
        public static string StrFilter(string str, string bantext)
        {
            string text1 = "";
            string text2 = "";
            string[] textArray1 = SplitString(bantext, "\r\n");
            for (int num1 = 0; num1 < textArray1.Length; num1++)
            {
                text1 = textArray1[num1].Substring(0, textArray1[num1].IndexOf("="));
                text2 = textArray1[num1].Substring(textArray1[num1].IndexOf("=") + 1);
                str = str.Replace(text1, text2);
            }
            return str;
        }
        /// <summary>
        /// 字段串是否为Null或为""(空)
        /// </summary>
        /// <param name="str">待判断的字符串</param>
        /// <returns>判断的结果</returns>
        public static bool StrIsNullOrEmpty(string str)
        {
            if (str == null || str.Trim() == "")
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 过滤字符串数组中每个元素为合适的大小
        /// 当长度小于minLength时，忽略掉,-1为不限制最小长度
        /// 当长度大于maxLength时，取其前maxLength位
        /// 如果数组中有null元素，会被忽略掉
        /// </summary>
        /// <param name="minLength">单个元素最小长度</param>
        /// <param name="maxLength">单个元素最大长度</param>
        /// <returns>过滤后的结果</returns>
        public static string[] PadStringArray(string[] strArray, int minLength, int maxLength)
        {
            if (minLength > maxLength)
            {
                int t = maxLength;
                maxLength = minLength;
                minLength = t;
            }

            int iMiniStringCount = 0;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (minLength > -1 && strArray[i].Length < minLength)
                {
                    strArray[i] = null;
                    continue;
                }
                if (strArray[i].Length > maxLength)
                {
                    strArray[i] = strArray[i].Substring(0, maxLength);
                }
                iMiniStringCount++;
            }

            string[] result = new string[iMiniStringCount];
            for (int i = 0, j = 0; i < strArray.Length && j < result.Length; i++)
            {
                if (strArray[i] != null && strArray[i] != string.Empty)
                {
                    result[j] = strArray[i];
                    j++;
                }
            }


            return result;
        }

        #region GetRegexMatchValue
        /// <summary>
        /// 唯一匹配 Match
        /// </summary>
        /// <param name="str">待处理的字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <returns>返回唯一的匹配值</returns>
        public static string GetRegexMatchValue(string str, string regex)
        {
            string valueString = string.Empty;
            Match match = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase).Match(str);
            if (match != null)
            {
                valueString = ReplaceLineBreak(match.Value);
            }
            return valueString;
        }
        /// <summary>
        /// 在开始字符串和全文结束之间查找一个匹配的项
        /// </summary>
        /// <param name="str">待处理的字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <param name="startString">开始的字符串</param>
        /// <returns>返回唯一的匹配值</returns>
        public static string GetRegexMatchValue(string str, string regex, string startString)
        {
            return GetRegexMatchValue(str, regex, startString, string.Empty);
        }
        /// <summary>
        /// 在开始字符串和结束字符串之间查找一个匹配的项
        /// </summary>
        /// <param name="str">待处理的字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <param name="startString">开始的字符串</param>
        /// <param name="endString">结束的字符串</param>
        /// <returns>返回唯一的匹配值</returns>
        public static string GetRegexMatchValue(string str, string regex, string startString, string endString)
        {
            string temp = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase).Match(str).Value;
            if (temp == string.Empty)
            {
                return string.Empty;
            }
            int startIndex = 0;
            if (startString != null && startString != string.Empty)
            {
                startIndex = temp.IndexOf(startString);
                startIndex = startIndex > -1 ? startIndex + startString.Replace("\r", string.Empty).Replace("\n", string.Empty).Length : 0;
            }
            int endStringLength = 0;
            if (endString != null && endString != string.Empty)
            {
                endStringLength = endString.Replace("\r", string.Empty).Replace("\n", string.Empty).Length;
            }
            int strLength = temp.Length - endStringLength >= 0 ? temp.Length - endStringLength : 0;
            temp = temp.Substring(startIndex).Substring(0, temp.Length - endStringLength);
            return ReplaceLineBreak(temp);
        }
        #endregion

        #region GetRegexMatchesCount
        /// <summary>
        /// 获取匹配 Matches总数
        /// </summary>
        /// <param name="str">待处理的字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <returns>匹配个数</returns>
        public static int GetRegexMatchesCount(string str, string regex)
        {
            int count = 0;
            MatchCollection matches = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase).Matches(str);
            if (matches != null)
            {
                count = matches.Count;
            }
            return count;
        }
        #endregion

        #region ReplaceLineBreak
        /// <summary>
        /// 替换\r\n为空，清空换行
        /// </summary>
        /// <param name="str">待处理的字符串</param>
        /// <returns>处理后的字符串内容</returns>
        public static string ReplaceLineBreak(string str)
        {
            return str.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }
        #endregion


        /// <summary>
        /// 判断Object对象是否为Null或""
        /// </summary>
        /// <param name="aobj_data">Object数据</param>
        /// <returns>
        /// 检测结果
        /// True:Object对象为空
        /// False:Object对象不为空
        /// </returns>
        /// <example>
        /// <code>
        /// [C#]
        /// object data = "123";
        /// bool result = StringHelper.ObjectIsNullOrEmpty(data);
        /// </code>
        /// </example>
        public static bool ObjectIsNullOrEmpty(object aobj_data)
        {
            if (aobj_data == null || string.IsNullOrEmpty(aobj_data.ToString()))
                return true;

            //增加对DbNull.Value的判断 add by Vincent.Q 11.05.05
            if (aobj_data == DBNull.Value)
                return true;

            return false;
        }

        /// <summary>
        /// Object数据类型转为Bool数据类型
        /// </summary>
        /// <param name="objData">Object对象</param>
        /// <param name="defaultValue">Bool默认值</param>
        /// <returns>Bool数据类型值</returns>
        /// <example>
        /// <code>
        /// [C#]
        /// object obj = "12345";
        /// bool defaultValue = true;
        /// bool returnBool = StringHelper.ObjectToBool(obj, defaultValue);
        /// </code>
        /// </example>
        public static bool ObjectToBool(object objData, bool defaultValue)
        {
            //参数检测
            if (StringHelper.ObjectIsNullOrEmpty(objData))
            {
                return defaultValue;
            }

            //强制转换
            if (objData is bool)
            {
                return (bool)objData;
            }

            bool resultValue = false;
            bool result = bool.TryParse(objData.ToString(), out resultValue);
            if (result == true)
            {
                return resultValue;
            }

            //此处将1/0等常用变量转为true/false
            //定义:0,false|1,true
            string s = objData.ToString();
            if (s == "1" || s == "yes")
            {
                resultValue = true;
            }
            else if (s == "0" || s == "no")
            {
                resultValue = false;
            }

            return resultValue;
        }

        /// <summary>
        /// Object数据类型转为DateTime数据类型
        /// </summary>
        /// <param name="objData">Object对象</param>
        /// <param name="defaultValue">DateTime默认值</param>
        /// <returns>DateTime数据类型值</returns>
        /// <example>
        /// <code>
        /// [C#]
        /// object obj = "12345";
        /// DateTime defaultValue = DateTime.Now;
        /// DateTime returnBool = StringHelper.ObjectToDateTime(obj, defaultValue);
        /// </code>
        /// </example>
        public static DateTime ObjectToDateTime(object objData, DateTime defaultValue)
        {
            //参数检测
            if (StringHelper.ObjectIsNullOrEmpty(objData))
            {
                return defaultValue;
            }

            if (objData is DateTime)
            {
                return (DateTime)objData;
            }

            DateTime returnValue = new DateTime(1990, 1, 1);
            bool result = DateTime.TryParse(objData.ToString(), out returnValue);
            if (result == false)
            {
                return defaultValue;
            }

            return returnValue;
        }

        /// <summary>
        /// object对象转为日期型
        /// </summary>
        /// <param name="aobj_data">Object对象</param>
        /// <returns>DateTime数据类型值</returns>
        /// <example>
        /// <code>
        /// [C#]
        /// object obj = "12345";
        /// DateTime returnValue = StringHelper.ObjectToDateTime(obj);
        /// </code>
        /// </example>
        public static DateTime ObjectToDateTime(object aobj_data)
        {
            return StringHelper.ObjectToDateTime(aobj_data, DateTime.MinValue);
        }

        /// <summary>
        /// Object数据类型转为Int数据类型
        /// </summary>
        /// <param name="objData">Object对象</param>
        /// <param name="defaultValue">Int默认值</param>
        /// <returns>Int数据类型值</returns>
        /// <example>
        /// <code>
        /// [C#]
        /// object obj = "12345";
        /// int defaultValue = 10;
        /// int returnInt = StringHelper.ObjectToInt(obj, defaultValue);
        /// </code>
        /// </example>
        public static int ObjectToInt(object objData, int defaultValue)
        {
            //参数检测
            if (objData == null || objData == DBNull.Value)
            {
                return defaultValue;
            }

            //强制转换
            if (objData is int)
            {
                return (int)objData;
            }

            int returnValue = 0;
            bool result = int.TryParse(objData.ToString(), out returnValue);

            if (result == false)
            {
                //此处可生成日志
                return defaultValue;
            }

            return returnValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core
{
    /// <summary>
    /// 前台Ajax请求的统一返回结果类
    /// </summary>
    public class AjaxResult
    {
        private bool iserror = false;

        /// <summary>
        /// 是否产生错误
        /// </summary>
        public bool IsError { get { return iserror; } }

        /// <summary>
        /// 错误信息，或者成功信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 成功可能时返回的数据
        /// </summary>
        public object Data { get; set; }

        public AjaxResult()
        {
        }

        #region Error
        /// <summary>
        /// 输错错误信息
        /// </summary>
        /// <returns></returns>
        public static AjaxResult Error()
        {
            return new AjaxResult()
            {
                iserror = true
            };
        }
        /// <summary>
        /// 输错错误信息
        /// </summary>
        /// <param name="message">附属消息</param>
        /// <returns></returns>
        public static AjaxResult Error(string message)
        {
            return new AjaxResult()
            {
                iserror = true,
                Message = message
            };
        }
        /// <summary>
        /// 输错错误信息
        /// </summary>
        /// <param name="data">附属数据</param>
        /// <returns></returns>
        public static AjaxResult Error(object data)
        {
            return new AjaxResult()
            {
                iserror = true,
                Data = data
            };
        }
        /// <summary>
        /// 输错错误信息
        /// </summary>
        /// <param name="data">附属数据</param>
        /// <param name="message">附属消息</param>
        /// <returns></returns>
        public static AjaxResult Error(object data, string message)
        {
            return new AjaxResult()
            {
                iserror = true,
                Data = data,
                Message = message
            };
        }

        #endregion

        #region Success
        /// <summary>
        /// 输出成功信息
        /// </summary>
        /// <returns></returns>
        public static AjaxResult Success()
        {
            return new AjaxResult()
            {
                iserror = false
            };
        }
        /// <summary>
        /// 输出成功信息
        /// </summary>
        /// <param name="message">附属消息</param>
        /// <returns></returns>
        public static AjaxResult Success(string message)
        {
            return new AjaxResult()
            {
                iserror = false,
                Message = message
            };
        }
        /// <summary>
        /// 输出成功信息
        /// </summary>
        /// <param name="data">附属数据</param>
        /// <returns></returns>
        public static AjaxResult Success(object data)
        {
            return new AjaxResult()
            {
                iserror = false,
                Data = data
            };
        }
        /// <summary>
        /// 输出成功信息
        /// </summary>
        /// <param name="data">附属数据</param>
        /// <param name="message">附属消息</param>
        /// <returns></returns>
        public static AjaxResult Success(object data, string message)
        {
            return new AjaxResult()
            {
                iserror = false,
                Data = data,
                Message = message
            };
        }

        #endregion
    }
}

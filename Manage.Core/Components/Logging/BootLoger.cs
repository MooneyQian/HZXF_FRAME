using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Manage.Framework;

namespace Manage.Core.Logging
{
    /// <summary>
    /// 启动日志书写器
    /// </summary>
    public class BootLoger
    {
        private static StreamWriter _logWrite;
        private static string _filename = "";
        static BootLoger()
        {
            _filename = RequestHelper.MapPath("/Log/" + DateTime.Now.ToString("yyyy-MM-dd") + "/AppStart_" + DateTime.Now.Ticks.ToString() + ".txt");
            string dirctoryname = Path.GetDirectoryName(_filename);
            if (!Directory.Exists(dirctoryname))
            {
                Directory.CreateDirectory(dirctoryname);
            }

        }

        public static void Open()
        {
            _logWrite = new StreamWriter(_filename, true, Encoding.UTF8);
        }

        /// <summary>
        /// 写入启动日志
        /// </summary>
        /// <param name="happenTime">发生时间</param>
        /// <param name="error">错误信息</param>
        public static void WriteLog(DateTime happenTime, string message,bool needSplit=false)
        {
#if !DEBUG
            if (_logWrite == null)
            {
                Open();
            }
            _logWrite.WriteLine(happenTime.ToString() + " : " + message);
            if (needSplit)
            {
                _logWrite.WriteLine("=====================================================================");
            }
            _logWrite.Flush();
#endif
        }

        private static bool _isDispose = false;
        /// <summary>
        /// 关闭日志读写器
        /// </summary>
        public static void Dispose()
        {
            _isDispose = true;
            if (_logWrite != null)
            {
                _logWrite.Close();
                _logWrite.Dispose();
            }
        }

        ~BootLoger()
        {
            if (!_isDispose)
            {
                _logWrite.Close();
                _logWrite.Dispose();
            }
        }
    }
}

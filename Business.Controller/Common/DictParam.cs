using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Controller.Common
{
    /// <summary>
    /// 对应数据库字典表
    /// </summary>
    public static class DictParam
    {
        /// <summary>
        /// 车型
        /// </summary>
        public static string CarType { get { return "CarType"; } }

        /// <summary>
        /// 班组
        /// </summary>
        public static string TeamCode { get { return "TeamCode"; } }

        /// <summary>
        /// 违规类型对应fs_reward_daily字段
        /// </summary>
        public static string FoulType { get { return "FoulType"; } }


        /// <summary>
        /// 设备参数（品牌类型）
        /// </summary>
        public static string EquipmentParameters
        {
            get { return "EquipmentParameters"; }
        }

        /// <summary>
        /// 设备系统巡检项目
        /// </summary>
        public static string MonitorItem
        {
            get { return "MonitorItem"; }
        }

        /// <summary>
        /// 获取系统内置字典
        /// </summary>
        public static string Sysdictionary
        {
            get { return "SYS"; }
        }
    }
}

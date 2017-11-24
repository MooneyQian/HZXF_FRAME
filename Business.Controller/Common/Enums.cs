using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 用户类型
    /// </summary>
    public enum EMUSERTYPE
    {
        超级管理员 = 0,
        管理员 = 2,
        用户 = 1,
        匿名用户 = 3
    }

  
    /// <summary>
    /// 处理状态
    /// </summary>
    public enum EMDELCSTATUS
    {
        未处理 = 0,
        已处理 = 1,
        重复报 = 2
    }

    /// <summary>
    /// 上报状态
    /// </summary>
    public enum EMREPORTCSTATUS
    {
        失败 = 0,
        成功 = 1

    }
}

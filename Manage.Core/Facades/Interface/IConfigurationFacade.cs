
using Manage.Core.Models;
using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Facades
{

    public interface IConfigurationFacade : IBaseFacade
    {
  

        /// <summary>
        /// 根据当前登陆用户获取用户配置信息
        /// 如果没有配置信息则 创建一天空的配置信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        Configuration_S GetConfigurationByUserId(string UserId);

        //更新
        Boolean UpdateConfiguration(Configuration_U cu);
         
    }
}
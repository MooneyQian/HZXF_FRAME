using Manage.Core.Facades.SSOFactory;
using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Facades
{
    /// <summary>
    /// 单点登录操作类转换器
    /// </summary>
    public class SSOFacadeAdapter
    {
        /// <summary>
        /// 通用配置
        /// </summary>
        public static AppConfig appConfig
        {
            get
            {
                return ConfigManager.Instance.Single<AppConfig>();
            }
        }
        /// <summary>
        /// 获取UserFacade类实例
        /// </summary>
        /// <returns></returns>
        public static IUserFacade UserInstance()
        {
            if (appConfig.SSOEnable)
            {
                switch (appConfig.SSOType)
                {
                    case 2:
                        return new SSOUserFacade();
                    case 3:
                        return new UserFacade(appConfig.SSOFactoryConfig);
                    default:
                        return new UserFacade();
                }
            }
            return new UserFacade();
        }

        /// <summary>
        /// 获取AppRegisterFacade类实例
        /// </summary>
        /// <returns></returns>
        public static IAppRegisterFacade AppRegisterInstance()
        {
            if (appConfig.SSOEnable)
            {
                switch (appConfig.SSOType)
                {
                    case 2:
                        return new SSOAppRegisterFacade();
                    case 3:
                        return new AppRegisterFacade(appConfig.SSOFactoryConfig);
                    default:
                        return new AppRegisterFacade();
                }
            }
            return new AppRegisterFacade();
        }
    }
}

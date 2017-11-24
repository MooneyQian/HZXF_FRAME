using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    public class SSOBaseFacade : IBaseFacade
    {
        /// <summary>
        /// 【基础方法】应用系统配置信息
        /// </summary>
        protected AppConfig appConfig;

        protected string _DBConfigPath = string.Empty;

        public SSOBaseFacade()
        {
            appConfig = ConfigManager.Instance.Single<AppConfig>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configFileName">数据库配置文件</param>
        public SSOBaseFacade(string configFileName)
        {
            _DBConfigPath = configFileName;
            appConfig = ConfigManager.Instance.Single<AppConfig>();
        }

        public Type SelectModel;

        #region 共用方法-不实现

        /// <summary>
        /// 【基础方法】获取单个对象
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T GetByID<T>(string ID) where T : BaseModel, new()
        {
            return null;
        }

        /// <summary>
        /// 【基础方法】获取所有对象
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <returns></returns>
        public List<T> GetAll<T>() where T : BaseModel, new()
        {
            return null;
        }

        /// <summary>
        /// 【基础方法】获取所有分页对象
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <returns></returns>
        public List<T> GetPaged<T>(PageInfo pi) where T : BaseModel, new()
        {
            return null;
        }

        /// <summary>
        /// 【基础方法】新增对象
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <param name="model">需要新增的实体</param>
        /// <returns>新增对象ID</returns>
        public void Add<T>(T model) where T : BaseModel, new()
        {
        }

        /// <summary>
        /// 【基础方法】【慎用】更新对象中所有字段
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <param name="model">需要更新的实体</param>
        public void Edit<T>(T model) where T : BaseModel, new()
        {
        }

        /// <summary>
        /// 【基础方法】删除多个对象
        /// </summary>
        /// <param name="IDs">需要删除数据的ID，使用“,”分隔</param>
        public void Delete(string IDs)
        {
        }

        #endregion
    }
}

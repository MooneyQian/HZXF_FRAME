using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    public class BaseFacade<P> : IBaseFacade where P : AggregateRoot, new()
    {
        /// <summary>
        /// 【基础方法】应用系统配置信息
        /// </summary>
        protected AppConfig appConfig;

        protected string _DBConfigPath = string.Empty;

        public BaseFacade()
        {
            appConfig = ConfigManager.Instance.Single<AppConfig>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configFileName">数据库配置文件</param>
        public BaseFacade(string configFileName)
        {
            _DBConfigPath = configFileName;
            appConfig = ConfigManager.Instance.Single<AppConfig>();
        }

        public Type SelectModel;

        #region 共用方法

        /// <summary>
        /// 【基础方法】获取单个对象
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T GetByID<T>(string ID) where T : BaseModel, new()
        {
            using (var factory = new BaseAccess(_DBConfigPath))
            {
                return factory.GetSingle<P>(ID).Adapter<T>(new T());
            }
        }

        /// <summary>
        /// 【基础方法】获取所有对象
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <returns></returns>
        public List<T> GetAll<T>() where T : BaseModel, new()
        {
            using (var factory = new BaseAccess(_DBConfigPath))
            {
                return factory.GetAll<P>().Adapter<P, T>(new List<T>());
            }
        }

        /// <summary>
        /// 【基础方法】获取所有分页对象
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <returns></returns>
        public List<T> GetPaged<T>(PageInfo pi) where T : BaseModel, new()
        {
            using (var factory = new BaseAccess(_DBConfigPath))
            {
                return factory.GetPage<P>(pi).Adapter<P, T>(new List<T>());
            }
        }

        /// <summary>
        /// 【基础方法】新增对象
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <param name="model">需要新增的实体</param>
        /// <returns>新增对象ID</returns>
        public void Add<T>(T model) where T : BaseModel, new()
        {
            using (var factory = new BaseAccess(_DBConfigPath))
            {
                if (string.IsNullOrEmpty(model.ID))
                    model.ID = Guid.NewGuid().ToString();
                factory.Insert<P>(model.Adapter<P>(new P()));
            }
        }

        /// <summary>
        /// 【基础方法】【慎用】更新对象中所有字段
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <param name="model">需要更新的实体</param>
        public void Edit<T>(T model) where T : BaseModel, new()
        {
            using (var factory = new BaseAccess(_DBConfigPath))
            {
                if (!string.IsNullOrEmpty(model.ID))
                {
                    var model_old = factory.GetSingle<P>(model.ID);
                    model_old = model.Adapter<P>(model_old);//将页面对象的属性转换到数据库对象modle中
                    factory.Update<P>(model_old);
                }
            }
        }

        /// <summary>
        /// 【基础方法】删除多个对象
        /// </summary>
        /// <param name="IDs">需要删除数据的ID，使用“,”分隔</param>
        public void Delete(string IDs)
        {
            using (var factory = new BaseAccess(_DBConfigPath))
            {
                factory.Delete<P>(IDs);
            }
        }

        #endregion
    }
}

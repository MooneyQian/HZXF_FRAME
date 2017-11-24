using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    public interface IBaseFacade
    {
        /// <summary>
        /// 【基础方法】通过ID获取单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ID"></param>
        /// <returns></returns>
        T GetByID<T>(string ID) where T : BaseModel, new();

        /// <summary>
        /// 【基础方法】获取所有对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetAll<T>() where T : BaseModel, new();
        
        /// <summary>
        /// 【基础方法】获取所有分页对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetPaged<T>(PageInfo pi) where T : BaseModel, new();

        /// <summary>
        /// 【基础方法】新增对象
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <param name="model">需要新增的实体(ID会自动创建)</param>
        /// <returns>新增对象ID</returns>
        void Add<T>(T model) where T : BaseModel, new();
        
        /// <summary>
        /// 【基础方法】【慎用】更新对象中所有字段
        /// </summary>
        /// <typeparam name="T">继承BaseModel</typeparam>
        /// <param name="model">需要更新的实体</param>
        void Edit<T>(T model) where T : BaseModel, new();
                
        /// <summary>
        /// 【基础方法】删除多个对象
        /// </summary>
        /// <param name="IDs">需要删除数据的ID，使用“,”分隔</param>
        void Delete(string IDs);
    }
}

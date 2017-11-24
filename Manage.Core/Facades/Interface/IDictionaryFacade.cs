using System;
using Manage.Core.Models;
using System.Collections.Generic;
using Manage.Framework;
namespace Manage.Core.Facades
{
    public interface IDictionaryFacade : IBaseFacade
    {
        /// <summary>
        /// 获取数据字典（包含标题项）
        /// </summary>
        /// <param name="DictType"></param>
        /// <returns></returns>
        List<Dictionary_S> GetAllDict(string DictType);

        /// <summary>
        /// 获取数据字典（不包含标题项）
        /// </summary>
        /// <param name="DictType"></param>
        /// <returns></returns>
        List<Dictionary_S> GetDict(string DictType);

        /// <summary>
        /// 判断字典类型是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DictType"></param>
        /// <returns></returns>
        bool IsExists(string ID, string DictType);
        
        /// <summary>
        /// 判断字典编号是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DictType"></param>
        /// <param name="DictCode"></param>
        /// <returns></returns>
        bool IsExists(string ID, string DictType, string DictCode);
                
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        void Add(Dictionary_I model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        void Edit(Dictionary_U model);
        
        /// <summary>
        /// 删除多个对象
        /// </summary>
        /// <param name="IDs">需要删除数据的ID，使用“,”分隔</param>
        void Del(string IDs);

        /// <summary>
        /// 根据字典code 获取字典名称
        /// </summary>
        /// <param name="DicCody"></param>
        /// <returns></returns>
        string GetDicNameByDicCode(string DicCody);
    }
}

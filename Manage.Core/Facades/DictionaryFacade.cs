using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;
using Manage.Core.Models;
using Manage.SSO.Entity;
using Manage.Core.DAL;
using Manage.Core.Entitys;
using Manage.Core.Common;

namespace Manage.Core.Facades
{
    public class DictionaryFacade : BaseFacade<Sys_DictionaryEntity>, IDictionaryFacade
    {
        /// <summary>
        /// 获取数据字典（包含标题项）
        /// </summary>
        /// <param name="DictType"></param>
        /// <returns></returns>
        public List<Dictionary_S> GetAllDict(string DictType)
        {
            using (var factory = new BaseAccess())
            {
                var list = factory.GetAll<Sys_DictionaryEntity>(Specification<Sys_DictionaryEntity>.Create(c => c.DictType == DictType)) ?? new List<Sys_DictionaryEntity>();
                return list.OrderBy(o => o.DictOrder).ThenBy(t => t.DictCode).ToList().Adapter<Sys_DictionaryEntity, Dictionary_S>(new List<Dictionary_S>());
            }
        }
        /// <summary>
        /// 获取数据字典（不包含标题项）
        /// </summary>
        /// <param name="DictType"></param>
        /// <returns></returns>
        public List<Dictionary_S> GetDict(string DictType)
        {
            using (var factory = new BaseAccess())
            {
                var list = factory.GetAll<Sys_DictionaryEntity>(Specification<Sys_DictionaryEntity>.Create(c => c.DictType == DictType && c.ParDictID != null && c.ParDictID != "" && c.ParDictID != Define._TOPPARENTID)) ?? new List<Sys_DictionaryEntity>();
                return list.OrderBy(o => o.DictOrder).ThenBy(t => t.DictCode).ToList().Adapter<Sys_DictionaryEntity, Dictionary_S>(new List<Dictionary_S>());
            }
        }

        /// <summary>
        /// 判断字典类型是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DictType"></param>
        /// <returns></returns>
        public bool IsExists(string ID, string DictType)
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<Sys_DictionaryEntity>.Create(c => c.DictType == DictType);
                if (!string.IsNullOrEmpty(ID))
                    spec &= Specification<Sys_DictionaryEntity>.Create(c => c.ID != ID);
                return factory.IsExists<Sys_DictionaryEntity>(spec);
            }
        }

        /// <summary>
        /// 判断字典编号是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DictType"></param>
        /// <param name="DictCode"></param>
        /// <returns></returns>
        public bool IsExists(string ID, string DictType, string DictCode)
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<Sys_DictionaryEntity>.Create(c => c.DictType == DictType && c.DictCode == DictCode);
                if (!string.IsNullOrEmpty(ID))
                    spec &= Specification<Sys_DictionaryEntity>.Create(c => c.ID != ID);
                return factory.IsExists<Sys_DictionaryEntity>(spec);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        public void Add(Dictionary_I model)
        {
            using (var factory = new BaseAccess())
            {
                if (string.IsNullOrEmpty(model.ID))
                    model.ID = Guid.NewGuid().ToString();
                factory.Insert<Sys_DictionaryEntity>(model.Adapter<Sys_DictionaryEntity>(new Sys_DictionaryEntity()));

                //清理缓存
                if ((model.IsCache ?? 0) == 1)
                    Manage.Open.CacheshipFactory.Instance.ClearDictionaryCache(model.DictType);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public void Edit(Dictionary_U model)
        {
            using (var factory = new BaseAccess())
            {
                try
                {
                    if (!string.IsNullOrEmpty(model.ID))
                    {
                        var model_old = factory.GetSingle<Sys_DictionaryEntity>(model.ID);
                        model_old = model.Adapter<Sys_DictionaryEntity>(model_old);//将页面对象的属性转换到数据库对象modle中
                        factory.Update<Sys_DictionaryEntity>(model_old, false);
                        if ((model.IsCache ?? 0) != (model.IsCache_Old ?? 0))
                        {
                            //更新其他所有DictType的IsCache
                            string type = model_old.DictType;
                            factory.Update<Sys_DictionaryEntity>(model_old, new string[] { "IsCache" }, Specification<Sys_DictionaryEntity>.Create(c => c.DictType == model_old.DictType && c.ParDictID != Define._TOPPARENTID), false);
                        }
                        factory.Commit();

                        //清除缓存
                        if ((model_old.IsCache ?? 0) == 1)
                            Manage.Open.CacheshipFactory.Instance.ClearDictionaryCache(model_old.DictType);
                    }
                }
                catch (Exception ex)
                {
                    factory.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 删除多个对象
        /// </summary>
        /// <param name="IDs">需要删除数据的ID，使用“,”分隔</param>
        public void Del(string IDs)
        {
            using (var factory = new BaseAccess())
            {
                try
                {
                    foreach (var id in IDs.Split(','))
                    {
                        var model = factory.GetSingle<Sys_DictionaryEntity>(id);
                        if (model != null)
                        {
                            //获取所有子节点
                            List<Sys_DictionaryEntity> childrens = new List<Sys_DictionaryEntity>() { model };
                            childrens = GetChilds(factory, model.ID, childrens);
                            foreach (var c in childrens)
                            {
                                //清除缓存
                                if ((c.IsCache ?? 0) == 1)
                                    Manage.Open.CacheshipFactory.Instance.ClearDictionaryCache(c.DictType);

                                factory.Delete<Sys_DictionaryEntity>(c, false);
                            }
                        }
                    }
                    factory.Commit();
                }
                catch (Exception ex)
                {
                    factory.Rollback();
                    throw ex;
                }
            }
        }
        private List<Sys_DictionaryEntity> GetChilds(BaseAccess factory, string ParDictID, List<Sys_DictionaryEntity> entitys)
        {
            if (entitys == null)
                entitys = new List<Sys_DictionaryEntity>();
            var list = factory.GetAll<Sys_DictionaryEntity>(Specification<Sys_DictionaryEntity>.Create(c => c.ParDictID == ParDictID));
            entitys.AddRange(list);
            foreach (var m in list)
            {
                entitys.AddRange(GetChilds(factory, m.ID, entitys));
            }
            return entitys;
        }

        /// <summary>
        /// 根据字典code 获取字典名称
        /// </summary>
        /// <param name="DicCody"></param>
        /// <returns></returns>
        public string GetDicNameByDicCode(string DicCody)
        {
            using (var factory  = new BaseAccess())
            {
                var spec = Specification<Sys_DictionaryEntity>.Create(c => c.DictCode == DicCody);
                return factory.GetSingle<Sys_DictionaryEntity>(spec).DictName;
            }
        }
    }
}

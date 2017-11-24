using Manage.Core.Cache;
using Manage.Core.Common;
using Manage.Core.Facades;
using Manage.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Manage.Open
{
    /// <summary>
    /// 数据字典对外开放接口
    /// </summary>
    public class DictionaryshipFactory
    {
        /// <summary>
        /// 单例对象
        /// </summary>
        public static readonly DictionaryshipFactory Instance = new DictionaryshipFactory();

        /// <summary>
        /// facade延迟对象
        /// </summary>
        private Lazy<IDictionaryFacade> _facade = new Lazy<IDictionaryFacade>(() => { return new DictionaryFacade(); }, true);

        /// <summary>
        /// 【缓存优先】获取数据字典
        /// </summary>
        /// <param name="DictType"></param>
        /// <returns></returns>
        public List<SysDictionary> GetDictionary(string DictType)
        {
            var dicts = DictionaryCacheStorage.Current.Get(DictType);
            if (dicts == null)
            {
                var entitys = _facade.Value.GetAllDict(DictType);
                dicts = entitys.Where(w => w.ParDictID != Define._TOPPARENTID).ToList().Adapter<Dictionary_S, SysDictionary>(new List<SysDictionary>());
                foreach (var dict in dicts)
                {
                    var parDict = dicts.Where(w => w.ID == dict.ParDictID).FirstOrDefault();
                    dict.ParDictCode = parDict == null ? Define._TOPPARENTID : parDict.DictCode;
                }
                //判断并设置缓存
                if (entitys.Where(w => w.ParDictID == Define._TOPPARENTID).Select(s => (s.IsCache ?? 0) == 1).FirstOrDefault())
                    DictionaryCacheStorage.Current.Set(DictType, dicts);
            }
            return dicts;
        }

        /// <summary>
        /// 【缓存优先】获取数据字典下拉框数据格式
        /// </summary>
        /// <param name="DictType"></param>
        /// <param name="hasDefault">是否包含默认空值项（--请选择--）</param>
        /// <param name="defaultText">默认空值项显示值（默认值：--请选择--）</param>
        /// <returns></returns>
        public List<SelectListItem> GetDictSelectList(string DictType, bool hasDefault = false, string defaultText = "--请选择--")
        {
            var list = GetDictionary(DictType).Select(s => new SelectListItem()
            {
                Text = s.DictName,
                Value = s.DictCode
            }).ToList();
            if (hasDefault || (string.IsNullOrEmpty(defaultText) && defaultText != "--请选择--"))
                list.Insert(0, new SelectListItem() { Value = "", Text = defaultText });
            return list;
        }

        /// <summary>
        /// 【缓存优先】获取数据字典显示值
        /// </summary>
        /// <param name="DictType">字典表类型</param>
        /// <param name="DictCode">字典值</param>
        /// <param name="isRecursive">是否递归显示名称（用于树字典）</param>
        /// <returns></returns>
        public string GetDictName(string DictType, string DictCode, bool isRecursive = false)
        {
            if (string.IsNullOrEmpty(DictType) || string.IsNullOrEmpty(DictCode))
                return string.Empty;
            var dict = GetDictionary(DictType).Where(w => w.DictCode == DictCode).FirstOrDefault();
            if (dict != null)
            {
                //处理递归显示父节点名称
                if (isRecursive && !string.IsNullOrEmpty(dict.ParDictCode) && dict.ParDictCode != Define._TOPPARENTID)
                {
                    var parName = GetDictName(DictType, dict.ParDictCode, isRecursive);
                    if (!string.IsNullOrEmpty(parName))
                        return parName + "-" + dict.DictName;
                    else
                        return dict.DictName;
                }
                else
                    return dict.DictName;
            }
            return string.Empty;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manage.Framework;
using Manage.Core.Facades;
using Manage.Core.Models;
using Manage.Core.Common;

namespace Manage.Core.Controllers
{
    public class DictionaryController : BaseController
    {
        #region 定义
        //延迟加载业务处理器
        Lazy<IDictionaryFacade> _DictionaryFacade = new Lazy<IDictionaryFacade>(() => { return new DictionaryFacade(); }, true);
        //视图地址
        string _path = "/Views/Dictionary/";
        #endregion

        #region 视图
        /// <summary>
        /// 【视图】字典列表
        /// </summary>
        /// <returns></returns>
        public ViewResult List()
        {
            return View();
        }

        /// <summary>
        /// 【视图】添加子字典
        /// </summary>
        /// <param name="pid">父节点ID，0表示顶层</param>
        /// <returns></returns>
        public ViewResult Add(string pid)
        {
            Dictionary_S model = new Dictionary_S();
            if (pid == "root")
            {
                model = new Dictionary_S()
                {
                    ParDictID = "0",
                    ParDictName = "",
                    LevelNO = 1,
                    IsCache = 1
                };
            }
            else
            {
                var perDictionary = _DictionaryFacade.Value.GetByID<Dictionary_S>(pid);
                model = new Dictionary_S()
                {
                    ParDictID = pid,
                    ParDictName = perDictionary.DictName,
                    ParDictCode = perDictionary.DictCode,
                    LevelNO = perDictionary.LevelNO + 1,
                    DictType = perDictionary.DictType,
                    IsCache = perDictionary.IsCache
                };
            }
            ViewBag.ActionUrl = "_Add";
            ViewBag.OperType = "Add";
            return View(model);
        }

        /// <summary>
        /// 【视图】编辑字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult Edit(string id)
        {
            var model = _DictionaryFacade.Value.GetByID<Dictionary_S>(id);
            if (model != null)
            {
                var perDictionary = _DictionaryFacade.Value.GetByID<Dictionary_S>(model.ParDictID);
                if (perDictionary != null)
                    model.ParDictName = perDictionary.DictName;
            }
            ViewBag.ActionUrl = "_Edit";
            ViewBag.OperType = "Edit";
            return View("Add", model);
        }

        /// <summary>
        /// 【视图】查看字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult Show(string id)
        {
            var model = _DictionaryFacade.Value.GetByID<Dictionary_S>(id);
            if (model != null)
            {
                var perDictionary = _DictionaryFacade.Value.GetByID<Dictionary_S>(model.ParDictID);
                if (perDictionary != null)
                    model.ParDictName = perDictionary.DictName;
            }
            return View(model);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取字典Json
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDictionaryTree()
        {
            var tree = _DictionaryFacade.Value.GetAll<Dictionary_S>().Select(s => new
            {
                id = s.ID,
                pId = s.ParDictID == Define._TOPPARENTID ? "root" : s.ParDictID,//增加一个ID为root的顶层虚拟字典
                name = s.DictName,
                title = s.DictCode,
                order = s.DictOrder ?? 0,
                level = s.LevelNO ?? 1,
                open = false
            }).ToList();
            tree.Add(new
            {
                id = "root",
                pId = "0",
                name = "顶层虚拟字典",
                title = "虚拟字典",
                order = 0,
                level = -1,
                open = true
            });
            return Json(AjaxResult.Success(tree));
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public JsonResult _Add(Dictionary_I menu)
        {
            try
            {
                menu.IsCache = menu.IsCache ?? 0;
                _DictionaryFacade.Value.Add(menu);
                var model = (new
                {
                    id = menu.ID,
                    pId = menu.ParDictID == Define._TOPPARENTID ? "root" : menu.ParDictID,
                    name = menu.DictName,
                    title = menu.DictCode,
                    order = menu.DictOrder,
                    level = menu.LevelNO
                });
                return Json(AjaxResult.Success(model, "字典新增成功!"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("字典新增失败!错误原因：" + ex.Message));
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public JsonResult _Edit(Dictionary_U menu)
        {
            try
            {
                menu.IsCache = menu.IsCache ?? 0;
                _DictionaryFacade.Value.Edit(menu);
                var model = (new
                {
                    id = menu.ID,
                    name = menu.DictName,
                    title = menu.DictCode,
                    order = menu.DictOrder
                });
                return Json(AjaxResult.Success(model, "字典更新成功!"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("字典更新失败!错误原因：" + ex.Message));
            }
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult _Delete(string ids)
        {
            try
            {
                _DictionaryFacade.Value.Del(ids);
                return Json(AjaxResult.Success("用户删除成功！"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error(ex.Message));
            }
        }

        /// <summary>
        /// 判断字典类型是否有重复
        /// </summary>
        /// <param name="DictType"></param>
        /// <returns></returns>
        public JsonResult _DictTypeIsSuc(string ID, string DictType)
        {
            try
            {
                var res = _DictionaryFacade.Value.IsExists(ID, DictType);
                return Json(res);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("错误！"));
            }
        }

        /// <summary>
        /// 判断字典编号是否有重复
        /// </summary>
        /// <param name="DictType"></param>
        /// <param name="DictCode"></param>
        /// <returns></returns>
        public JsonResult _DictCodeIsSuc(string ID, string DictType, string DictCode)
        {
            try
            {
                var res = _DictionaryFacade.Value.IsExists(ID, DictType, DictCode);
                return Json(res);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("错误！"));
            }
        }


        #endregion

        #region 扩展

        #endregion
    }
}

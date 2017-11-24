using Manage.Framework.Cache.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Manage.Core.Controllers
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    public class CacheManagerController : BaseController
    {
        /// <summary>
        /// 【视图】列表
        /// </summary>
        /// <returns></returns>
        public ViewResult List()
        {
            //var sysCaches = new[]{
            //    new {name="Default",value = "默认缓存域"},
            //    new {name="CACHE_ORGAN_ROLE_MENU",value = "角色、分组、菜单缓存域"},
            //    new {name="CACHE_LOGIN_USER",value = "登录用户信息缓存域"},
            //    new {name="CACHE_SYS_DICTIONARY",value = "数据字典缓存域"}
            //}.ToList();
            //var otherCaches = MemoryCacheClient.GetAllInstanceName().Where(c => !sysCaches.Select(s => s.name).Contains(c)).ToList();

            //ViewBag.sysCaches = sysCaches.Select(s => s.ToDynamic()).ToList();
            //ViewBag.otherCaches = otherCaches;

            return View();
        }


        public JsonResult _List() 
        {
            var data = new[]{
                new {name="Default",value = "默认缓存域"},
                new {name="CACHE_ORGAN_ROLE_MENU",value = "角色、分组、菜单缓存域"},
                new {name="CACHE_LOGIN_USER",value = "登录用户信息缓存域"},
                new {name="CACHE_SYS_DICTIONARY",value = "数据字典缓存域"}
            }.ToList();
            var result = new
            {
                Rows = data,
            };
            return Json(result);
        
        }
        /// <summary>
        /// 清理所有缓存
        /// </summary>
        /// <returns></returns>
        public JsonResult _ClearAll()
        {
            try
            {
                //清理所有缓存空间
                var allCaches = MemoryCacheClient.GetAllInstanceName();
                foreach (var name in allCaches)
                {
                    var cache = MemoryCacheClient.GetInstance(name);
                    if (cache != null)
                        cache.RemoveAll();
                }
                return Json(AjaxResult.Success("清理全部完成！"));
            }
            catch (Exception e)
            {
                return Json(AjaxResult.Error("清理全部失败，原因：" + e.Message));
            }
        }

        /// <summary>
        /// 清理指定缓存
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult _ClearCache(string name)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var cache = MemoryCacheClient.GetInstance(name);
                    if (cache != null)
                        cache.RemoveAll();
                }
                return Json(AjaxResult.Success(name + "：清理完成！"));
            }
            catch (Exception e)
            {
                return Json(AjaxResult.Error(name+"：清理失败，原因：" + e.Message));
            }
        }
    }

}

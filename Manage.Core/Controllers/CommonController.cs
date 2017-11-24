using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manage.Framework;
using Manage.Core.Facades;
using Manage.Core.Models;

namespace Manage.Core.Controllers
{
    public class CommonController : BaseController
    {
        #region 定义
        //延迟加载业务处理器
        Lazy<IUserFacade> _UserFacade = new Lazy<IUserFacade>(() => { return SSOFacadeAdapter.UserInstance(); }, true);
        //视图地址
        string _path = "/Views/Common/";
        #endregion

        #region 视图
        #endregion

        #region 字典相关
        /// <summary>
        /// 【公共方法】获取字典表数据 {Text,Value}
        /// </summary>
        /// <param name="DictType"></param>
        /// <returns></returns>
        public JsonResult _GetDictionary(string DictType)
        {
            return Json(Manage.Open.DictionaryshipFactory.Instance.GetDictSelectList(DictType), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 【公共方法】获取字典表树结构数据{id,pId,name,title}
        /// </summary>
        /// <param name="DictType"></param>
        /// <returns></returns>
        public JsonResult _GetDictionaryTree(string DictType)
        {
            return Json(Manage.Open.DictionaryshipFactory.Instance.GetDictionary(DictType)
                .Select(s => new
                {
                    id = s.DictCode,
                    pId = s.ParDictCode,
                    name = s.DictName,
                    title = s.DictDesc
                }).ToList(),
                JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 分组相关
        /// <summary>
        /// 获取组织机构树
        /// </summary>
        /// <returns></returns>
        public JsonResult _GetOrganTree()
        {
            try
            {
                var organs = Manage.Open.MembershipFactory.Instance.GetAllOrgans_Cache();
                var data = organs.Select(s => new
                {
                    id = s.ID,
                    pid = s.OrganParentID,
                    text = s.OrganName,
                    name = s.OrganName,
                    title = s.OrganDesc,
                    order = s.OrganOrder,
                    level = s.LevelNO,
                    open = s.LevelNO <= 1
                }).ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}

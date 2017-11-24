
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;
using Manage.Core.Entitys;
using Manage.Core.Models;
using Manage.Open;
using System.Data;

namespace Manage.Core.Facades
{

    public class ConfigurationFacade : BaseFacade<SYS_ConfigurationEntity>, IConfigurationFacade
    {


        /// <summary>
        /// 根据当前登陆用户获取用户配置信息
        /// 如果没有配置信息则 创建一天空的配置信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Configuration_S GetConfigurationByUserId(string UserId)
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<SYS_ConfigurationEntity>.Create(c => c.USERID == UserId);

                try
                {
                    if (factory.IsExists<SYS_ConfigurationEntity>(spec))
                    {
                        var entity = factory.GetSingle<SYS_ConfigurationEntity>(spec);
                        return entity.Adapter<Configuration_S>(new Configuration_S());
                    }
                    else
                    {
                        var entity = new SYS_ConfigurationEntity();
                        entity.ID = Guid.NewGuid().ToString();
                        entity.USERID = UserId;
                        factory.Insert<SYS_ConfigurationEntity>(entity, false);
                        factory.Commit();
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    factory.Rollback();
                    return null;
                }

            }
        }


        //更新
        public Boolean UpdateConfiguration(Configuration_U model)
        {
            using (var factory = new BaseAccess())
            {
                if (!string.IsNullOrEmpty(model.USERID))
                {
                    var spec = Specification<SYS_ConfigurationEntity>.Create(c => c.USERID == model.USERID);
                    try
                    {
                        var model_old = factory.GetSingle<SYS_ConfigurationEntity>(spec);
                        string id = model_old.ID;
                        model_old = model.Adapter<SYS_ConfigurationEntity>(model_old);//将页面对象的属性转换到数据库对象modle中
                        model_old.ID = id;
                        factory.Update<SYS_ConfigurationEntity>(model_old, false);
                        factory.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        factory.Rollback();
                        return false;
                    }
                }
                return false;
            }
        }
    }
}
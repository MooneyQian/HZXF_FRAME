using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework;
using Manage.Core.Models;
using Manage.SSO.Entity;
using Manage.Core.DAL;
using Manage.Open;
using Manage.Core.SSO;

namespace Manage.Core.Facades
{
    public class OrganizationFacade : BaseFacade<OrganizationEntity>, IOrganizationFacade
    {
        /// <summary>
        /// 判断分组编号是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="OrganNO"></param>
        /// <returns></returns>
        public bool IsExists(string ID, string OrganNO)
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<OrganizationEntity>.Create(c => c.OrganNO == OrganNO);
                if (!string.IsNullOrEmpty(ID))
                    spec &= Specification<OrganizationEntity>.Create(c => c.ID != ID);
                return factory.IsExists<OrganizationEntity>(spec);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        public void Add(Organization_I model)
        {
            using (var factory = new BaseAccess())
            {
                if (string.IsNullOrEmpty(model.ID))
                    model.ID = Guid.NewGuid().ToString();
                factory.Insert<OrganizationEntity>(model.Adapter<OrganizationEntity>(new OrganizationEntity()));

                //清理缓存
                CacheshipFactory.Instance.ClearOrganCache();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        public void Edit(Organization_U model)
        {
            using (var factory = new BaseAccess())
            {
                if (!string.IsNullOrEmpty(model.ID))
                {
                    var model_old = factory.GetSingle<OrganizationEntity>(model.ID);
                    model_old = model.Adapter<OrganizationEntity>(model_old);//将页面对象的属性转换到数据库对象modle中
                    factory.Update<OrganizationEntity>(model_old);

                    //清理缓存
                    CacheshipFactory.Instance.ClearOrganCache();
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
                        var model = factory.GetSingle<OrganizationEntity>(id);
                        if (model != null)
                        {
                            //获取所有子节点
                            List<OrganizationEntity> childrens = new List<OrganizationEntity>() { model };
                            childrens = GetChilds(factory, model.ID, childrens);
                            foreach (var c in childrens)
                            {
                                factory.Delete<OrganizationEntity>(c, false);
                            }
                        }
                    }
                    factory.Commit();

                    //清理缓存
                    CacheshipFactory.Instance.ClearOrganCache();
                }
                catch (Exception ex)
                {
                    factory.Rollback();
                    throw ex;
                }
            }
        }
        private List<OrganizationEntity> GetChilds(BaseAccess factory, string OrganParentID, List<OrganizationEntity> entitys)
        {
            if (entitys == null)
                entitys = new List<OrganizationEntity>();
            var list = factory.GetAll<OrganizationEntity>(Specification<OrganizationEntity>.Create(c => c.OrganParentID == OrganParentID));
            entitys.AddRange(list);
            foreach (var m in list)
            {
                entitys.AddRange(GetChilds(factory, m.ID, entitys));
            }
            return entitys;
        }


        /// <summary>
        /// 获取分组用户列表
        /// </summary>
        /// <param name="OrganizationID"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public List<OrganUser_S> GetUserWithOrgan(string OrganizationID, string LoginName, string DisplayName)
        {
            using (var factory = new BaseAccess())
            {
                var OrganUser = factory.GetAll<UserOrganizationEntity>(Specification<UserOrganizationEntity>.Create(c => c.OrganizationID == OrganizationID));
                var AllUsers = Manage.Open.MembershipFactory.Instance.GetAllUsers(LoginName, DisplayName);
                var list = AllUsers.Adapter<UserInfo, OrganUser_S>(new List<OrganUser_S>());
                list.ForEach(f => f.IsHas = OrganUser.Select(s => s.UserID).Contains(f.ID));
                list.ForEach(f => f.IsMain = (f.IsHas && OrganUser.Count(w => w.UserID == f.ID && w.IsDefault == (int)YesNo.Yes) > 0));
                list = list.OrderBy(o => !o.IsHas).ThenBy(t => t.UserDisplayName).ToList();
                return list;
            }
        }

        /// <summary>
        /// 获取分组用户列表
        /// </summary>
        /// <param name="OrganizationID"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public List<UserOrganizationEntity> GetUserWithOrgan(string OrganizationID, string UserID)
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<UserOrganizationEntity>.Create(c => true);
                if (!string.IsNullOrEmpty(OrganizationID))
                    spec &= Specification<UserOrganizationEntity>.Create(c => c.OrganizationID == OrganizationID);
                else
                {
                    var listorgan = factory.GetAll<UserOrganizationEntity>(Specification<UserOrganizationEntity>.Create(c => c.UserID == UserID));
                    string strOrgans = string.Empty;
                    listorgan.ForEach(c => strOrgans += c.OrganizationID + ",");
                    spec &= Specification<UserOrganizationEntity>.Create(c => strOrgans.Contains(c.OrganizationID));
                }
                var list = factory.GetAll<UserOrganizationEntity>(spec);
                return list;
            }
        }


        /// <summary>
        /// 设置分组用户
        /// </summary>
        /// <param name="OrganID"></param>
        /// <param name="UserID"></param>
        /// <param name="IsHas">ture设置 false移除</param>
        public void SetUserOrgan(string OrganID, string UserID, bool IsHas)
        {
            using (var factory = new BaseAccess())
            {
                //获取用户的所有分组
                var UserAllOrgan = factory.GetAll<UserOrganizationEntity>(Specification<UserOrganizationEntity>.Create(c => c.UserID == UserID));
                if (IsHas)
                {
                    //增加
                    if (UserAllOrgan == null || UserAllOrgan.Count == 0 || UserAllOrgan.Count(c => c.OrganizationID == OrganID) == 0)
                    {
                        var organUser = new UserOrganizationEntity()
                        {
                            OrganizationID = OrganID,
                            UserID = UserID,
                            IsDefault = UserAllOrgan.Count(c => c.IsDefault == (int)YesNo.Yes) == 0 ? (int)YesNo.Yes : (int)YesNo.No
                        };
                        factory.Insert<UserOrganizationEntity>(organUser);
                    }
                }
                else
                {
                    //移除
                    if (UserAllOrgan != null && UserAllOrgan.Count(c => c.OrganizationID == OrganID) > 0)
                    {
                        factory.Delete<UserOrganizationEntity>(UserAllOrgan.Where(c => c.OrganizationID == OrganID).First());
                    }
                }
            }
        }

        /// <summary>
        /// 设置为主分组
        /// </summary>
        /// <param name="OrganID"></param>
        /// <param name="UserID"></param>
        public void SetDefaultOrgan(string OrganID, string UserID)
        {
            using (var factory = new BaseAccess())
            {
                try
                {
                    //获取用户的所有分组
                    var UserAllOrgan = factory.GetAll<UserOrganizationEntity>(Specification<UserOrganizationEntity>.Create(c => c.UserID == UserID));
                    var defaultOrgan = UserAllOrgan.Where(c => c.IsDefault == (int)YesNo.Yes).ToList() ?? new List<UserOrganizationEntity>();
                    var setOrgan = UserAllOrgan.Where(w => w.OrganizationID == OrganID).FirstOrDefault();
                    if (setOrgan != null)
                    {
                        //取消所有原主分组（做荣誉操作，正常情况下不会有多个主分组）
                        foreach (var m in defaultOrgan)
                        {
                            m.IsDefault = (int)YesNo.No;
                            factory.Update<UserOrganizationEntity>(m, false);
                        }
                        //设置需要设置的分组为主分组
                        setOrgan.IsDefault = (int)YesNo.Yes;
                        factory.Update<UserOrganizationEntity>(setOrgan, false);

                        factory.Commit();
                    }
                }
                catch (Exception e)
                {
                    factory.Rollback();
                    throw e;
                }
            }
        }

        #region MembershipFactory

        /// <summary>
        /// 获取所有分组，从数据库取
        /// </summary>
        /// <returns></returns>
        public List<OrganizationInfo> GetAllOrgans()
        {
            using (var factory = new BaseAccess())
            {
                return factory.GetAll<OrganizationEntity>(Specification<OrganizationEntity>.Create(c => c.RecordStatus != (int)RecordStatus.UnEnable)).Adapter<OrganizationEntity, OrganizationInfo>(new List<OrganizationInfo>());
            }
        }

        /// <summary>
        /// 获取所有分组，从数据库取
        /// </summary>
        /// <returns></returns>
        public List<OrganizationInfo> GetAllOrgans(Organization_S model)
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<OrganizationEntity>.Create(c => c.RecordStatus != (int)RecordStatus.UnEnable);
                if (!string.IsNullOrWhiteSpace(model.OrganName))
                {
                    spec &= Specification<OrganizationEntity>.Create(c => c.OrganName.Contains(model.OrganName));
                }
                if (!string.IsNullOrWhiteSpace(model.Extend4))
                {
                    spec &= Specification<OrganizationEntity>.Create(c => c.Extend4 == model.Extend4);
                }

                return factory.GetAll<OrganizationEntity>(spec).Adapter<OrganizationEntity, OrganizationInfo>(new List<OrganizationInfo>());
            }
        }

        /// <summary>
        /// 通过分组ID获取分组，从数据库中取
        /// </summary>
        /// <returns></returns>
        public OrganizationInfo GetOrganByID(string ID)
        {
            using (var factory = new BaseAccess())
            {
                var model = factory.GetSingle<OrganizationEntity>(Specification<OrganizationEntity>.Create(c => c.ID == ID && c.RecordStatus != (int)RecordStatus.UnEnable));
                if (model != null)
                    return model.Adapter<OrganizationInfo>(new OrganizationInfo());
                else
                    return new OrganizationInfo();
            }
        }


        /// <summary>
        /// 通过分组名称获取分组
        /// </summary>
        /// <param name="OrgainName"></param>
        /// <returns></returns>
        public OrganizationInfo GetOrganByName(string OrgainName)
        {
            using (var factory = new BaseAccess())
            {
                var model = factory.GetSingle<OrganizationEntity>(Specification<OrganizationEntity>.Create(c => c.OrganName ==OrgainName.Trim()));
                if (model != null)
                    return model.Adapter<OrganizationInfo>(new OrganizationInfo());
                else
                    return new OrganizationInfo();
            }
        }
        /// <summary>
        /// 获取用户的默认分组
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public UserOrganizationEntity GetDefaultOrganByUser(string UserID)
        {
            using (var factory = new BaseAccess())
            {
                var spec = Specification<UserOrganizationEntity>.Create(c => true);
                spec &= Specification<UserOrganizationEntity>.Create(c => c.UserID == UserID && c.IsDefault == 1);
                return factory.GetSingle<UserOrganizationEntity>(spec);

            }
        }
        /// <summary>
        /// 获取用户的所有分组
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<UserOrganizationEntity> GetOrgansByUser(string UserID)
        {
            using (var factory = new BaseAccess())
            {
                return factory.GetAll<UserOrganizationEntity>(Specification<UserOrganizationEntity>.Create(c => c.UserID == UserID));
            }
        }

        #endregion
    }
}

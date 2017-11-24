using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manage.Framework;
using Manage.Core;
using Business.Controller.Facades;
using Business.Model.Models;
using Manage.Open;
using Business.Controller.Common;
using Business.Controller.Common.Helpers;
using Manage.Core.Facades;
using Manage.Core.SSO;
namespace Business.Controller.Common
{
    ///// <summary>
    ///// 登录权限控制类
    ///// </summary>
    //public class AuthorityCl : BaseController
    //{
    //    public static Authority GetAuthorityInfo(OrganizationInfo Orgain)
    //    {
    //        Lazy<ISchoolFacade> _SchoolFacade = new Lazy<ISchoolFacade>(() => { return new SchoolFacade(); }, true);
    //        Lazy<ISupplierFacade> _SupplierFacade = new Lazy<ISupplierFacade>(() => { return new SupplierFacade(); }, true);
    //        Lazy<IOrganizationFacade> _OrganizationFacade = new Lazy<IOrganizationFacade>(() => { return new OrganizationFacade(); }, true);
    //        var ParentOrgan = _OrganizationFacade.Value.GetOrganByID(Orgain.OrganParentID);
    //        Authority AU = new Authority();
    //        if (ParentOrgan.OrganName == "供应商")
    //        {
    //            var supplier = _SupplierFacade.Value.GetSupplierByOrganID(Orgain.ID);   //获取当前登录的供应商信息
    //            AU.SCHOOLLIST = _SchoolFacade.Value.GetSchoolListBySupplier(supplier.ID); //获取属于这个供应商的学校
    //            AU.SUPPLIERLIST = _SupplierFacade.Value.GetSupplierList(supplier.Cname); //获取当前登录的供应商
    //            AU.SUPPLIERID = supplier.ID;  //获取供应商id
    //        }
    //        else if (ParentOrgan.OrganName == "学校")
    //        {

    //            var school = _SchoolFacade.Value.GetSchoolByOrganID(Orgain.ID);
    //            AU.SCHOOLLIST = new List<School_S>();
    //            AU.SCHOOLLIST.Add(_SchoolFacade.Value.GetSchoolByID(school.ID)); //获取当前登录的学校
    //            AU.SUPPLIERLIST = _SupplierFacade.Value.GetSupplierListBySupplier(school.ID);
    //            AU.SCHOOLID = school.ID;
    //        }

    //        else if (ParentOrgan.OrganName == "监管层")
    //        {
    //            AU.SCHOOLLIST = _SchoolFacade.Value.GetSchoolList(); //获取所有学校列表
    //            AU.SUPPLIERLIST = _SupplierFacade.Value.GetSupplierList(""); //获取所有供应商列表
    //        }
    //        else
    //        {
    //            AU.SCHOOLLIST = _SchoolFacade.Value.GetSchoolList(); //获取所有学校列表
    //            AU.SUPPLIERLIST = _SupplierFacade.Value.GetSupplierList(""); //获取所有供应商列表
    //        }
    //        return AU;
    //    }
    //}
}

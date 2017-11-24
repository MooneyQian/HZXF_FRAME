/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 菜单管理列表模块
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "qian", "TreeGridList"], function (exports) {
    var TreeGridList = layui.TreeGridList;
    var qian = layui.qian;
    var $ = layui.jquery;

    function _MenuRole_List() { };

    //页面初始化
    _MenuRole_List.prototype.init = function () {
        TreeGridList.init();
    }

    /************************** 重写父类方法 start **************************/

    TreeGridList.dataGrid = function () {
        return {
            url: '_RoleMenuList?' + qian.getQueryString(),
            grid: [
                { display: "菜单名", name: "MenuName", type: "text", align: "left", width: 300 },
                { display: "类型", name: "MenuType", type: "text", align: "left", width: 100 },
                { display: "编号", name: "MenuCode", type: "text", align: "left", width: 100 },
                { display: "地址", name: "MenuPath", type: "text", align: "left" }
            ],
            parentKey: "PerMenuID",
            key: "ID",
            check: true,
            checkKey: "IsHas"
        };
    }

    TreeGridList.toSubmit = function () {
        var menuids = TreeGridList.getCheckKeys();
        console.log(menuids);


        //$.ajax({
        //    url: '_SetRoleMenu',
        //    type: "post",
        //    data: {
        //        RoleID: qian.getQueryString("RoleID"),
        //        MenuIDs: menuids,
        //        n: Math.random()
        //    },
        //    traditional: true,//传递数组时需要设置传统模式
        //    success: function (res) {
        //        qian.tips("分配成功");
        //    }
        //});


        qian.ajax({
            url: '_SetRoleMenu',
            type: 'post',
            traditional: true,
            data: {
                RoleID: qian.getQueryString("RoleID"),
                MenuIDs: menuids,
                n: Math.random()
            },
            success: function (res) {
                qian.tips("分配成功");
            }
        });
    }



    /************************** 重写父类方法 end **************************/


    var MenuRoleList = new _MenuRole_List();

    exports('MenuRoleList', MenuRoleList);
});
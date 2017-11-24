/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 角色管理列表模块
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "qian", "TreeGridList"], function (exports) {
    var TreeGridList = layui.TreeGridList;
    var qian = layui.qian;
    var $ = layui.jquery;

    function _RoleUser_List() { };

    //页面初始化
    _RoleUser_List.prototype.init = function () {
        TreeGridList.init();
    }

    /************************** 重写父类方法 start **************************/

    TreeGridList.dataGrid = function () {
        return {
            url: '_UserRoleList?' + qian.getQueryString(),
            grid: [
                { display: "角色名", name: "RoleName", type: "text", align: "left", width: 100 },
                    { display: "描述", name: "RoleDesc", type: "text", align: "left", width: 110 },
                    { display: "状态", name: "IsHas", type: "text", align: "center", width: 80 }
            ],
            //parentKey: "PerMenuID",
            key: "ID",
            check: true,
            checkKey: "IsHas"
        };
    }

    TreeGridList.toSubmit = function () {
        var ids = TreeGridList.getCheckKeys().join(",");
        qian.ajax({
            url: '_SetRole',
            type: 'post',
            //traditional: true,
            data: {
                //IsHas:true,
                UserID: qian.getQueryString("UserID"),
                RoleID: ids,
                n: Math.random()
            },
            success: function (res) {
                qian.tips("分配成功");
            }
        });
    }



    /************************** 重写父类方法 end **************************/


    var RoleUserList = new _RoleUser_List();

    exports('RoleUserList', RoleUserList);
});
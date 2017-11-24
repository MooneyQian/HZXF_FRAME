/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 角色管理模块
 * +----------------------------------------------------------------------
 */
layui.define(["List"], function (exports) {
    var List = layui.List;

    var _this;

    function _Role_List() {
        _this = this;
    };

    //页面初始化
    _Role_List.prototype.init = function () {
        List.init();
    }

    /************************** 重写父类方法 start **************************/

    //定义数据结构
    List.dataGrid = function () {
        return {
            url: '_RoleList',
            grid: [
                { display: "角色名称", name: "RoleName", type: "text", align: "left", width: 100 },
                 { display: "角色描述", name: "RoleDesc", type: "text", align: "left", width: 400 },
                { display: "状态", name: "RecordStatus", type: "text", align: "center", width: 80 },
            ], checked: true
        };
    }


    /************************** 重写父类方法 end **************************/


    var RoleList = new _Role_List();

    exports('RoleList', RoleList);
});
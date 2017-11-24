/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 用户管理模块
 * +----------------------------------------------------------------------
 */
layui.define(["List"], function (exports) {
    var List = layui.List;

    var _this;

    function _User_List() {
        _this = this;
    };

    //页面初始化
    _User_List.prototype.init = function () {
        List.init();
    }

    /************************** 重写父类方法 start **************************/

    //定义数据结构
    List.dataGrid = function () {
        return {
            url: '_UserList',
            grid: [
               { display: "姓名", name: "UserDisplayName", type: "text", align: "left", width: 100 },
               { display: "登录名", name: "UserLoginName", type: "text", align: "left", width: 110 },
               { display: "手机号", name: "UserPhone", type: "text", align: "left", width: 110 },
               { display: "员工编号", name: "Extend1", type: "text", align: "left", width: 100 },
             //{ display: "E-Mail", name: "Extend2", type: "text", align: "left", width: 150 },
             //{ display: "短号", name: "Extend3", id: "Extend3", type: "text", align: "left", width: 100 },
             //{ display: "座机", name: "Extend4", type: "text", align: "left", width: 100 },
                { display: "状态", name: "RecordStatus", type: "text", align: "center", width: 80 },
            ], checked: true
        };
    }


    /************************** 重写父类方法 end **************************/


    var UserList = new _User_List();

    exports('UserList', UserList);
});
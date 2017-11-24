/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 部门管理列表模块
 * +----------------------------------------------------------------------
 */
layui.define(["TreeList"], function (exports) {
    var TreeList = layui.TreeList;

    function _Organization_List() { };

    //页面初始化
    _Organization_List.prototype.init = function () {
        TreeList.init();
        listObj = this;
    }

    //更新节点
    _Organization_List.prototype.updateNode = function (type, data) {
        if (type == "Add") {
            TreeList.addNode(data);
        } else {
            TreeList.updateNode(data);
        }
    }
    

    /************************** 重写父类方法 start **************************/

    TreeList.settings = function () {
        return {
            loadUrl: 'GetOrganizationTree',
            addUrl: 'Add?pid',
            delUrl: '_Delete?ids',
            editUrl:'Edit?id'
        };
    }

    /************************** 重写父类方法 end **************************/


    var OrganizationList = new _Organization_List();

    exports('OrganizationList', OrganizationList);
});
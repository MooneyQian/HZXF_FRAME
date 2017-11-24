/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 部门用户管理列表模块
 * +----------------------------------------------------------------------
 */
layui.define(["TreeList"], function (exports) {
    var TreeList = layui.TreeList;

    function _OrganizationUser_List() { };

    //页面初始化
    _OrganizationUser_List.prototype.init = function () {
        TreeList.init();
        listObj = this;
    }

    //更新节点
    _OrganizationUser_List.prototype.updateNode = function (type, data) {
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
            editUrl: '/User/List?menuid=' + self.frameElement.getAttribute('data-id') + '&id'
        };
    }

    /************************** 重写父类方法 end **************************/


    var OrganizationUserList = new _OrganizationUser_List();

    exports('OrganizationUserList', OrganizationUserList);
});
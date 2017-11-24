/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 角色管理模块
 * +----------------------------------------------------------------------
 */
layui.define(["qian", "List"], function (exports) {
    var qian = layui.qian;
    var List = layui.List;

    var _this;

    function _CacheManager_List() {
        _this = this;
    };

    //页面初始化
    _CacheManager_List.prototype.init = function () {
        List.init();
    }

    /************************** 重写父类方法 start **************************/

    //定义数据结构
    List.dataGrid = function () {
        return {
            url: '_List',
            grid: [
                { display: "缓存值", name: "value", type: "text", align: "left", width: 600 },
                 { display: "缓存名", name: "name", type: "text", align: "left", width: 400 }               
            ], checked: false, page:false
        };
    }

    List.afterPost = function (res) {
        qian.tips(res.Message);
    }


    /************************** 重写父类方法 end **************************/


    var CacheManagerList = new _CacheManager_List();

    exports('CacheManagerList', CacheManagerList);
});
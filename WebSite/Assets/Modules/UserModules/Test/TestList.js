/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 测试列表模块
 * +----------------------------------------------------------------------
 */
layui.define(["List"], function (exports) {
    var List = layui.List;

    var _this;

    function _Test_List() {
        _this = this;
    };

    //页面初始化
    _Test_List.prototype.init = function () {
        List.init();
    }

    /************************** 重写父类方法 start **************************/

    //定义数据结构
    List.dataGrid = function () {
        return {
            url: '_List',
            grid: [
                { display: "名称", name: "DEVNAME", type: "text", align: "left", width: '100', sort: true },
                { display: "IP 地址", name: "DEVIP", type: "text", align: "left", width: '120' },
                { display: "端口", name: "DEVPORT", type: "text", align: "left", width: '60' },
                { display: "位置", name: "DEVADDR", type: "text", align: "left", width: '150' },
                { display: "负责人", name: "LINKMAN", type: "text", align: "left", width: '100' },
                { display: "负责人电话", name: "LINKTEL", type: "text", align: "left", width: '110' },
                { display: "品牌类型", name: "EQUIPNAME", type: "text", align: "left", width: '150' },
                { display: "录入时间", name: "FILLDT", type: "date", align: "left", width: '180', sort: true }
            ], checked: true
        };
    }
    

    /************************** 重写父类方法 end **************************/


    var TestList = new _Test_List();

    exports('TestList', TestList);
});
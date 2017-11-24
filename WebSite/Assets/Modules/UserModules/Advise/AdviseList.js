/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 违停劝离列表模块
 * +----------------------------------------------------------------------
 */
layui.define(["List"], function (exports) {
    var List = layui.List;

    var _this;

    function _Advise_List() {
        _this = this;
    };

    //页面初始化
    _Advise_List.prototype.init = function () {
        List.init();
    }

    /************************** 重写父类方法 start **************************/

    //定义数据结构
    List.dataGrid = function () {
        return {
            url: '/LegalCase/_List',
            grid: [
                 { display: "名称", name: "DEVNAME", type: "text", align: "left", width: 100, sort: true },
                 { display: "IP 地址", name: "DEVIP", type: "text", align: "left", width: 120 },
                 { display: "违停照片", name: "PICURL", type: "img", align: "center", width: 80 },
                 { display: "车牌号", name: "PLATENUMBER", type: "text", align: "left", width: 100 },
                 { display: "违停时间", name: "LEGALDATE", type: "date", align: "left", width: 200 },
                 { display: "处理状态", name: "CSTATUSSTR", type: "text", align: "left", width: 80 },
            ], checked: true
        };
    }


    /************************** 重写父类方法 end **************************/


    var AdviseList = new _Advise_List();

    exports('AdviseList', AdviseList);
});
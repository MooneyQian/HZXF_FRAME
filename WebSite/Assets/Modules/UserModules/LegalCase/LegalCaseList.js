/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 违停处理列表模块
 * +----------------------------------------------------------------------
 */
layui.define(["List"], function (exports) {
    var List = layui.List;

    var _this;

    function _LegalCase_List() {
        _this = this;
    };

    //页面初始化
    _LegalCase_List.prototype.init = function () {
        List.init();
    }

    /************************** 重写父类方法 start **************************/

    //定义数据结构
    List.dataGrid = function () {
        return {
            url: '_List',
            grid: [
                    { display: "设备名称", name: "DEVNAME", type: "text", align: "center", width: 150 },
                    { display: "IP 地址", name: "IP", type: "text", align: "left", width: 150 },
                    { display: "违停照片", name: "PICURL", type: "img", align: "center", width: 80},
                    { display: "车牌号", name: "PLATENUMBER", type: "text", align: "left", width: 150 },
                    { display: "违停时间", name: "LEGALDATE", type: "date", align: "left",width:200, sort:true},
                    { display: "处理状态", name: "CSTATUSSTR", type: "text", align: "left", width: 80 }
            ], checked: true
        };
    }
    

    /************************** 重写父类方法 end **************************/


    var LegalCaseList = new _LegalCase_List();

    exports('LegalCaseList', LegalCaseList);
});
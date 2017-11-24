/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 违停失败列表模块
 * +----------------------------------------------------------------------
 */
layui.define(["List"], function (exports) {
    var List = layui.List;

    var _this;

    function _FailCase_List() {
        _this = this;
    };

    //页面初始化
    _FailCase_List.prototype.init = function () {
        List.init();
    }

    /************************** 重写父类方法 start **************************/

    //定义数据结构
    List.dataGrid = function () {
        return {
            url: '_List',
            grid: [
                  { display: "车牌号", name: "PLATENUMBER", type: "text", align: "left", width: 100 },
                  { display: "设备名称", name: "DEVNAME", type: "text", align: "center", width: 150 },
                  { display: "违停照片", name: "PICURL", type: "img", align: "center", width: 80 },
                  { display: "违停时间", name: "LEGALDATE", type: "date", align: "left", width: 200 },
                  { display: "处理时间", name: "HANDLEDATE", type: "date", align: "left", width: 160 },
                  { display: "处理人", name: "HANDLEMAN", type: "text", align: "left", width: 100 },
                  { display: "处理状态", name: "CSTATUSSTR", type: "text", align: "left", width: 80 },
                  { display: "上报状态", name: "REPORTSTATUS", type: "text", align: "left", width: 80 }
            ], checked: true
        };
    }
    

    /************************** 重写父类方法 end **************************/


    var FailCaseList = new _FailCase_List();

    exports('FailCaseList', FailCaseList);
});
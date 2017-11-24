/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 设备管理列表模块
 * +----------------------------------------------------------------------
 */
layui.define(["List"], function (exports) {
    var List = layui.List;

    var _this;

    function _EquipmentInfo_List() {
        _this = this;
    };

    //页面初始化
    _EquipmentInfo_List.prototype.init = function () {
        List.init();
    }

    /************************** 重写父类方法 start **************************/

    //定义数据结构
    List.dataGrid = function () {
        return {
            url: '_List',
            grid: [
                { display: "名称", name: "DEVNAME", type: "text", align: "left", sort: true },
                { display: "IP 地址", name: "DEVIP", type: "text", align: "center"},
                { display: "端口", name: "DEVPORT", type: "text", align: "left"},
                { display: "位置", name: "DEVADDR", type: "text", align: "left"},
                { display: "负责人", name: "LINKMAN", type: "text", align: "left"},
                { display: "负责人电话", name: "LINKTEL", type: "text", align: "left"},
                { display: "品牌类型", name: "EQUIPNAME", type: "text", align: "left"},
                { display: "录入时间", name: "FILLDT", type: "date", align: "left", sort: true }
            ], checked: true
        };
    }
    

    /************************** 重写父类方法 end **************************/


    var EquipmentInfoList = new _EquipmentInfo_List();

    exports('EquipmentInfoList', EquipmentInfoList);
});
/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 设备管理编辑模块
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "qian", "html", "Add"], function (exports) {
    var $ = layui.jquery;
    var qian = layui.qian;
    var html = layui.html;
    var Add = layui.Add;

    var _this;

    function _LegalCase_Add() {
        _this = this;
    };

    //页面初始化
    _LegalCase_Add.prototype.init = function () {
        Add.init();
    }

    /************************** 重写父类方法 start **************************/

    //页面初始化方法
    Add.pageReady = function () {
        //加载品牌信息
        html.toGroupSelect({
            url: '/Selecter/GetEquipmentParametersTreeForComboBox?rn=' + Math.random(),
            dom: 'DEVTYPE-S',
            selectedValue: $("#DEVTYPE").val(),
            text: '请选择品牌',
            change: function (data) {
                $("#DEVTYPE").val(data.value)
            }
        });
    }

    Add.checkForm = function () {
        return {
            "q-three": function (v, item) {
                if (v.length < 3) {
                    return '至少3个';
                }
                return '';
            }
        }
    }


    Add.complexCheck = function () {
        return true;
    }

    //监听开关
    Add.listenSwitch = function (id, obj, isChecked, v) {
    }

    //监听radio
    Add.listenRadio = function (obj, v) {
        
    }

    Add.listenSelect = function (id, obj, v) {

    }

    /************************** 重写父类方法 end **************************/


    var LegalCase_Add = new _LegalCase_Add();
    exports('LegalCaseAdd', LegalCase_Add);
});
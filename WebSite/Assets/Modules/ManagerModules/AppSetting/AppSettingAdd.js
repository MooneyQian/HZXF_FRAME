/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 系统设置编辑模块
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "element", "form", "qian", "html", "Add"], function (exports) {
    var element = layui.element;
    var $ = layui.jquery;
    var qian = layui.qian;
    var html = layui.html;
    var Add = layui.Add;

    function _AppSetting_Add() { };

    //页面初始化
    _AppSetting_Add.prototype.init = function () {
        Add.init();
    }

    /************************** 重写父类方法 start **************************/

    //页面初始化方法
    Add.pageReady = function () {

    }

    //表单校验
    Add.checkForm = function () {
        return {
            "q-EnableCookie": function (v, item) {
                if ($(".EnableCookie").is(":hidden")) {
                    return 'stop';//停止后续校验
                }
                return '';
            }
        };
    }

    //页面操作完成后续动作
    Add.onSuccess = function (obj) {
        qian.tips(obj.Message);
    }

    //监听开关
    Add.listenSwitch = function (id, obj, isChecked, v) {
    }

    //监听radio
    Add.listenRadio = function (obj, v) {
        if (obj.name == "SSOType") {//单点登录类型
            if (v == "1") {
                html.show(".SSOType3", false);
                html.show(".SSOType2", false);
                html.show(".SSOType1", true);
            } else if (v == "2") {
                html.show(".SSOType3", false);
                html.show(".SSOType1", false);
                html.show(".SSOType2", true);
            } else if (v == "3") {
                html.show(".SSOType2", false);
                html.show(".SSOType1", false);
                html.show(".SSOType3", true);
            } else {
                html.show(".SSOType3", false);
                html.show(".SSOType1", false);
                html.show(".SSOType2", false);
            }
        }
    }

    //监听下拉框
    Add.listenSelect = function (id, obj, v) {
    }



    /************************** 重写父类方法 end **************************/


    var AppSetting_Add = new _AppSetting_Add();
    exports('AppSettingAdd', AppSetting_Add);
});
/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 用户管理编辑模块
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "qian", "html", "Add"], function (exports) {
    var $ = layui.jquery;
    var qian = layui.qian;
    var html = layui.html;
    var Add = layui.Add;

    var _this;

    function _User_Add() {
        _this = this;
    };

    //页面初始化
    _User_Add.prototype.init = function () {
        Add.init();
    }

    /************************** 重写父类方法 start **************************/

    //页面初始化方法
    Add.pageReady = function () {
    }

    Add.checkForm = function () {
        return {
            "q-two": function (v, item) {
                if (v.length < 3) {
                    return '用户姓名不少于2位!';
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


    var User_Add = new _User_Add();
    exports('UserAdd', User_Add);
});
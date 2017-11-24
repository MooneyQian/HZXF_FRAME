/*
 * @Author: qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 部门管理编辑模块
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "qian", "html", "Add"], function (exports) {
    var $ = layui.jquery;
    var qian = layui.qian;
    var html = layui.html;
    var Add = layui.Add;

    function _Organization_Add() { };

    //页面初始化
    _Organization_Add.prototype.init = function () {
        Add.init();
    }

    /************************** 重写父类方法 start **************************/

    //页面初始化方法
    Add.pageReady = function () {

    }

    //表单自定义校验
    Add.validForm = function () {
    
        return true;
    }
    Add.checkForm = function () {
        return {
            'q-samecheck': function (v, item) {
                var result = '';
                qian.ajax({
                    url: '_OrganNOIsSuc',
                    type: 'post',
                    async: false,
                    data: {
                        OrganNO: $("#OrganNO").val(),
                        ID: $("#ParDictID").val(), //父节点id
                    },
                    success: function (res) {
                        if (res == true) {
                            result = '同级字典中已经存在该字典编号！';
                        }
                    }
                });
                return result;
            }

        }
    }


    Add.complexCheck = function () {
        return true;
    }

    Add.onSuccess = function (obj) {
        parent.updateNode(Add.operType(), obj.Data);
        qian.tips("操作成功");
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


    var Organization_Add = new _Organization_Add();
    exports('OrganizationAdd', Organization_Add);
});
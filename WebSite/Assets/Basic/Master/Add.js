/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 通用新增/修改页
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "element", "layer", "form", "util", "qian", "html"], function (exports) {
    var util = layui.util;
    var form = layui.form;
    var layer = layui.layer;
    var qian = layui.qian;
    var $ = layui.jquery;

    var html = layui.html;

    var _this;

    function _Add() {
        _this = this;
    }

    _Add.prototype.index = qian.getQueryString("index");

    
    _Add.prototype.init = function (funcs) {
        $(document).ready(function () {
            //返回顶部
            util.fixbar();
            $("#base-return").click(function () {
                parent.layer.close(_this.index);
            });

            //监听表单提交
            form.on("submit(*)", function (data) {
                var actionurl = $("#actionurl").val();
                if ($.trim(actionurl) == "") {
                    qian.tips("actionurl为空，无法提交");
                    return false;
                }

                if (!_this.validForm()) {
                    return false;
                }
                qian.ajax({
                    url: actionurl,
                    type: 'post',
                    data: _this.proccessField(data.field),
                    beforeSend:_this.complexCheck(),
                    success: function (res) {
                        _this.onSuccess(res);
                    }
                });
                return false;
            });

            //表单校验
            form.verify(mergeVerify());

            //监听表单元素
            listenFormElement();

            //自定义页面初始化方法
            pageReadyPre();
        });
    }

    //当前操作类型
    _Add.prototype.operType = function () {
        return $("#opertype").val();
    }

    //当前提交按钮
    _Add.prototype.obtainSubmit = function () {
        return "base-submit";
    }
    

    /**************************** 子类可复写方法 start ********************************/

    //处理表单提交的字段
    _Add.prototype.proccessField = function (fields) {
        return fields;
    }

    //表单提交成功后执行的方法
    _Add.prototype.onSuccess = function (res) {
        qian.tips("执行成功");
        parent.layer.close(_this.index);
    }

    //页面初始化方法后续
    _Add.prototype.pageReady = function () { }

    //表单自定义校验
    _Add.prototype.validForm = function () {
        return true;
    }

    //表单自定义校验规则
    _Add.prototype.checkForm = function () {
        return {};
    }

    //表单复杂校验
    _Add.prototype.complexCheck = function () {
        return true;
    }

    //下拉框监听方法后续
    _Add.prototype.listenSelect = function (id, obj, v) { }

    //switch监听方法后续
    _Add.prototype.listenSwitch = function (id, obj, isChecked, v) { }

    //radio监听方法后续
    _Add.prototype.listenRadio = function (obj, v) { }


    /**************************** 子类可复写方法 end ********************************/



    /**************************** 成员方法 start ********************************/

    //页面初始化方法
    function pageReadyPre() {
        _this.pageReady();
        triggerDom();
    }


    //值转义
    function proccessValue(v) {
        var r = false;
        if (!qian.isEmpty(v)) {
            if (v == "False" || v == "True") {
                if (v == "True") {
                    r = true;
                }
            } else if (v == "1") {
                r = true;
            }
        }
        return r;
    }

    //值转义
    function proccessSetValue(isChecked, v) {
        var r = "0";
        if (!qian.isEmpty(v)) {
            if (v == "False" || v == "True") {
                if (isChecked) {
                    r = "True";
                } else {
                    r = "False";
                }
            } else if (v == "0" || v == "1") {
                if (isChecked) {
                    r = "1";
                } else {
                    r = "0";
                }
            }
        }
        return r;
    }

    //触发相应事件
    function triggerDom() {
        //触发radio
        $("input[type='radio']:checked").each(function () {
            layui.event("form", "radio", {
                elem: this,
                value: $(this).val()
            });
        });

        //触发select
        $("select").each(function () {
            layui.event("form", "select", {
                elem: this,
                value: $(this).val()
            });
        });

        //触发switch
        $('input[lay-skin=switch]').each(function () {
            $(this).attr("checked", "");
            if (proccessValue($(this).val())) {
                $(this).removeAttr("checked");
            }
        });
        $(".layui-form-switch").trigger("click");
    }

    //下拉框监听方法
    function listenSelectPre(id, obj, v) {
        _this.listenSelect(id, obj, v);
    }

    //switch监听方法
    function listenSwitchPre(id, obj, isChecked, v) {
        $("#" + id).val(proccessSetValue(isChecked, v));
        html.show("." + id, isChecked);
        _this.listenSwitch(id, obj, isChecked, v);
    }

    //radio监听方法
    function listenRadioPre(obj, v) {
        _this.listenRadio(obj, v);
    }



    //监听表单元素
    function listenFormElement() {
        //监听下拉框
        form.on("select", function (data) {
            listenSelectPre(data.elem.id, data.elem, data.value);
        });

        //监听switch开关
        form.on("switch", function (data) {
            listenSwitchPre(data.elem.id, data.elem, data.elem.checked, data.value);
        });

        //监听radio
        form.on("radio", function (data) {
            listenRadioPre(data.elem, data.value);
        });
    }

    //整合校验规则
    function mergeVerify() {
        var defaultV = defaultVerify();
        return $.extend(defaultV, _this.checkForm());
    }

    //默认检验
    function defaultVerify() {
        return {
            //必填
            "q-required": function (v, item) {
                if (!/[\S]+/.test(v)) {
                    return $(item).attr("placeholder");
                }
                return '';
            },
            //手机号
            "q-phone": function (v, item) {
                if ($.trim(v) != "") {
                    if (!/^1\d{10}$/.test(v)) {
                        return '请输入正确的手机号';
                    }
                }
                return '';
            },
            //邮箱
            "q-email": function (v, item) {
                if ($.trim(v) != "") {
                    if (!/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(v)) {
                        return '邮箱格式不正确';
                    }
                }
                return '';
            },
            //链接
            "q-url": function (v, item) {
                if ($.trim(v) != "") {
                    if (!/(^#)|(^http(s*):\/\/[^\s]+\.[^\s]+)/.test(v)) {
                        return '链接格式不正确';
                    }
                }
                return '';
            },
            //数字
            "q-number": function (v, item) {
                if ($.trim(v) != "") {
                    if (!value || isNaN(value)) {
                        return '只能填写数字'
                    }
                }
                return '';
            },
            //日期
            "q-date": function (v, item) {
                if ($.trim(v) != "") {
                    if (!/^(\d{4})[-\/](\d{1}|0\d{1}|1[0-2])([-\/](\d{1}|0\d{1}|[1-2][0-9]|3[0-1]))*$/.test(v)) {
                        return '日期格式不正确';
                    }
                }
                return '';
            },
            //身份证号
            "q-identity": function (v, item) {
                if ($.trim(v) != "") {
                    if (!/(^\d{15}$)|(^\d{17}(x|X|\d)$)/.test(v)) {
                        return '请输入正确的身份证号';
                    }
                }
                return '';
            },
            //base64编码
            "q-base64": function (v, item) {
                if ($.trim(v) != "") {
                    $(item).val(html.base64Encode(v));
                }
            }
        };
    }

    /**************************** 成员方法 end ********************************/

    var Add = new _Add();
    exports('Add', Add);
});
/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 页面元素帮助类
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "form", "base64", "qian"], function (exports) {
    var $ = layui.jquery;
    var qian = layui.qian;
    var form = layui.form;
    var base64 = layui.base64;
    
    var _this;

    function _html() {
        _this = this;
    };

    //生成radiolist
    _html.prototype.generRadioList = function (params) {
        var target = params.target || "";
        var data = params.data || [];
        if (!target || data.length == 0) {
            qian.tips("参数错误");
            return false;
        }
        var result = "";
        var name = params.name;
        var defaultV = params.defaultV;
        var compareV = params.compareV || defaultV;
        for (var i = 0; i < data.length; i++) {
            var tmp = data[i];
            result += '<input type="radio" id="' + name + '_' + (i + 1) + '" value="' + tmp.key + '" name="' + name + '" title="' + tmp.name + '" ';
           
            if (tmp.key == compareV) {
                result += 'checked="" ';
            }
            result += '>';
        }
        $("#" + target).html(result);
        form.render("radio");
    }


    //根据名称获取radio选中的值
    _html.prototype.getRadioValueByName = function (name) {
        if (qian.isEmpty(name)) {
            return "";
        }
        var r = "";
        $("input[name='" + name + "']:checked").each(function () {
            r = $(this).val();
            return false;
        });
        return r;
    }

    //base64编码
    _html.prototype.base64Encode = function (str) {
        return base64.encode(str);
    }

    //base64解码
    _html.prototype.base64Decode = function (str) {
        return base64.decode(str);
    }


    //控制元素显示隐藏
    _html.prototype.show = function (dom, isShow, speed) {
        if (!speed) {
            speed = 400;
        }
        if (isShow) {
            $(dom).show(speed);
        } else {
            $(dom).hide(speed);
        }
    }

    //生成普通下拉框
    _html.prototype.toSelect = function (opts) {
        opts.common = true;
        generSelect(opts, opts.data);
    }

    //生成分组下拉框
    _html.prototype.toGroupSelect = function (opts) {
        var result = "";
        if (!opts.url || !opts.dom) {
            return false;
        }
        qian.ajax({
            url: opts.url,
            type: 'post',
            success: function (res) {
                opts.label = true;
                generSelect(opts, res.Data);
            }
        });
    }
    

    function generSelect(opts, data) {
        var r = '';
        var text = '请选择';
        if (opts.text) {
            text = opts.text;
        }
        r += '<option value="">' + text + '</option>';
        for (var i = 0; i < data.length; i++) {
            if (opts.label) {//分组下拉框
                var parent = data[i];
                r += '<optgroup label="' + parent.text + '">';
                var childlist = parent.children;
                for (var j = 0; j < childlist.length; j++) {
                    var tmp = childlist[j];
                    if (tmp.id == opts.selectedValue) {
                        r += '<option value="' + tmp.id + '" selected>' + tmp.text + '</option>';
                    } else {
                        r += '<option value="' + tmp.id + '">' + tmp.text + '</option>';
                    }
                }
                r += '</optgroup>';
            } else if (opts.common) {//普通下拉框
                var tmp = data[i];
                if (tmp.key == opts.selectedValue) {
                    r += '<option value="' + tmp.key + '" selected ';
                } else {
                    r += '<option value="' + tmp.key + '" ';
                }
                if (opts.haspic) {
                    r += 'pics = "{src:\'' + tmp[opts.pickey] + '\'}" ';
                }
                r += '>' + tmp.name + '</option>';
            }
        }
        $("#" + opts.dom).html(r);
        $("#" + opts.dom).val(opts.selectedValue);
        form.render('select');

        if (opts.change && typeof opts.change == 'function') {
            var change = opts.change;
            form.on('select(' + opts.dom + ')', function (data) {
                change(data);
            });
        }
    }

    

    var html = new _html();
    exports('html', html);
});
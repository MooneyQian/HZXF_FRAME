/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 基础通用工具帮助类
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "layer", "laytpl", "form"], function (exports) {
    var $ = layui.jquery;

    //弹层对象
    var layer = layui.layer;
    //模板对象
    var laytpl = layui.laytpl;

    var _this;

    function _qian() { 
        _this = this;
    };

    //异步请求
    _qian.prototype.ajax = function (opts) {

        if (_this.isEmpty(opts) || _this.isEmpty(opts.url)) {
            _this.tips("请求参数错误");
            return false;
        }

        var defaultOpts = $.extend({
            type: "get",
            data:{},
            hasLoading: true,
            loadingText: "",
            loadingIndex:-99,
        }, opts);

        if (defaultOpts.hasLoading) {
            defaultOpts.loadingIndex = _this.loading(defaultOpts.loadingText);
            defaultOpts.completeclose = function (xhr, des) {
                if (defaultOpts.loadingIndex >= 0) {
                    layer.close(defaultOpts.loadingIndex)
                }
            }
        }

        $.ajax({
            type: defaultOpts.type,
            url: defaultOpts.url,
            async:defaultOpts.async == undefined ? true : false,//同步请求
            data: defaultOpts.data,
            traditional: defaultOpts.traditional == undefined ? false : defaultOpts.traditional,//传递数组
            dataType:defaultOpts.dataType || 'json',
            success: function (res) {
                if (!res.IsError) {
                    var successFunc = defaultOpts.success || "";
                    if (successFunc && typeof successFunc == "function") {
                        successFunc(res);
                    } else {
                        _this.tips("执行成功");
                    }
                } else {
                    var errorFunc = defaultOpts.error || "";
                    if (errorFunc && typeof errorFunc == "function") {
                        errorFunc(res);
                    } else {
                        _this.tips(res.Message);
                    }
                }
                
            },
            error: defaultOpts.error || function (xhr, errortype, e) {
                _this.tips("执行失败");
            },
            beforeSend:function(xhr) {
                var bs = defaultOpts.beforeSend;
                if (bs && typeof bs == "function") {
                    var r = bs();
                    if (!r) {
                        if (defaultOpts.loadingIndex >= 0) {
                            layer.close(defaultOpts.loadingIndex)
                        }
                    }
                    return r;
                }
                return true;
            },
            complete: function (xhr, des) {
                var completeclose = defaultOpts.completeclose;
                if (completeclose) {
                    completeclose();
                }
                var dc = defaultOpts.complete;
                if (dc && typeof dc == "function") {
                    dc(xhr, des);
                } else {
                    //_this.tips("执行完毕");
                }
            }
        });
    }

    //请求模板
    _qian.prototype.ajaxTemplate = function (opts) {
        var defaultOpts = {
            dataType: 'html',
            url: '/Assets/' + opts.path,
            hasLoading: opts.hasLoading,
            success: function (res) {
                laytpl(res).render(opts.data || {}, function (html) {
                    if (opts.dom) {
                        $("#" + opts.dom).html(html);
                    }
                    var render = opts.render || "";
                    if (render && typeof render == "function") {
                        render(html);
                    }
                });
            }
        };

        _this.ajax(defaultOpts);

    }

    //请求数据文件 json
    _qian.prototype.ajaxDataFile = function (opts) {
        return {
            skin: [{
                name: '',
                path: '/Assets/Res/Images/skin/bg10.jpg'
            }]
        }

    }


    //渲染模板
    _qian.prototype.renderTemplate = function (target, tpl, data) {
        if (!target || !tpl || !data) {
            return;
        }
        var tplHtml = $("#" + tpl).html();
        laytpl(tplHtml).render(data || {}, function (html) {
            $("#" + target).html(html);
        });

    }

    //请求提示
    _qian.prototype.loading = function (msg) {
        if (_this.isEmpty(msg)) {
            return layer.load(2);
        }
        return layer.msg(msg, {
            icon: 16,
            time: false,
            shade:false
        });
    }

    //弹窗提示
    _qian.prototype.tips = function (msg) {
        if (!_this.isEmpty(msg)) {
            layer.msg(msg, {
                shift: 5
            });
        }
    }

    //确认框
    _qian.prototype.confirm = function (confirmInfo, func) {
        if (_this.isEmpty(confirmInfo)) {
            confirmInfo = '确认要继续操作吗？';
        }
        var index = layer.confirm(confirmInfo, {
            btn: ['确定', '取消'], //按钮
            icon: 3,
            title: '确认信息',
            anim: 5,
            isOutAnim:false,
        }, function (index) {
            if (func && typeof func == 'function') {
                func();
            }
            layer.close(index);
        });
    }


    //判断字符串是否为空
    _qian.prototype.isEmpty = function(str) {
        if (str == undefined || str == null || (str != "0" && !str)) {
            return true;
        }
        return false;
    }    

    //打开窗口
    _qian.prototype.to = function (url, name, endFunc) {
        var title = name || '信息';
        title = '<i class="layui-icon" style="margin-right:10px;">&#xe63c;</i>' + title;
        var index = layer.open({
            type: 2,
            skin: 'qui-skin-common',
            title: title,
            shadeClose: false,
            shade: 0.8,
            anim: 5,
            isOutAnim:false,
            content: url.indexOf("?") >= 0 ? url + '&index=' + (layer.index + 1) : url + '?index=' + (layer.index + 1),
            end: (endFunc && typeof endFunc == 'function') ? endFunc : function () {

            }
        });
        layer.full(index);
    }

    //自定义打开窗口
    _qian.prototype.sepcialTo = function (params) {
        var title = params.title || '信息';
        title = '<i class="layui-icon" style="margin-right:10px;">&#xe63c;</i>' + title;
        var endFunc = params.endFunc || '';
        var url = params.url;
        var index = layer.open({
            type: 2,
            skin: 'qui-skin-common',
            title: title,
            shadeClose: true,
            shade: 0.7,
            area: params.area || ['620px', '600px'],
            anim: 5,
            isOutAnim: false,
            content: url.indexOf("?") >= 0 ? url + '&index=' + (layer.index + 1) : url + '?index=' + (layer.index + 1),
            end: (endFunc && typeof endFunc == 'function') ? endFunc : function () {

            }
        });
    }

    //获取url中的参数
    _qian.prototype.getQueryString = function (name) {
        if (!_this.isEmpty(name)) {
            var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)', 'i');
            var r = window.location.search.substr(1).match(reg);
            if (r != null) {
                return decodeURIComponent(r[2]);
            }
            return null;
        } else {
            return decodeURIComponent(window.location.search.substr(1));
        }
    }

    //将url参数转成json
    _qian.prototype.queryToJSON = function (str) {
        var d = {};
        if (!_this.isEmpty(str)) {
            var strlist = str.split("&");
            for (var i = 0; i < strlist.length; i++) {
                if (_this.isEmpty(strlist[i])) {
                    continue;
                }
                var vlist = strlist[i].split("=");
                d[vlist[0]] = _this.isEmpty(vlist[1]) ? "" : vlist[1];
            }
        }
        return d;
    }

    //将json对象拼接到url中
    _qian.prototype.contactToUrl = function(url, json) {
        if (!url || !json) {
            return url;
        }
        var r = "";
        for (var item in json) {
            r += "&" + item + "=" + json[item];
        }
        if (url.indexOf("?") >= 0) {
            r = url + r;
        } else {
            r = url + "?" + r.substring(1);
        }
        return r;
    }

    //显示图片
    _qian.prototype.showImg = function (opts) {
        var data = [];

        if (opts.single) {//显示单张图片
            data.push({
                "src": opts.src //原图地址
            });
        } else {
            data = opts.data;
        }


        var json = {
            "title": "点击阴影处关闭", //相册标题
            "id": new Date().getTime(), //相册id
            "start": 0, //初始显示的图片序号，默认0
            "data": data
        }
        layer.photos({
            photos: json, 
            anim: 5 
        });
    }

    //对象拷贝
    _qian.prototype.deepCopy = function (source) {
        var sourceCopy = source instanceof Array ? [] : {};
        for (var item in source) {
            sourceCopy[item] = typeof source[item] === 'object' ? this.deepCopy(source[item]) : source[item];
        }
        return sourceCopy;
    }
    

    //时间格式化 :/Date(1504670848000)/
    _qian.prototype.formatDate = function (data, fmt) {
        if (!fmt) {
            fmt = 'yyyy-MM-dd hh:mm:ss';
        }
        var r = eval(data.replace(/\/Date\((\d+)\)\//gi, "new Date($1)")).format(fmt);
        return r;
    }

    //时间格式化方法
    Date.prototype.format = function (fmt) {
        var o = {
            "M+": this.getMonth() + 1, //月份 
            "d+": this.getDate(), //日 
            "h+": this.getHours(), //小时 
            "m+": this.getMinutes(), //分 
            "s+": this.getSeconds(), //秒 
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
            "S": this.getMilliseconds() //毫秒 
        };
        fmt = fmt || "yyyy-MM-dd";
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }

    var qian = new _qian();
    exports('qian', qian);
});
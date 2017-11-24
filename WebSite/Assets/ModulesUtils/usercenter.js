/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 个人中心帮助类
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "qian", "html", "res"], function (exports) {
    var $ = layui.jquery;
    var qian = layui.qian;
    var html = layui.html;
    var res = layui.res;

    var _this;

    //个人中心默认配置
    var mystr = {
        "centeravatar": '/Assets/Res/Images/default.jpg',//默认头像
        "centersound": "1",//声音提示
        "centersoundtips": "1",//提示音源
        "centerskin": "0"//皮肤
    };

    function _usercenter() {
        _this = this;
        this.initCenterInfo();
    }

    //初始化个人中心配置
    _usercenter.prototype.initCenterInfo = function () {
        qian.ajax({
            type: "post",
            url: "/Configuration/_Add",
            async:false,
            success: function (res) {
                if (res.Rows) {
                    if (!qian.isEmpty(res.Rows.SETTEXT)) {
                        mystr = JSON.parse(html.base64Decode(res.Rows.SETTEXT));
                    }
                }
            }
        });
    }

    //获取个人配置
    _usercenter.prototype.obtainCenterInfo = function () {
        return mystr;
    }

    //重新加载个人配置
    _usercenter.prototype.reloadCenterInfo = function (str, needparent) {
        usercenter.setCenterInfo(str);
        usercenter.loadCenterInfo(needparent);
    }

    //加载个人配置
    _usercenter.prototype.loadCenterInfo = function (needparent) {
        //加载头像配置
        if (needparent) {
            parent.document.getElementById("info-trigger-avatar").src = mystr.centeravatar;
        } else {
            $("#info-trigger-avatar").attr("src", mystr.centeravatar);
        }
        
        //加载皮肤配置
        res.changeSkinByKey(mystr.centerskin, needparent);
        
    }

    //处理个人配置字符串
    _usercenter.prototype.proccessCenterInfo = function (fields) {
        return html.base64Encode(JSON.stringify(fields));
    }

    //设置个人配置字符串
    _usercenter.prototype.setCenterInfo = function (str) {
        if (qian.isEmpty(str)) {
            return;
        }
        mystr = JSON.parse(html.base64Decode(str));
    }

    //异步保存个人配置
    _usercenter.prototype.updateCenterInfo = function (params) {
        var data = $.extend(mystr, params);
        qian.ajax({
            type: "post",
            url: "/Configuration/_SaveAll",
            hasLoading: false,
            data: "mystr=" + _this.proccessCenterInfo(data),
            success: function (res) {
                if (!qian.isEmpty(res.Data)) {
                    mystr = JSON.parse(html.base64Decode(res.Data));
                    //重新加载配置
                    _this.loadCenterInfo();
                }
            }
        });
    }

    //播放声音
    _usercenter.prototype.playSound = function () {
        if (mystr.centersound == "1") {
            res.playSound(mystr.centersoundtips);
        }
    }

    //更换皮肤
    _usercenter.prototype.changeSkin = function (key) {
        _this.updateCenterInfo({
            centerskin:key
        });
    }
   
    var usercenter = new _usercenter();
    exports("usercenter", usercenter);
});
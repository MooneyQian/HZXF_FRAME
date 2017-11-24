/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 账户设置编辑模块
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "qian", "html", "Add", "res", "usercenter"], function (exports) {
    var $ = layui.jquery;
    var qian = layui.qian;
    var html = layui.html;
    var Add = layui.Add;
    var upload = layui.upload;
    var res = layui.res;
    var usercenter = layui.usercenter;

    var _this;

    function _UserCenter_Add() {
        _this = this;
    };

    //页面初始化
    _UserCenter_Add.prototype.init = function () {
        Add.init();
    }

    /************************** 重写父类方法 start **************************/

    //页面初始化方法
    Add.pageReady = function () {
        var mystr = usercenter.obtainCenterInfo();

        //皮肤设置
        html.toSelect({
            dom: 'centerskin',
            text: '皮肤列表',
            haspic: true,
            pickey: 'smallpath',
            selectedValue: mystr.centerskin,
            data: res.obtainSkinRes()
        });

        //消息设置
        $("#centersound").val(mystr.centersound);
        html.generRadioList({
            target: 'radiolist',
            data: res.obtainSoundRes(),
            name: 'centersoundtips',
            compareV: mystr.centersoundtips,
            defaultV: '1'
        });

        //头像设置
        $("#centeravatar-img").attr("src", mystr.centeravatar);
        $("#centeravatar").val(mystr.url);

        res.upload({
            elem: '#centeravatar-area',
            size: 120,//文件大小 KB
            done: function (res, index, upload) {
                if (res.status == "success") {
                    $("#centeravatar-img").attr("src", res.url);
                    qian.tips("上传成功");
                    $("#centeravatar").val(res.url);
                }
            }
        });

        //修改密码
        $("#passchange").click(function () {
            passchange();
        });
       
    }

    //表单验证
    Add.validForm = function () {
         var center_skin = $("#centerskin").val();
         if ($.trim(center_skin) == "") {
             qian.tips("请选择皮肤");
             return false;
         }
         return true;
    }

   
    //监听radio
    Add.listenRadio = function (obj, v) {
        res.playSound(v);
    }

    //字段处理
    Add.proccessField = function (fields) {
        if (fields["FileData"]) {
            delete fields["FileData"];
        }

        var result = "mystr=" + usercenter.proccessCenterInfo(fields);
        return result;
    }

    Add.onSuccess = function (res) {
        usercenter.reloadCenterInfo(res.Data, true);
        qian.tips("设置成功");
    }

    /************************** 重写父类方法 end **************************/


    //修改密码
    function passchange() {
        qian.sepcialTo({
            url: '/Account/ChangePassword',
            title: '密码设置',
            area:['580px', '320px']
        });
    }

    var UserCenter_Add = new _UserCenter_Add();
    exports('UserCenterAdd', UserCenter_Add);
});
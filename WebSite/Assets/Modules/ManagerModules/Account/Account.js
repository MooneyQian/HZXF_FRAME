/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 登录首页
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "element", "form", "qian"], function (exports) {
    var element = layui.element;
    var $ = layui.jquery;
    var qian = layui.qian;

    var _this;

    function _Account() {
        _this = this;
    };

    var loginStatus = 0;

    //页面初始化
    _Account.prototype.init = function () {
        if (window.top.document.location.href != document.location.href)
            window.top.document.location.href = document.location.href;

        cloud();
        login();
    }

    function login() {

        setTimeout(function () { $("#txtUsername").focus(); }, 100);
        $("#txtUsername").keydown(function (event) {
            if (event.keyCode == "13") {
                /*eggs*/
                if ($(this).val() == "*#chbg#*") {
                    changeBackground();
                }
                else
                    $("#txtPassword").focus();
            }
        });
        $("#txtPassword").keydown(function (event) {
            if (event.keyCode == "13") {
                if ($("#txtValidateCode").length > 0)
                    $("#txtValidateCode").focus();
                else
                    logon();
            }
        });
        $("#txtValidateCode").keydown(function (event) {
            if (event.keyCode == "13")
                logon();
        });
        $("#txtUsername").watermark("用户名");
        $("#txtPassword").watermark("密码");
        $("#txtValidateCode").watermark("验证码");

        $("#doLog").click(function () {
            logon();
        });

        $("#txtIsKeepLogon").click(function () {
            if ($(this).prop("checked")) {
                HS.Cookies.set("HS_KeepLogon", "1", 30);
            } else {
                {
                    HS.Cookies.remvoe("HS_KeepLogon");
                    HS.Cookies.set("HS_KeepLogonUsername", "", 30);
                    HS.Cookies.set("HS_KeepLogonPassword", "", 30);
                }
            }
        });

        if (HS.Cookies.get("HS_KeepLogon") == "1") {
            $("#txtIsKeepLogon").prop("checked", true);
            $("#txtUsername").val(HS.Cookies.get("HS_KeepLogonUsername"));
            $("#txtPassword").val(HS.Cookies.get("HS_KeepLogonPassword"));
        }


        setSize();

        window.onresize = setSize;

        //清除
        $(".clear span").click(function () {
            $("#txtUsername").val("");
            $("#txtPassword").val("");
            $("#txtUsername").focus();
        });

        //选择系统
        $(".sys_sel").hover(function () { $(this).children(".sys_sel_ul").slideDown(200); }, function () { $(this).children(".sys_sel_ul").slideUp(200); });
        $(".sys_op_li").click(function () {
            debugger;
            $(".sys_sel").children("i").text($(this).text());
            $("#txtLoginSystemID").val($(this).attr("data-sysid"));
            $(this).parent(".sys_sel_ul").slideUp(200);
        });

        //验证码
        $("#imgValidateCode").click(function () {
            $(this).attr("src", $(this).attr("catch-src") + "?n=" + Math.random());
        });
    }

    function cloud() {
        var $main = $cloud = mainwidth = null;
        var offset1 = 450;
        var offset2 = 0;

        var offsetbg = 0;

        $(document).ready(function () {
            $main = $("#mainBody");
            $body = $("body");
            $cloud1 = $("#cloud1");
            $cloud2 = $("#cloud2");

            mainwidth = $main.outerWidth();

        });

        /// 飘动
        setInterval(function flutter() {

            if (offset1 >= mainwidth) {
                offset1 = -580;
            }

            if (offset2 >= mainwidth) {
                offset2 = -580;
            }
            offset1 += 1.1;
            offset2 += 1;
            $cloud1.css("background-position", offset1 + "px 100px")

            $cloud2.css("background-position", offset2 + "px 460px")
        }, 70);


        setInterval(function bg() {
            if (offsetbg >= mainwidth) {
                offsetbg = -580;
            }
            offsetbg += 0.9;
            $body.css("background-position", -offsetbg + "px 0")
        }, 90);
    }


    function logon() {
        if (loginStatus == 0) {
            loginStatus = 1;
            var userName = $("#txtUsername").val();
            var userPwd = $("#txtPassword").val();
            var validateCode = $("#txtValidateCode").val();
            var loginSystemID = $("#txtLoginSystemID").val();
            var IsKeepLogon = $("#txtIsKeepLogon:checked").val();
            HS.Cookies.set("HS_KeepLogonUsername", userName, 30);
            HS.Cookies.set("HS_KeepLogonPassword", userPwd, 30);
            if (userName == "" || userPwd == "" || $("#txtUsername").hasClass("watermark") || $("#txtPassword").hasClass("watermark")) {
                qian.tips("请输入帐号密码！");
                loginStatus = 0
                return;
            }
            if ($("#txtValidateCode").length > 0 && (validateCode == "" || $("#txtValidateCode").hasClass("watermark"))) {
                qian.tips("请输入验证码！");
                loginStatus = 0
                return;
            }

            qian.ajax({
                type: "post", 
                url: loginUrl + '?rn=' + Math.random(),
                data: [
                        { name: 'ReUrl', value: resUrl },
                        { name: 'username', value: userName },
                        { name: 'password', value: userPwd },
                        { name: 'ValidateCode', value: validateCode },
                        { name: 'LoginSystemID', value: loginSystemID },
                        { name: 'IsKeepLogon', value: IsKeepLogon }
                ],
                success: function (result) {
                    if (result.IsError) {
                        loginStatus = 0
                        qian.tips(result.Message);
                        $("#txtUsername").focus();
                        $("#imgValidateCode").click();
                        return;
                    } else {
                        if (result.Data && result.Data != null && result.Data != "null") {
                            location.href = result.Data;
                        }
                    }
                },
                error: function (err) {
                    loading.close();
                    loginStatus = 0
                    qian.tips("发送系统错误,请与系统管理员联系！");
                }
            });
        }
    }

    function setSize() {
        if ($(window).width() / $(window).height() < 0.5625)
            $("#login_frame .login_main").width($(window).width()).height($(window).width() * 0.5625);  //16:9
        $("#login_frame .login_main").css("padding-top", ($("#login_frame").height() - $("#login_frame .login_main").height()) / 2);

        if (($("#login_frame .login_main").height() * 0.1) > $("#login_frame .login_main .login_top").height())
            $("#login_frame .login_main .login_top").height($("#login_frame .login_main").height() * 0.1);
        if (($("#login_frame .login_main").height() * 0.07) > $("#login_frame .login_main .login_bottom").height())
            $("#login_frame .login_main .login_bottom").height($("#login_frame .login_main").height() * 0.07);
        $("#login_frame .login_main .login_content").height($("#login_frame .login_main").height() - $("#login_frame .login_main .login_top").height() - $("#login_frame .login_main .login_bottom").height());

        setImgSize();
    }

    //图片设置大小，控制图片永远局中且满屏
    function setImgSize() {
        if (!$("#login_frame .login_main .login_content .login_bgimg").is(":visible")) {
            $("#login_frame .login_main .login_content .login_bgimg").show();
        }
        var img = $("#login_frame .login_main .login_content .login_bgimg");
        var frame = $("#login_frame .login_main .login_content");
        if ((frame.width() / frame.height()) > (img.width() / img.height())) {
            img.css({ 'width': "100%", 'height': "auto", 'left': 0, 'top': "-" + (((frame.width() * img.height() / img.width()) - frame.height()) / 2) + "px" });
        } else {
            img.css({ 'height': "100%", 'width': "auto", 'top': 0, 'left': "-" + (((frame.height() * img.width() / img.height()) - frame.width()) / 2) + "px" });
        }
    }

    var chbg = 1;
    function changeBackground() {
        chbg += 1;
        if (chbg > 4) chbg = 1;
        $(".login_bgimg").attr("src", "/Content/Login/images/login_bg_0" + chbg + ".jpg");//Math.round(Math.random() * 3 + 1)
    }

    var Account = new _Account();
    exports('Account', Account);
});
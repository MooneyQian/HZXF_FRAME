/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 首页模板
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "element", "qian", "res", "Layout", "usercenter"], function (exports) {
    var element = layui.element;
    var $ = layui.jquery;
    var qian = layui.qian;
    var res = layui.res;
    var Layout = layui.Layout;
    var usercenter = layui.usercenter;

    var mf = "mainframe";

    //模板文件路径
    var tplPath = "Basic/Master/tpl/mainindex/";

    var _this;

    function _MainIndex() {
        _this = this;
    };

    /****************************** 重写父类方法 start **************************/

    //框架初始化
    Layout.customerInit = function () {
        //左侧菜单滚动条美化
        $(".layui-side").niceScroll({
            horizrailenabled: false,
            cursorcolor: "#cdcdcd"
        });

        //监听tab点击事件
        element.on('tab(tabmain)', function () {
            var id = $(this).attr('lay-id');
            if (id == '1') {
                _this.loadTabMain();
            }
            listenTabClick();
        });

        //移动端适配
        mobileAdapt();

        //加载头部
        loadHeaderMain();
        //加载左侧菜单
        loadLeftMenusMain();
        //加载首页
        _this.loadTabMain();

        preventRight("1");
    }

    //框架适配
    Layout.resize = function () {
        $('#framebase').css('height', $(window).height());
        $('.' + mf).css('height', Layout.getHeight());
        $('.layui-tab-content').css('height', Layout.getHeight(true));
        $('#frameheight').css('height', getMainHeight());
    }

    /****************************** 重写父类方法 end **************************/



    /****************************** 可重写方法 start **************************/

    //加载头部
    _MainIndex.prototype.loadHeader = function () { }


    //左侧菜单url
    _MainIndex.prototype.loadLeftMenus = "";


    //加载首页
    _MainIndex.prototype.loadTabMain = function () {
        qian.ajaxTemplate({
            path: tplPath + "mainBodyTpl.html",
            dom: "mainBody"
        });
    }

    /****************************** 可重写方法 end **************************/




    /****************************** 公用方法 start **************************/

    //框架初始化
    _MainIndex.prototype.init = function () {
        $('#framebase').css('height', $(window).height());
        //加载布局
        Layout.init();
        $('#frameheight').css('height', getMainHeight());
        usercenter.loadCenterInfo();
    }

    //获取框架高度
    _MainIndex.prototype.getHeight = function () {
        return $("#frameheight").css("height");
    }

    //打开tab页
    _MainIndex.prototype.tabAdd = function (obj) {
        if (obj) {
            obj = $(obj).attr("q-data");
            var params = JSON.parse(decodeURIComponent(obj));
            var t = new Date().getTime();
            var frame_height = Layout.getHeight() + "px";


            var id = params.id || new Date().getTime();
            var title = params.name || "未指定标题";
            var url = params.menupath;
            var icon = params.icon;

            var isexisted = existed(id);
            if (!isexisted) {
                title += '<i class="layui-icon layui-unselect layui-tab-close forrefresh" data-id="' + id + '">&#x1002;</i>';

                title += '<i class="layui-icon layui-unselect layui-tab-close forclose" data-id="' + id + '">&#x1006;</i>';

                var content = '<iframe id="mainframe' + id + '" height="' + frame_height + '" class="qui-frame-mainframe mainframe" src="' + url + '" data-id="' + id + '"></iframe>';

                //新增一个Tab项
                element.tabAdd('tabmain', {
                    title: title,
                    content: content,
                    id: id
                });
                //监听
                listenTab(id);
            }
            element.tabChange("tabmain", id);
            resize(id);
            preventRight(id);
        } else {
            qian.tips("参数错误，无法打开");
        }
    };

    //关闭其他
    _MainIndex.prototype.closeOthers = function (id) {
        var objTab = getTabObj();
        objTab.titleBox.find('li').each(function () {
            var key = $(this).attr('lay-id');
            if (key != "1" && key != id) {
                element.tabDelete(objTab.tabFilter, key);
                element.init();
            }
        });

    }

    //关闭全部
    _MainIndex.prototype.closeAll = function () {
        var objTab = getTabObj();
        objTab.titleBox.find('li').each(function () {
            var key = $(this).attr('lay-id');
            if (key != "1") {
                element.tabDelete(objTab.tabFilter, key);
                element.init();
            }
        });

    }


    /****************************** 公用方法 end **************************/




    /****************************** 私有方法 start **************************/


    //获取主页面高度
    function getMainHeight() {
        return Layout.getHeight() + 56;
    }

    //判断tab是否存在
    function existed(id) {
        var objTab = getTabObj();
        var isexisted = false;
        objTab.titleBox.find('li').each(function (i, e) {
            if ($(this).attr("lay-id") == id) {
                isexisted = true;
                return false;
            };
        });
        return isexisted;
    };

    //页面重新适配
    function resize(id) {
        $('#framebase').css('height', $(window).height());
        $('#' + mf + id).css('height', Layout.getHeight());
        $('.layui-tab-content').css('height', Layout.getHeight(true));
        $('#frameheight').css('height', getMainHeight());
    }

    //阻止右键事件
    function preventRight(id) {
        if (id) {
            removeTabListen();
            $("li[lay-id='" + id + "']").bind("contextmenu", function () {
                return false;
            });
            $("li[lay-id='" + id + "']").mousedown(function (e) {
                var _that = this;
                //右键为3
                if (e.which == 3) {
                    var tabid = 'tabid-' + id;
                    if (!document.getElementById(tabid)) {
                        removeTabListen();
                        qian.ajaxTemplate({
                            path: tplPath + 'tabRightTpl.html',
                            data: {
                                id: tabid,
                                key:id
                            },
                            hasLoading: false,
                            render: function (html) {
                                $(_that).append(html);
                            }
                        });
                    }
                } else {//其他则关闭
                    if (e.which == 1) {

                    } else {
                        removeTabListen();
                    }
                }
            })
        }

    }

    //关闭所有右键菜单
    function removeTabListen() {
        $(".tab-listen").remove();
    }

    //tab默认点击事件
    function listenTabClick() {
        removeTabListen();
    }

    //监听图标事件
    function listenTab(id) {
        var objTab = getTabObj();
        //监听刷新事件
        objTab.titleBox.find('li').children('i.forrefresh[data-id=' + id + ']').on('click', function () {
            $("#" + mf + id).attr("src", $("#" + mf + id).attr("src"));
        });
        //监听关闭事件
        objTab.titleBox.find('li').children('i.forclose[data-id=' + id + ']').on('click', function () {
            element.tabDelete(objTab.tabFilter, $(this).parent('li').attr('lay-id'));
            element.init();
        });

    }

    //获取tab对象
    function getTabObj() {
        var objTab = {};
        var tabcontainer = $(".tabcontainer");
        objTab.titleBox = tabcontainer.children('ul.layui-tab-title');
        objTab.contentBox = tabcontainer.children('div.layui-tab-content');
        objTab.tabFilter = tabcontainer.attr('lay-filter');
        return objTab;
    };
    

    function loadMainIndex() {
        
    }

    //加载头部
    function loadHeaderMain() {
        //日历
        $("#date-com").click(function () {
            alert();
        });


        //时钟
        $('#digital-clock').clock({
            offset: '+8',
            type: 'digital'
        });

        //右部下拉菜单事件绑定
        //更换皮肤
        $("#h-change-skin").click(function () {
            loadSkin();
        });

        _this.loadHeader();
    }

    //加载左侧菜单主方法
    function loadLeftMenusMain() {
        var url = _this.loadLeftMenus;
        if (!url || qian.isEmpty(url)) {
            return;
        }
        qian.ajax({
            url: url,
            type: 'post',
            success: function (res) {
                //处理菜单数据
                var result = sonsTree(res.Data);
                qian.ajaxTemplate({
                    path: tplPath + "leftMenusTpl.html",
                    dom: "leftMenus",
                    data: result,
                    render: function () {
                        $(".child").accordionze({
                            accordionze: true,
                            speed: 450,
                            closedSign: '<img src="/Assets/Res/Images/plus.png">',
                            openedSign: '<img src="/Assets/Res/Images/minus.png">'
                        });

                        infoTrigger();
                    }
                });
            }
        });
    }

    //子孙树，从顶级往下找到是有的子子孙孙
    function sonsTree(arr, id) {
        var temp = [];
        var forFn = function (arr, parent) {
            for (var i = 0; i < arr.length; i++) {
                var item = arr[i];
                var id;
                if (!parent) {
                    id = "0";
                } else {
                    id = parent.id;
                }
                if (item.pId == id) {
                    if (!item.childList) {
                        item.childList = [];
                    }
                    if (id == "0") {
                        temp.push(item);
                    } else {
                        parent.childList.push(item);
                    }
                    forFn(arr, item);
                }
            }
        };
        forFn(arr, id);
        return temp;
    }


    //处理菜单层级关系
    function proccessLevel(data, pid) {
        var result = [];

        for (var i = 0; i < data.length; i++) {
            var tmp = data[i];
        }



        var parentList = [];
        var childMap = {};

        var data = res.Data;
        for (var i = 0; i < data.length; i++) {
            var tmp = data[i];
            if (tmp.pId == "0") {//父菜单
                parentList.push(tmp);
            } else {
                if (!childMap[tmp.pId]) {
                    childMap[tmp.pId] = [tmp];
                } else {
                    childMap[tmp.pId].push(tmp);
                }
            }

        }
    }



    //触发个人设置中心
    function infoTrigger() {
        $(".info-trigger").mouseenter(function () {
            $(".info-settings").show(10);
            
        });
        $(".info-trigger").mouseleave(function () {
            $(".info-settings").hide();
        });

        $(".info-settings").click(function () {
            qian.sepcialTo({
                url: "/Configuration/Add",
                title: '账户设置'

            });
        });
    }


    //加载皮肤
    function loadSkin() {
        var skinList = res.obtainSkinRes();
    
        qian.ajaxTemplate({
            path: tplPath + "topSkinTpl.html",
            data: skinList,
            render: function (html) {
                layer.open({
                    type: 1,
                    title:"皮肤列表",
                    skin: 'layui-layer-demo', //样式类名
                    shade:false,
                    shadeClose: true, //开启遮罩关闭
                    content: html
                });
            }
        });
    }

    //手机设备的简单适配
    function mobileAdapt() {
        var treeMobile = $('.site-tree-mobile')
        , shadeMobile = $('.site-mobile-shade')

        treeMobile.on('click', function () {
            $('body').addClass('site-mobile');
        });

        shadeMobile.on('click', function () {
            $('body').removeClass('site-mobile');
        });
    }


    /****************************** 私有方法 end **************************/
  
    var MainIndex = new _MainIndex();
    exports('MainIndex', MainIndex);
});
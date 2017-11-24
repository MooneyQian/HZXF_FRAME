/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 用户首页
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "element", "form", "qian", "MainIndex", "usercenter"], function (exports) {
    var element = layui.element;
    var $ = layui.jquery;
    var qian = layui.qian;
    var MainIndex = layui.MainIndex;
    var usercenter = layui.usercenter;

    //模板文件路径
    var tplPath = "Modules/UserModules/CustomerHome/tpl/";

    var sysmsg;

    function _CustomerHome() { };

    //加载头部
    MainIndex.loadHeader = function () {
        //消息提醒
        loadTips();
        //更改密码
        $("#h-change-pwd").click(function () {

        });

        //退出登录
        $("#h-logout").click(function () {
            logout();
        });
    }

    //加载左侧菜单
    MainIndex.loadLeftMenus = "/Home/GetMenuTree";

    //打开首页
    MainIndex.loadTabMain = function () {
        qian.ajax({
            url: '/CustomerHome/_GetHomeData',
            type: 'post',
            hasLoading: false,
            success: function (res) {
                qian.ajaxTemplate({
                    path: tplPath + "mainBodyTpl.html",
                    dom: "mainBody",
                    data: res,
                    render: function () {
                        $(".content-scroll").css("height", MainIndex.getHeight());
                        loadMap();
                        loadCharts();
                    }
                });
            }
        });

    }

    //页面初始化
    _CustomerHome.prototype.init = function () {
        MainIndex.init();
        
        $(window).resize(function () {
            $(".content-scroll").css("height", MainIndex.getHeight());
        });
    }


    //图标跳动
    _CustomerHome.prototype.msgblink = function (t, objname) {
        msgblink(t, objname);
    }

    //获得案件提醒
    _CustomerHome.prototype.getCaseInfoOut = function () {
        getCaseInfo();
    }

    

    //图标跳动主方法
    function msgblink(t, objname) {
        var soccer = document.getElementById(objname);
        if (t) {
            soccer.style.visibility = "visible";
        } else {
            soccer.style.visibility = (soccer.style.visibility == "hidden") ? "visible" : "hidden";
        }

    }


    //提醒设置
    function loadTips() {
        //点击事件
        $(".case-remind").click(function () {
            alert(11);
        });
        //请求案件提醒数量
        getCaseInfo();

        setInterval("layui.CustomerHome.getCaseInfoOut()", 1000 * 60 * 2);//2分钟执行一次
    }

    //获得案件提醒
    function getCaseInfo() {
        qian.ajax({
            url: '/LegalCase/_GetCaseInfo',
            type: 'post',
            hasLoading: false,
            success: function (res) {
                if (res > 0) {
                    $("#case-badge").removeClass("layui-hide");
                    clearSysMsg();
                    soccerOnload();
                    obtainSoundtips();
                } else {
                    $("#case-badge").addClass("layui-hide");
                    clearSysMsg();
                }
                $("#case-badge").html(res);
            }

        });
    }


    //清除计时器
    function clearSysMsg() {
        if (typeof (sysmsg) == "undefined") {

        } else {
            window.clearInterval(sysmsg);
            msgblink("1", "case-tips");
        }
    }

    //图标跳动
    function soccerOnload(type) {
        sysmsg = window.setInterval("layui.CustomerHome.msgblink('', 'case-tips')", 500);
    }

    //获取声音配置
    function obtainSoundtips() {
        usercenter.playSound();
    }


    //退出登录
    function logout() {
        window.location.href = '/Account/LogOff';
    }

    function loadMap() {
        return;
        try {
            map = new T.Map('mapDiv');
            map.centerAndZoom(new T.LngLat(119.67534, 30.64112), 14);
            //map.enableHandleMouseScroll(); //允许鼠标双击放大地图   

            loadMapData();

        } catch (err) {
            alert('天地图加载不成功，请稍候再试！你可以先使用其他功能！');
        }
    }

    //加载地图数据
    function loadMapData() {
        qian.ajax({
            url: '/EquipmentInfo/_GetALLWebcamJson',
            type: 'post',
            data: {
                devtype: "Webcam"
            },
            success: function (res) {
                drawTMaker(res);
            }
        });
    }


    //往地图上添加一个marker。传入参数坐标信息lnglat。传入参数图标信息。
    function drawTMaker(res) {
        var icon = new T.Icon({

            iconUrl: "/Assets/Res/Images/ico/jk.png",

            iconSize: new T.Point(30, 30),

            iconAnchor: new T.Point(30, 30)
        });
        var icon1 = new T.Icon({

            iconUrl: "/Assets/Res/Images/ico/jk1.png",

            iconSize: new T.Point(30, 30),

            iconAnchor: new T.Point(30, 30)
        });
        for (var i = 0; i < res.length; i++) {
            var iconuse = icon;
            if (res[i].Status != 1) {
                iconuse = icon1;
            }
            var marker = new T.Marker(new T.LngLat(res[i].L, res[i].B), { icon: iconuse });
            map.addOverLay(marker);
        }
    }

    //加载图表
    function loadCharts() {
        var myChart = echarts.init(document.getElementById('charts1'));

        var myChart2 = echarts.init(document.getElementById('charts2'));

        var option = {
            title: {
                text: '未来一周气温变化',
                subtext: '纯属虚构'
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: ['最高气温', '最低气温']
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: { readOnly: false },
                    magicType: { type: ['line', 'bar'] }
                }
            },
            xAxis: {
                type: 'category',
                boundaryGap: false,
                data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
            },
            yAxis: {
                type: 'value',
                axisLabel: {
                    formatter: '{value} °C'
                }
            },
            series: [
                {
                    name: '最高气温',
                    type: 'line',
                    data: [11, 11, 15, 13, 12, 13, 10],
                    markPoint: {
                        data: [
                            { type: 'max', name: '最大值' },
                            { type: 'min', name: '最小值' }
                        ]
                    },
                    markLine: {
                        data: [
                            { type: 'average', name: '平均值' }
                        ]
                    }
                },
                {
                    name: '最低气温',
                    type: 'line',
                    data: [1, -2, 2, 5, 3, 2, 0],
                    markPoint: {
                        data: [
                            { name: '周最低', value: -2, xAxis: 1, yAxis: -1.5 }
                        ]
                    },
                    markLine: {
                        data: [
                            { type: 'average', name: '平均值' },
                            [{
                                symbol: 'none',
                                x: '90%',
                                yAxis: 'max'
                            }, {
                                symbol: 'circle',
                                label: {
                                    normal: {
                                        position: 'start',
                                        formatter: '最大值'
                                    }
                                },
                                type: 'max',
                                name: '最高点'
                            }]
                        ]
                    }
                }
            ]
        };

        myChart.setOption(option);
        myChart2.setOption(option);
    }

    var CustomerHome = new _CustomerHome();
    exports('CustomerHome', CustomerHome);
});
/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 通用列表页
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "element", "form", "layer", "laypage", "table", "laydate", "qian"], function (exports) {
    var $ = layui.jquery;
    var form = layui.form
    var qian = layui.qian;
    var table = layui.table;
    var laydate = layui.laydate;

    var btns = [];//按钮集合

    var navBtns = []; //导航操作按钮

    var toolBtns = []; //记录操作按钮

    var tableCollection = null;//当前表格容器

    var currentConditions = {}; //当前查询条件

    var _this;

    function _List() {
        _this = this;
    }


    //初始化
    _List.prototype.init = function () {
        this.loadListBody();
    }

    //加载当前页面
    _List.prototype.loadListBody = function () {
        
        var menukey = getCurrentMenuKey();
        if (!menukey) {
            qian.tips("无法获取菜单");
            return;
        }
        qian.ajax({
            url: '/Menu/_GetMenuRole',
            type: 'post',
            data: {
                menuid: menukey
            },
            success: function (obj) {
                //加载按钮
                proccessBtns(obj.Rows);
                //加载导航按钮
                _this.proccessNavBtns();
                //加载搜索区域
                _this.loadSearchAreas();
                //加载页面内容
                _this.loadListAreas();
            }
        });
       
    }


    //加载搜索区域
    _List.prototype.loadSearchAreas = function () {
        var obj = document.getElementById("searchAreas");
        if (obj) {
            
            $(".search-input").click(function () {
                _this.expand();
            });

            $(".search-icon-button").click(function () {
                $("#btn-submit").trigger("click");
            });

            $("#btn-reset").click(function () {
                $("#form1")[0].reset();
                return false;
            });

            $("#btn-close").click(function () {
                _this.expand();
                return false;
            });

            form.on("submit(search)", function (data) {
                currentConditions = data.field;
                addConditionTab();
                reload(true)
                return false;
            });
        } else {
            emptyNavBtns();
        }
    }
    
    //删除条件tab
    _List.prototype.delTab = function (k) {
        $("#" + k + "_tab").remove();
        $("#" + k).val("");
        $("#btn-submit").trigger("click");
    }


    //展开关闭搜索栏
    _List.prototype.expand = function () {
        if ($(".search-ele-area").is(":hidden")) {
            $(".search-ele-area").slideDown(400);
        } else {
            $(".search-ele-area").slideUp(300);
        }
    }

    //加载列表数据
    _List.prototype.loadListAreas = function () {
        var dataList = _this.dataGrid();
        if (!dataList) {
            return false;
        }
        loadTable(dataList);
    }

    //处理导航操作按钮
    _List.prototype.proccessNavBtns = function () {
        qian.renderTemplate('navBtnsAreas', 'navBtnsTpl', navBtns);

    }


    //处理记录操作列
    _List.prototype.proccessToolBtns = function (r) {
        var result = qian.deepCopy(toolBtns);
        for (var i = 0; i < result.length; i++) {
            var tmp = result[i];
            tmp.url = tmp.url.replace('{KEY}', r.ID);
        }
        return result;
    }

    //处理按钮样式
    _List.prototype.proccessBtnStyles = function (obj, from) {
        var backcolor = obj.backcolor;
        var classname = '';
        var stylename = '';
        if (backcolor == 'default') {
            classname = 'qui-btn-normal';
        } else if (backcolor == 'alarm') {
            classname = 'layui-btn-danger';
        } else {
            stylename = 'style="background-color:' + backcolor + '"';
        }
        var icon = obj.icon;
        if (icon) {
            if (icon.indexOf("icon") >= 0) {
                icon = '<i class="' + icon + '"></i>';
            } else {
                if (!from) {
                    icon = '<i class="layui-icon" style="font-size:14px;margin-right: -3px;">' + icon + '</i>';
                } else {
                    icon = '<i class="layui-icon" style="font-size:16px;">' + icon + '</i>';
                }
            }
        } else {
            icon = '';
        }


        return {
            stylename:stylename,
            classname: classname,
            icon:icon
        };
    }

    //显示图片
    _List.prototype.imgShow = function (obj) {
        qian.showImg({
            single: true,
            src:$(obj).attr("src")
        });
    }

    //处理工具栏按钮跳转方式
    _List.prototype.proccessNavsTo = function (obj) {
        if (!obj) {
            return;
        }
        var params = JSON.parse(decodeURIComponent(obj));

        var key = "ID";
        var allowRowOper = params.allowRowOper;
        var plist = "";
        if (allowRowOper) {//允许行操作
            var checkStatus = table.checkStatus('listTable');
            if (checkStatus.data.length == 0) {
                qian.tips("请至少选择一条记录");
                return;
            }
            for (var i = 0; i < checkStatus.data.length; i++) {
                plist += checkStatus.data[i][key] + ',';
            }
            params.url = params.url.replace("{KEY}", plist);
        }
        
        this.proccessToolsTo(encodeURIComponent(JSON2.stringify(params)));
    }

    //处理工具栏按钮跳转方式
    _List.prototype.proccessToolsTo = function (obj, ext) {
        if (!obj) {
            return;
        }
        var d = {};

        if (ext) {
            var plist = $(ext).parent().parent().parent().find("td");
            plist.each(function () {
                var dfv = $(this).find("div").html();
                if (dfv.indexOf("checkbox") < 0 && dfv.indexOf("button") < 0) {
                    var df = $(this).attr("data-field");
                    d[df] = dfv;
                }
            });
        }
        var params = JSON.parse(decodeURIComponent(obj));
        if (params.hasConfirm) {//操作提示
            qian.confirm(params.confirmInfo, function () {
                //post方式提交
                if (params.postSubmit) {
                    qian.ajax({
                        url: params.url,
                        type: 'post',
                        data:d,
                        success: function (res) {
                            reload();
                        }
                    });
                } else {
                    qian.to(qian.contactToUrl(params.url, d), params.name, function () {
                        if (params.rightType != 'SView') {
                            reload();
                        }
                    });
                }
            });
        } else {
            if (params.postSubmit) {
                qian.ajax({
                    url: params.url,
                    type: 'post',
                    data: d,
                    success: function (res) {
                        reload();
                        _this.afterPost(res);
                    }
                });
            } else {
                qian.to(qian.contactToUrl(params.url, d), params.name, function () {
                    if (params.rightType != 'SView') {
                        reload();
                    }
                });
            }
        }

        
    }

    /******************** 可复写方法 start **********************/

    //定义数据结构
    _List.prototype.dataGrid = function () {
        return {};
    }

    //post提交后回调方法
    _List.prototype.afterPost = function (res) {

    }

    /******************** 可复写方法 end **********************/


    /******************** 私有方法 start **********************/

    function proccessWay() {

    }

    //重载表格
    function reload(isreset) {
        if (!tableCollection) {
            qian.tips("参数错误");
            return;
        }
        tableCollection.reload({
            where: currentConditions,
            currpage: isreset ? 1 : 0
        });
    }

    //添加搜索条件tab
    function addConditionTab() {
        var pl = "点击展开搜索条件";
        var r = "";
        if (!currentConditions) {
            $("#search-input").attr("placeholder", pl);
        } else {
            $("#search-input").attr("placeholder", "");
            var r = "";
            for (var k in currentConditions) {
                r += conditionTabHTML(k, currentConditions[k]);
            }
        }
        $("#search-tab").html(r);
    }

    //tab内容
    function conditionTabHTML(k, v) {
        if (qian.isEmpty(v)) {
            return "";
        }
        return '<div id="' + k + '_tab" class="layui-btn btn-bit-small search-tab-item">' +
               '<span class="">' + v + '</span>' +
               '<i class="layui-icon layui-unselect" onclick="layui.List.delTab(\'' + k + '\');">&#x1006;</i>' +
               '</div>';
    }


    //获取当前菜单标识
    function getCurrentMenuKey() {
        var fid = self.frameElement.getAttribute('data-id') || qian.getQueryString("menuid");
        return fid || false;
    }


    //处理按钮
    function proccessBtns(obj) {
        for (var i = 0; i < obj.length; i++) {
            var tmp = obj[i];

            var isNav = tmp.IsNav == '1' ? true : false;
            var isTool = tmp.IsTool == '1' ? true : false;
            var isPost = tmp.IsPost == '1' ? true : false;
            var needConfirm = tmp.NeedConfirm == '1' ? true : false;
            var confirmInfo = tmp.ConfirmInfo || '确定要继续操作吗？';
            var allowRowOper = tmp.Extend2 == '1' ? true : false;

            var tmpBtn = {
                name: tmp.MenuName,
                code: tmp.MenuCode,
                url: tmp.MenuPath,
                navDisplay: tmp.IsNav,
                toolDisplay: tmp.IsTool,
                postSubmit: isPost,
                hasConfirm: needConfirm,
                confirmInfo: confirmInfo,
                icon: tmp.MenuIcon || "",
                backcolor: tmp.Extend1 || "default",
                allowRowOper: allowRowOper,
                rightType:tmp.Extend3

            };
            if (isNav) {
                navBtns.push(tmpBtn);
            }
            if (isTool) {
                toolBtns.push(tmpBtn);
            }
            btns.push(tmpBtn);
        }
    }

    //处理导航按钮为空的情况
    function emptyNavBtns() {
        if (navBtns.length == 0) {
            $("#navBtnsAreas").html("");
        }
    }
  

    //加载列表内容
    function loadTable(obj) {
        //执行渲染
        tableCollection = table.render({
            url: obj.url,
            method: 'post',
            id: 'listTable',
            elem: '#listTable',
            height: 'full-60',
            cols: proccessDataGrid(obj.grid, obj.checked),
            done: function (res, curr, count) {
            },
            page: _this.dataGrid().page == undefined ? true : _this.dataGrid().page,
            limits: [15, 30, 60],
            limit: 30, //默认采用30
            loading: true,
            //skin: 'line', //行边框风格, 
            even: true, //开启隔行背景
            size: 'sm', //小尺寸的表格
            request: {
                sortOrder: 'Asc',
                sortName: 'ID',
                limitName: 'pageSize'
            },
            response: {
                //statusName: 'status', //数据状态的字段名称，默认：code
                //statusCode: 200, //成功的状态码，默认：0
                dataName: 'Rows',
                countName: 'Total'
            },
            where: qian.queryToJSON(qian.getQueryString())
        });
    }

    //返回符合要求的数据结构
    function proccessDataGrid(data, checked) {
        var oneArray = [];
        var twoArray = [{ checkbox: true, fixed: true }];
        if (checked == false) {
            twoArray = [];
        }
        var toolBar = {};

        for (var i = 0; i < data.length; i++) {
            var tmp = data[i];
            var r = {
                coltype: tmp.type || 'text',
                field: tmp.name,
                title: tmp.display,
                width: tmp.width,
                fixed: tmp.fixed || false,
                sort: tmp.sort || false,
                type:tmp.coltype || 'normal'
            };

            var templet = "";
            if (r.coltype == "img") {
                templet = '<img class="img-show" src="{{ d.' + r.field + ' }}" onclick="layui.List.imgShow(this)">';
            } else if (r.coltype == "date") {
                templet = '{{ layui.qian.formatDate(d.' + r.field + ') }}';
            } else if (r.coltype == 'dict') {

            }

            if (tmp.link) {
                if (templet == "") {
                    templet = '{{ d.' + r.field + ' }}';
                }
                templet = '<a href="javascript:;" onclick="layui.qian.to(\'' + tmp.link + '\')">' + templet + '</a>';
            }

            if (templet != '') {
                templet = '<div>' + templet + '</div>';
                r.templet = templet;
            }
            twoArray.push(r);
        }
        if (toolBtns.length > 0) {//操作列
            toolBar = {
                fixed: 'right',
                title: '操作',
                width: 80 * toolBtns.length,
                align: 'center',
                templet: "#toolBtnsTpl"
            }

            twoArray.push(toolBar);
        }
        oneArray.push(twoArray);
        return oneArray;
    }


    /******************** 私有方法 end **********************/
       

    var List = new _List();
    exports('List', List);
});
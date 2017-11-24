/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 树型列表页
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "form", "layer", "qian"], function (exports) {
    var $ = layui.jquery;
    var qian = layui.qian;
    var layer = layui.layer;
    var form = layui.form;

    var _this;
    var _config = {
        id: "treeGridTable",
        width: "800",
        renderTo: "treeGridList",
        align: "center",
        headerHeight: "30",
        dataAlign: "left",
        indentation: "20",
        expandLayer: 0,
        folderOpenIcon: "/Assets/Res/Images/folderOpen.png",
        folderCloseIcon: "/Assets/Res/Images/folderClose.png",
        defaultLeafIcon: "/Assets/Res/Images/defaultLeaf.gif",
        hoverRowBackground: "false",
        folderColumnIndex: "0",
        itemClick: "itemClickEvent",
        grid: [],
        data: [],
        parentKey: 0,
        key: 0
    };

    var s = "";
    var rownum = 0;
    var _root;

    var _selectedData = null;
    var _selectedId = null;
    var _selectedIndex = null;

    var COUNT = 0;

    function _TreeGridList() {
        _this = this;
    }

    //定义数据结构
    _TreeGridList.prototype.dataGrid = function () {
        return null;
    }

    //提交
    _TreeGridList.prototype.toSubmit = function () {

    }
    

    _TreeGridList.prototype.check = function (obj) {
        var parentTR = $(obj).parent().parent();
        var currentTD = parentTR.find("td").get(0);

        var currentCheck = $(currentTD).find("input").get(0);
        var currentCheckDiv = $(currentTD).find(".layui-form-checkbox").get(0);


        var ischecked = false;
        if ($(currentCheckDiv).hasClass("layui-form-checked")) {
            $(currentCheckDiv).removeClass("layui-form-checked");
            $(currentCheck).removeAttr("checked");
        } else {
            $(currentCheckDiv).addClass("layui-form-checked");
            $(currentCheck).attr("checked", "");
            ischecked = true;
        }

        //处理子菜单
        var pid = $(parentTR).attr("pid");
        var id = $(parentTR).attr("id");
        var searchid = id;
       

        $("input[type=checkbox][name^='" + searchid + "']").each(function (i, item) {
            $(item).parent().parent().find(".layui-form-checkbox").each(function () {
                if (pid != item.name) {
                    if (ischecked) {
                        $(this).addClass("layui-form-checked");
                        $(item).attr("checked", "");
                    } else {
                        $(this).removeClass("layui-form-checked");
                        $(item).removeAttr("checked");
                    }
                }
            });
        });
    }

    //获取选中行的key
    _TreeGridList.prototype.getCheckKeys = function () {
        var keys = []
        $(".trbody").each(function () {
            var currentTD = $(this).find("td").get(0);
            var currentCheck = $(currentTD).find("input").get(0);
            var currentCheckDiv = $(currentTD).find(".layui-form-checkbox").get(0);
            if ($(currentCheckDiv).hasClass("layui-form-checked")) {
                var tmp = str2json($(this).attr("data"));
                keys.push(tmp[_config.key]);
            }
        });
        return keys;
    }
   


    //主函数
    _TreeGridList.prototype.init = function () {
        _config = $.extend(_config, getDataGrid() || {});


        this.id = _config.id || ("TreeGridList" + COUNT++);

        s += "<table id='" + this.id + "' cellspacing=0 cellpadding=0 style='color:#333;' width='" + (_config.width || "100%") + "' class='layui-table TreeGrid' lay-size='sm'>";
        drawHeader();
        drawData();
        s += "</table>";
        _root = $("#" + _config.renderTo);
        _root.append(s);
        form.render();


        form.on('checkbox', function (data) {
            //console.log(data.elem); //得到checkbox原始DOM对象
            //console.log(data.elem.checked); //是否被选中，true或者false
            //console.log(data.value); //复选框value值，也可以通过data.elem.value得到
            //console.log(data.othis); //得到美化后的DOM对象
        });


        $("#q-submit").click(function () {
            _this.toSubmit();
        });

        //初始化动作
        init();

      
    }

    //展开或收起所有节点
    _TreeGridList.prototype.expandAll = function (isOpen) {
        var trs = _root.find("tr[pid='']");
        for (var i = 0; i < trs.length; i++) {
            var trid = trs[i].id || trs[i].getAttribute("id");
            showHiddenNode(trid, isOpen);
        }
    }

    //取得当前选中的行记录
    _TreeGridList.prototype.getSelectedItem = function () {
        return new TreeGridItem(_root, _selectedId, _selectedIndex, str2json(_selectedData));
    }

    _TreeGridList.prototype.Item = TreeGridListItem;


    function getDataGrid() {
        var dg = _this.dataGrid();
        if (!dg || !dg.url) {
            return {};
        }
        qian.ajax({
            url: dg.url,
            type: 'post',
            async: false,
            success: function (res) {
                if (dg.parentKey) {
                    _config.parentKey = dg.parentKey;
                    _config.key = dg.key;
                    dg.data = proccessDataGrid(res.Rows);
                } else {
                    _config.key = dg.key;
                    dg.data = res.Rows;
                }
            }
        });
        return dg;
    }

    function getDom(arr, source, parentobj) {
        if (!arr) {
            return null;
        }
        parentobj["children"] = arr;
        for (var i = 0; i < arr.length; i++) {
            getDom(source[arr[i][_config.key]], source, arr[i]);
        }

    }

    function proccessDataGrid(data) {
        var groups = {};

        for (var i = 0; i < data.length; i++) {
            if (groups[data[i][_config.parentKey]]) {
                groups[data[i][_config.parentKey]].push(data[i]);
            } else {
                groups[data[i][_config.parentKey]] = [];
                groups[data[i][_config.parentKey]].push(data[i]);
            }
        }
        var parentobj = {};
        getDom(groups["0"], groups, parentobj);

        return parentobj.children || [];
    }


    //显示表头行
    function drawHeader() {
        s += "<thead><tr class='header' height='" + (_config.headerHeight || "25") + "'>";
        s += "<td width='30px'><input type='checkbox' lay-skin='primary'/></td>"
        var cols = _config.grid;
        for (i = 0; i < cols.length; i++) {
            var col = cols[i];
            s += "<td align='" + (col.align || _config.align || "center") + "' width='" + (col.width || "") + "'>" + (col.display || "") + "</td>";
        }
        s += "</tr></thead>";
    }

    //递归显示数据行
    function drawData() {
        var rows = _config.data;
        var cols = _config.grid;
        drawRowData(rows, cols, 1, "");
    }


    function drawRowData(_rows, _cols, _level, _pid) {
        var folderColumnIndex = (_config.folderColumnIndex || 0);
        for (var i = 0; i < _rows.length; i++) {
            var id = _pid + "_" + i; //行id
            var row = _rows[i];

            var open = "N";
            if (_level <= _config.expandLayer) open = "Y";

            var dispaly = "none";
            if (_level <= _config.expandLayer + 1) dispaly = ""; //显示层等于展开层+1

            var pid = (_pid == "") ? "TR" + id + "_root" : ("TR" + _pid);

            s += "<tr class='trbody'  id='TR" + id + "' pid='" + pid + "' open='" + open + "' data=\"" + json2str(row) + "\" rowIndex='" + rownum++ + "' style='display:" + dispaly + "'>";
            if (_rows[i][_config.checkKey]) {
                s += '<td><input type="checkbox" name="TR' + id + '"  lay-skin="primary" checked/></td>';
            } else {
                s += '<td><input type="checkbox" name="TR' + id + '"  lay-skin="primary"/></td>';
            }
            for (var j = 0; j < _cols.length; j++) {
                var col = _cols[j];
                if (j == folderColumnIndex) {
                    s += "<td align='" + (col.dataAlign || _config.dataAlign || "left") + "'";
                } else {
                    s += "<td align='" + (col.dataAlign || _config.dataAlign || "left") + "'";
                }

                //层次缩进
                if (j == folderColumnIndex) {
                    s += " style='text-indent:" + (parseInt((_config.indentation || "20")) * (_level - 1)) + "px;'> ";
                } else {
                    s += ">";
                }

                //节点图标
                if (j == folderColumnIndex) {
                    if (row.children) { //有下级数据
                        if (open == "Y") {
                            s += "<img folder='Y' trid='TR" + id + "' src='" + _config.folderOpenIcon + "' class='image_hand'>";
                        } else {
                            s += "<img folder='Y' trid='TR" + id + "' src='" + _config.folderCloseIcon + "' class='image_hand'>";
                        }
                    } else {
                        s += "<img src='" + _config.defaultLeafIcon + "' class='image_nohand'>";
                    }
                }
                
                //单元格内容
                if (col.handler) {
                    if ((col.folderHidden || false) && row.children) {//是否隐藏字段值
                        s += "</td>";
                    } else {
                        s += (eval(col.handler + ".call(new Object(), row, col)") || "") + "</td>";
                    }
                } else {
                    s += '<span onclick="layui.TreeGridList.check(this)" class="checktdclass">' + (row[col.name] || '') + '</span></td>';
                }
            }
            s += "</tr>";

            //递归显示下级数据
            if (row.children) {
                drawRowData(row.children, _cols, _level + 1, id);
            }
        }
    }

    /*
        单击数据行后触发该事件
        id：行的id
        index：行的索引。
        data：json格式的行数据对象。
    */
    function itemClickEvent(id, index, data) {
        $("#currentRow").val(id + ", " + index + ", " + json2str(data));
    }

    function customCheckBox(row, col) {
        return '<input name="cblbox" type="checkbox" lay-skin="primary">';
    }

    function customOrgName(row, col) {
        var name = row[col.name] || "";
        return name;
    }

    function customLook(row, col) {

        return "<a href='javascript:void(0)' onclick='showrowname(\"" + row.code + "\",\"" + row.name + "\")' style='color:blue;'>查看</a>";
    }

    function showrowname(code, name) {
        alert(code + name);

    }


    function init() {
        //以新背景色标识鼠标所指行
        if ((_config.hoverRowBackground || "false") == "true") {
            _root.find("tr").hover(
				function () {
				    if ($(this).prop("class") && $(this).prop("class") == "header") return;
				    $(this).addClass("row_hover");
				},
				function () {
				    $(this).removeClass("row_hover");
				}
			);
        }

        //将单击事件绑定到tr标签
        _root.find("tr").bind("click", function () {
            _root.find("tr").removeClass("row_active");
            $(this).addClass("row_active");

            //获取当前行的数据
            _selectedData = this.data || this.getAttribute("data");
            _selectedId = this.id || this.getAttribute("id");
            _selectedIndex = this.rownum || this.getAttribute("rowIndex");

            //行记录单击后触发的事件
            if (_config.itemClick) {
                eval(_config.itemClick + "(_selectedId, _selectedIndex, str2json(_selectedData))");
            }
        });

        //展开、关闭下级节点
        //对于属性操作：1.6.2之前用attr，1.6.2开始用prop
        _root.find("img[folder='Y']").bind("click", function () {
            var trid = this.trid || this.getAttribute("trid"); //html属性值获取方式
            var isOpen = $("#" + trid).prop("open");
            isOpen = (isOpen == "Y") ? "N" : "Y";
            _root.find("#" + trid).prop("open", isOpen);
            showHiddenNode(trid, isOpen);
        });
    }

    //显示或隐藏子节点数据
    function showHiddenNode(_trid, _open) {
        if (_open == "N") { //隐藏子节点
            _root.find("#" + _trid).find("img[folder='Y']").prop("src", _config.folderCloseIcon);
            _root.find("tr[id^=" + _trid + "_]").css("display", "none");
        } else { //显示子节点
            _root.find("#" + _trid).find("img[folder='Y']").prop("src", _config.folderOpenIcon);
            showSubs(_trid);
        }
    }

    //递归检查下一级节点是否需要显示
    function showSubs(_trid) {
        var isOpen = _root.find("#" + _trid).prop("open");
        if (isOpen == "Y") {
            var trs = _root.find("tr[pid=" + _trid + "]");
            trs.css("display", "");

            for (var i = 0; i < trs.length; i++) {
                showSubs(trs[i].id);
            }
        }
    }

    //数据行对象
    function TreeGridListItem(_root, _rowId, _rowIndex, _rowData) {
        var _root = _root;

        this.id = _rowId;
        this.index = _rowIndex;
        this.data = _rowData;

        this.getParent = function () {
            var pid = $("#" + this.id).prop("pid");
            if (pid != "") {
                var rowIndex = $("#" + pid).prop("rowIndex");
                var data = $("#" + pid).prop("data");
                return new TreeGridListItem(_root, pid, rowIndex, str2json(data));
            }
            return null;
        }

        this.getChildren = function () {
            var arr = [];
            var trs = $(_root).find("tr[pid='" + this.id + "']");
            for (var i = 0; i < trs.length; i++) {
                var tr = trs[i];
                arr.push(new TreeGridListItem(_root, tr.id, tr.rowIndex, str2json(tr.data)));
            }
            return arr;
        }
    };




    //将json对象转换成字符串
    function json2str(obj) {
        var arr = [];
        var fmt = function (s) {
            if (typeof s == 'object' && s != null) {
                if (s.length) {
                    var _substr = "";
                    for (var x = 0; x < s.length; x++) {
                        if (x > 0) _substr += ", ";
                        _substr += json2str(s[x]);
                    }
                    return "[" + _substr + "]";
                } else {
                    return json2str(s);
                }
            }
            return /^(string|number)$/.test(typeof s) ? "'" + s + "'" : s;
        }

        for (var i in obj) {
            if (typeof obj[i] != 'object') { //暂时不包括子数据
                arr.push(i + ":" + fmt(obj[i]));
            }
        }

        return '{' + arr.join(', ') + '}';
    }

    function str2json(s) {
        var json = null;
        if (/msie/.test(navigator.userAgent.toLowerCase())) {
            json = eval("(" + s + ")");
        } else {
            json = new Function("return " + s)();
        }
        return json;
    }




    var TreeGridList = new _TreeGridList();
    exports('TreeGridList', TreeGridList);
});
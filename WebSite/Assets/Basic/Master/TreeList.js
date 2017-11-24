/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 树型列表页
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "layer", "qian"], function (exports) {
    var $ = layui.jquery;
    var qian = layui.qian;
    var layer = layui.layer;

    var _this;

    function _TreeList() {
        _this = this;
    }


    var mf = "rightFrame";
    

    //当前页面重新适配
    _TreeList.prototype.resize = function () {
        $('#treeArea').css('height', getHeight());
        $('#rightFrame').css('height', getHeight());
    };

    _TreeList.prototype.init = function () {
        $(window).resize(function () {
            _this.resize();
        });
        loadTree();
    }

    _TreeList.prototype.settings = function () {
        return null;
    }

    _TreeList.prototype.changeTreeArea = function () {
        var ishide = $('#treeArea').is(":hidden");
        if (!ishide) {
            $('#treeArea').hide(300);
            $('.contentArea').removeClass("layui-col-md9 layui-col-sm9 layui-col-xs9 layui-col-lg9");
            $('.contentArea').addClass("layui-col-md12 layui-col-sm12 layui-col-xs12 layui-col-lg12");

            $("#col-span-text").html("显示菜单");
        } else {
            $('#treeArea').show(300);
            $('.contentArea').removeClass("layui-col-md12 layui-col-sm12 layui-col-xs12 layui-col-lg12");
            $('.contentArea').addClass("layui-col-md9 layui-col-sm9 layui-col-xs9 layui-col-lg9");
            $("#col-span-text").html("隐藏菜单");

        }

    }

    //当前树对象
    _TreeList.prototype.curTree = null;

    //添加节点
    _TreeList.prototype.addNode = function (data) {
        var tree = _this.curTree;
        if (tree) {
            var perNode = null;
            if (data.pId == "0") {//顶层虚拟菜单
                data.pId = "root";
                
            }
            perNode = tree.getNodeByParam("id", data.pId);
            tree.addNodes(perNode, data);
        }
    }

    //更新节点
    _TreeList.prototype.updateNode = function (data) {
        var tree = _this.curTree;
        if (tree) {
            var node = tree.getNodeByParam("id", data.id);
            if (node) {
                tree.updateNode($.extend(node, data));
            }
        }
    }

    //移除节点
    _TreeList.prototype.removeNode = function (id) {
        var tree = _this.curTree;
        if (tree) {
            tree.removeNode(tree.getNodeByParam("id", id));
        }
    }

    //加载树型菜单
    function loadTree() {
        var settings = _this.settings();
        if (!settings) {
            qian.tips("参数配置错误");
            return false;
        }
        qian.ajax({
            url: settings.loadUrl,
            type: 'post',
            success: function (res) {
                _this.curTree = $.fn.zTree.init($("#menutree"), {
                    data: {
                        simpleData: {
                            enable: true,  //开启简单数据模式(Array)  
                            idKey: "id",
                            pIdKey: "pId",
                            rootPId: 0
                        },
                        key: {
                            title: "title"
                        }
                    },
                    callback: {
                        onClick: function (event, treeId, treeNode) {
                            if (treeNode.id == "root") {
                                $("#rightFrame").attr("src", "");
                                return false;
                            }
                            $("#rightFrame").attr("src", settings.editUrl + "=" + treeNode.id)
                        },
                        beforeRemove: function (treeId, treeNode) {
                            qian.confirm("确定要删除吗?", function () {
                                qian.ajax({
                                    url: settings.delUrl + '=' + treeNode.id,
                                    type: 'post',
                                    async: false,
                                    success: function (res) {
                                        _this.removeNode(treeNode.id);
                                        $("#rightFrame").attr("src", "");
                                    }
                                });
                            });
                            return false;
                        },
                        beforeEditName: function (treeId, treeNode) {
                            return false;
                        }
                    },
                    edit: {
                        enable: true,
                        showRemoveBtn: function (treeId, treeNode) {
                            return treeNode.id != "root";
                        },
                        showRenameBtn: false
                    },
                    view: {
                        addHoverDom: function (treeId, treeNode) {
                            var sObj = $("#" + treeNode.tId + "_span");
                            if (treeNode.editNameFlag || $("#addBtn_" + treeNode.tId).length > 0) return;
                            var addStr = "<span class='button add' id='addBtn_" + treeNode.tId
                                + "' title='添加子菜单' onfocus='this.blur();'></span>";
                            sObj.after(addStr);
                            var btn = $("#addBtn_" + treeNode.tId);
                            if (btn) btn.bind("click", function () {
                                $("#rightFrame").attr("src", settings.addUrl + '=' + treeNode.id);
                                return false;
                            });
                        },
                        removeHoverDom: function (treeId, treeNode) {
                            $("#addBtn_" + treeNode.tId).unbind().remove();
                        }
                    }
                }, res.Data);
            },
            complete: function (xhr, des) {
                _this.resize();
                $(".col-span-btn").click(function () {
                    _this.changeTreeArea()
                });
            }
        });
    }
    

    //获取页面高度
    function getHeight(pix) {
        return ($(".layui-tab-content", parent.document).height());
    }

    var TreeList = new _TreeList();
    exports('TreeList', TreeList);
});
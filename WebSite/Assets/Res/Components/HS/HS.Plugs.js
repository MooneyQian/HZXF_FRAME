var HS = {
    UI: {
        ///组织机构下拉框
        ///option:下拉框参数{width:宽度(默认180),selectBoxWidth:下拉框宽度(默认200),selectBoxHeight:下拉框高度(默认200)}
        ///url:获取数据地址(默认/Common/_GetOrganTree)
        OrganDropdownList: function (obj, option, url) {
            //debugger;
            var opt = {
                width: 180,
                selectBoxWidth: 200,
                selectBoxHeight: 200,
                resize: true,
                valueField: "id",
                textField: "text",
                valueFieldID: obj.attr("id"),
                treeLeafOnly: false,
                initValue: obj.val() || "",
                initText: "--请选择--",
                tree: {
                    url: url || "/Common/_GetOrganTree",
                    checkbox: false,
                    parentIDFieldName: "pid",
                    ajaxType: "get"
                }
            };
            if (option) opt = $.extend(opt, option);
            obj.ligerComboBox(opt);
            $(obj).attr("id", obj.attr("id") + "_name");
            $(obj).attr("name", obj.attr("id"));
        },
        ///树形字典表下拉框
        ///dicttype:字典类型
        ///option:下拉框参数,参照Ligerui设置{width:宽度(默认180),selectBoxWidth:下拉框宽度(默认200),selectBoxHeight:下拉框高度(默认200),treeLeafOnly:是否只可选子节点,onSelected:选中事件}
        DictTreeDropdownList: function (obj, dicttype, option) {
            var id = obj.attr("id");
            $(obj).attr("id", obj.attr("id") + "_name");
            $(obj).attr("name", obj.attr("id"));
            //debugger;
            option = $.extend({
                width: 180,
                selectBoxWidth: 200,
                selectBoxHeight: 200,
                resize: true,
                cancelable: false,
                valueField: "id",
                textField: "name",
                valueFieldID: id,
                treeLeafOnly: true,
                initValue: obj.val() || "",
                initText: "--请选择--",
                tree: {
                    url: "/Common/_GetDictionaryTree?DictType=" + dicttype,
                    checkbox: false,
                    parentIDFieldName: "pId",
                    textFieldName: "name",
                    ajaxType: "get"
                },
                onSelected: function (val, text) {
                    //显示数的父节点名称
                    var tree = this.getTree();
                    if (val && tree) {
                        var txt = text;
                        var node = tree.getDataByID(val);
                        if (node) {
                            var parNode = node;
                            do {
                                parNode = tree.getDataByID(parNode.pId);
                                if (parNode)
                                    txt = parNode.name + "-" + txt;
                            } while (parNode);
                            this.setText(txt);
                        }
                    }
                }
            }, option || {});

            var dp = obj.ligerComboBox(option);
            return dp;
        },
        ///状态开关
        ///obj:jQuery对象
        ///on:表示开的值(默认1)
        ///off:表示关的值(默认0)
        ///开关事件function(state){state:开关状态(bool类型)}
        OnOffBtn: function (obj, on, off, fn) {
            if ($.isFunction(on)) { fn = on; on = null; }
            if (obj.val() == "") obj.val(on || "1");
            var div = $("<div>");
            if (obj.val() == (off || "0"))
                div.addClass("onoffbtn_off");
            else
                div.addClass("onoffbtn_on");
            div.insertBefore(obj);
            obj.hide();
            div.click(function () {
                if ($(this).hasClass("onoffbtn_on")) {
                    $(this).removeClass("onoffbtn_on");
                    $(this).addClass("onoffbtn_off");
                    obj.val(off || 0);
                } else {
                    $(this).removeClass("onoffbtn_off");
                    $(this).addClass("onoffbtn_on");
                    obj.val(on || 1);
                }
                if (fn) fn($(this).hasClass("onoffbtn_on"));
            });
            //初始化状态
            if (fn) fn(obj.val() == (on || "1"));
        }
    },
    Cookies: (function ()
    {
        var fn = function (){};
        fn.prototype.get = function (name)
        {
            var cookieValue = "";
            var search = name + "=";
            if (document.cookie.length > 0)
            {
                offset = document.cookie.indexOf(search);
                if (offset != -1)
                {
                    offset += search.length;
                    end = document.cookie.indexOf(";", offset);
                    if (end == -1) end = document.cookie.length;
                    cookieValue = decodeURIComponent(document.cookie.substring(offset, end))
                }
            }
            return cookieValue;
        };
        fn.prototype.set = function (cookieName, cookieValue, DayValue)
        {
            var expire = "";
            var day_value = 1;
            if (DayValue != null)
            {
                day_value = DayValue;
            }
            expire = new Date((new Date()).getTime() + day_value * 86400000);
            expire = "; expires=" + expire.toGMTString();
            document.cookie = cookieName + "=" + encodeURIComponent(cookieValue) + ";path=/" + expire;
        }
        fn.prototype.remvoe = function (cookieName)
        {
            var expire = "";
            expire = new Date((new Date()).getTime() - 1);
            expire = "; expires=" + expire.toGMTString();
            document.cookie = cookieName + "=" + escape("") + ";path=/" + expire;
            /*path=/*/
        };

        return new fn();
    })()
};
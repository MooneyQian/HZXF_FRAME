/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 通用详情页
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "element", "layer", "util", "qian"], function (exports) {
    var util = layui.util;
    var layer = layui.layer;
    var qian = layui.qian;
    var $ = layui.jquery;

    function _Show() { }

    _Show.prototype.init = function () {
        $(document).ready(function () {
            //返回顶部
            util.fixbar();
            var index = qian.getQueryString("index");
            $("#base-return").click(function () {
                parent.layer.close(index);
            });
        });
    }


    var Show = new _Show();
    exports('Show', Show);
});
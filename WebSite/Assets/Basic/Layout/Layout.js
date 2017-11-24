/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 布局类
 * +----------------------------------------------------------------------
 */
layui.define(["jquery"], function (exports) {
    var $ = layui.jquery;

    var _this;

    function _Layout() {
        _this = this;
    };


    //布局初始化
    _Layout.prototype.init = function (funcs, tabFunc) {
        $(document).ready(function () {
            _this.customerInit();

            window.global = {
                preview: function () {
                    var preview = document.getElementById('LAY_preview');
                    return preview ? preview.innerHTML : '';
                }()
            };
           
            $(window).resize(function () {
                _this.resize();
            });
        });
    }

    //获取页面高度
    _Layout.prototype.getHeight = function (pix) {
        if (pix) {
            return ($(window).height() - _this.minus - _this.content_minus);
        }
        return ($(window).height() - _this.minus);
    }

    //当前页面重新适配
    _Layout.prototype.resize = function () { };

    _Layout.prototype.customerInit = function () { }
   
    _Layout.prototype.minus = 160;

    _Layout.prototype.content_minus = 20;
    
    
    var Layout = new _Layout();
    exports('Layout', Layout);
});
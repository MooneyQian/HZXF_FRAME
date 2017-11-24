/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 密码设置编辑模块
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "qian", "Add"], function (exports) {
    var $ = layui.jquery;
    var qian = layui.qian;
    var Add = layui.Add;

    var _this;

    function _Account_Pass() {
        _this = this;
    };

    //页面初始化
    _Account_Pass.prototype.init = function () {
        Add.init();
    }

    /************************** 重写父类方法 start **************************/

    //表单校验规则
    Add.checkForm = function () {
        return {
            'q-confirmpass': function (v, item) {//密码确认
                if (v != $("#NewPassword").val()) {
                    return '两次密码输入不一致';
                }
            }
        }
    }
    

    /************************** 重写父类方法 end **************************/


    var Account_Pass = new _Account_Pass();
    exports('AccountPass', Account_Pass);
});
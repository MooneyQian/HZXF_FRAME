/*
 * @Author: ying.qian
 * @Date:   2017-09-26
 * @lastModify 2017-09-26
 * +----------------------------------------------------------------------
 * | Description: 资源帮助类
 * +----------------------------------------------------------------------
 */
layui.define(["jquery", "qian", "upload"], function (exports) {

    var _this;
    var soundRes;
    var skinRes;

    var upload = layui.upload;
    var qian = layui.qian;

    function _res() {
        _this = this;
        soundRes = this.obtainSoundRes();
        skinRes = this.obtainSkinRes();
    }

    //获取声音资源
    _res.prototype.obtainSoundRes = function () {
        if (!soundRes) {
            soundRes = [{
                name: '清脆',
                key: '1',
                src: '/Assets/Res/Sound/tips.mp3'
            }, {
                name: '播报',
                key: '2',
                src: '/Assets/Res/Sound/msg.wav'
            }];
        }
        return soundRes;
    }

    //根据关键字获得声音资源
    _res.prototype.getSoundResByKey = function (key) {
        var result = {};
        if (!key) {
            key = "1";
        }
        for (var i = 0; i < soundRes.length; i++) {
            var tmp = soundRes[i];
            if (tmp.key == key) {
                result = tmp;
                break;
            }
        }
        return result;
    }

    //播放声音
    _res.prototype.playSound = function (key) {
        var au = _this.getSoundResByKey(key).src;
        if (au) {
            var borswer = window.navigator.userAgent.toLowerCase();
            if (borswer.indexOf("ie") >= 0) {
                //IE内核浏览器
                var strEmbed = '<embed name="embedPlay" src="' + au + '" autostart="true" hidden="true" loop="false"></embed>';
                $("body").find("embed").remove();
                $("body").append(strEmbed);
                var embed = document.embedPlay;

                //浏览器不支持 audion，则使用 embed 播放
                embed.volume = 100;
                //embed.play();这个不需要
            } else {
                //非IE内核浏览器
                var strAudio = "<audio id='audioPlay' src='" + au + "' hidden='true'>";
                $("body").find("audio").remove();
                $("body").append(strAudio);
                var audio = document.getElementById("audioPlay");
                //浏览器支持 audion
                audio.play();
            }
        }
    }

    //获取皮肤资源
    _res.prototype.obtainSkinRes = function () {
        if (!skinRes) {
            skinRes = [{
                name: '经典皮肤1',
                key: '0',
                smallpath: '/Assets/Res/Images/skin/small-bg.jpg',
                bigpath: '/Assets/Res/Images/skin/bg.jpg'
            }, {
                name: '经典皮肤2',
                key: '2',
                smallpath: '/Assets/Res/Images/skin/small-bg2.jpg',
                bigpath: '/Assets/Res/Images/skin/bg2.jpg'
            }, {
                name: '经典皮肤3',
                key: '5',
                smallpath: '/Assets/Res/Images/skin/small-bg5.jpg',
                bigpath: '/Assets/Res/Images/skin/bg5.jpg'
            }, {
                name: '经典皮肤4',
                key: '8',
                smallpath: '/Assets/Res/Images/skin/small-bg8.jpg',
                bigpath: '/Assets/Res/Images/skin/bg8.jpg'
            }, {
                name: '经典皮肤5',
                key: '9',
                smallpath: '/Assets/Res/Images/skin/small-bg9.jpg',
                bigpath: '/Assets/Res/Images/skin/bg9.jpg'
            }, {
                name: '经典皮肤6',
                key: '10',
                smallpath: '/Assets/Res/Images/skin/small-bg10.jpg',
                bigpath: '/Assets/Res/Images/skin/bg10.jpg'
            }, {
                name: '经典皮肤7',
                key: '11',
                smallpath: '/Assets/Res/Images/skin/small-bg11.jpg',
                bigpath: '/Assets/Res/Images/skin/bg11.jpg'
            }, {
                name: '经典皮肤8',
                key: '12',
                smallpath: '/Assets/Res/Images/skin/small-bg12.jpg',
                bigpath: '/Assets/Res/Images/skin/bg12.jpg'
            }, {
                name: '经典皮肤9',
                key: '13',
                smallpath: '/Assets/Res/Images/skin/small-bg13.jpg',
                bigpath: '/Assets/Res/Images/skin/bg13.jpg'
            }];
        }
        return skinRes;
    }

    //更换皮肤
    _res.prototype.changeSkinByPath = function (path) {
        changeSkin(path);
    }

    //更换皮肤
    _res.prototype.changeSkinByKey = function (key, needparent) {
        var src = _this.getSkinResByKey(key).bigpath;
        changeSkin(src, needparent);
    }

    //根据关键字获得皮肤资源
    _res.prototype.getSkinResByKey = function (key) {
        var result = {};
        if (!key) {
            key = "0";
        }
        for (var i = 0; i < skinRes.length; i++) {
            var tmp = skinRes[i];
            if (tmp.key == key) {
                result = tmp;
                break;
            }
        }
        return result;
    }

    //上传组件
    _res.prototype.upload = function (opts) {
        var params = $.extend({
            elem: '',	//指向容器选择器，如：elem: '#id'。也可以是DOM对象	string/object	-
            url: '/Assets/Res/Components/Uploadify/net/UploadHandler.ashx',	//服务端上传接口，返回的数据规范请详见下文	string	-
            method: 'post',	//上传接口的 HTTP 类型	string	post
            data: {
                fileType:'images'
            },    //请求上传接口的额外参数	object	-
            accept: 'images',  //指定允许上传的文件类型，可选值有：images（图片）、file（所有文件）、video（视频）、audio（音频）	string	images
            exts: 'jpg|png|gif|bmp|jpeg',	//允许上传的文件后缀。一般结合 accept 参数类设定。假设 accept 为 file 类型时，那么你设置 exts: 'zip|rar|7z' 即代表只允许上传压缩格式的文件。如果 accept 未设定，那么限制的就是图片的文件格式	string	jpg|png|gif|bmp|jpeg
            auto: true,	//是否选完文件后自动上传。如果设定 false，那么需要设置 bindAction 参数来指向一个其它按钮提交上传	boolean	true
            bindAction: '',	//指向一个按钮触发上传，一般配合 auto: false 来使用。值为选择器或DOM对象，如：bindAction: '#btn'	string/object	-
            field: 'Filedata',	//设定文件域的字段名	string	file
            size: 0,	//设置文件最大可允许上传的大小，单位 KB。不支持ie8/9	number	0（即不限制）
            multiple: false,	//是否允许多文件上传。设置 true即可开启。不支持ie8/9	boolean	false
            drag: true,	//是否接受拖拽的文件上传，设置 false 可禁用。不支持ie8/9	boolean	true
            choose: function (obj) {
                previewFile(this, obj);
            }, //选择文件后的回调函数。返回一个object参数，详见下文	function	-
            before: function (obj) {
                //layer.load("1"); //上传loading
            }, //文件提交上传前的回调。返回一个object参数（同上），详见下文	function	-
            done: function (res, index, upload) {

            }, //执行上传请求后的回调。返回三个参数，分别为：res（服务端响应信息）、index（当前文件的索引）、upload（重新上传的方法，一般在文件上传失败后使用）。详见下文	function	-
            error: function (index, upload) {

            } //执行上传请求出现异常的回调（一般为网络异常、URL 404等）。返回两个参数，分别为：index（当前文件的索引）、upload（重新上传的方法）。详见下文	function	-
        }, opts);

        upload.render(params);
    }

    //文件预览
    function previewFile(that, fileobj) {
        var files = fileobj.pushFile();

        if ($("#photo-upload-list").length == 0) {
            $(that.elem).after('<div class="photo-upload-list" id="photo-upload-list">');
        }
        fileobj.preview(function (index, file, result) {
            if (!that.multiple) {
                $("#photo-upload-list").html("");
                files = {};
                files[index] = file;
            }

            if ($('#photo-upload-item' + index).length == 0) {
                $('#photo-upload-list').append('<div class="photo-upload-item" id="photo-upload-item' + index + '">');
                $('#photo-upload-item' + index).append('<div class="tools" id="tools' + index + '" style="display:none">');
                $('#tools' + index).append('<i class="icon-trash" id="toolsicondel' + index + '"></i>');
                if (that.accept == "images") {//图片预览
                    $('#photo-upload-item' + index).append('<img src="' + result + '" alt="' + file.name + '" id="' + index + '">');
                    $('#tools' + index).append('<i class="icon-zoom-in" id="toolsiconbig' + index + '"></i>');

                    $('#toolsiconbig' + index).click(function () {
                        qian.showImg({
                            single: true,
                            src: result
                        });
                    });
                } else {//其他文件

                }
                

                $('#photo-upload-item' + index).mouseenter(function () {
                    $('#tools' + index).show();
                });

                $('#photo-upload-item' + index).mouseleave(function () {
                    $('#tools' + index).hide();
                });

                $('#toolsicondel' + index).click(function () {
                    $('#photo-upload-item' + index).remove();
                    delete files[index];
                    if ($.isEmptyObject(files)) {
                        $("#photo-upload-list").remove();
                    }
                });
            }
        });
    }

    //更换皮肤
    function changeSkin(path, needparent) {
        if (needparent) {
            parent.document.getElementsByClassName("qui-body-background")[0].style.background = 'url(' + path + ') no-repeat top center';
        } else {
            $(".qui-body-background").css("background", 'url(' + path + ') no-repeat top center');
        }
    }

    var res = new _res();
    exports("res", res);
});
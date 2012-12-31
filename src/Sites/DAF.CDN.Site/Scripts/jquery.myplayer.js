var swfPlayerPath = "/Content/";

(function ($) {
    $.widget('daf.audio', {
        options: {
            title: null,
            mediaFile: null,
            width: "205px",
            height: "110px",
            visible: true,
            cls: "",
            mediaTypes: "mp3,m4a,oga",
            swfPath: swfPlayerPath
        },
        _create: function () {
            var self = this,
                ops = self.options,
                ele = self.element;

            var playerId = $.now();
            var audioEle = $('<div id="audio_' + playerId + '" class="cp-jplayer"></div>');
            $(ele).append(audioEle);
            self.audioEle = audioEle;

            if (ops.visible) {
                var playerEle = $('\
<div id="audioplayer_' + playerId + '" class="jp-audio">\
    <div class="jp-type-single">\
        <div class="jp-gui jp-interface">\
            <ul class="jp-controls">\
                <li><a href="javascript:;" class="jp-play" tabindex="1">播放</a></li>\
                <li><a href="javascript:;" class="jp-pause" tabindex="1">暂停</a></li>\
                <li><a href="javascript:;" class="jp-stop" tabindex="1">停止</a></li>\
                <li><a href="javascript:;" class="jp-mute" tabindex="1" title="静音">静音</a></li>\
                <li><a href="javascript:;" class="jp-unmute" tabindex="1" title="播音">播音</a></li>\
                <li><a href="javascript:;" class="jp-volume-max" tabindex="1" title="最大音量">最大音量</a></li>\
            </ul>\
            <div class="jp-progress">\
                <div class="jp-seek-bar">\
                    <div class="jp-play-bar"></div>\
                </div>\
            </div>\
            <div class="jp-volume-bar">\
                <div class="jp-volume-bar-value"></div>\
            </div>\
            <div class="jp-current-time"></div>\
            <div class="jp-duration"></div>\
            <ul class="jp-toggles">\
                <li><a href="javascript:;" class="jp-repeat" tabindex="1" title="重复播放">重复播放</a></li>\
                <li><a href="javascript:;" class="jp-repeat-off" tabindex="1" title="播放一次">播放一次</a></li>\
            </ul>\
        </div>\
        <div class="jp-title">\
            <ul>\
                <li></li>\
            </ul>\
        </div>\
        <div class="jp-no-solution">\
            <span>当前浏览器不支持，需要更新才可能播放</span>\
            你需要更新你的浏览器或下载最新的<a href="http://get.adobe.com/flashplayer/" target="_blank">Flash插件</a>，才可以正常播放。 \
        </div>\
    </div>\
</div>');
                $(ele).append(playerEle);
                self.playerEle = playerEle;

                var audioOps = {
                    swfPath: ops.swfPath,
                    supplied: ops.mediaTypes,
                    cssSelectorAncestor: '#audioplayer_' + playerId,
                    size: { width: ops.width, height: ops.height, cssClass: ops.cls },
                    ready: function (event) {
                        self.setMedia();
                    },
                    play: function (event) {
                        $(this).jPlayer("pauseOthers");
                    }
                };
                $(audioEle).jPlayer(audioOps);

                $(playerEle).dialog({
                    title: ops.title,
                    width: parseInt(ops.width) + 16,
                    autoOpen: false,
                    open: function () {
                        $(self.audioEle).jPlayer("play");
                    },
                    close: function () {
                        if (self.options.visible) {
                            $(self.audioEle).jPlayer("stop");
                        }
                    }
                });
            }
        },
        _init: function () {
        },
        _setOption: function (option, value) {
            $.Widget.prototype._setOption.apply(this, arguments);
            if (option == "mediaFile" || option == "mediaTypes") {
                this.setMedia();
            }
            if (option == "title") {
                this.setTitle();
            }
            if (option == "visible") {
                if (this.playerEle) {
                    $(this.playerEle).dialog('close');
                }
            }
        },
        setTitle: function (title) {
            var ops = this.options;
            if (title) {
                ops.title = title;
            }
            $(this.playerEle).dialog('option', 'title', ops.title);
            $(this.playerEle).find('div.jp-title>ul>li:first').html(ops.title);
        },
        setMedia: function (file, types) {
            var ops = this.options;
            if (file) {
                ops.mediaFile = file;
            }
            if (types) {
                ops.mediaTypes = types;
            }
            if (ops.mediaFile && ops.mediaFile.length > 0 && (!ops.title || ops.title.length <= 0)) {
                var idx = ops.mediaFile.lastIndexOf("/");
                ops.title = idx <= 0 ? ops.mediaFile : ops.mediaFile.substr(idx + 1);
                this.setTitle();
            }
            if (ops.mediaFile && ops.mediaTypes && ops.mediaFile.length > 0 && ops.mediaTypes.length > 0) {
                var mediaUrls = {};
                $.each(ops.mediaTypes.split(','), function (i, n) {
                    eval('mediaUrls.' + n + ' = ops.mediaFile + ".' + n + '";');
                });
                $(this.audioEle).jPlayer("stop");
                $(this.audioEle).jPlayer("setMedia", mediaUrls);
            }
        },
        play: function () {
            if (this.options.visible) {
                $(this.playerEle).dialog('open');
            } else {
                $(this.audioEle).jPlayer("play");
            }
        },
        stop: function() {
            if (this.options.visible) {
                $(this.playerEle).dialog('close');
            } else {
                $(this.audioEle).jPlayer("stop");
            }
        },
        destroy: function () {
            $.Widget.prototype.destroy.call(this);
        }
    });
})(jQuery);

(function ($) {
    $.widget('daf.video', {
        options: {
            title: null,
            mediaFile: null,
            width: "640px",
            height: "360px",
            cls: "jp-video-360p",
            mediaTypes: "webmv,ogv,m4v",
            swfPath: swfPlayerPath
        },
        _create: function () {
            var self = this,
                ops = self.options,
                ele = self.element;
            var playerId = $.now();
            var playerEle = $('\
<div id="videoplayer_' + playerId + '" class="jp-video">\
    <div class="jp-type-single">\
        <div id="video_' + playerId + '" class="jp-jplayer"></div>\
        <div class="jp-gui">\
            <div class="jp-video-play">\
                <a href="javascript:;" class="jp-video-play-icon" tabindex="1">播放</a>\
            </div>\
            <div class="jp-interface">\
                <div class="jp-progress">\
                    <div class="jp-seek-bar">\
                        <div class="jp-play-bar"></div>\
                    </div>\
                </div>\
                <div class="jp-current-time"></div>\
                <div class="jp-duration"></div>\
                <div class="jp-title">\
                    <ul>\
                        <li></li>\
                    </ul>\
                </div>\
                <div class="jp-controls-holder">\
                    <ul class="jp-controls">\
                        <li><a href="javascript:;" class="jp-play" tabindex="1">播放</a></li>\
                        <li><a href="javascript:;" class="jp-pause" tabindex="1">暂停</a></li>\
                        <li><a href="javascript:;" class="jp-stop" tabindex="1">停止</a></li>\
                        <li><a href="javascript:;" class="jp-mute" tabindex="1" title="静音">静音</a></li>\
                        <li><a href="javascript:;" class="jp-unmute" tabindex="1" title="播音">播音</a></li>\
                        <li><a href="javascript:;" class="jp-volume-max" tabindex="1" title="最大音量">最大音量</a></li>\
                    </ul>\
                    <div class="jp-volume-bar">\
                        <div class="jp-volume-bar-value"></div>\
                    </div>\
                    <ul class="jp-toggles">\
                        <li><a href="javascript:;" class="jp-full-screen" tabindex="1" title="全屏">全屏</a></li>\
                        <li><a href="javascript:;" class="jp-restore-screen" tabindex="1" title="原屏">原屏</a></li>\
                        <li><a href="javascript:;" class="jp-repeat" tabindex="1" title="重复播放">重复播放</a></li>\
                        <li><a href="javascript:;" class="jp-repeat-off" tabindex="1" title="播放一次">播放一次</a></li>\
                    </ul>\
                </div>\
            </div>\
        </div>\
        <div class="jp-no-solution">\
            <span>当前浏览器不支持，需要更新才可能播放</span>\
            你需要更新你的浏览器或下载最新的<a href="http://get.adobe.com/flashplayer/" target="_blank">Flash插件</a>，才可以正常播放。 \
        </div>\
    </div>\
</div>');
            $(ele).append(playerEle);
            self.playerEle = playerEle;

            var videoOps = {
                swfPath: ops.swfPath,
                supplied: ops.mediaTypes,
                cssSelectorAncestor: '#videoplayer_' + playerId,
                size: { width: ops.width, height: ops.height, cssClass: ops.cls },
                ready: function (event) {
                    self.setMedia();
                },
                play: function (event) {
                    $(this).jPlayer("pauseOthers");
                }
            };

            var videoEle = $('#video_' + playerId);
            self.videoEle = videoEle;

            videoEle.jPlayer(videoOps);
            $(playerEle).dialog({
                title: self.title,
                width: parseInt(ops.width) + 16,
                autoOpen: false,
                open: function () {
                    $(self.videoEle).jPlayer("play");
                },
                close: function () {
                    $(self.videoEle).jPlayer("stop");
                }
            });
        },
        _init: function () {
        },
        _setOption: function (option, value) {
            $.Widget.prototype._setOption.apply(this, arguments);
            if (option == "mediaFile" || option == "mediaTypes") {
                this.setMedia();
            }
            if (option == "title") {
                this.setTitle();
            }
        },
        setTitle: function (title) {
            var ops = this.options;
            if (title) {
                ops.title = title;
            }
            $(this.playerEle).dialog('option', 'title', ops.title);
            $(this.playerEle).find('div.jp-title>ul>li:first').html(ops.title);
        },
        setMedia: function (file, types) {
            var ops = this.options;
            if (file) {
                ops.mediaFile = file;
            }
            if (types) {
                ops.mediaTypes = types;
            }
            if (ops.mediaFile && ops.mediaFile.length > 0 && (!ops.title || ops.title.length <= 0)) {
                var idx = ops.mediaFile.lastIndexOf("/");
                ops.title = idx <= 0 ? ops.mediaFile : ops.mediaFile.substr(idx + 1);
                this.setTitle();
            }
            if (ops.mediaFile && ops.mediaTypes && ops.mediaFile.length > 0 && ops.mediaTypes.length > 0) {
                var mediaUrls = {};
                $.each(ops.mediaTypes.split(','), function (i, n) {
                    eval('mediaUrls.' + n + ' = ops.mediaFile + ".' + n + '";');
                });
                $(this.videoEle).jPlayer("stop");
                $(this.videoEle).jPlayer("setMedia", mediaUrls);
            }
        },
        play: function () {
            $(this.playerEle).dialog('open');
        },
        stop: function () {
            $(this.playerEle).dialog('close');
        },
        destroy: function () {
            $.Widget.prototype.destroy.call(this);
        }
    });
})(jQuery);

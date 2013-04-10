/*
options = {
        maxFileSize: 5 * 1024 * 1024,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
        maxFiles: -1,
        autoStart: false,
        baseUrl: '',
        owner:'',
        property:''

        add: function(file) {},
        uploaded: function(file){},
        canceled: function(file){},
        deleted: function(file){},
        progress: function(file){},
        error: function(error){}
};
*/

function FUVM(uploadFormId, options) {
    var self = this;
    self.options = $.extend({}, {
        maxFileSize: 5 * 1024 * 1024,
        acceptFileTypes: /.+$/i,
        maxFiles: -1,
        autoStart: false
    }, options);

    self.UploadForm = null;
    self.Files = ko.observableArray([]);
    self.Uploader = function () {
        return self.UploadForm.data('fileupload');
    };
    self.GetFile = function (fname) {
        var fs = $.grep(self.Files(), function (n, i) { return n.name() == fname; });
        if (fs.length > 0)
            return fs[0];
        return null;
    };
    self.AddFile = function (data, file, path) {
        var f = self.GetFile(file.name);
        if (f)
            return;
        f = {
            group: ko.observable(null),
            name: ko.observable(file.name),
            type: ko.observable(file.type),
            size: ko.observable(file.size),
            progress: ko.observable(0),
            local_url: ko.observable(path),
            url: ko.observable(null),
            thumbnail_url: ko.observable(null),
            delete_url: ko.observable(null),
            delete_type: ko.observable(null),
            error: ko.observable(null)
        };
        f.data = data;
        self.Files.push(f);
        if (self.options.add) {
            self.options.add(f);
        }
    };
    self.CanAddFile = function () {
        if (self.options.maxFiles < 0)
            return true;
        return self.Files().length < self.options.maxFiles;
    };
    self.Uploading = function (item) {
        return item && item.progress() > 0 && item.progress() < 100;
    };
    self.Uploaded = function (item) {
        return item && item.progress() >= 100;
    };

    self.CancelFile = function (item) {
        var data = item.data;
        var uploader = self.Uploader();
        if (uploader && data) {
            if (!data.jqXHR) {
                data.errorThrown = 'abort';
                uploader._trigger('fail', null, data);
            } else {
                data.jqXHR.abort();
            }
        }
        if (self.options.canceled) {
            self.options.canceled(item);
        }
    };

    self.RemoveFile = function (item) {
        var uploader = self.Uploader();
        if (uploader) {
            if (self.Uploaded(item)) {
                var data = {
                    url: item.delete_url() + '&action=DELETE',
                    type: 'GET', //item.delete_type() || 'DELETE',
                    dataType: uploader.options.dataType
                };
                if (data.url) {
                    $.ajax(data);
                }
                uploader._trigger('destroyed', null, data);
            }
        }
        if (self.options.deleted) {
            self.options.deleted(item);
        }
        self.Files.remove(item);
        if (typeof PostMessage == "function") {
            delete item.data;
            PostMessage("fileremoved", { owner: self.options.owner, property: self.options.property, file: ko.toJS(item) });
        }
    };

    self.StartUpload = function () {
        $.each(self.Files(), function (i, n) {
            if (!self.Uploaded(n) && n.data) {
                n.data.submit();
            }
        });
    };

    self.GetImageUrl = function (item) {
        return item.thumbnail_url(); // || "file:///" + item.local_url().replace(/\\/g, "/");
    };

    self._init = function () {
        self.UploadForm = $('#' + uploadFormId).fileupload({
            maxFileSize: self.options.maxFileSize,
            acceptFileTypes: self.options.acceptFileTypes,
            add: function (e, data) {
                var that = $(this).data('fileupload'),
                    options = that.options,
                    files = data.files;
                data.files.valid = data.isValidated = self._validate(files);
                if (data.isValidated) {
                    $(this).fileupload('process', data).done(function () {
                        self.AddFile(data, files[files.length - 1], $(e.srcElement).val());
                        if (self.options.autoStart) {
                            data.submit();
                        }
                    });
                }
                else {
                    var msg = '';
                    $.each(data.files, function (i, n) {
                        if (n.error) {
                            msg += n.error;
                        }
                    });
                    alert(msg);
                }
            },
            done: function (e, data) {
                var f = self.GetFile(data.files[0].name);
                if (f) {
                    var objs = eval(data.jqXHR.responseText);
                    var fos = $.grep(objs, function (n, i) { return n.originalName == f.name(); });
                    if (fos.length > 0) {
                        var host = self.options.baseUrl || '';
                        var fo = fos[0];
                        f.name(fo.name);
                        f.size(fo.size);
                        f.type(fo.type);
                        f.url(host + fo.url);
                        if (fo.thumbnail_url.indexOf('data:') >= 0) {
                            f.thumbnail_url(fo.thumbnail_url);
                        } else {
                            f.thumbnail_url(host + fo.thumbnail_url);
                        }
                        f.delete_url(host + fo.delete_url);
                        f.delete_type(fo.delete_type);
                        f.error(fo.error);
                    }
                    if (typeof PostMessage == "function") {
                        delete f.data;
                        PostMessage("fileuploaded", { owner: self.options.owner, property: self.options.property, file: ko.toJS(f) });
                    }
                }
            },
            progress: function (e, data) {
                var f = self.GetFile(data.files[0].name);
                if (f) {
                    var pg = parseInt(data.loaded / data.total * 100, 10);
                    f.progress(pg);
                    if (self.options.progress) {
                        self.options.progress(f);
                    }
                }
            }
        });
    };

    self._hasError = function (file) {
        if (file.error) {
            return file.error;
        }
        var uploader = self.Uploader();
        var ops = uploader.options;
        // The number of added files is subtracted from
        // maxNumberOfFiles before validation, so we check if
        // maxNumberOfFiles is below 0 (instead of below 1):
        if (ops.maxNumberOfFiles < 0) {
            return '超出最大允许文件数'; //'maxNumberOfFiles';
        }
        // Files are accepted if either the file type or the file name
        // matches against the acceptFileTypes regular expression, as
        // only browsers with support for the File API report the type:
        if (!(ops.acceptFileTypes.test(file.type) ||
                ops.acceptFileTypes.test(file.name))) {
            return '不允许的文件类型'; //'acceptFileTypes';
        }
        if (ops.maxFileSize &&
                file.size > ops.maxFileSize) {
            return '文件太大，超出限制'; //'maxFileSize';
        }
        if (typeof file.size === 'number' &&
                file.size < ops.minFileSize) {
            return '文件太小'; //'minFileSize';
        }
        return null;
    };

    self._validate = function (files) {
        var valid = !!files.length;
        $.each(files, function (index, file) {
            file.error = self._hasError(file);
            if (file.error) {
                valid = false;
            }
        });
        return valid;
    };

    self.formatFileSize = function (bytes) {
        if (typeof bytes !== 'number') {
            return '';
        }
        if (bytes >= 1000000000) {
            return (bytes / 1000000000).toFixed(2) + ' GB';
        }
        if (bytes >= 1000000) {
            return (bytes / 1000000).toFixed(2) + ' MB';
        }
        return (bytes / 1000).toFixed(2) + ' KB';
    };

    self.formatBitrate = function (bits) {
        if (typeof bits !== 'number') {
            return '';
        }
        if (bits >= 1000000000) {
            return (bits / 1000000000).toFixed(2) + ' Gbit/s';
        }
        if (bits >= 1000000) {
            return (bits / 1000000).toFixed(2) + ' Mbit/s';
        }
        if (bits >= 1000) {
            return (bits / 1000).toFixed(2) + ' kbit/s';
        }
        return bits + ' bit/s';
    };

    self.formatTime = function (seconds) {
        var date = new Date(seconds * 1000),
            days = parseInt(seconds / 86400, 10);
        days = days ? days + 'd ' : '';
        return days +
            ('0' + date.getUTCHours()).slice(-2) + ':' +
            ('0' + date.getUTCMinutes()).slice(-2) + ':' +
            ('0' + date.getUTCSeconds()).slice(-2);
    };

    self.formatPercentage = function (floatValue) {
        return (floatValue * 100).toFixed(2) + ' %';
    };


    self._init();
}

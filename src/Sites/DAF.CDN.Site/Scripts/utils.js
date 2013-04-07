var ajaxTimeOut = 50000;

//var isCrossDomain = window.parent !== window.self && (GetQueryString("cros") == "1" || GetOrigin(document.location) !== GetOrigin(window.parent.location));

$(function () {
    window.onload = function () {
        $(window).trigger('resizeWindow');
    };
    window.onresize = function () {
        $(window).trigger('resizeWindow');
    };
    window.onmessage = function (e) {
        var p = JSON.parse((e || event).data);
        $(window).trigger(p.event, [p.msg]);
    }

    OnReceiveMessage("resizeiframe", function (msg) { OnResizeIFrame(msg.frmId, msg.width, msg.height); });
    OnReceiveMessage("opendialog", function (msg) { OnOpenDialog(msg.title, msg.url, msg.width, msg.height, msg.opener); });
    OnReceiveMessage("closedialog", function (msg) { OnCloseDialog(msg.id); });
    OnReceiveMessage("showmessage", function (msg) { OnShowMessage(msg.title, msg.msg); });
    OnReceiveMessage("progress", function (msg) { OnProgress(msg.val); });
    OnReceiveMessage("overlay", function (msg) { OnOverlay(msg.enable); });
    OnReceiveMessage("reload", function (msg) { OnReload(msg); });
    ShowLocalMessage();
});

function SetAuthHeader(xhr) {
    var sid = GetCookieValue('sid');
    if (sid) {
        xhr.setRequestHeader('Authorization', "Basic " + sid);
    }
}

function GetCookieValue(name) {
    if (document.cookie && document.cookie != '') {
        var cookies = document.cookie.split(';');
        for (var i = 0; i < cookies.length; i++) {
            var cookie = jQuery.trim(cookies[i]);
            if (cookie.substr(0, name.length + 1) == (name + '=')) {
                return decodeURIComponent(cookie.substr(name.length + 1));
            }
        }
    }
    return null;
}

function OpenSelf(url) {
    if (window.top !== window.self && typeof window.top.OpenSelf == "function") {
        return window.top.OpenSelf(url);
    }
    url = FormatUrl(url);
    window.location.href = url;
}

function OpenBlank(url) {
    if (window.top !== window.self && typeof window.top.OpenBlank == "function") {
        return window.top.OpenBlank(url);
    }
    url = FormatUrl(url);
    window.open(url, '_blank');
}

function OpenIFrame(frmId, url) {
    url = FormatUrl(url);
    url = AppendUrl(url, 'frame', frmId);
    $('#' + frmId).prop('src', url);
}

function ResizeIFrame(frmId, width, height) {
    if (!frmId) {
        frmId = GetQueryString('frame');
    }
    if (!height) {
        height = $('body').height() + 22;
    } else if (height <= $('body').height() + 22) {
        return false;
    }

    PostMessage("resizeiframe", { frmId: frmId, width: width, height: height }, window.top);
    return false;
}

function OnResizeIFrame(frmId, width, height) {
    if (frmId && frmId.length > 0) {
        var frm = $('#' + frmId, (window.top || window.parent).document);
        frm.height(height);
        if (width) {
            frm.width(width);
        }
    }
    return false;
}

function OnReload(msg) {
    if (msg) {
        window.localStorage.setItem("alert", JSON.stringify(msg));
    }
    window.location.reload();
}

function ShowLocalMessage() {
    var str = window.localStorage.getItem("alert");
    if (str && str.length > 0) {
        var msg = JSON.parse(str);
        ShowMessage(msg.Title || '提示信息', msg.Message);
        window.localStorage.removeItem("alert");
    }
}

function ShowDialog(ele, title, funcOK, funcCancel, width, height) {
    var options = {
        modal: true,
        title: title,
        zIndex: 1200,
        buttons: [
            {
                "text": "OK",
                "class": "btn btn-success",
                "click": function () { if (funcOK) { if (!funcOK()) { $(this).dialog("close"); }; } else { $(this).dialog("close"); } }
            },
            {
                "text": "Cancel",
                "class": "btn",
                "click": function () { if (funcCancel) { funcCancel(); } $(this).dialog("close"); }
            }
        ]
    };

    if (width) {
        options.width = parseInt(width);
    }
    if (height) {
        options.height = parseInt(height);
        ResizeIFrame(null, null, options.height + 30);
    }

    if ($.browser.msie && $.browser.version < 8.0) {
        options.open = function (event, ui) {
            $('select').css('visibility', 'hidden');
        };
        options.close = function (event, ui) {
            $('select').css('visibility', 'visible');
        };
    }
    $(ele).dialog(options);
}

function OpenDialog(title, url, width, height, opener) {
    PostMessage("opendialog", { title: title, url: url, width: width, height: height, opener: null }, window.top);
    return false;
}

function OnOpenDialog(title, url, width, height, opener) {
    if (!opener) {
        opener = window;
    }
    if (!window.dialogs) {
        window.dialogs = [];
    }
    $(window).trigger('openingDialog');
    var id = new Date().getTime();
    url = AppendUrl(url, "dialogId", id);
    if (IsCrossDomain(url))
        url = AppendUrl(url, "cros", "1");

    var maxWidth = $(window).width() - 200;
    var maxHeight = $(window).height() - 100
    var dialog = $('<div id="' + id + '"><iframe frameborder="0" style="width:100%;height:98%;" src="' + url + '" scrolling="auto"></iframe></div>');
    var options = {
        modal: true,
        title: title,
        zIndex: 1100,
        //maxWidth: maxWidth,
        //maxHeight: maxHeight,
        minWidth: 250,
        minHeight: 200,
        width: maxWidth,
        height: maxHeight,
        close: function (event, ui) { $('#' + id).remove(); }
    };

    if (width) {
        options.width = parseInt(width);
    }
    if (height) {
        options.height = parseInt(height);
        if (options.height >= $(window).height) {
            ResizeIFrame(null, null, options.height + 30);
        }
    }

    if ($.browser.msie && $.browser.version < 8.0) {
        options.open = function (event, ui) {
            $('select').css('visibility', 'hidden');
        };
        options.close = function (event, ui) {
            $('select').css('visibility', 'visible');
            $('#' + id).remove();
        };
    }
    else {
        options.close = function (event, ui) {
            $('#' + id).remove();
        };
    }
    window.dialogs.push({ dialog: id, opener: opener });
    $(dialog).dialog(options);
    $(window).trigger('openedDialog');
}

function GetDialogOpener(dialogId) {
    var win = $.grep(window.dialogs, function (n, i) { return n.dialog == dialogId; })[0];
    if (win) {
        return win.opener;
    }
    return undefined;
}

function CloseDialog(id) {
    PostMessage("closedialog", { id: id }, window.top);
    return false;
}

function OnCloseDialog(id) {
    $(window).trigger('closingDialog');
    if (id) {
        $('#' + id).dialog('close').remove();
    }
    else {
        $('div.ui-dialog-content').dialog('close').remove();
    }
    $(window).trigger('closedDialog');
}

function ShowMessage(title, msg, funcClose) {
    PostMessage("showmessage", { title: title, msg: msg }, window.top);
    return false;
}

function OnShowMessage(title, msg, funcClose) {
    $(window).trigger('showingMessage');
    var id = new Date().getTime();
    var dialog = $('<div id="' + id + '" class="info"><p>' + msg + '</p></div>');
    var options = {
        modal: true,
        title: title,
        zIndex: 1200,
        buttons: [
            {
                "text": "OK",
                "class": "btn btn-success",
                "click": function () { $(this).dialog("close"); }
            }
        ]
    };
    if ($.browser.msie && $.browser.version < 8.0) {
        options.open = function (event, ui) {
            $('select').css('visibility', 'hidden');
        };
        options.close = function (event, ui) {
            $('select').css('visibility', 'visible');
            $('#' + id).remove(); if (funcClose) { funcClose(); }
        };
    }
    else {
        options.close = function (event, ui) {
            $('#' + id).remove(); if (funcClose) { funcClose(); }
        };
    }
    $(dialog).dialog(options);
    $(window).trigger('showedMessage');
}

function ShowConfirm(title, msg, funcOK, funcCancel) {
    $(window).trigger('showingConfirm');
    var id = new Date().getTime();
    var dialog = $('<div id="' + id + '" class="info"><p>' + msg + '</p></div>');
    var options = {
        modal: true,
        title: title,
        zIndex: 1200,
        buttons: {
            "OK": function () { if (funcOK) { funcOK(); } $(this).dialog("close"); },
            "Cancel": function () { if (funcCancel) { funcCancel(); } $(this).dialog("close"); }
        }
    };
    if ($.browser.msie && $.browser.version < 8.0) {
        options.open = function (event, ui) {
            $('select').css('visibility', 'hidden');
        };
        options.close = function (event, ui) {
            $('select').css('visibility', 'visible');
            $('#' + id).remove();
        };
    }
    else {
        options.close = function (event, ui) {
            $('#' + id).remove();
        };
    }
    $(dialog).dialog(options);
    $(window).trigger('showedConfirm');
}

function EnableFormValidation(form, submitHandler) {
    if ($.validator && $.validator.unobtrusive && form) {
        $(form).data("validator", null)
        $.validator.unobtrusive.parse(form);
        var validator = $(form).data("validator");
        if (validator) {
            validator.settings.submitHandler = submitHandler || function () { return true; };
        }
    }
}

function Progress(val) {
    PostMessage("progress", { val: val }, window.top);
    return false;
}

function OnProgress(val) {
    $(window).trigger('progressing');
    var progressbar = $('#progressbar');
    if (!progressbar.is('div')) {
        $('body').append('<div id="progressbar" class="progressbar"></div>');
        progressbar = $('#progressbar');
        progressbar.progressbar();
    }
    try {
        if (val && !isNaN(val)) {
            progressbar.progressbar("value", val);
            progressbar.show();
            Overlay(true);
        }
        else {
            progressbar.hide();
            Overlay(false);
        }
    } catch (ex) { }
    ResizeWindow();
    $(window).trigger('progressed');
}

function Overlay(enable) {
    PostMessage("overlay", { enable: enable }, window.top);
    return false;
}

function OnOverlay(enable) {
    $(window).trigger('overlaying');
    var overlay = $('#overlay');
    if (!overlay.is('div')) {
        $('body').append('<div id="overlay" class="ui-widget-overlay"></div>');
        overlay = $('#overlay');
        ResizeWindow();
    }
    try {
        if (enable) {
            overlay.show();
        } else {
            overlay.hide();
        }
    } catch (ex) { }
    $(window).trigger('overlayed');
}

function ClearScreen() {
    Progress();
    CloseDialog(false);
}

////////////////////////////////////////////////////////////////////////////////////////

function GetQueryString(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

function AppendUrlWithPager(url, pageIndex, pageSize) {
    url = AppendUrl(url, "pi", pageIndex);
    url = AppendUrl(url, "ps", pageSize);
    return url;
}

function AppendUrl(uri, paraName, paraValue, includeEmpty) {
    if (!paraName || !paraValue || paraName.length <= 0 || paraValue.length <= 0) {
        if (!includeEmpty) {
            return uri;
        }
    }
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    var re = new RegExp("([?|&])" + paraName + "=.*?(&|$)", "i");
    if (uri.match(re)) {
        return uri.replace(re, '$1' + paraName + "=" + escape(paraValue) + '$2');
    }
    else {
        return uri + separator + paraName + "=" + escape(paraValue);
    }
}

function GetOrigin(location) {
    if (location.origin) {
        return location.origin;
    } else {
        return (window.location.protocol + "//" + window.location.host + ":" + window.location.port + "/" + window.location.pathname);
    }
}

function IsCrossDomain(url) {
    if (url.indexOf('://') >= 0) {
        var fidx = url.indexOf("/", url.indexOf("://") + 3);
        var host = url.substr(0, fidx).toLowerCase();
        var fromUrl = GetOrigin(window.location).toLowerCase();
        return fromUrl.indexOf(host) < 0;
    }
    return false;
}

function FormatUrl(url, crossDomain, httpMethod) {
    var frmId = GetQueryString('frame');
    if (frmId) {
        url = AppendUrl(url, "frame", frmId);
    }
    /*
    if (url.indexOf('://') >= 0) {
        var fidx = url.indexOf("/", url.indexOf("://") + 3);
        var host = url.substr(0, fidx).toLowerCase();
        var fromUrl = GetOrigin(window.location).toLowerCase();
        if (fromUrl.indexOf(host) >= 0) {
            url = url.substr(fidx);
            crossDomain = false;
        }
        else {
            crossDomain = true;
        }
        // alert("from url:" + fromUrl + "\r\n" + "to url:" + url + "\r\n" + "cross domain:" + crossDomain);
    }
    if (crossDomain) {
        if (!httpMethod)
            httpMethod = "Get";
        url = AppendUrl(url, "cros", "1");
        url = '/CrossSite/' + httpMethod + '?url=' + escape(url);
    }
    */
    url = AppendUrl(url, "v", $.now());

    return url;
}

function LoadHtml(ele, url, funcSuccess, funcError, crossDomain) {
    if (typeof CleanEditors == "function") {
        CleanEditors();
    }
    url = FormatUrl(url, crossDomain);
    $(ele).empty().load(url, function (response, status, xhr) {
        Progress();
        if (status == "error") {
            if (funcError) {
                funcError(xhr.statusText, url);
            }
        }
        else {
            if (funcSuccess) {
                funcSuccess();
            }
        }
    });
}

function GetJsonp(url, data, funcSuccess, funcError) {
    url = AppendUrl(url, "v", $.now());
    if (typeof (data) == "function") {
        funcError = funcSuccess;
        funcSuccess = data;
        data = null;
    }
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'jsonp',
        crossDomain: true,
        data: data,
        traditional: true,
        timeout: ajaxTimeOut,
        success: function (data, status, xhr) {
            if (funcSuccess) {
                funcSuccess(data);
            }
        },
        error: function (xhr, status, error) {
            if (funcError) {
                funcError(error, url);
            }
        }
    });
}

function GetJson(url, data, funcSuccess, funcError) {
    url = FormatUrl(url);
    if (typeof (data) == "function") {
        funcError = funcSuccess;
        funcSuccess = data;
        data = null;
    }
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        data: data,
        traditional: true,
        timeout: ajaxTimeOut,
        success: function (data, status, xhr) {
            if (funcSuccess) {
                funcSuccess(data);
            }
        },
        error: function (xhr, status, error) {
            if (funcError) {
                funcError(error, url);
            }
        }
    });
}

function PostJson(url, data, funcSuccess, funcError) {
    url = FormatUrl(url, null, 'Post');
    if (typeof (data) == "function") {
        funcError = funcSuccess;
        funcSuccess = data;
        data = null;
    }
    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify(data),
        timeout: ajaxTimeOut,
        contentType: 'application/json; charset=utf-8',
        success: function (data, status, xhr) {
            if (funcSuccess) {
                funcSuccess(data);
            }
        },
        error: function (xhr, status, error) {
            if (funcError) {
                funcError(error, url);
            }
        }
    });
}

function PostMessage(event, msg, target, targetOrgin) {
    if (!targetOrgin) {
        targetOrgin = '*';
    }
    var dialogId = msg.dialogId || GetQueryString("dialogId");
    if (!target) {
        if (dialogId && window.dialogs) {
            var win = $.grep((window.dialogs || []), function (n, i) { return n.dialog == dialogId; })[0];
            if (win) {
                target = win.opener;
            }
        }
        if (!target) {
            target = window.parent || window.top;
        }
        $(window.self).trigger(event, [msg]);
    }
    if (target.postMessage && !(target == window.self)) {
        target.postMessage(JSON.stringify({ event: event, msg: msg }), targetOrgin);
    } else {
        $(target).trigger(event, [msg]);
    }
}

function OnReceiveMessage(event, func) {
    $(window).on(event, function (e, msg) {
        if (func) {
            func(msg);
        }
    });
}

/////////////////////////////////////////////////////////////////////////////////////////
function CreateStyle(id) {
    var css = $('head').find('#' + id);
    if (!css || !css.is('style')) {
        css = $('<style id="' + id + '" type="text/css"></style>');
        css.appendTo('head');
    }
    return css;
}

function SetStyles(id, styles) {
    var css = $('#' + id).get(0);
    if (css.styleSheet) css.styleSheet.cssText = $('#css-code').val();
    else css.appendChild(document.createTextNode($('#css-code').val()));
}

/////////////////////////////////////////////////////////////////////////////////////////
function BeautifyHtml(html) {
    if (typeof (style_html) === 'function' && html && html.length > 0) {
        try {
            return style_html(html);
        } catch (ex) { }
    }
    return html;
}

function BeautifyCss(css) {
    if (typeof (css_beautify) === 'function' && css && css.length > 0) {
        try {
            return css_beautify(css);
        } catch (ex) { }
    }
    return css;
}

function BeautifyJs(js) {
    if (typeof (js_beautify) === 'function' && js && js.length > 0) {
        try {
            return js_beautify(js);
        } catch (ex) { }
    }
    return js;
}

/////////////////////////////////////////////////////////////////////////////////////////

function GetDataStates() {
    return [
        { name: '正常', value: 1, icon: 'icon-ok', btn: 'btn-success' },
        { name: '只读', value: 2, icon: 'icon-exclamation-sign', btn: 'btn-warning' },
        { name: '锁定', value: 3, icon: 'icon-lock', btn: 'btn-inverse' },
        { name: '删除', value: 9, icon: 'icon-remove', btn: 'btn-danger' }
    ];
}

var IdGenerator = {
    NewId: function (arg) {
        if (arg) {
            if (arg.toLowerCase() == "guid") {
                return Guid.NewGuid().ToString();
            }
        }
        var ran = Math.floor(Math.random() * 100);
        if (ran < 100)
            ran += 99;
        return new Date().getTime().toString() + ran.toString();
    }
}

//表示全局唯一标识符 (GUID)。
function Guid(g) {
    var arr = new Array(); //存放32位数值的数组

    if (typeof (g) == "string") { //如果构造函数的参数为字符串
        InitByString(arr, g);
    }
    else {
        InitByOther(arr);
    }
    //返回一个值，该值指示 Guid 的两个实例是否表示同一个值。
    this.Equals = function (o) {
        if (o && o.IsGuid) {
            return this.ToString() == o.ToString();
        }
        else {
            return false;
        }
    }
    //Guid对象的标记
    this.IsGuid = function () { }
    //返回 Guid 类的此实例值的 String 表示形式。
    this.ToString = function (format) {
        if (typeof (format) == "string") {
            if (format == "N" || format == "D" || format == "B" || format == "P") {
                return ToStringWithFormat(arr, format);
            }
            else {
                return ToStringWithFormat(arr, "D");
            }
        }
        else {
            return ToStringWithFormat(arr, "D");
        }
    }
    //由字符串加载
    function InitByString(arr, g) {
        g = g.replace(/\{|\(|\)|\}|-/g, "");
        g = g.toLowerCase();
        if (g.length != 32 || g.search(/[^0-9,a-f]/i) != -1) {
            InitByOther(arr);
        }
        else {
            for (var i = 0; i < g.length; i++) {
                arr.push(g[i]);
            }
        }
    }
    //由其他类型加载
    function InitByOther(arr) {
        var i = 32;
        while (i--) {
            arr.push("0");
        }
    }
    /*
    根据所提供的格式说明符，返回此 Guid 实例值的 String 表示形式。
    N  32 位： xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    D  由连字符分隔的 32 位数字 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
    B  括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
    P  括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
    */
    function ToStringWithFormat(arr, format) {
        switch (format) {
            case "N":
                return arr.toString().replace(/,/g, "");
            case "D":
                var str = arr.slice(0, 8) + "-" + arr.slice(8, 12) + "-" + arr.slice(12, 16) + "-" + arr.slice(16, 20) + "-" + arr.slice(20, 32);
                str = str.replace(/,/g, "");
                return str;
            case "B":
                var str = ToStringWithFormat(arr, "D");
                str = "{" + str + "}";
                return str;
            case "P":
                var str = ToStringWithFormat(arr, "D");
                str = "(" + str + ")";
                return str;
            default:
                return new Guid();
        }
    }
}
//Guid 类的默认实例，其值保证均为零。
Guid.Empty = new Guid();
//初始化 Guid 类的一个新实例。
Guid.NewGuid = function () {
    var g = "";
    var i = 32;
    while (i--) {
        g += Math.floor(Math.random() * 16.0).toString(16);
    }
    return new Guid(g);
}

/////////////////////////////////////////////////////////////////////////////////////////
// prototype extensions
Array.prototype.select = function (funcSelect) {
    var arr = this;
    var selectedArr = [];
    for (var i = arr.length; i--;) {
        selectedArr.unshift(funcSelect(arr[i]));
    }
    return selectedArr;
}
Array.prototype.unique = function (funcCompare) {
    var arr = this;
    var uniqueArr = [];
    for (var i = arr.length; i--;) {
        var item = arr[i];
        if (funcCompare) {
            var fs = $.grep(uniqueArr, function (n, i) { return funcCompare(n, item); })[0];
            if (!fs) {
                uniqueArr.unshift(item);
            }
        }
        else {
            if ($.inArray(item, uniqueArr) === -1) {
                uniqueArr.unshift(item);
            }
        }
    }
    return uniqueArr;
}

function StopEvent(e) {
    e.preventDefault();
    e.stopPropagation();
}

////////////////////////////////////////////////////////////////////////////////
function Str2Dic(val, comma, equal, leftComma, rightComma) {
    if (val.length <= 0)
        return [];
    if (!comma) { comma = '&'; }
    if (!equal) { equal = '='; }
    if (!leftComma) { leftComma = ''; }
    if (!rightComma) { rightComma = ''; }
    var dic = new Array();
    var startIdx = 0;
    var equalIdx = 0;
    var leftIdx = 0, rightIdx = 0;
    var key = "", value = "";
    while (startIdx < val.length)
    {
        equalIdx = val.indexOf(equal, startIdx);
        key = val.substr(startIdx, equalIdx - startIdx);
        if (leftComma.length <= 0)
            leftIdx = equalIdx + 1;
        else
            leftIdx = val.indexOf(leftComma, equalIdx) + leftComma.length;
        if (rightComma.length <= 0)
            rightIdx = val.indexOf(comma, leftIdx);
        else
            rightIdx = val.indexOf(rightComma, leftIdx);
        if (rightIdx < 0)
            rightIdx = val.length;
        value = val.substr(leftIdx, rightIdx - leftIdx);
        var obj = { "Key": key, "Value": value };
        dic.push(obj);
        rightIdx = val.indexOf(comma, rightIdx);
        if (rightIdx < 0)
            rightIdx = val.length;
        startIdx = rightIdx + comma.length;
    }
    return dic;
}

function Dic2Str(dic, comma, equal, leftComma, rightComma) {
    if(dic.length <= 0)
        return "";
    if (!comma) { comma = '&'; }
    if (!equal) { equal = '='; }
    if (!leftComma) { leftComma = ''; }
    if (!rightComma) { rightComma = ''; }
    var val = "";
    for (var i = 0; i < dic.length; i++) {
        if (val.length > 0)
            val += comma;
        val += dic[i].Key + equal + leftComma + dic[i].Value + rightComma;
    }
    return val;
}
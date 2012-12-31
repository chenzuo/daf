
var defaultPaperOptions = {
    portrait: true,
    header: '',
    footer: '',
    marginLeft: 5,
    marginRight: 5,
    marginTop: 5,
    marginBottom: 5,
    // ie 下默认边距为 5，如果设置为0，ie会自动设置为2-3mm
};

function Initialize(app_url) {
    if ($.browser.msie) {
        // 此控件只能运行在ie 32位上，ie 64位的不支持
        var printer = $('<object style="width: 0; height: 0; visibility: hidden;" id="printer" classid="clsid:1663ED61-23EB-11D2-B92F-008048FDD814" codebase="' + app_url + '/Content/smsx.cab#Version=7,0,0,8" viewastext ></object>');
        printer.appendTo('body');
        var menuitem = $('<li><a href="' + app_url + '/Content/ScriptX.msi">下载打印控件</a></li>');
        $('#menu-bar>ul').append(menuitem);
    }
    else if ($.browser.chrome) {
        CreateStyle('printSetup');
    }
    else if ($.browser.mozilla) {
        if (!jsPrintSetup) {
            var params = {
                "JS Print Setup":
                 {
                     URL: app_url + "/Content/jsprintsetup-0.9.2.xpi", IconURL: null,
                     toString: function () { return this.URL; }
                 }
            };
            InstallTrigger.install(params);
        }
        return false;
    }
    else if ($.browser.safari) {
    }
    else {
        CreateStyle('printSetup');
    }
}

function SetupPaper(options) {
    var ops = $.extend(options, defaultPaperOptions);
    if ($.browser.msie) {
        printer.printing.header = ops.header;
        printer.printing.footer = ops.footer;
        printer.printing.portrait = ops.portrait;
        printer.printing.leftMargin = ops.marginLeft;
        printer.printing.topMargin = ops.marginTop;
        printer.printing.rightMargin = ops.marginRight;
        printer.printing.bottomMargin = ops.marginBottom;
    }
    else if ($.browser.chrome) {
        var styles = '\
@page {\
    margin: ' + ops.marginTop + ' ' + ops.marginRight + ' ' + ops.marginBottom + ' ' + ops.marginLeft + ';\
}\
            ';
        SetStyles('printSetup', styles);
    }
    else if ($.browser.mozilla) {
        jsPrintSetup.setOption('orientation', ops.portrait ? jsPrintSetup.kPortraitOrientation : jsPrintSetup.kPortraitOrientation);
        jsPrintSetup.setOption('marginTop', ops.marginTop);
        jsPrintSetup.setOption('marginBottom', ops.marginBottom);
        jsPrintSetup.setOption('marginLeft', ops.marginLeft);
        jsPrintSetup.setOption('marginRight', ops.marginRight);
        jsPrintSetup.setOption('headerStrLeft', '');
        jsPrintSetup.setOption('headerStrCenter', ops.header);
        jsPrintSetup.setOption('headerStrRight', '');
        jsPrintSetup.setOption('footerStrLeft', '');
        jsPrintSetup.setOption('footerStrCenter', ops.footer);
        jsPrintSetup.setOption('footerStrRight', '');
    }
    else if ($.browser.safari) {
    }
    else {
        var styles = '\
@page {\
    margin: ' + ops.marginTop + ' ' + ops.marginRight + ' ' + ops.marginBottom + ' ' + ops.marginLeft + ';\
}\
            ';
        SetStyles('printSetup', styles);
    }
}

function PageSetup() {
    if ($.browser.msie) {
        printer.printing.PageSetup();
    }
    else if ($.browser.chrome) {
    }
    else if ($.browser.mozilla) {
    }
    else if ($.browser.safari) {
    }
}

function PrintSetup() {
    if ($.browser.msie) {
        printer.printing.PrintSetup();
    }
    else if ($.browser.chrome) {
    }
    else if ($.browser.mozilla) {
    }
    else if ($.browser.safari) {
    }
}

function Preview() {
    $(document).trigger('onpreview');
    if ($.browser.msie) {
        printer.printing.Preview();
    }
    else if ($.browser.chrome) {
        window.print();
    }
    else if ($.browser.mozilla) {
    }
    else if ($.browser.safari) {
    }
}

function Print() {
    $(document).trigger('onprint');
    if ($.browser.msie) {
        printer.printing.Print(true);
    }
    else if ($.browser.chrome) {
    }
    else if ($.browser.mozilla) {
        jsPrintSetup.clearSilentPrint();
        jsPrintSetup.setOption('printSilent', 1);
        jsPrintSetup.print();
    }
    else if ($.browser.safari) {
    }
}
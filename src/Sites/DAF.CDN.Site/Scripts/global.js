$(function () {
    $(document).on('click', '.dialog', function () {
        var url = $(this).attr('href');
        var title = $(this).attr('title');
        if (url) {
            if (url.indexOf('#') == 0) {
                ShowDialog(url, title, eval($(this).attr('dialog-ok')), eval($(this).attr('dialog-cancel')), $(this).attr('dialog-width'), $(this).attr('dialog-height'));
            }
            else {
                OpenDialog(title, url, $(this).attr('dialog-width'), $(this).attr('dialog-height'));
            }
        }
        return false;
    });
    $(document).keydown(function (e) {
        // press 'esc'
        if (e.keyCode == '27') {
            Progress();
        }
    });
    $.ajaxSetup({
        global: true,
        beforeSend: function (jqXHR, settings) {
            if (typeof (Progress) == "function")
                Progress(100);
        },
        complete: function (jqXhr, status) {
            if (typeof (Progress) == "function")
                Progress();
            if (typeof (InitEditors) == "function")
                InitEditors();
        }
    });

    $(document).on('resizeWindow', $(window), ResizeWindow);

});

function ResizeWindow() {
    var winheight = $(window).height();
    var winwidth = $(window).width();
    var l = (winwidth - $("#progressbar").width()) / 2;
    var t = (winheight - $("#progressbar").height()) / 2;
    $("#progressbar").css('top', t + 'px').css('left', l + 'px');
    $('#overlay').width($(document).width()).height($(document).height());
}


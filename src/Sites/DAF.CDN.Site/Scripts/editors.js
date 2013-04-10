$(function () {
    InitEditors();
});

function InitEditors() {
    if ($.datepicker) {
        $('.date:not([class*="hasDatepicker"])').datepicker({ dateFormat: "yy-mm-dd" });
    }
    if ($.timepicker) {
        $('.time:not([class*="hasDatepicker"])').timepicker({ timeFormat: "HH:mm" });
        $('.datetime:not([class*="hasDatepicker"])').datetimepicker({ dateFormat: "yy-mm-dd", timeFormat: "HH:mm" });
    }
    if ($.fn.dropdown) {
        $('.dropdown-toggle').dropdown();
    }
    if ($.fn.tabs) {
        $('.tabs').tabs();
    }

    if (window.CKEDITOR && window.CKFinder) {
        var baseUrl = GetOrigin(window.location);
        window.CKEDITOR.config.language = (window.locale || "zh-cn").toLowerCase();
        window.CKEDITOR.config.baseHref = baseUrl;

        $('textarea.html').each(function () {
            var id = $(this).prop('id');
            $(this).prop('rows', '21');
            $(this).prop('cols', '128');
            var options = {
                toolbar: $(this).prop('toolbar') || 'Default'
            };
         
            var editor = window.CKEDITOR.replace(id, options);
            window.CKFinder.setupCKEditor(editor, baseUrl + 'ckfinder/');
        });
    }
}

function CleanEditors() {
    if (window.CKEDITOR) {
        for (var a in window.CKEDITOR.instances) {
            window.CKEDITOR.instances[a].destroy();
        }
    }
}



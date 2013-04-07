(function ($) {
    $.fn.dropdown = function (options) {
        return $.fn.base("dropdown", this, options,
            function (ele, ops) { return new $.dropdown(ele, ops); },
            function (instance, ops) { instance.option(ops); });
    };

    $.dropdown = function (ele, ops) {
        this.ele = $(ele);
        this.popup = null;
        this.options = $.extend({ onshow: null, onhide: null, getOffset: null, popup: null, noclick: false }, ops || {});
        this._init();
    };

    $.dropdown.prototype = {
        _init : function () {
            var self = this;
            this.ele.addClass('dropdown-toggle');
            if (this.ele.attr('data-popup'))
                this.options.popup = this.ele.attr('data-popup');
            if (this.ele.attr('data-without-click') == 'true')
                this.options.noclick = true;

            if (!this.options.noclick) {
                this.ele.on('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    self.toggle();
                });
            }
            this.popup = $(this.options.popup);
            $(document).on('click', 'body', function (e) {
                self.hide();
            });
        },
        option: function(ops){
        },
        show : function () {
            if (this.popup.data('srcEle') != this.ele)
                this.hide();
            this.ele.closest('div.btn-group').addClass('open');
            var ops = { my: "right bottom", at: "right top", of: this.ele };
            if (this.options.getOffset) {
                ops.offset = this.options.getOffset(this.ele);
            }
            this.popup.css({ left: '0px', top: '0px' }).position(ops).show();
            if (this.options.onshow) {
                this.options.onshow(this.ele, popup);
            }
            this.popup.data('srcEle', this.ele);
        },
        hide : function () {
            var srcEle = this.popup.data('srcEle') || this.ele;
            srcEle.closest('div.btn-group').removeClass('open');
            this.popup.hide()
            if (this.options.onhide) {
                this.options.onhide(srcEle, this.popup);
            }
            this.popup.data('srcEle', null);
        },
        toggle : function () {
            if (this.popup.is(':visible')) {
                var srcEle = this.popup.data('srcEle');
                if (srcEle == this.ele)
                    this.hide();
                else {
                    this.hide();
                    this.show();
                }
            }
            else {
                this.show();
            }
        }
    };
})(jQuery);
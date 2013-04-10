(function ($) {
    $.fn.dropdown = function (options) {
        return $.fn.base("dropdown", this, options,
            function (ele, ops) { return new $.dropdown(ele, ops); },
            function (instance, ops) { instance.option(ops); });
    };

    $.dropdown = function (ele, ops) {
        this.ele = $(ele);
        this.dropdown = null;
        this.options = {};
        this.options = $.extend({}, {
            dropdown: null,
            my: null,
            at: null,
            of: null,
            collision: null,
            getOffset: null,
            onshow: null, onhide: null,
            noclick: false
        }, ops);
        this._init();
    };

    $.dropdown.prototype = {
        _init: function () {
            var self = this;
            if (this.ele.attr('data-toggle'))
                this.options.dropdown = this.ele.attr('data-toggle');
            if (this.ele.attr('data-without-click') == 'true')
                this.options.noclick = true;

            if (!this.options.noclick) {
                this.ele.on('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    self.toggle();
                });
            }
            this.ele.addClass('dropdown-toggle');
            this.dropdown = $(this.options.dropdown).css({ position: 'absolute', zIndex: 2000, left: '0px', top: '0px' }).addClass('dropdown-menu');
            $(this.dropdown).on('click', 'a, button', function (e) {
                self.hide();
            });
            $(document).on('mousedown', 'body', function (e) {
                if ($(e.target).is('.dropdown-menu *, .dropdown-menu'))
                    return false;
                self.hide();
            });
        },
        option: function (ops) {
        },
        show: function () {
            if (this.dropdown.data('srcEle') != this.ele)
                this.hide();
            (this.ele.closest('.btn-group') || this.ele.parent()).addClass('open');
            var ops = {
                my: this.options.my || "right top",
                at: this.options.at || "right bottom",
                of: this.options.of || this.ele,
                collision: this.options.collision || 'flip',
            };
            var offset = (typeof this.options.getOffset == "function") ? this.options.getOffset(this.ele) : { top: $(window).scrollTop(), left: $(window).scrollLeft() };
            if (offset) {
                ops.offset = offset.left.toString() + ' ' + offset.top.toString();
            }
            this.dropdown.css({ left: '0px', top: '0px' }).position(ops).show();
            if (this.options.onshow) {
                this.options.onshow(this.ele, dropdown);
            }
            this.dropdown.data('srcEle', this.ele);
        },
        hide: function () {
            var srcEle = this.dropdown.data('srcEle') || this.ele;
            (srcEle.closest('.btn-group') || srcEle.parent()).removeClass('open');
            this.dropdown.hide()
            if (this.options.onhide) {
                this.options.onhide(srcEle, this.dropdown);
            }
            this.dropdown.data('srcEle', null);
        },
        toggle: function () {
            if (this.dropdown.is(':visible')) {
                var srcEle = this.dropdown.data('srcEle');
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
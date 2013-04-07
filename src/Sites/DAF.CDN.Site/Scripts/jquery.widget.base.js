$.fn.base = function (name, widget, options, funcNewInstance, funcApplyOptions) {
    var isMethodCall = typeof options === "string",
        args = Array.prototype.slice.call(arguments, 1),
        returnValue = widget;

    // allow multiple hashes to be passed on init
    options = !isMethodCall && args.length ?
        $.extend.apply(null, [true, options].concat(args)) :
        options;

    // prevent calls to internal methods
    if (isMethodCall && options.charAt(0) === "_") {
        return returnValue;
    }

    if (isMethodCall) {
        widget.each(function () {
            var instance = $(this).data(name);
            var methodValue = instance && $.isFunction(instance[options]) ?
                    instance[options].apply(instance, args) :
                    instance;
            if (methodValue !== instance && methodValue !== undefined) {
                returnValue = methodValue;
                return false;
            }
        });
    } else {
        widget.each(function () {
            var instance = $(this).data(name);
            if (instance) {
                if (funcApplyOptions) {
                    funcApplyOptions(instance, options || {});
                }
            } else {
                $(this).data(name, funcNewInstance(this, options));
            }
        });
    }

    return returnValue;
}
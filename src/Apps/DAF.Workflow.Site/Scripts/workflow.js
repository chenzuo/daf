$(function () {
    $('.time-node').mouseover(function (e) {
        $('div.node-info').hide();
        var offset = $(this).offset();
        var left = offset.left + 30;
        var top = offset.top + 20;
        if (offset.top + $(this).next().height() > $(window).height()) {
            top = offset.top - $(this).next().height() - 20;
        }
        $(this).next().css('left', left).css('top', top).show();
    });
});
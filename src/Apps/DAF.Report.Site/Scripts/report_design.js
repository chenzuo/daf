var designArea = '#designArea';
var multiselect = false;
var mm;

$(function () {
    $(designArea).delegate('.rpt-page-holder .rpt-page .ele', 'dblclick', function () {
        SwitchMode("edit");
    });
    $(designArea).delegate('.rpt-page-holder .rpt-page .ele', 'click', function () {
        SelectElements([$(this)]);
        return false;
    });
    $(designArea).delegate('.rpt-page-holder .rpt-page table.ele th', 'click', function () {
        SelectHeadCells([$(this)]);
        return false;
    });
    $(designArea).delegate('.rpt-page-holder .rpt-page table.ele td', 'click', function () {
        SelectCells([$(this)]);
        return false;
    });
    $(document).click(function (e) {
        if ($(e.target).is('.rpt-page-holder .rpt-page *.ele'))
            return;
        SwitchMode("design");
    });
    $(document).keydown(function (e) {
        if (e.keyCode = '17') {
            multiselect = true;
        }
    });
    $(document).keyup(function (e) {
        multiselect = false;
    });
    SwitchMode("design");

    $(window).on('resizeWindow', ResizeArea);
    var element = $('<div style="display:none;height:10in;border:0;margin:0;padding:0"></div>').appendTo('body');
    var inch = element.height() / 10.;
    mm = inch / 25.4;
    element.remove();
});

function ResizeArea() {
}

function GetCurrentPage() {
    return $('div.rpt-page-holder .rpt-page:visible', $(designArea));
}

function AddPage(pageName, pageType) {
    $(designArea).children('div.rpt-page-holder').hide();
    var ph = $('div.rpt-page-holder[id="ph_' + pageName + '"]', $(designArea));
    if (ph.is('div')) {
        var newPage = $('<div id="' + pageName + '" class="page ' + pageType + '"></div>"');
        ph.append(newPage);
    }
}

function SetPageSize(pageName, w, h) {
    $(designArea).children('div.rpt-page-holder').hide();
    var ph = $('#design-panel div.rpt-page-holder[id="ph_' + pageName + '"]');
    if (ph.is('div')) {
        var pg = $('#' + pageName, ph);
        if (!pg.is('div')) {
            var newPage = $('<div id="' + pageName + '" class="rpt-page" style="width:' + w + 'mm;height:' + h + 'mm;"></div>"');
            ph.append(newPage);
        } else {
            pg.css({ width: w + 'mm', height: h + 'mm' });
        }
    }
}

function AddElement(eleType, w, h, id) {
    var page = GetCurrentPage();
    var pageId = page.attr('id');
    var count = page.children('.ele').length + 1;
    if (!id) {
        id = pageId + '_ele_' + count;
    }
    if (!w) {
        w = 20;
    }
    if (!h) {
        h = 6;
    }
    var newEle = $('<div id="' + id + '" class="ele ' + eleType + '" style="z-index:' + count + ';width:' + w + 'mm;height:' + h + 'mm;"></div>"');
    page.append(newEle);
    SwitchMode('design', newEle);
    SelectElements([newEle]);
}

function ClearElementSelection() {
    $('.rpt-page-holder .rpt-page:visible .ele').removeClass('ele-selected');
}

function SelectElements(eles) {
    ClearHeadCellSelection();
    ClearCellSelection();
    if (!multiselect) {
        ClearElementSelection();
    }
    $.each(eles, function () {
        $(this).addClass('ele-selected');
    });
}

function GetSelectedElements() {
    return $('div.rpt-page-holder div.rpt-page:visible .ele-selected', $(designArea)).not('td').not('th');
}

function DeleteSelectedElements() {
    var eles = GetSelectedElements();
    if (eles && eles.length > 0) {
        eles.remove();
    }
    var cells = GetSelectedCells();
    if (cells && cells.length > 0) {
        cells.closest('table').remove();
    }
}

function DeleteAllElements() {
    return $('div.rpt-page-holder div.rpt-page:visible .ele', $(designArea)).not('td').remove();
}

function AddTable(rows, cols, hrows, ch, cw, hh, id) {
    var page = GetCurrentPage();
    var pageId = page.attr('id');
    var count = page.children('.ele').length + 1;
    if (!id) {
        id = pageId + '_ele_' + count;
    }
    if (!hh) {
        hh = 6;
    }
    if (!ch) {
        ch = 6;
    }
    if (!cw) {
        cw = 20;
    }
    var w = cols * cw;
    var h = hh * hrows + ch * rows;
    var html = '<table id="' + id + '" class="ele" border="1" cellspacing="0" cellpadding="0" style="width:' + w + 'mm;height:' + h + 'mm;z-index:' + count + ';">';
    if (hrows && hrows > 0) {
        html += '<thead>';
        for (var i = 0; i < hrows; i++) {
            html += "<tr>";
            for (var j = 0; j < cols; j++) {
                html += '<th row="' + i + '" col="' + j + '" style="width:' + cw + 'mm;height:' + hh + 'mm;"></th>';
            }
            html += "</tr>";
        }
        html += '</thead>';
    }
    html += '<tbody>';
    for (var i = 0; i < rows; i++) {
        html += "<tr>";
        for (var j = 0; j < cols; j++) {
            html += '<td row="' + i + '" col="' + j + '" style="width:' + cw + 'mm;height:' + ch + 'mm;"></td>';
        }
        html += "</tr>";
    }
    html += '</tbody>';
    html += '</table>';
    var newEle = $(html);
    page.append(newEle);
    SelectElements([newEle]);
}

function ClearHeadCellSelection() {
    $('div.rpt-page-holder div.rpt-page table th', $(designArea)).removeClass('ele-selected');
}

function SelectHeadCells(cells) {
    ClearElementSelection();
    ClearCellSelection();
    if (!multiselect) {
        ClearHeadCellSelection();
    }
    $.each(cells, function () {
        $(this).addClass('ele-selected');
    });
}

function GetSelectedHeadCells() {
    return $('div.rpt-page-holder div.rpt-page table th.ele-selected');
}

function ClearCellSelection() {
    $('div.rpt-page-holder div.rpt-page table td', $(designArea)).removeClass('ele-selected');
}

function SelectCells(cells) {
    ClearHeadCellSelection();
    ClearElementSelection();
    if (!multiselect) {
        ClearCellSelection();
    }
    $.each(cells, function () {
        $(this).addClass('ele-selected');
    });
}

function GetSelectedCells() {
    return $('div.rpt-page-holder div.rpt-page table td.ele-selected');
}

function SetRowHeight(h) {
    var cells = GetSelectedCells();
    if (cells && cells.length > 0) {
        cells.each(function () {
            $(this).parent().children('td').height(h + 'mm');
        });
    }
}

function SetColumnWidth(w) {
    var cells = GetSelectedCells();
    if (cells && cells.length > 0) {
        cells.each(function () {
            var cidx = $(this).attr('col');
            $('td[col=' + cidx + ']', $(this).closest('table')).width(w + 'mm');
        });
    }
}

function MergeCells() {
    var cells = GetSelectedCells();
    if (cells && cells.length > 0) {

    }
}

function SetId(id) {
    var eles = GetSelectedElements().add(GetSelectedCells());
    if (eles && eles.length > 0) {
        eles.each(function () {
            $(this).attr('id', id);
        });
    }
}

function AddClass(cls) {
    var eles = GetSelectedElements().add(GetSelectedCells());
    if (eles && eles.length > 0) {
        eles.each(function () {
            $(this).addClass(cls);
        });
    }
}

function RemoveClass(cls) {
    var eles = GetSelectedElements().add(GetSelectedCells());
    if (eles && eles.length > 0) {
        eles.each(function () {
            $(this).removeClass(cls);
        });
    }
}

function SetBorder(l, t, r, b) {
    var eles = GetSelectedElements().add(GetSelectedCells());
    if (eles && eles.length > 0) {
        eles.removeClass('border-l').removeClass('border-t').removeClass('border-r').removeClass('border-b');
        if (l) { eles.addClass('border-l') };
        if (t) { eles.addClass('border-t') };
        if (r) { eles.addClass('border-r') };
        if (b) { eles.addClass('border-b') };
    }
}

function SetBorderWidth(w) {
    var eles = GetSelectedElements().add(GetSelectedCells());
    if (eles && eles.length > 0) {
        eles.each(function () {
            $(this).css('border-width', w + 'px');
        });
    }
}

function SetFontFamily(font) {
    var eles = GetSelectedElements().add(GetSelectedCells());
    if (eles && eles.length > 0) {
        eles.css('font-family', font);
    }
}

function SetFontStyle(font) {
    var eles = GetSelectedElements().add(GetSelectedCells());
    if (eles && eles.length > 0) {
        eles.css('font-style', font);
    }
}

function SetFontSize(font) {
    var eles = GetSelectedElements().add(GetSelectedCells());
    if (eles && eles.length > 0) {
        eles.css('font-size', font);
    }
}

function SetColor(hex) {
    var eles = GetSelectedElements().add(GetSelectedCells());
    if (eles && eles.length > 0) {
        eles.css('color', hex);
    }
}

function SetBorderColor(hex) {
    var eles = GetSelectedElements().add(GetSelectedCells());
    if (eles && eles.length > 0) {
        eles.css('border-color', hex);
    }
}

function SetBackgroundColor(hex) {
    var eles = GetSelectedElements().add(GetSelectedCells());
    if (eles && eles.length > 0) {
        eles.css('background-color', hex);
    }
}

function GetDbBind() {
    var eles = GetSelectedElements();
    if (eles && eles.length > 0) {
        return eles.eq(0).attr('data-bind');
    }
    var cells = GetSelectedCells();
    if (cells.length > 1) {
        var tbody = $(cells).closest('tbody') || $(cells).closest('table');
        return tbody.attr('data-bind');
    }
    else if (cells.length == 1) {
        return cells.eq(0).attr('data-bind');
    }
}

function SetDbBind(dbbind) {
    var eles = GetSelectedElements();
    if (eles && eles.length > 0) {
        eles.attr('data-bind', dbbind);
        eles.attr('title', '[databind:' + dbbind + ']');
    }
    var cells = GetSelectedCells();
    if (cells.length > 1) {
        var tbody = $(cells).closest('tbody') || $(cells).closest('table');
        tbody.attr('data-bind', dbbind);
    }
    else if (cells.length == 1) {
        cells.attr('data-bind', dbbind);
    }
}

function ToggleDbBindTitle(shown) {
    if (shown) {
        $('*[data-bind]', $(designArea)).each(function () {
            var databind = $(this).attr('data-bind');
            $(this).attr('title', '[databind:' + databind + ']');
        });
    } else {
        $('*[data-bind]', $(designArea)).each(function () {
            $(this).removeAttr('title');
        });
    }
}

function ToggleBorder() {
    var eles = $('.rpt-page-holder .rpt-page:visible .ele', $(designArea));
    eles.toggleClass('with-border');
}

function TogglePrint() {
    var eles = GetSelectedElements();
    if (eles && eles.length > 0) {
        eles.toggleClass('no-print');
    }
}

function SwitchMode(mode, eles) {
    if (!eles || eles.length <= 0) {
        eles = $('.rpt-page-holder .rpt-page:visible .ele', $(designArea));
    }
    if (mode == "design") {
        //window.document.designMode = "off";
        eles.removeAttr('contenteditable');
        eles.draggable({
            containment: 'parent', grid: [5, 5], scroll: false, disabled: false,
            stop: function (event, ui) {
                var ele = ui.helper[0];

            }
        }).resizable({
            disabled: false,
            stop: function (event, ui) {
            }
        });
        ToggleDbBindTitle(true);
    }
    else if (mode == "edit") {
        //window.document.designMode = "on";
        eles.attr('contenteditable', 'true');
        eles.draggable({ disabled: true }).resizable({ disabled: true });
    }
    else { // mode == "view"
        ClearElementSelection();
        //window.document.designMode = "off";
        eles.attr('contenteditable', 'true');
        eles.draggable({ disabled: true }).resizable({ disabled: true });
    }
    ResizeIFrame();
}

function GetPageTrimmedHtml(pageName) {
    var pg = $('div.rpt-page[id="' + pageName + '"]', $(designArea));
    if (pg.length > 0) {
        var cpg = pg.clone();
        var eles = cpg.children('.ele');
        eles.removeClass('ui-draggable ui-resizable ele-selected with-border');
        eles.removeAttr('aria-disabled');
        eles.children('div').remove();

        ChangeElesToMM(eles);
        ToggleDbBindTitle(false);
        return cpg.get(0).outerHTML;
    }
    return "";
}

function ChangeElesToMM(eles) {
    $.each(eles, function () {
        var w = $(this).css('width');
        if (w && w.indexOf('px') > 0) {
            var dw = parseInt(w);
            if (!isNaN(dw)) {
                var mmw = Math.round(dw / mm * 100) / 100;
                $(this).css('width', mmw + 'mm');
            } else {
                $(this).css('width', '20mm');
            }
        } else {
            $(this).css('width', '20mm');
        }
        var h = $(this).css('height');
        if (h && h.indexOf('px') > 0) {
            var dh = parseInt(h);
            if (!isNaN(dh)) {
                var mmh = Math.round(dh / mm * 100) / 100;
                $(this).css('height', mmh + 'mm');
            } else {
                $(this).css('height', '5mm');
            }
        } else {
            $(this).css('height', '5mm');
        }
        var t = $(this).css('top');
        if (t && t.indexOf('px') > 0) {
            var dt = parseInt(t);
            if (!isNaN(dt)) {
                var mmt = Math.round(dt / mm * 100) / 100;
                $(this).css('top', mmt + 'mm');
            } else {
                $(this).css('top', '0mm');
            }
        } else {
            $(this).css('top', '0mm');
        }
        var l = $(this).css('left');
        if (l && l.indexOf('px') > 0) {
            var dl = parseInt(l);
            if (!isNaN(dl)) {
                var mml = Math.round(dl / mm * 100) / 100;
                $(this).css('left', mml + 'mm');
            } else {
                $(this).css('left', '0mm');
            }
        } else {
            $(this).css('left', '0mm');
        }
        var celes = $(this).children();
        if (celes.length > 0) {
            ChangeElesToMM(celes);
        }
    });
}

function ParseSize(s) {
    var idx = s.indexOf(',');
    return {
        width: parseFloat(s.substr(0, idx)),
        height: parseFloat(s.substr(idx + 1))
    };
}

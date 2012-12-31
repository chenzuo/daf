/*
options = {
    pageIndex: 0,
    pageSize: 50,

    trackDataChange: true,
    clearChangesWhenBindData: true,
    getQueryUrl: function () { return ""; },
    bindData: function(data) { return bindedData; },
    onQuerySuccess: function (bindedData) { },
    onQueryError: function (error) { },

    getSaveUrl: function () { return ""; },
    beforeSave: function (data) {},
    onSaveSuccess: function (info) {},
    onSaveError: function(error) {}

    onInit: function(self) {},
    createItem: function () { return {}; },
    canAdd: function() { return true; },
    canEdit: function(item) { return true; },
    canRemove: function(item) { return true; },
    onAdd： function(item) {},
    onEdit： function(item) {},
    onRemove： function(item) {},
    validate: function() { return true; }
};
*/
var DataState = {
    New: 1,
    Modified: 2,
    Deleted: 3,
    None: 0
}

function KVM(options) {
    var self = this;
    self.options = $.extend({
        pageIndex: 1,
        pageSize: 50,
        appendPagingData: false,
        trackDataChange: true,
        clearChangesWhenBindData: true
    }, options || {});

    self.PagingInfo = ko.observable({
        PageIndex: ko.observable(self.options.pageIndex),
        PageSize: ko.observable(self.options.pageSize),
        StartPosition: ko.observable(1),
        EndPosition: ko.observable(1),
        TotalItemCount: ko.observable(0),
        TotalPageCount: ko.observable(0)
    });
    self.Data = ko.observableArray([]);
    self.Filter = ko.observable(null);

    self.EditingItem = ko.observable(null);
    self.SelectedItems = ko.observableArray([]);
    self.NewItems = ko.observableArray([]);
    self.ModifiedItems = ko.observableArray([]);
    self.DeletedItems = ko.observableArray([]);

    self._lastAddedItem = null;
    self._lastEditedItem = null;
    self._lastDeletedItem = null;

    self.trackChange = function (obj) {
        if (obj.DataState)
            return;
        for (var pn in obj) {
            self.trackPropertyChange(pn, obj);
        }
        obj.DataState = DataState.None;
    };

    self.trackPropertyChange = function (prop, obj) {
        var value = obj[prop];
        if (ko.isObservable(value)) {
            var val = value();
            if (val instanceof Array) {
                for (var i = 0; i < val.length; i++) {
                    self.trackChange(val[i]);
                }
            }
            else {
                value.subscribe(function (newValue) {
                    if (obj.DataState == DataState.None) {
                        obj.DataState = DataState.Modified;
                        self.ModifiedItems.push(obj);
                    }
                }, self);
            }
        }
    };
    self.GetPages = function (pageCount) {
        if (!pageCount)
            page = 10;
        var pinfo = self.PagingInfo();
        if (pinfo) {
            var spi = pinfo.PageIndex() - Math.ceil(pageCount / 2);
            if (spi < 1) { spi = 1 }
            var epi = pinfo.PageIndex() + Math.ceil(pageCount / 2);
            if (epi > pinfo.TotalPageCount()) { epi = pinfo.TotalPageCount(); }
            var pages = [];
            for (var i = spi; i <= epi; i++) {
                pages.push(i);
            }
            return pages;
        }
        return [];
    };
    self.IsFirstPage = function () {
        return self.PagingInfo().PageIndex() <= 1;
    };
    self.IsLastPage = function () {
        return self.PagingInfo().PageIndex() >= self.PagingInfo().TotalPageCount();
    };
    self.IsCurrentPage = function (p) {
        return self.PagingInfo().PageIndex() == p;
    };
    self.GotoPage = function (op) {
        var pidx = self.PagingInfo().PageIndex();
        if (typeof (op) == "number") {
            pidx = op;
        } else if (typeof (op) == "object") {
            if (op.page) {
                pidx = op.page;
            } else if (op.offset) {
                pidx += op.offset;
            }
        }
        if (pidx > self.PagingInfo().TotalPageCount()) {
            pidx = self.PagingInfo().TotalPageCount();
        }
        if (pidx < 1) {
            pidx = 1;
        }
        if (pidx == self.PagingInfo().PageIndex()) {
            return false;
        }
        return self.GetData(pidx);
    };
    self.GetData = function (pi, ps, bindData, onSuccess, onError) {
        var url = self.options.getQueryUrl();
        url = AppendUrl(url, 'pi', pi || self.options.pageIndex);
        url = AppendUrl(url, 'ps', ps || self.options.pageSize);
        var filterData = null;
        if (self.Filter) {
            filterData = ko.mapping.toJS(self.Filter);
            for (var n in filterData) {
                if (filterData[n] && typeof (filterData[n].toJSON) == "function") {
                    filterData[n] = filterData[n].toJSON();
                }
            }
        }
        GetJson(url, filterData, function (data) {
            if (!data) {
                var error = "未返回任何数据！";
                if (onError) {
                    onError(error);
                } else if (self.options.onQueryError) {
                    self.options.onQueryError(error);
                } else {
                    ShowMessage('出现错误', error + '\r\n' + url);
                }
            }
            if (data.Status && data.Message && data.Status > 0) {
                if (onError) {
                    onError(data.Message);
                } else if (self.options.onQueryError) {
                    self.options.onQueryError(data.Message);
                } else {
                    ShowMessage('出现错误', data.Message + '\r\n' + url);
                }
                return false;
            }
            if (self.options.clearChangesWhenBindData) {
                self.NewItems().splice(0, self.NewItems().length);
                self.ModifiedItems().splice(0, self.ModifiedItems().length);
                self.DeletedItems().splice(0, self.DeletedItems().length);
            }
            var udata = data;
            if (data.PagingInfo) {
                if (self.options.appendPagingData) {
                    if (self.PagingInfo()) {
                        data.PagingInfo.StartPosition = self.PagingInfo().StartPosition || 1;
                    } else {
                        data.PagingInfo.StartPosition = 1;
                    }
                }
                ko.mapping.fromJO(data.PagingInfo, null, self.PagingInfo);
            }
            if (data.Filter) {
                ko.mapping.fromJO(data.Filter, null, self.Filter);
            }
            if (data.Data) {
                udata = data.Data;
            }
            var bindedData = ko.observableArray([]);
            if (bindData) {
                bindedData = bindData(udata);
            } else if (self.options.bindData) {
                bindedData = self.options.bindData(udata);
            } else {
                ko.mapping.fromJS(udata, null, bindedData);
                if (self.options.appendPagingData && self.Data() && self.Data().length > 0) {
                    for (var i = 0; i < udata.length; i++) {
                        self.Data.push(ko.observable(udata[i]));
                    }
                } else {
                    ko.mapping.fromJS(udata, null, self.Data);
                }
            }
            if (self.options.trackDataChange) {
                for (var i = 0; i < self.Data().length; i++) {
                    var obj = self.Data()[i];
                    self.trackChange(obj);
                }
            }
            if (onSuccess) {
                onSuccess(bindedData);
            } else if (self.options.onQuerySuccess) {
                self.options.onQuerySuccess(bindedData);
            }
            if (typeof (ResizeIFrame) == "function") {
                ResizeIFrame();
            }
            //if (typeof (Progress) == "function") {
            //    Progress();
            //}
        }, function (error, url) {
            if (onError) {
                onError(error);
            } else if (self.options.onQueryError) {
                self.options.onQueryError(error);
            } else {
                ShowMessage('出现错误', error + '\r\n' + url);
            }
        });

    };

    self.SelectItem = function (item) {
        self.SelectedItems.push(item);
    };

    self.UnselectItem = function (item) {
        self.SelectedItems.remove(item);
    };

    self.ClearSelection = function () {
        self.SelectedItems.clear();
    };

    self.IsNew = function (item) {
        return item && item.DataState == DataState.New;
    };

    self.IsModified = function (item) {
        return item && item.DataState == DataState.Modified;
    };

    self.IsDeleted = function (item) {
        return item && item.DataState == DataState.Deleted;
    };

    self.AddItem = function () {
        if (self.options.canAdd) {
            if (!self.options.canAdd()) {
                return false;
            }
        }
        var item = self.options.createItem();
        item.DataState = DataState.New;
        self.NewItems.push(item);
        self.Data.push(item);
        self.EditingItem(item);
        self._lastAddedItem = item;
        if (self.options.onAdd) {
            self.options.onAdd(item);
        }
    };

    self.UndoAddItem = function () {
        if (self._lastAddedItem) {
            var item = self._lastAddedItem;
            self.NewItems.remove(item);
            self.Data.remove(item);
            self._lastAddedItem = null;
        }
    };

    self.EditItem = function (item) {
        if (self.options.canEdit) {
            if (!self.options.canEdit(item)) {
                return false;
            }
        }
        self.EditingItem(item);
        self._lastEditedItem = ko.toJS(item);
        if (self.options.onEdit) {
            self.options.onEdit(item);
        }
    };

    self.UndoEditItem = function () {
        if (self._lastEditedItem) {
            var item = vm.EditingItem();
            self.ModifiedItems.remove(item);
            for (var pn in self._lastEditedItem) {
                if (typeof (item[pn]) == "function") {
                    item[pn](self._lastEditedItem[pn]);
                }
            }
            item.DataState = DataState.None;
            self.ModifiedItems.remove(item);
            self._lastEditedItem = null;
        }
    };

    self.RemoveItem = function (item) {
        if (self.options.canRemove) {
            if (!self.options.canRemove(item)) {
                return false;
            }
        }
        if (self.IsNew(item)) {
            self.NewItems.remove(item);
        }
        ShowConfirm("提示信息", "确认删除？", function () {
            self.Data.remove(item);
            self.DeletedItems.push(item);
            self._lastDeletedItem = item;
            if (self.options.onRemove) {
                self.options.onRemove(item);
            }
        });
    };

    self.UndoRemoveItem = function () {
        if (self._lastDeletedItem) {
            var item = self._lastDeletedItem;
            if (item.IsNew) {
                self.NewItems.push(item);
            }
            else {
                item.DataState = DataState.None;
                self.Data.push(item);
            }
            self._lastDeletedItem = null;
        }
    };

    self.Save = function (beforeSave, onSuccess, onError) {
        if (self.options.validate) {
            if (!self.options.validate()) {
                return false;
            }
        }
        var url = self.options.getSaveUrl();
        var data = self.GetChangedData();
        if (beforeSave) {
            beforeSave(data);
        } else if (self.options.beforeSave) {
            self.options.beforeSave(data);
        }
        PostJson(url, data,
            function (info) {
                if (info.Status && info.Message && info.Status > 0) {
                    if (onError) {
                        onError(info.Message);
                    } else if (self.options.onSaveError) {
                        self.options.onSaveError(info.Message);
                    } else {
                        ShowMessage('出现错误', info.Message + '\r\n' + url);
                    }
                    return false;
                }
                if (onSuccess) {
                    onSuccess(info);
                } else if (self.options.onSaveSuccess) {
                    self.options.onSaveSuccess(info);
                } else {
                    self.GetData();
                    ShowMessage('提示信息', info.Message);
                }
                self.NewItems().splice(0, self.NewItems().length);
                self.ModifiedItems().splice(0, self.ModifiedItems().length);
                self.DeletedItems().splice(0, self.DeletedItems().length);
            },
            function (error) {
                ShowMessage('出现错误', error + '\r\n' + url);
                if (onError) {
                    onError(error);
                } else if (self.options.onSaveError) {
                    self.options.onSaveError(error);
                }
            });
    };

    self.GetChangedData = function () {
        return {
            NewItems: ko.mapping.toJS(self.NewItems),
            ModifiedItems: ko.mapping.toJS(self.ModifiedItems),
            DeletedItems: ko.mapping.toJS(self.DeletedItems)
        };
    };

    if (self.options.onInit) {
        self.options.onInit(self);
    }
}

function KVO(options) {
    var self = this;
    self.options = $.extend({
    }, options || {});

    self.Data = ko.observable(null);

    self.GetData = function (bindData, onSuccess, onError) {
        var url = self.options.getQueryUrl();
        GetJson(url, function (data) {
            if (!data) {
                var error = "未返回任何数据！";
                if (onError) {
                    onError(error);
                } else if (self.options.onQueryError) {
                    self.options.onQueryError(error);
                } else {
                    ShowMessage('出现错误', error + '\r\n' + url);
                }
            }
            if (data.Status && data.Message && data.Status > 0) {
                if (onError) {
                    onError(data.Message);
                } else if (self.options.onQueryError) {
                    self.options.onQueryError(data.Message);
                } else {
                    ShowMessage('出现错误', data.Message + '\r\n' + url);
                }
                return false;
            }
            var bindedData = null;
            if (bindData) {
                bindedData = bindData(data);
            } else if (self.options.bindData) {
                bindedData = self.options.bindData(data);
            } else {
                ko.mapping.fromJO(data, null, self.Data);
                bindedData = self.Data;
            }
            if (onSuccess) {
                onSuccess(bindedData);
            } else if (self.options.onQuerySuccess) {
                self.options.onQuerySuccess(bindedData);
            }
            if (typeof (ResizeIFrame) == "function") {
                ResizeIFrame();
            }
            //if (typeof (Progress) == "function") {
            //    Progress();
            //}
        }, function (error) {
            if (onError) {
                onError(error);
            } else if (self.options.onQueryError) {
                self.options.onQueryError(error);
            } else {
                ShowMessage('出现错误', error + '\r\n' + url);
            }
        });
    }

    self.Save = function (beforeSave, onSuccess, onError) {
        if (self.options.validate) {
            if (!self.options.validate()) {
                return false;
            }
        }
        var url = self.options.getSaveUrl();
        var data = ko.toJS(self.Data);
        if (beforeSave) {
            beforeSave(data);
        } else if (self.options.beforeSave) {
            self.options.beforeSave(data);
        }
        PostJson(url, data,
            function (info) {
                if (info.Status && info.Message && info.Status > 0) {
                    if (onError) {
                        onError(info.Message);
                    } else if (self.options.onSaveError) {
                        self.options.onSaveError(info.Message);
                    } else {
                        ShowMessage('出现错误', info.Message + '\r\n' + url);
                    }
                    return false;
                }
                if (onSuccess) {
                    onSuccess(info);
                } else if (self.options.onSaveSuccess) {
                    self.options.onSaveSuccess(info);
                } else {
                    self.GetData();
                    ShowMessage('提示信息', info.Message);
                }
            },
            function (error) {
                if (onError) {
                    onError(error);
                } else if (self.options.onSaveError) {
                    self.options.onSaveError(error);
                } else {
                    ShowMessage('出现错误', error + '\r\n' + url);
                }
            });
    };

    if (self.options.onInit) {
        self.options.onInit(self);
    }
}

function AfterRender(eles, data) {
    var rowSuffix = $.now().toString();
    var form = $(eles).eq(0).closest('form');
    form.find('input:not([type="hidden"]), select').each(function () {
        $(this).attr('id', $(this).attr('id') + rowSuffix);
        $(this).attr('name', $(this).attr('name') + rowSuffix);
        $(this).next().attr('data-valmsg-for', $(this).next().attr('data-valmsg-for') + rowSuffix);
    });
    EnableFormValidation(form);
}


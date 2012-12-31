var locations = [];

function LoadLocation(ns) {
    var loc = null;
    ns = ns.toLowerCase();
    for (var i = 0; i < locations.length; i++) {
        if (locations[i].NameSpace == ns) {
            loc = locations[i];
        }
    }
    if (loc == null) {
        var jsFile = '/Localization/GetLocalScript/' + ns;
        $.ajax({
            url: jsFile,
            dataType: "json",
            global: false,
            async: false,
            success: function (data) {
                loc = new Object();
                loc.NameSpace = ns;
                loc.Resources = data;
                locations.push(loc);
            },
            error: function () {
            }
        });
    }

    return loc;
}

function Local(ns, resName) {
    var val = resName;
    var loc = LoadLocation(ns);
    if (loc && loc.Resources) {
        val = eval('loc.Resources.' + resName);
    }
    return val;
}

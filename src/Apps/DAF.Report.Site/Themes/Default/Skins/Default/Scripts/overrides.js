function OpenSelf(url) {
    if (IsCrossDomain(url)) {
        url = '/?url=' + escape('/CrossSite/Get?url=' + escape(url));
        url = AppendUrl(url, "cros", "1");
    }
    window.location.href = url;
    return false;
}

function OpenBlank(url) {
    var frame = $('#home-body').is('div') ? 'home-body' : 'admin-body';
    var home = $('#home-body').is('div') ? 'Home' : 'Admin';
    url = AppendUrl(url, 'frame', frame);
    if (IsCrossDomain(url)) {
        url = '/?url=' + escape('/CrossSite/Get?url=' + escape(url));
        url = AppendUrl(url, "cros", "1");
    }
    url = '/site/' + home + '/Index?url=' + escape(url);
    window.open(url, '_blank');
    return false;
}

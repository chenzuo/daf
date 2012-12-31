function OpenSelf(url) {
    LoadHtml("#admin-body", url);
    return false;
}

function OpenBlank(url) {
    window.open(url, '_blank');
    return false;
}

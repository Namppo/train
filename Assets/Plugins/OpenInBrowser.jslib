mergeInto(LibraryManager.library, {
    OpenInBrowser: function (url) {
        var jsUrl = UTF8ToString(url);
        window.open(jsUrl, '_blank');
    }
});

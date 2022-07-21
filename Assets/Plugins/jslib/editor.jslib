mergeInto(LibraryManager.library, {
    selectTrans:function (artData) {
        var data = UTF8ToString(artData);
        __UnityLib__.get_art_data(data);
    },
    unSelectTrans:function () {
        __UnityLib__.unSelect();
    },
    changeMouseStyle:function (styleName) {
        var data = UTF8ToString(artData);
        __UnityLib__.change_mouse(styleName);
    },
    updateEnd:function () {
        __UnityLib__.update_end();
    },
});
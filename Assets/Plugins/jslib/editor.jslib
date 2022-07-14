mergeInto(LibraryManager.library, {
    selectTrans:function (artData) {
        var data = Pointer_stringify(artData);
        __UnityLib__.get_art_data(data);
    },
    unSelectTrans:function () {
        __UnityLib__.unSelect();
    },
    changeMouseStyle:function (styleName) {
        var data = Pointer_stringify(artData);
        __UnityLib__.change_mouse(styleName);
    },
});
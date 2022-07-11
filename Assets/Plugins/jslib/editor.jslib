mergeInto(LibraryManager.library, {
    selectTrans:function (artData) {
        var data = Pointer_stringify(artData);
        __UnityLib__.select_trans(data);
    },
});
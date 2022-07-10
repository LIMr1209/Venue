mergeInto(LibraryManager.library, {
    selectTrans:function (artName) {
        var data = Pointer_stringify(artName);
        __UnityLib__.select_trans(data);
    },
});
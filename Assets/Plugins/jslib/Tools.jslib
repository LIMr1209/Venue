mergeInto(LibraryManager.library, {
    showDialog: function (str) {
        var data = Pointer_stringify(str);
        __UnityLib__.showDialog(data);
      },
    loaded: function () {
        __UnityLib__.loaded();
      },
});
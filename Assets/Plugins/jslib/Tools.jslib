mergeInto(LibraryManager.library, {
    showDialog: function (str) {
        var data = Pointer_stringify(str);
        __UnityLib__.showDialog(data);
      },
    loadScene: function () {
        __UnityLib__.loadScene();
      },
});
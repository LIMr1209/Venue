mergeInto(LibraryManager.library, {
    showDialog: function (str) {
        var data = Pointer_stringify(str);
        __UnityLib__.errorMessage(data);
    },
    loadScene: function () {
        __UnityLib__.loadScene();
    },
    showFocusWindow: function () {
        __UnityLib__.showFocusWindow();
    },
    showFocusTipsWindow: function () {
        __UnityLib__.showFocusTipsWindow();
    },
});
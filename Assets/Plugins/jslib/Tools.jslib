mergeInto(LibraryManager.library, {
    showDialog: function (str) {
        var data = Pointer_stringify(str);
        __UnityLib__.errorMessage(data);
    },
    loadScene: function () {
        __UnityLib__.loadScene();
    },
    showFocusWindow: function (id) {
        __UnityLib__.focus_end(id);
    },
    showFocusTipsWindow: function (show) {
        __UnityLib__.focus_tab(show);
    },
    sendProcess: function (p) {
        __UnityLib__.loading(p);
    },
});
mergeInto(LibraryManager.library, {
    showDialog: function (str) {
        var data = UTF8ToString(str);
        __UnityLib__.errorMessage(data);
    },
    loadScene: function () {
        __UnityLib__.loadScene();
    },
    showFocusWindow: function (artId) {
        var data = UTF8ToString(artId);
        __UnityLib__.focus_end(data);
    },
    showFocusTipsWindow: function (show) {
        __UnityLib__.focus_tab(show);
    },
    sendProcess: function (p) {
        __UnityLib__.loading(p);
    },
    canalFocus:function () {
        __UnityLib__.canal_focus();
    },
});
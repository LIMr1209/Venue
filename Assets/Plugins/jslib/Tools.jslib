mergeInto(LibraryManager.library, {
    showDialog: function (str) {
        var data = Pointer_stringify(str);
        __UnityLib__.errorMessage(data);
    },
    loadScene: function () {
        __UnityLib__.loadScene();
    },
    showFocusWindow: function (artId) {
        var data = Pointer_stringify(artId);
        console.log("unity send finish 999:"+data)
        __UnityLib__.focus_end(data);
    },
    showFocusTipsWindow: function (show) {
        __UnityLib__.focus_tab(show);
    },
    sendProcess: function (p) {
        console.log("load process:"+p)
        __UnityLib__.loading(p);
    },
    canalFocus:function () {
        __UnityLib__.canal_focus();
    },
});
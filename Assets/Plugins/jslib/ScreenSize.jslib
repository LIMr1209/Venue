mergeInto(LibraryManager.library, {
    GetWindowWidth:function()
    {
        var width = window.screen.availWidth;
        return width;
    },

    GetWindowHeight:function()
    {
        var height = window.screen.availHeight;
        return height;
    },
    ResetCanvasSize:function(width,height)
    {
        document.getElementById("unity-canvas").style.width = width + "px";
        document.getElementById("unity-canvas").style.height = height + "px";
    },
});
mergeInto(LibraryManager.library, {
    GetInviteCode:function()
    {
        var url = window.location.href;
        var index = url.lastIndexOf("\/");
        return url.substring(index + 1,url.length);
    }, 
    GetProjectId:function()
    {
        var url = window.location.href;
        var index = url.lastIndexOf("\/");
        return url.substring(index + 1,url.length);
    }, 

    GetUserToken:function(tokeName)
    {
        var v = document.cookie.match('(^|;) ?' + tokeName + '=([^;]*)(;|$)');
        return v ? v[2] : ''
    },
    GoodView:function(id)
    {
        // display work view 
    },
    showDialog: function (str) {
        var data = Pointer_stringify(str);
        __UnityLib__.showDialog(data);
      },
});
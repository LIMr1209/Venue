mergeInto(LibraryManager.library, {
    GetSceneId:function()
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
        
    },
});
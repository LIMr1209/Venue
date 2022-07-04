using System.IO;
using UnityEngine;
using static DefaultNamespace.JsonData;

namespace DefaultNamespace
{
    // js send message unity 接收
    public class JsSend : MonoBehaviour
    {
        // 获取初始化参数 场景url 实例化场景对象
        public void GetInitParam(string strParams)
        {
            Debug.Log("接收到的场景url: "+strParams);
            FindObjectOfType<InitialScene>().sceneUrl = Path.Combine(strParams, "scene.ab").Replace("\\","/");
            // 默认脚本组件禁用, 接收到前端的参数 启用脚本组件
            FindObjectOfType<AbInit>().enabled = true;
            FindObjectOfType<InitialScene>().enabled = true;
        }
        
        // 接收画框名字 作品id 作品url  替换场景中画框的材质贴图
        public void JsReplaceArtImage(string strParams)
        {
            Debug.Log("接收作品");
            Debug.Log(strParams);
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            OpusShow.ReplaceArtImage(data,true);
        }

        public void JsSetframeArt(string strParams)
        {
            Debug.Log("接收画框");
            Debug.Log(strParams);
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            OpusShow.ReplaceArtImage(data);
        }

        public void JsSetShowcaseArt(string strParams)
        {
            Debug.Log("接收画父物体");
            Debug.Log(strParams);
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            OpusShow.ReplaceArtImage(data);
        }

        // 接收画框名 聚焦画框
        public void JsFocusArt(string strParams)
        {
            Debug.Log("接收到的画框名:"+strParams);
            FindObjectOfType<OpusShow>().OnFocusArt(strParams);
        }
        
        // 取消聚焦画框
        public void JsCancelFocusArt()
        {
            FindObjectOfType<OpusShow>().CancelFocusArt();
        }

        
        public void JsConsoleFps()
        {
            Debug.Log(Mathf.Ceil(FindObjectOfType<InitialScene>().fps));
        }

    }
}
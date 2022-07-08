using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static DefaultNamespace.JsonData;

namespace DefaultNamespace
{
    // js send message unity 接收
    public class JsSend : MonoBehaviour
    {
        private InitialScene _initialScene;
        private AbInit _abInit;
        private OpusShow _opusShow;
        private AddController _addController;

        public void Start()
        {
            _initialScene = FindObjectOfType<InitialScene>();
            _abInit = FindObjectOfType<AbInit>();
            _opusShow = FindObjectOfType<OpusShow>();
            _addController = FindObjectOfType<AddController>();

        }

        // 获取初始化参数 场景url 实例化场景对象
        public void GetInitParam(string strParams)
        {
            Debug.Log("接收到的场景url: "+strParams);
            _initialScene.sceneUrl = Path.Combine(strParams, "scene.ab").Replace("\\","/");
            // 默认脚本组件禁用, 接收到前端的参数 启用脚本组件
            _abInit.enabled = true;
            _initialScene.enabled = true;
        }
        
        // 接收画框名字 作品id 作品url  替换场景中画框的材质贴图
        public void JsReplaceArtImage(string strParams)
        {
            Debug.Log("接收作品");
            Debug.Log(strParams);
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            OpusShow.ReplaceArtImage(data);
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
            _opusShow.OnFocusArt(strParams);
        }
        
        // 取消聚焦画框
        public void JsCancelFocusArt()
        {
            _opusShow.CancelFocusArt();
        }
        
        // 更换角色
        public void JsUpdateCharacter(string characterId)
        {
            _addController.characterId = characterId;
            _addController.UpdateCharacter();
        }

        public void JsConsoleFps()
        {
            Debug.Log(Mathf.Ceil(_initialScene.fps));
        }

    }
}
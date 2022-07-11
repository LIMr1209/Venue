using System.IO;
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
        // private EditorOpusShow _editorOpusShow;
        private AddController _addController;

        public void Start()
        {
            _initialScene = FindObjectOfType<InitialScene>();
            _abInit = FindObjectOfType<AbInit>();
            _opusShow = FindObjectOfType<OpusShow>();
            // _editorOpusShow = FindObjectOfType<EditorOpusShow>();
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
        
        
        public void JsConsoleFps()
        {
            Debug.Log(Mathf.Ceil(_initialScene.fps));
        }
        
        // 更换角色
        public void JsUpdateCharacter(string characterId)
        {
            _addController.characterId = characterId;
            _addController.UpdateCharacter();
        }

        // 编辑器中 js 发送画框位置 图片等信息 有就更换 没有就忽略
        public void JsUpdateTransArt(string strParams)
        {
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            OpusShow.ReplaceArtImage(data);
        }
        
        // 编辑器中 js 删除画框 清空 纹理 对象等内存
        public void JsDeleteArt(string artName)
        {
            OpusShow.DeleteArt(artName);
        }
        
        // 编辑器中 js 复制画框 新名字 位置一样 引用得材质 需要复制一份
        public void JsCopyArt(string artName)
        {
            OpusShow.CopyArt(artName);
        }

    }
}
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        private ArtUpdateTrans _artUpdateTrans;
        private ArtInit _artInit;

        public void Awake()
        {
            _initialScene = FindObjectOfType<InitialScene>();
            _abInit = FindObjectOfType<AbInit>();
            _opusShow = FindObjectOfType<OpusShow>();
            _artInit = FindObjectOfType<ArtInit>();
            _addController = FindObjectOfType<AddController>();
            _artUpdateTrans = FindObjectOfType<ArtUpdateTrans>();
        }

        // 获取初始化参数 场景url 实例化场景对象
        public void GetInitParam(string strParams)
        {
            Debug.Log("接收到的场景url: " + strParams);
            _initialScene.sceneUrl = Path.Combine(strParams, "scene.ab").Replace("\\", "/");
            _initialScene.showcaseUrl = Path.Combine(strParams, "showcaseroot.ab").Replace("\\", "/");
            // 默认脚本组件禁用, 接收到前端的参数 启用脚本组件
            _abInit.enabled = true;
            _initialScene.enabled = true;
        }

        // 编辑模式 启用 transformGizmo 组件
        public void JsSetModeEditor()
        {
            _artUpdateTrans.enabled = true;
        }

        // 接收画框名 聚焦画框
        public void JsFocusArt(string strParams)
        {
            Debug.Log("接收到的画框名:" + strParams);
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
        public void JsReplaceArtImage(string strParams)
        {
            Debug.Log("接收初始化参数");
            Debug.Log(strParams);
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            _artInit.InitArtImage(data);
        }

        // 编辑器中 js 发送已删除的画框
        public void JsInitDeleteArt(string strParams)
        {
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            _artInit.InitDeleteArt(data);
        }

        public void JsUpdateTrans(string strParams)
        {
            Debug.Log("接收编辑参数");
            Debug.Log(strParams);
            ArtData data = JsonUtility.FromJson<ArtData>(strParams);
            _artInit.UpdateArt(data);
        }

        // 编辑器中 js 删除画框 清空 纹理 对象等内存
        public void JsDeleteArt(string artName)
        {
            _artInit.DeleteArt(artName);
        }

        // 编辑器中 js 复制画框 新名字 位置一样 引用得材质 需要复制一份
        public void JsCopyArt(string artName)
        {
            _artInit.CopyArt(artName, true);
        }

        // 锁定画框
        public void JsLockArt(string artName)
        {
            _artInit.LockArt(artName, true);
        }

        // 解锁画框
        public void JsUnlockArt(string artName)
        {
            _artInit.LockArt(artName, false);
        }
        
        // 重置场景
        public void JsReloadScene()
        {
            _abInit.ReloadManifest();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
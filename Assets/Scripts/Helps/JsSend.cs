using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DefaultNamespace.JsonData;

namespace DefaultNamespace
{
    // js send message unity ����
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

        // ��ȡ��ʼ������ ����url ʵ������������
        public void GetInitParam(string strParams)
        {
            Debug.Log("���յ��ĳ���url: " + strParams);
            _initialScene.sceneUrl = Path.Combine(strParams, "scene.ab").Replace("\\", "/");
            _initialScene.showcaseUrl = Path.Combine(strParams, "showcaseroot.ab").Replace("\\", "/");
            // Ĭ�Ͻű��������, ���յ�ǰ�˵Ĳ��� ���ýű����
            _abInit.enabled = true;
            _initialScene.enabled = true;
        }

        // �༭ģʽ ���� transformGizmo ���
        public void JsSetModeEditor()
        {
            _artUpdateTrans.enabled = true;
        }

        // ���ջ����� �۽�����
        public void JsFocusArt(string strParams)
        {
            Debug.Log("���յ��Ļ�����:" + strParams);
            _opusShow.OnFocusArt(strParams);
        }

        // ȡ���۽�����
        public void JsCancelFocusArt()
        {
            _opusShow.CancelFocusArt();
        }

        public void JsConsoleFps()
        {
            Debug.Log(Mathf.Ceil(_initialScene.fps));
        }

        // ������ɫ
        public void JsUpdateCharacter(string characterId)
        {
            _addController.characterId = characterId;
            _addController.UpdateCharacter();
        }

        // �༭���� js ���ͻ���λ�� ͼƬ����Ϣ �о͸��� û�оͺ���
        public void JsReplaceArtImage(string strParams)
        {
            Debug.Log("���ճ�ʼ������");
            Debug.Log(strParams);
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            _artInit.InitArtImage(data);
        }

        // �༭���� js ������ɾ���Ļ���
        public void JsInitDeleteArt(string strParams)
        {
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            _artInit.InitDeleteArt(data);
        }

        public void JsUpdateTrans(string strParams)
        {
            Debug.Log("���ձ༭����");
            Debug.Log(strParams);
            ArtData data = JsonUtility.FromJson<ArtData>(strParams);
            _artInit.UpdateArt(data);
        }

        // �༭���� js ɾ������ ��� ���� ������ڴ�
        public void JsDeleteArt(string artName)
        {
            _artInit.DeleteArt(artName);
        }

        // �༭���� js ���ƻ��� ������ λ��һ�� ���õò��� ��Ҫ����һ��
        public void JsCopyArt(string artName)
        {
            _artInit.CopyArt(artName, true);
        }

        // ��������
        public void JsLockArt(string artName)
        {
            _artInit.LockArt(artName, true);
        }

        // ��������
        public void JsUnlockArt(string artName)
        {
            _artInit.LockArt(artName, false);
        }
        
        // ���ó���
        public void JsReloadScene()
        {
            _abInit.ReloadManifest();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
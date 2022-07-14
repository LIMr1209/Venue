using System.IO;
using UnityEngine;
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

        public void Awake()
        {
            _initialScene = FindObjectOfType<InitialScene>();
            _abInit = FindObjectOfType<AbInit>();
            _opusShow = FindObjectOfType<OpusShow>();
            _addController = FindObjectOfType<AddController>();
            _artUpdateTrans = FindObjectOfType<ArtUpdateTrans>();
        }

        // ��ȡ��ʼ������ ����url ʵ������������
        public void GetInitParam(string strParams)
        {
            Debug.Log(11111111);
            Debug.Log("���յ��ĳ���url: " + strParams);
            _initialScene.sceneUrl = Path.Combine(strParams, "scene.ab").Replace("\\", "/");
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
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            _opusShow.InitArtImage(data);
        }

        // �༭���� js ������ɾ���Ļ���
        public void JsInitDeleteArt(string strParams)
        {
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            _opusShow.InitDeleteArt(data);
        }

        public void JsUpdateTrans(string strParams)
        {
            Debug.Log("���ձ༭����");
            Debug.Log(strParams);
            ArtData data = JsonUtility.FromJson<ArtData>(strParams);
            _opusShow.UpdateArt(data);
        }

        // �༭���� js ɾ������ ��� ���� ������ڴ�
        public void JsDeleteArt(string artName)
        {
            _opusShow.DeleteArt(artName);
        }

        // �༭���� js ���ƻ��� ������ λ��һ�� ���õò��� ��Ҫ����һ��
        public void JsCopyArt(string artName)
        {
            _opusShow.CopyArt(artName, true);
        }

        // ��������
        public void JsLockArt(string artName)
        {
            _opusShow.LockArt(artName, true);
        }

        // ��������
        public void JsUnlockArt(string artName)
        {
            _opusShow.LockArt(artName, false);
        }
    }
}
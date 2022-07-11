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

        // ��ȡ��ʼ������ ����url ʵ������������
        public void GetInitParam(string strParams)
        {
            Debug.Log("���յ��ĳ���url: "+strParams);
            _initialScene.sceneUrl = Path.Combine(strParams, "scene.ab").Replace("\\","/");
            // Ĭ�Ͻű��������, ���յ�ǰ�˵Ĳ��� ���ýű����
            _abInit.enabled = true;
            _initialScene.enabled = true;
        }
        
        // ���ջ������� ��Ʒid ��Ʒurl  �滻�����л���Ĳ�����ͼ
        public void JsReplaceArtImage(string strParams)
        {
            Debug.Log("������Ʒ");
            Debug.Log(strParams);
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            OpusShow.ReplaceArtImage(data);
        }

        public void JsSetframeArt(string strParams)
        {
            Debug.Log("���ջ���");
            Debug.Log(strParams);
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            OpusShow.ReplaceArtImage(data);
        }

        public void JsSetShowcaseArt(string strParams)
        {
            Debug.Log("���ջ�������");
            Debug.Log(strParams);
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            OpusShow.ReplaceArtImage(data);
        }

        // ���ջ����� �۽�����
        public void JsFocusArt(string strParams)
        {
            Debug.Log("���յ��Ļ�����:"+strParams);
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
        public void JsUpdateTransArt(string strParams)
        {
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            OpusShow.ReplaceArtImage(data);
        }
        
        // �༭���� js ɾ������ ��� ���� ������ڴ�
        public void JsDeleteArt(string artName)
        {
            OpusShow.DeleteArt(artName);
        }
        
        // �༭���� js ���ƻ��� ������ λ��һ�� ���õò��� ��Ҫ����һ��
        public void JsCopyArt(string artName)
        {
            OpusShow.CopyArt(artName);
        }

    }
}
using System.IO;
using UnityEngine;
using static DefaultNamespace.JsonData;

namespace DefaultNamespace
{
    // js send message unity ����
    public class JsSend : MonoBehaviour
    {
        // ��ȡ��ʼ������ ����url ʵ������������
        public void GetInitParam(string strParams)
        {
            Debug.Log("���յ��ĳ���url: "+strParams);
            FindObjectOfType<InitialScene>().sceneUrl = Path.Combine(strParams, "scene.ab").Replace("\\","/");
            // Ĭ�Ͻű��������, ���յ�ǰ�˵Ĳ��� ���ýű����
            FindObjectOfType<AbInit>().enabled = true;
            FindObjectOfType<InitialScene>().enabled = true;
        }
        
        // ���ջ������� ��Ʒid ��Ʒurl  �滻�����л���Ĳ�����ͼ
        public void JsReplaceArtImage(string strParams)
        {
            Debug.Log("������Ʒ");
            Debug.Log(strParams);
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            OpusShow.ReplaceArtImage(data,true);
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
            FindObjectOfType<OpusShow>().OnFocusArt(strParams);
        }
        
        // ȡ���۽�����
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
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
            if (string.IsNullOrEmpty(Globle.sceneId))
            {
                FindObjectOfType<InitialScene>().sceneUrl = strParams;
                // Ĭ�Ͻű��������, ���յ�ǰ�˵Ĳ��� ���ýű����
                FindObjectOfType<AbInit>().enabled = true;
                FindObjectOfType<InitialScene>().enabled = true;
            }
        }
        
        // ���ջ������� ��Ʒid ��Ʒurl  �滻�����л���Ĳ�����ͼ
        public void JsReplaceArtImage(string strParams)
        {
            ArtData[] data = JsonHelper.GetJsonArray<ArtData>(strParams);
            OpusShow.ReplaceArtImage(data);
        }
        // ���ջ����� �۽�����
        public void JsFocusArt(string strParams)
        {
            FindObjectOfType<OpusShow>().FocusArt(strParams);
        }


    }
}
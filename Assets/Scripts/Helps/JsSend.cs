using UnityEngine;

namespace DefaultNamespace
{
    // js send message unity ����
    public class JsSend : MonoBehaviour
    {
        // ��ȡ��ʼ������ ����url ʵ������������
        public void GetInitParam(string strParams)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
        {
            string[] words = strParams.Split('#');
            Globle.sceneId = words[0];
            Debug.Log("ǰ�˷��Ͳ��� sceneId: "+Globle                                                                                                                                                                                  .sceneId);
            
            if (string.IsNullOrEmpty(Globle.sceneId))
            {
                // Ĭ�Ͻű��������, ���յ�ǰ�˵Ĳ��� ���ýű����
                FindObjectOfType<AbInit>().enabled = true;
                FindObjectOfType<InitialScene>().enabled = true;
            }
        }
        
        // ���ջ������� ��Ʒid ��Ʒurl  �滻�����л���Ĳ�����ͼ
        public void ReplaceArtImage()
        {
            
        }
        // ���ջ���id �۽�����
        public void FocusArt()
        {
            
        }


    }
}
using UnityEngine;

namespace DefaultNamespace
{
    // js send message unity ����
    public class JsSend : MonoBehaviour
    {
        // ��ȡ��ʼ������ ����id ��Ŀid ������ 
        public void GetInitParam(string strParams)
        {
            string[] words = strParams.Split('#');
            Globle.sceneId = words[0];
            Globle.projectId = words[1];
            Globle.inviteCode = words[2];
            Globle.token = words[3];
            if (string.IsNullOrEmpty(Globle.sceneId))
            {
                // Ĭ�Ͻű��������, ���յ�ǰ�˵Ĳ��� ���ýű����
                FindObjectOfType<GameManager>().enabled = true;
                FindObjectOfType<InitialScene>().enabled = true;
            }
        }
    }
}
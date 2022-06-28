using System.Runtime.InteropServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class Tools : MonoBehaviour
    {
        [DllImport("__Internal")]
        public static extern void showDialog(string message);  // ��ʾ������ʾ
        
        [DllImport("__Internal")]
        public static extern void loadScene(); // ����������֪ͨ��ʾ ui
        
        [DllImport("__Internal")]
        public static extern void showFocusWindow(string artId); // �۽�����ʾ�۽����� ui
        
        [DllImport("__Internal")]
        public static extern void showFocusTipsWindow(bool show); // ����������ʾ�۽���ʾ ui
        
        [DllImport("__Internal")]
        public static extern void sendProcess(float p); // ���ͽ�������ǰ��
        
        [DllImport("__Internal")]
        public static extern void canalFocus(); // ���ͽ�������ǰ��
        
    }
}
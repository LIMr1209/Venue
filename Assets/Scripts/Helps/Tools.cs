using System.Runtime.InteropServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class Tools : MonoBehaviour
    {
        [DllImport("__Internal")]
        public static extern void showDialog(string message);  // ��ʾ������ʾ
        
        [DllImport("__Internal")]
        public static extern void loaded(); // ����������֪ͨ��ʾ ui
        
    }
}
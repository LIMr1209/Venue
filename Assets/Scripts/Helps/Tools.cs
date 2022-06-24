using System.Runtime.InteropServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class Tools : MonoBehaviour
    {
        [DllImport("__Internal")]
        public static extern void showDialog(string message);  // 显示错误提示
        
        [DllImport("__Internal")]
        public static extern void loaded(); // 场景加载完通知显示 ui
        
    }
}
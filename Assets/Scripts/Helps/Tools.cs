using System.Runtime.InteropServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class Tools : MonoBehaviour
    {
        [DllImport("__Internal")]
        public static extern void showDialog(string message);  // 显示错误提示
        
        [DllImport("__Internal")]
        public static extern void loadScene(); // 场景加载完通知显示 ui
        
        [DllImport("__Internal")]
        public static extern void showFocusWindow(); // 聚焦后显示聚焦窗口 ui
        
        [DllImport("__Internal")]
        public static extern void showFocusTipsWindow(); // 靠近画框显示聚焦提示 ui
        
        [DllImport("__Internal")]
        public static extern void sendProcess(float p); // 发送进度条给前端
        
    }
}
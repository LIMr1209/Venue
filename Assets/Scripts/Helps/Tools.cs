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
        public static extern void showFocusWindow(string artId); // 聚焦后显示聚焦窗口 ui
        
        [DllImport("__Internal")]
        public static extern void showFocusTipsWindow(bool show); // 靠近画框显示聚焦提示 ui
        
        [DllImport("__Internal")]
        public static extern void sendProcess(float p); // 发送进度条给前端
        
        [DllImport("__Internal")]
        public static extern void canalFocus(); // 发送进度条给前端
        
        [DllImport("__Internal")]
        public static extern void selectTrans(string artData); // 选中时通知前端 选中的名字 显示ui 前端记录 选中的画框 更改位置时需要传过来
        
        [DllImport("__Internal")]
        public static extern void unSelectTrans(); // 取消选中
        
        [DllImport("__Internal")]
        public static extern void updateEnd(); // 更新角色完成
        
        
        
    }
}
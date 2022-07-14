using UnityEngine;

namespace DefaultNamespace
{
    // 给游戏物体 设置自定义属性 记录和前端交互的信息
    public class CustomAttr : MonoBehaviour
    {
        public string id = ""; // 记录自定义属性 artId
        public string imageUrl = ""; // 
        public float[] location = new[] {0.0f, 0.0f, 0.0f}; // 记录增量位置
        public float rotateS = 0.0f; // 记录增量旋转
        public float scaleS = 1.0f; // 记录增量缩放
        public string cloneBase = ""; // 如果是复制出来的 需要记录基体
        public bool isDel = false;
        public int nKind = 0;
        public Vector3 oldLocation;
        public Quaternion oldRotate;
        public Vector3 oldScale;
    }
}
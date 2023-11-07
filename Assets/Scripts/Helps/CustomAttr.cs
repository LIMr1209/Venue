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
        public int kind = 0;
        public Vector3 oldLocation;
        public Quaternion oldRotate;
        public Vector3 oldScale; // 换画后 记录自适应的值
        public Vector3 originScale; // 最初的缩放
        public Vector2 realSize = Vector2.zero;
        
        public static CustomAttr GetCustomAttr(GameObject art)
        {
            CustomAttr customAttr = art.GetComponent<CustomAttr>();
            if (!customAttr)
            {
                Debug.Log("添加自定义属性组件");
                customAttr = art.AddComponent(typeof(CustomAttr)) as CustomAttr;
                Transform transform = art.transform;
                customAttr.oldLocation = transform.localPosition;
                customAttr.oldRotate = transform.localRotation;
                customAttr.originScale = customAttr.oldScale = transform.localScale;
            }

            return customAttr;
        }

        public void SetArtData(JsonData.ArtData artData)
        {
            if(!string.IsNullOrEmpty(artData.id))  id = artData.id;
            location = artData.location;
            rotateS = artData.rotateS;
            scaleS = artData.scaleS;
            kind = artData.kind;
            nKind = artData.nKind;
        }
        
        public void GetArtData(ref JsonData.ArtData artData)
        {
            artData.id = id;
            artData.imageUrl = imageUrl;
            artData.isDel = isDel;
            artData.nKind = nKind;
            artData.cloneBase = cloneBase;
            artData.location = location;
            artData.scaleS = scaleS;
            artData.rotateS = rotateS;
            artData.kind = kind;
        }
    }
}
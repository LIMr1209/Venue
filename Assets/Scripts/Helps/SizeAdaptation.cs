using UnityEngine;

namespace DefaultNamespace
{
    public class SizeAdaptation
    {
        public static Vector3 GetSize(GameObject obj)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            Collider collider = obj.GetComponent<Collider>();
            if (renderer)
            {
                return renderer.bounds.size;
            }

            if (collider)
            {
                return collider.bounds.size;
            }

            return Vector3.zero;
        }

        public static void SetSize(GameObject obj, Vector2 contentSize)
        {
            Transform parent = obj.transform.parent;
            Vector2 realSize = Vector2.zero;
            CustomAttr customAttr = parent.GetComponent<CustomAttr>();
            if (customAttr.realSize ==Vector2.zero)
            {
                Vector3 objVector3Size = GetSize(obj);
                realSize = new Vector2(objVector3Size.x, objVector3Size.y);
                customAttr.realSize = realSize;
            }
            if (realSize == Vector2.zero) return;
            float contentAspectRatio = contentSize.x / contentSize.y;
            float objAspectRatio = realSize.x / realSize.y;
            Vector2 newObjSize = Vector2.zero;
            if (contentAspectRatio > 1.0f)
            {
                if (objAspectRatio > 1.0f)
                {
                    // 高缩小
                    newObjSize.x = realSize.x;
                    newObjSize.y = realSize.x / contentAspectRatio;
                }
                else
                {
                    // 宽放大
                    newObjSize.x = realSize.y * contentAspectRatio;
                    newObjSize.y = realSize.y;
                }
            }
            else
            {
                if (objAspectRatio > 1.0f)
                {
                    // 宽缩小
                    newObjSize.x = realSize.y * contentAspectRatio;
                    newObjSize.y = realSize.y;
                }
                else
                {
                    // 高放大
                    newObjSize.x = realSize.x;
                    newObjSize.y = realSize.x / contentAspectRatio;
                }
            }

            Vector2 scaleRatio = new Vector2(newObjSize.x / realSize.x, newObjSize.y / realSize.y);

            parent.localScale = new Vector3(1, 1 * scaleRatio.x, 1 * scaleRatio.y);

            customAttr.oldScale = parent.localScale;
            // parent.GetComponent<CustomAttr>().oldLocation = parent.localPosition;
        }
    }
}
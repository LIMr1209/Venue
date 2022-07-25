using UnityEngine;

namespace DefaultNamespace
{
    public class SizeAdaptation
    {
        public static Vector3 GetSize(GameObject obj)
        {
            // Renderer renderer = obj.GetComponent<Renderer>();
            Collider collider = obj.GetComponent<Collider>();
            // if (renderer)
            // {
            //     return renderer.localBounds.size;
            // }

            if (collider)
            {
                return collider.bounds.size;
            }

            return Vector3.zero;
        }

        public static void SetSize(GameObject obj, Vector2 contentSize)
        {
            Transform parent = obj.transform.parent;
            CustomAttr customAttr = CustomAttr.GetCustomAttr(parent.gameObject);
            Vector2 realSize = customAttr.realSize;
            if (customAttr.realSize == Vector2.zero)
            {
                Vector3 objVector3Size = GetSize(obj);
                realSize = new Vector2(objVector3Size.z, objVector3Size.y);
                customAttr.realSize = realSize;
            }

            if (realSize == Vector2.zero) return;
            float contentAspectRatio = contentSize.x / contentSize.y;
            float objAspectRatio = realSize.x / realSize.y;
            Vector2 newObjSize = Vector2.zero;
            if (contentAspectRatio > 1.0f)
            {
                if (objAspectRatio > 1.0f || objAspectRatio == 1f)
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
                if (objAspectRatio > 1.0f || objAspectRatio == 1f)
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

            parent.localScale = new Vector3(customAttr.originScale.x * customAttr.scaleS, customAttr.originScale.y * scaleRatio.x * customAttr.scaleS,
                customAttr.originScale.z * scaleRatio.y * customAttr.scaleS);

            customAttr.oldScale = parent.localScale;
            // parent.GetComponent<CustomAttr>().oldLocation = parent.localPosition;
        }
    }
}
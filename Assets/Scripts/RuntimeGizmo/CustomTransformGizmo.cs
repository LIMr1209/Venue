using RuntimeGizmos;
using UnityEngine;

namespace DefaultNamespace
{
    public class CustomTransformGizmo : TransformGizmo
    {
        public override void GetTarget()
        {
            if (nearAxis == Axis.None && Input.GetMouseButtonDown(0))
            {
                bool isAdding = Input.GetKey(AddSelection);
                bool isRemoving = Input.GetKey(RemoveSelection);

                RaycastHit hitInfo;
                if (Physics.Raycast(myCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity,
                    selectionMask))
                {
                    Transform target = hitInfo.transform;
                    target = target.parent; // ÐÞ¸Ä»­¿ò¸¸¼¶
                    if (isAdding)
                    {
                        AddTarget(target);
                    }
                    else if (isRemoving)
                    {
                        RemoveTarget(target);
                    }
                    else if (!isAdding && !isRemoving)
                    {
                        ClearAndAddTarget(target);
                    }
                }
                else
                {
                    if (!isAdding && !isRemoving)
                    {
                        ClearTargets();
                    }
                }
            }
        }

        public override void UpdateTransForm(Vector3 originalPivot, Vector3 planeNormal, Vector3 previousMousePosition,
            TransformType transType, Vector3 projectedAxis, Vector3 axis, Vector3 currentSnapMovementAmount,
            float currentSnapScaleAmount, float currentSnapRotationAmount, Vector3 otherAxis1, Vector3 otherAxis2)
        {
            base.UpdateTransForm(originalPivot, planeNormal, previousMousePosition, transType, projectedAxis, axis,
                currentSnapMovementAmount, currentSnapScaleAmount, currentSnapRotationAmount, otherAxis1,
                otherAxis2);
#if !UNITY_EDITOR && UNITY_WEBGL
            Transform target = targetRootsOrdered[0];
            JsonData.ArtData artData = new JsonData.ArtData();
            artData.name = target.gameObject.name;
            if (target.TryGetComponent<CustomAttr>(out CustomAttr customAttr))
            {
                artData.id = target.GetComponent<CustomAttr>().artId;
            }
            artData.scale = new float[] {target.localScale[0], target.localScale[1], target.localScale[2]};
            artData.location = new float[]
                {target.localPosition[0], target.localPosition[1], target.localPosition[2]};
            artData.rotate = new float[]
            {
                target.localRotation.eulerAngles[0], target.localRotation.eulerAngles[1],
                target.localRotation.eulerAngles[2]
            };
            Tools.selectTrans(JsonUtility.ToJson(artData));
#endif
        }
    }
}
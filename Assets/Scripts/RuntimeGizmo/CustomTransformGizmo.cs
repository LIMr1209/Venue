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
                    target = target.parent; // 修改画框父级
                    if (isAdding)
                    {
                        AddTarget(target);
                        // 选中后 发送数据
                        SendArtData(target);
                    }
                    else if (isRemoving)
                    {
                        RemoveTarget(target);
                    }
                    else if (!isAdding && !isRemoving)
                    {
                        ClearAndAddTarget(target);
                        // 选中后 发送数据
                        SendArtData(target);
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

        public override void UpdateTransForm(Vector3 originalPivot, Vector3 planeNormal, ref Vector3 previousMousePosition,
            TransformType transType, Vector3 projectedAxis, Vector3 axis, ref Vector3 currentSnapMovementAmount,
            ref float currentSnapScaleAmount, ref float currentSnapRotationAmount, Vector3 otherAxis1, Vector3 otherAxis2)
        {
            base.UpdateTransForm(originalPivot, planeNormal, ref previousMousePosition, transType, projectedAxis, axis,
                ref currentSnapMovementAmount, ref currentSnapScaleAmount, ref currentSnapRotationAmount, otherAxis1,
                otherAxis2);
            // 变换时 发送数据
            SendArtData(targetRootsOrdered[0]);
        }

        public void SendArtData(Transform target)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            JsonData.ArtData artData = new JsonData.ArtData();
            artData.name = target.gameObject.name;
            if (target.TryGetComponent<CustomAttr>(out CustomAttr customAttr))
            {
                artData.id = customAttr.artId;
            }
            artData.scaleS = target.localScale[1];
            artData.location = new float[]
                {target.localPosition[0], target.localPosition[1], target.localPosition[2]};
            artData.rotateS = target.localRotation.eulerAngles[1];
            Tools.selectTrans(JsonUtility.ToJson(artData));
#endif
        }
    }
}
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
                if (Physics.Raycast(myCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
                {
                    if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer(Globle.focusArtLayer))
                    {
                        if (!isAdding && !isRemoving)  ClearTargets();
                        return;
                    }
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

        public override void ClearTargets(bool addCommand = true)
        {
            base.ClearTargets();
#if !UNITY_EDITOR && UNITY_WEBGL
            Tools.unSelectTrans();
#endif
        }

        public override void UpdateTransForm(Vector3 originalPivot, Vector3 planeNormal, ref Vector3 previousMousePosition,
            TransformType transType, Vector3 projectedAxis, Vector3 axis, ref Vector3 currentSnapMovementAmount,
            ref float currentSnapScaleAmount, ref float currentSnapRotationAmount, Vector3 otherAxis1, Vector3 otherAxis2)
        {
            base.UpdateTransForm(originalPivot, planeNormal, ref previousMousePosition, transType, projectedAxis, axis,
                ref currentSnapMovementAmount, ref currentSnapScaleAmount, ref currentSnapRotationAmount, otherAxis1,
                otherAxis2);
            
            // 变换时
            // 计算并更新自定义属性增量
            if (movement != Vector3.zero)
            {
                UpdateCustomAttr(targetRootsOrdered[0], movement);
                // 发送数据
                SendArtData(targetRootsOrdered[0]);
            }
        }

        public void SendArtData(Transform target)
        {
#if !UNITY_EDITOR && UNITY_WEBGL            
            CustomAttr customAttr = target.GetComponent<CustomAttr>();
            JsonData.ArtData artData = new JsonData.ArtData();
            artData.name = target.gameObject.name;
            artData.id = customAttr.artId;
            // artData.scaleS = target.localScale[1];
            // artData.location = new float[]
            //     {target.localPosition[0], target.localPosition[1], target.localPosition[2]};
            // artData.rotateS = target.localRotation.eulerAngles[1];
            artData.location = customAttr.location;
            artData.scaleS = customAttr.scaleS;
            artData.rotateS = customAttr.rotateS;
            Tools.selectTrans(JsonUtility.ToJson(artData));
#endif
        }

        public void UpdateCustomAttr(Transform target, Vector3 movement)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            CustomAttr customAttr = target.GetComponent<CustomAttr>();
            customAttr.location[0] = customAttr.location[0] + movement.x;
            customAttr.location[1] = customAttr.location[0] + movement.y;
            customAttr.location[2] = customAttr.location[0] + movement.z;
#endif
        }
    }
}
using RuntimeGizmos;

namespace DefaultNamespace
{
    public class EditorOpusShow : OpusShow
    {

        private TransformGizmo _transformGizmo;

        public override void Start()
        {
            base.Start();
            _transformGizmo = FindObjectOfType<CustomTransformGizmo>();
        }

        public override void CancelFocusArt()
        {
            base.CancelFocusArt();
            _transformGizmo.RemoveTarget(TargetArt.parent);

        }
    }
}
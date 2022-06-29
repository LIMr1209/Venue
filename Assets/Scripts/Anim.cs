using UnityEngine;

namespace DefaultNamespace
{
    public class Anim : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            int animIDYes = Animator.StringToHash("Yes");
            bool yes = animator.GetBool(animIDYes);
            animator.SetBool(animIDYes, false);
            yes = animator.GetBool(animIDYes);
            Debug.Log(yes);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }
    }
}
using UnityEngine;

namespace StarterAssets
{
    public class Anim : StateMachineBehaviour
    {
        private StarterAssetsInputs starterAssetsInputs;
        private int _animIDYes;
        private int _animIDNo;
        private int _animIDApplaud;
        private int _animIDSmile;
        private int _animIDDead;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            starterAssetsInputs = animator.gameObject.GetComponent<StarterAssetsInputs>();
            _animIDYes = Animator.StringToHash("Yes");
            _animIDNo = Animator.StringToHash("No");
            _animIDApplaud = Animator.StringToHash("Applaud");
            _animIDSmile = Animator.StringToHash("Smile");
            _animIDDead = Animator.StringToHash("Dead");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.SetBool(_animIDYes, false);
            animator.SetBool(_animIDNo, false);
            animator.SetBool(_animIDApplaud, false);
            animator.SetBool(_animIDSmile, false);
            animator.SetBool(_animIDDead, false);
            starterAssetsInputs.yes = false;
            starterAssetsInputs.no = false;
            starterAssetsInputs.applaud = false;
            starterAssetsInputs.smile = false;
            starterAssetsInputs.dead = false;
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
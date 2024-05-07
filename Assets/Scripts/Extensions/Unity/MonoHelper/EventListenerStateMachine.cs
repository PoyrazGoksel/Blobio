using UnityEngine;

namespace Extensions.Unity.MonoHelper
{
    public abstract class EventListenerStateMachine<T> : StateMachineBehaviour
    {
        [SerializeField] protected string MyStateName;
        
        protected T InternalEvents;
        protected Animator MyAnimator;
        protected float AnimLength;

        public void Construct(T playerWeaponInternalEvents, Animator myAnimator)
        {
            InternalEvents = playerWeaponInternalEvents;
            MyAnimator = myAnimator;
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            AnimLength = stateInfo.length;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnAnimationComplete();
        }

        protected void Play()
        {
            MyAnimator.Play(MyStateName);
        }

        protected abstract void OnAnimationComplete();

        public void OnControllerEnable()
        {
            RegisterEvents();
        }

        public void OnControllerDisable()
        {
            UnRegisterEvents();
        }

        protected abstract void RegisterEvents();

        protected abstract void UnRegisterEvents();
    }
}
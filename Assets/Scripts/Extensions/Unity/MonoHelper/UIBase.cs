namespace Extensions.Unity.MonoHelper
{
    public abstract class UIBase : EventListenerMono
    {
        protected bool IsActive { get; private set; }
        
        public virtual void SetActive(bool isActive = true)
        {
            IsActive = isActive;
        }
        
        protected override void RegisterEvents() {}

        protected override void UnRegisterEvents() {}
    }
}
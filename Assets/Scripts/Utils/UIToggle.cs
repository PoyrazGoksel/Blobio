using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public abstract class UIToggle : EventListenerMono
    {
        [SerializeField] protected Toggle _toggle;

        protected override void RegisterEvents()
        {
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        protected abstract void OnValueChanged(bool isActive);

        protected override void UnRegisterEvents()
        {
            _toggle.onValueChanged.RemoveListener(OnValueChanged);
        }
    }
}
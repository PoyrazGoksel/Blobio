using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public abstract class UIBTN : EventListenerMono
    {
        [SerializeField] private Button _button;

        protected override void RegisterEvents()
        {
            _button.onClick.AddListener(OnClick);
        }

        protected abstract void OnClick();

        protected override void UnRegisterEvents()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}
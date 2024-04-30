using Events;
using Installers;
using Utils;

namespace UI.MainMenu.SettingsPanel
{
    public class VibrationToggle : UIToggle
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            _toggle.isOn = ProjectInstaller.Instance.PlayerData.VibrationVal;
        }

        protected override void OnValueChanged(bool isActive)
        {
            MainMenuEvents.VibrationValueChanged?.Invoke(isActive);
        }
    }
}
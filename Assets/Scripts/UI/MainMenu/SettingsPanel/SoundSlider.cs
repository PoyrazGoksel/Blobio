using Events;
using Installers;
using Utils;

namespace UI.MainMenu.SettingsPanel
{
    public class SoundSlider : UISlider
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            _slider.value = ProjectInstaller.Instance.PlayerData.SoundVal;
        }

        protected override void OnValueChanged(float val)
        {
            MainMenuEvents.SoundValueChanged?.Invoke(val);
        }
    }
}
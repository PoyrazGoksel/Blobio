using Events;
using Utils;

namespace UI.MainMenu
{
    public class SettingsBTN : UIBTN
    {
        protected override void OnClick()
        {
            MainMenuEvents.SettingsBTN?.Invoke();
        }
    }
}
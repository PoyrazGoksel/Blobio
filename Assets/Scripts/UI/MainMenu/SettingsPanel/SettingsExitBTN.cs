using Events;
using Utils;

namespace UI.MainMenu.SettingsPanel
{
    public class SettingsExitBTN : UIBTN
    {
        protected override void OnClick()
        {
            MainMenuEvents.SettingsExitBTN?.Invoke();
        }
    }
}
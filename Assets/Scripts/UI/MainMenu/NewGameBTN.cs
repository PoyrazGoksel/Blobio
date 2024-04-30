using Events;
using UnityEngine;
using Utils;

namespace UI.MainMenu
{
    public class NewGameBTN : UIBTN
    {
        protected override void OnClick()
        {
            MainMenuEvents.NewGameBTN?.Invoke();
        }
    }
}
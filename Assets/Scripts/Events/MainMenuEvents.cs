using UnityEngine.Events;

namespace Events
{
    public static class MainMenuEvents
    {
        public static UnityAction SettingsBTN;
        public static UnityAction SettingsExitBTN;
        public static UnityAction NewGameBTN;
        public static UnityAction<float> SoundValueChanged;
        public static UnityAction<bool> VibrationValueChanged;
    }
}
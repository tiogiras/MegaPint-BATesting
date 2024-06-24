#if UNITY_EDITOR
using MegaPint.Editor.Scripts.Settings;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial class storing saveData values (AlphaButton) </summary>
internal static partial class SaveValues
{
    public static class BATesting
    {
        private static CacheValue <bool> s_applyPSTaskManager = new() {defaultValue = true};

        private static SettingsBase s_settings;

        public static bool ApplyPSTaskManager
        {
            get => ValueProperty.Get("ApplyPSTaskManager", ref s_applyPSTaskManager, _Settings);
            set => ValueProperty.Set("ApplyPSTaskManager", value, ref s_applyPSTaskManager, _Settings);
        }

        private static SettingsBase _Settings
        {
            get
            {
                if (MegaPintSettings.Exists())
                    return s_settings ??= MegaPintSettings.instance.GetSetting("BATesting");

                return null;
            }
        }
    }
}

}
#endif

#if UNITY_EDITOR
using MegaPint.Editor.Scripts.Settings;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial class storing saveData values (AlphaButton) </summary>
internal static partial class SaveValues
{
    public static class BaTesting
    {
        private static CacheValue <bool> s_applyPSTaskManager = new() {defaultValue = true};
        private static CacheValue <int> s_currentTaskIndex = new() {defaultValue = 0};
        
        private static CacheValue <bool> s_termsAgreement = new() {defaultValue = false};
        private static CacheValue <int> s_logSaveInterval = new() {defaultValue = 10};

        private static SettingsBase s_settings;

        public static bool ApplyPSTaskManager
        {
            get => ValueProperty.Get("ApplyPSTaskManager", ref s_applyPSTaskManager, _Settings);
            set => ValueProperty.Set("ApplyPSTaskManager", value, ref s_applyPSTaskManager, _Settings);
        }        
        
        public static int CurrentTaskIndex
        {
            get => ValueProperty.Get("CurrentTaskIndex", ref s_currentTaskIndex, _Settings);
            set => ValueProperty.Set("CurrentTaskIndex", value, ref s_currentTaskIndex, _Settings);
        }
        
        public static bool AgreedToTerms
        {
            get => ValueProperty.Get("AgreedToTerms", ref s_termsAgreement, _Settings);
            set => ValueProperty.Set("AgreedToTerms", value, ref s_termsAgreement, _Settings);
        }        
        
        public static int LogSaveInterval
        {
            get => ValueProperty.Get("LogSaveInterval", ref s_logSaveInterval, _Settings);
            set => ValueProperty.Set("LogSaveInterval", value, ref s_logSaveInterval, _Settings);
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

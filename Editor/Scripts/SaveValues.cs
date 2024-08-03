#if UNITY_EDITOR
using System;
using MegaPint.Editor.Scripts.Settings;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial class storing saveData values (AlphaButton) </summary>
internal static partial class SaveValues
{
    public static class BaTesting
    {
        public static Action <int> onLogSaveIntervalChanged;
        private static CacheValue <bool> s_applyPSTaskManager = new() {defaultValue = true};
        private static CacheValue <bool> s_applyPSOverview = new() {defaultValue = true};
        private static CacheValue <bool> s_applyPSRequirement = new() {defaultValue = true};

        private static CacheValue <int> s_currentTaskIndex = new() {defaultValue = 0};

        private static CacheValue <bool> s_termsAgreement = new() {defaultValue = false};
        private static CacheValue <int> s_logSaveInterval = new() {defaultValue = 10};

        private static SettingsBase s_settings;

        public static bool ApplyPSTaskManager
        {
            get => ValueProperty.Get("ApplyPSTaskManager", ref s_applyPSTaskManager, _Settings);
            set => ValueProperty.Set("ApplyPSTaskManager", value, ref s_applyPSTaskManager, _Settings);
        }

        public static bool ApplyPSOverview
        {
            get => ValueProperty.Get("ApplyPSOverview", ref s_applyPSOverview, _Settings);
            set => ValueProperty.Set("ApplyPSOverview", value, ref s_applyPSOverview, _Settings);
        }

        public static bool ApplyPSRequirement
        {
            get => ValueProperty.Get("ApplyPSRequirement", ref s_applyPSRequirement, _Settings);
            set => ValueProperty.Set("ApplyPSRequirement", value, ref s_applyPSRequirement, _Settings);
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
            set
            {
                ValueProperty.Set("LogSaveInterval", value, ref s_logSaveInterval, _Settings);
                onLogSaveIntervalChanged?.Invoke(value);
            }
        }

        private static SettingsBase _Settings
        {
            get
            {
                if (MegaPintMainSettings.Exists())
                    return s_settings ??= MegaPintMainSettings.instance.GetSetting("BATesting");

                return null;
            }
        }
    }

    public static class TestData
    {
        private static SettingsBase s_settings;

        public static int GetValue(string key, string valueIdentifier, int defaultValue)
        {
            return _Settings.GetValue($"{key}_{valueIdentifier}", defaultValue);
        }
        
        public static bool GetValue(string key, string valueIdentifier, bool defaultValue)
        {
            return _Settings.GetValue($"{key}_{valueIdentifier}", defaultValue);
        }
        
        public static float GetValue(string key, string valueIdentifier, float defaultValue)
        {
            return _Settings.GetValue($"{key}_{valueIdentifier}", defaultValue);
        }

        public static void SetValue(string key, string valueIdentifier, int value)
        {
            _Settings.SetValue($"{key}_{valueIdentifier}", value);
        }
        
        public static void SetValue(string key, string valueIdentifier, bool value)
        {
            _Settings.SetValue($"{key}_{valueIdentifier}", value);
        }
        
        public static void SetValue(string key, string valueIdentifier, float value, bool suppressSaving = false)
        {
            _Settings.SetValue($"{key}_{valueIdentifier}", value, suppressSaving);
        }
        
        private static SettingsBase _Settings
        {
            get
            {
                if (MegaPintMainSettings.Exists())
                    return s_settings ??= MegaPintMainSettings.instance.GetSetting("TestData");

                return null;
            }
        }
    }
}

}
#endif

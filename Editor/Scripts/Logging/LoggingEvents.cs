using MegaPint.Editor.Scripts.Logic;
using MegaPint.Editor.Scripts.Windows;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace MegaPint.Editor.Scripts.Logging
{

[InitializeOnLoad]
internal static class LoggingEvents
{
#if USING_AUTOSAVE
    private static bool s_autoSaveSave;
#endif
    static LoggingEvents()
    {
#if USING_PLAYMODESTARTSCENE

        #region PlayModeStartScene

        SaveValues.PlayModeStartScene.onToggleChanged += PlayModeStartSceneToggle;
        SaveValues.PlayModeStartScene.onStartSceneChanged += PlayModeStartSceneSceneChanged;
        SaveValues.PlayModeStartScene.onDisplayToolbarToggleChanged += PlayModeStartSceneToolbarToggle;

        PlayModeStartScene.onEnteredPlaymode += EnteredPlayMode;
        PlayModeStartScene.onEnteredPlaymodeWithStartScene += PlayModeStartSceneEnteredPlayModeWith;

        EditorSceneManager.sceneOpened += SceneChanged;

        #endregion

#endif

#if USING_AUTOSAVE

        #region AutoSave

        SaveValues.AutoSave.onIsActiveChanged += AutoSaveToggle;
        SaveValues.AutoSave.onDisplayToolbarToggleChanged += AutoSaveToolbarToggle;

        SaveValues.AutoSave.onDuplicatePathChanged += AutoSaveDuplicatePath;
        SaveValues.AutoSave.onWarningChanged += AutoSaveWarning;
        SaveValues.AutoSave.onIntervalChanged += AutoSaveInterval;
        SaveValues.AutoSave.onSaveModeChanged += AutoSaveSaveMode;

        AutoSave.onOpen += AutoSaveOpen;
        AutoSave.onClose += AutoSaveClose;

        EditorSceneManager.sceneSaved += SceneSaved;

        AutoSaveTimer.onTimerSaving += AutoSaveSaving;
        AutoSaveTimer.onTimerSaved += AutoSaveSaved;

        #endregion

#endif
    }

    #region Private Methods

    private static void AddLog(string categoryName, string logText)
    {
        LoggingManager.LogToCurrentSession(categoryName, logText);
    }

    #endregion

#if USING_PLAYMODESTARTSCENE

    #region PlayModeStartScene

    private static void PlayModeStartSceneEnteredPlayModeWith()
    {
        AddLog("PlayModeStartScene / Entered PlayMode", "Entered PlayMode with PlayModeStartScene");
    }

    private static void PlayModeStartSceneToggle(bool newValue)
    {
        AddLog("PlayModeStartScene / On/Off", newValue ? "Enabled" : "Disabled");
    }

    private static void PlayModeStartSceneSceneChanged()
    {
        AddLog(
            "PlayModeStartScene / Changed Scene",
            $"Changed to: {SaveValues.PlayModeStartScene.GetStartScene()?.name}");
    }

    private static void PlayModeStartSceneToolbarToggle(bool newValue)
    {
        AddLog("PlayModeStartScene / Changed DisplayToolbarToggle", newValue ? "Enabled" : "Disabled");
    }

    private static void EnteredPlayMode()
    {
        AddLog("Entered PlayMode", "Entered PlayMode without PlayModeStartScene");
    }

    private static void SceneChanged(Scene scene, OpenSceneMode mode)
    {
        AddLog("Scene Changed", $"Changed to: {scene.name}");
    }

    #endregion

#endif

#if USING_AUTOSAVE

    #region AutoSave

    private static void AutoSaveSaved()
    {
        s_autoSaveSave = false;
    }

    private static void AutoSaveSaving()
    {
        s_autoSaveSave = true;
    }

    private static void SceneSaved(Scene scene)
    {
        AddLog(s_autoSaveSave ? "AutoSave / Scene Saved" : "Scene Saved", $"Saved: {scene.name}");
    }

    private static void AutoSaveOpen()
    {
        AddLog("AutoSave / Open/Close", "Opened");
    }

    private static void AutoSaveClose()
    {
        AddLog("AutoSave / Open/Close", "Closed");
    }

    private static void AutoSaveToggle(bool newValue)
    {
        AddLog("AutoSave / On/Off", newValue ? "Enabled" : "Disabled");
    }

    private static void AutoSaveToolbarToggle(bool newValue)
    {
        AddLog("AutoSave / Changed DisplayToolbarToggle", newValue ? "Enabled" : "Disabled");
    }

    private static void AutoSaveDuplicatePath(string newValue)
    {
        AddLog("AutoSave / Changed DuplicatePath", newValue);
    }

    private static void AutoSaveWarning(bool newValue)
    {
        AddLog("AutoSave / Changed Warning", newValue ? "Enabled" : "Disabled");
    }

    private static void AutoSaveInterval(int newValue)
    {
        AddLog("AutoSave / Changed Interval", $"{newValue} seconds");
    }

    private static void AutoSaveSaveMode(int newValue)
    {
        AddLog("AutoSave / Changed SaveMode", newValue == 0 ? "Save As Current" : "Save As Duplicate");
    }

    #endregion

#endif
}

}

using MegaPint.Editor.Scripts.Drawer;
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
        #region PlayModeStartScene

#if USING_PLAYMODESTARTSCENE
        SaveValues.PlayModeStartScene.onToggleChanged += PlayModeStartSceneToggle;
        SaveValues.PlayModeStartScene.onStartSceneChanged += PlayModeStartSceneSceneChanged;
        SaveValues.PlayModeStartScene.onDisplayToolbarToggleChanged += PlayModeStartSceneToolbarToggle;

        PlayModeStartScene.onEnteredPlaymode += EnteredPlayMode;
        PlayModeStartScene.onEnteredPlaymodeWithStartScene += PlayModeStartSceneEnteredPlayModeWith;

        EditorSceneManager.sceneOpened += SceneChanged;
#endif

        #endregion

        #region AutoSave

#if USING_AUTOSAVE
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
#endif

        
        #endregion

        #region Screenshot

#if USING_SCREENSHOT
        WindowCapture.onOpen += ScreenshotWindowCaptureOpen;
        WindowCapture.onClose += ScreenshotWindowCaptureClose;
        WindowCapture.onRefresh += ScreenshotWindowCaptureRefresh;
        WindowCapture.onRender += ScreenshotWindowCaptureRender;
        WindowCapture.onExport += ScreenshotWindowCaptureExport;

        ShortcutCapture.onOpen += ScreenshotShortcutCaptureOpen;
        ShortcutCapture.onClose += ScreenshotShortcutCaptureClose;
        ShortcutCapture.onRefresh += ScreenshotShortcutCaptureRefresh;
        ShortcutCapture.onChangeState += ScreenshotShortcutCaptureChangeState;

        ContextMenu.Screenshot.onCaptureNow += ScreenshotCaptureNow;
        CameraCaptureDrawer.onCameraCaptureRendered += ScreenshotCameraCaptureRendered;
        CameraCaptureDrawer.onCameraCaptureExported += ScreenshotCameraCaptureExported;
#endif

        #endregion
    }

    private static void ScreenshotShortcutCaptureRefresh()
    {
        AddLog("Screenshot / ShortcutCapture Refresh", "Refreshing");
    }

    private static void ScreenshotShortcutCaptureChangeState(string name, bool active)
    {
        AddLog("Screenshot / ShortcutCapture ChangeState", $"{name} set to {active}");
    }

    private static void ScreenshotWindowCaptureRefresh()
    {
        AddLog("Screenshot / WindowCapture Refresh", "Refreshing");
    }

    private static void ScreenshotWindowCaptureRender(string name)
    {
        AddLog("Screenshot / WindowCapture Rendered", name);
    }

    private static void ScreenshotWindowCaptureExport()
    {
        AddLog("Screenshot / WindowCapture Exported", "Exported");
    }

    private static void ScreenshotCameraCaptureExported(string name)
    {
        AddLog("Screenshot / CameraCapture Exported", name);
    }

    private static void ScreenshotCameraCaptureRendered(string name)
    {
        AddLog("Screenshot / CameraCapture Rendered", name);
    }

    private static void ScreenshotCaptureNow(int count)
    {
        AddLog("Screenshot / CaptureNow", $"{count} cameras rendered");
    }

    #region Private Methods

    private static void AddLog(string categoryName, string logText)
    {
        LoggingManager.LogToCurrentSession(categoryName, logText);
    }

    #endregion


    #region PlayModeStartScene

#if USING_PLAYMODESTARTSCENE
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
#endif

    #endregion
    
    #region AutoSave

#if USING_AUTOSAVE
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
#endif

    #endregion
    
    #region Screenshot

#if USING_SCREENSHOT
    private static void ScreenshotWindowCaptureOpen()
    {
        AddLog("Screenshot / WindowCapture Open/Close", "Opened");
    }

    private static void ScreenshotWindowCaptureClose()
    {
        AddLog("Screenshot / WindowCapture Open/Close", "Closed");
    }

    private static void ScreenshotShortcutCaptureOpen()
    {
        AddLog("Screenshot / ShortcutCapture Open/Close", "Opened");
    }

    private static void ScreenshotShortcutCaptureClose()
    {
        AddLog("Screenshot / ShortcutCapture Open/Close", "Closed");
    }
#endif

    #endregion
}

}

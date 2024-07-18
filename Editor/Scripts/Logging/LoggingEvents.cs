using UnityEditor;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Logging
{

[InitializeOnLoad]
internal static class LoggingEvents
{
    static LoggingEvents()
    {
        #region PlayModeStartScene

        SaveValues.PlayModeStartScene.onToggleChanged += PlayModeStartSceneToggle;
        SaveValues.PlayModeStartScene.onStartSceneChanged += PlayModeStartSceneScene;
        SaveValues.PlayModeStartScene.onDisplayToolbarToggleChanged += PlayModeStartSceneToolbarToggle;

        // TODO entered playmode with pmst
        // TODO entered playmode (manually)
        // TODO changed scene (manually)

        #endregion
    }

    #region Private Methods

    private static void AddLog(string categoryName, string logText)
    {
        LoggingManager.LogToCurrentSession(categoryName, logText);
    }

    #endregion

    #region PlayModeStartScene

    private static void PlayModeStartSceneToggle(bool newValue)
    {
        AddLog("PlayModeStartScene / On/Off", newValue ? "Enabled" : "Disabled");
    }

    private static void PlayModeStartSceneScene()
    {
        AddLog(
            "PlayModeStartScene / Changed Scene",
            $"Changed to: {SaveValues.PlayModeStartScene.GetStartScene()?.name}");
    }

    private static void PlayModeStartSceneToolbarToggle(bool newValue)
    {
        AddLog("PlayModeStartScene / Changed DisplayToolbarToggle", newValue ? "Enabled" : "Disabled");
    }

    #endregion
}

}

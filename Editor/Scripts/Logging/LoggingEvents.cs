using MegaPint.Editor.Scripts.Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace MegaPint.Editor.Scripts.Logging
{

[InitializeOnLoad]
internal static class LoggingEvents
{
    static LoggingEvents()
    {
        #region PlayModeStartScene

        SaveValues.PlayModeStartScene.onToggleChanged += PlayModeStartSceneToggle;
        SaveValues.PlayModeStartScene.onStartSceneChanged += PlayModeStartSceneSceneChanged;
        SaveValues.PlayModeStartScene.onDisplayToolbarToggleChanged += PlayModeStartSceneToolbarToggle;

        PlayModeStartScene.onEnteredPlaymode += EnteredPlayMode;
        PlayModeStartScene.onEnteredPlaymodeWithStartScene += PlayModeStartSceneEnteredPlayModeWith;

        EditorSceneManager.sceneOpened += SceneChanged;

        #endregion
    }

    #region Private Methods

    private static void AddLog(string categoryName, string logText)
    {
        LoggingManager.LogToCurrentSession(categoryName, logText);
    }

    #endregion

    #region General

    private static void EnteredPlayMode()
    {
        AddLog("Entered PlayMode", "Entered PlayMode without PlayModeStartScene");
    }

    private static void SceneChanged(Scene scene, OpenSceneMode mode)
    {
        AddLog("Scene Changed", $"Changed to: {scene.name}");
    }

    #endregion

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

    #endregion
}

}

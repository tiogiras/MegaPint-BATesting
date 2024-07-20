using System;
using MegaPint.Editor.Scripts.Drawer;
using MegaPint.Editor.Scripts.Internal;
using MegaPint.Editor.Scripts.Logic;
using MegaPint.Editor.Scripts.Windows;
using MegaPint.SerializeReferenceDropdown.Editor;
using MegaPint.ValidationRequirement;
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

        #region Validators

#if USING_VALIDATORS
        ValidatorView.onOpen += ValidatorsValidatorViewOpen;
        ValidatorView.onClose += ValidatorsValidatorViewClose;
        ValidatorView.onRefreshed += ValidatorsValidatorViewRefresh;
        ValidatorView.onTabChange += ValidatorsValidatorViewTabChange;
        ValidatorView.onItemSelected += ValidatorsValidatorViewItemSelected;
        ValidatorView.onIssueFixed += ValidatorsValidatorViewIssueFixed;
        ValidatorView.onAllIssueFixed += ValidatorsValidatorViewAllIssuesFixed;
        ValidatorView.onFixAll += ValidatorsValidatorViewFixAll;

        ValidationDrawer.onValidateButton += ValidatorsStatusValidateButton;
        ValidationDrawer.onFixAll += ValidatorsStatusFixAll;
        ValidationDrawer.onIssueFixed += ValidatorsStatusIssueFixed;

        ValidatableMonoBehaviour.onValidate += ValidatorsValidatableMonoBehaviourValidate;
        ValidatableMonoBehaviour.onRequirementsChanged += ValidatorsValidatableMonoBehaviourRequirementsChanged;
        ValidatableMonoBehaviour.onPrioritiesChanged += ValidatorsValidatableMonoBehaviourPrioritiesChanged;

        ValidatableMonoBehaviourDrawer.onImport += ValidatorsValidatableMonoBehaviourImport;
        ValidatableMonoBehaviourDrawer.onExport += ValidatorsValidatableMonoBehaviourExport;

        ValidatorSettings.onRequirementsChanged += ValidatorsValidatableMonoBehaviourRequirementsChanged;

        SerializeReferenceDropdownAdvancedDropdown.onSelectedItem += ValidatorsChangedRequirement;

        ScriptableValidationRequirement.onSeverityOverwrite += ValidatorsChangedRequirementSeverityOverwrite;
#endif

        #endregion

        #region BasePackage

        BaseWindow.onOpen += BasePackageBaseWindowOpen;
        BaseWindow.onClose += BasePackageBaseWindowClose;
        BaseWindow.onOpenWithLink += BasePackageBaseWindowOpenWithLink;
        BaseWindow.onSwitchTab += BasePackageBaseWindowSwitchTab;

        BaseWindow.onPackageItemSelected += BasePackageBaseWindowPackageItemSelected;
        BaseWindow.onPackageItemTabSelected += BasePackageBaseWindowPackageItemTabSelected;
        BaseWindow.onInfoItemSelected += BasePackageBaseWindowInfoItemSelected;
        BaseWindow.onSettingItemSelected += BasePackageBaseWindowSettingItemSelected;

        SaveValues.BasePackage.onEditorThemeChanged += BasePackageEditorThemeChanged;
        SaveValues.BasePackage.onUseIconsChanged += BasePackageUseIconsChanged;
        SaveValues.BasePackage.onTesterTokenChanged += BasePackageTesterTokenChanged;

        Windows.PackageManager.onOpen += BasePackagePackageManagerOpen;
        Windows.PackageManager.onClose += BasePackagePackageManagerClose;

        Windows.PackageManager.onItemSelected += BasePackagePackageManagerItemSelected;
        Windows.PackageManager.onImport += BasePackagePackageManagerImport;
        Windows.PackageManager.onImportVariation += BasePackagePackageManagerImportVariation;
        Windows.PackageManager.onImportSample += BasePackagePackageManagerImportSample;
        Windows.PackageManager.onRemove += BasePackagePackageManagerRemove;
        Windows.PackageManager.onUpdate += BasePackagePackageManagerUpdate;

        #endregion
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
#endif

    #endregion

    #region Validators

#if USING_VALIDATORS
    private static void ValidatorsChangedRequirementSeverityOverwrite(ValidationState state, Type type, string name)
    {
        AddLog("Validators / Requirement Severity Overwrite", $"Changed severity of {name}/{type.Name} to {state}");
    }

    private static void ValidatorsValidatableMonoBehaviourPrioritiesChanged(
        string arg1,
        int arg2,
        string arg3,
        int arg4)
    {
        AddLog("ValidatableMonoBehaviour / Priorities Changed", $"Changed priority of {arg1} from {arg2} to {arg4}");
        AddLog("ValidatableMonoBehaviour / Priorities Changed", $"Changed priority of {arg3} from {arg4} to {arg2}");
    }

    private static void ValidatorsValidatableMonoBehaviourImport(string path)
    {
        AddLog("ValidatableMonoBehaviour / Import", path);
    }

    private static void ValidatorsValidatableMonoBehaviourExport(string path)
    {
        AddLog("ValidatableMonoBehaviour / Export", path);
    }

    private static void ValidatorsChangedRequirement(string name)
    {
        AddLog("Validators / Requirement Changed", name);
    }

    private static void ValidatorsValidatableMonoBehaviourRequirementsChanged(string name, bool addedRequirement)
    {
        AddLog(
            "Validators / ValidatableMonoBehaviour RequirementsChanged",
            $"{(addedRequirement ? "Added" : "Removed")} requirement of {name}");
    }

    private static void ValidatorsValidatableMonoBehaviourValidate(string name)
    {
        AddLog("Validators / ValidatableMonoBehaviour Validate", name);
    }

    private static void ValidatorsStatusFixAll(string name)
    {
        AddLog("Validators / Status FixAll", $"Fixed all issues of {name}");
    }

    private static void ValidatorsStatusIssueFixed(string name, string errorName)
    {
        AddLog("Validators / Status IssueFixed", $"Fixed error {errorName} of {name}");
    }

    private static void ValidatorsStatusValidateButton(string name)
    {
        AddLog("Validators / Status ValidateButton", name);
    }

    private static void ValidatorsValidatorViewIssueFixed(string name, string errorName)
    {
        AddLog("Validators / ValidatorView IssueFixed", $"Fixed error {errorName} of {name}");
    }

    private static void ValidatorsValidatorViewAllIssuesFixed(string name)
    {
        AddLog("Validators / ValidatorView AllIssuesFixed", $"Fixed all issues of {name}");
    }

    private static void ValidatorsValidatorViewFixAll()
    {
        AddLog("Validators / ValidatorView FixAll", "Fix All Button");
    }

    private static void ValidatorsValidatorViewItemSelected(string name)
    {
        AddLog("Validators / ValidatorView ItemSelected", name);
    }

    private static void ValidatorsValidatorViewTabChange(bool tab)
    {
        AddLog("Validators / ValidatorView TabChange", tab ? "Scene" : "Project");
    }

    private static void ValidatorsValidatorViewOpen()
    {
        AddLog("Validators / ValidatorView Open/Close", "Opened");
    }

    private static void ValidatorsValidatorViewClose()
    {
        AddLog("Validators / ValidatorView Open/Close", "Closed");
    }

    private static void ValidatorsValidatorViewRefresh()
    {
        AddLog("Validators / ValidatorView Refresh", "Refreshing");
    }
#endif

    #endregion

    #region BasePackage

    private static void BasePackagePackageManagerImportSample(string package, string sample)
    {
        AddLog("BasePackage / PackageManager ImportSample", $"Imported Sample: {sample} of {package}");
    }

    private static void BasePackagePackageManagerImport(string package)
    {
        AddLog("BasePackage / PackageManager Import", $"Imported: {package}");
    }

    private static void BasePackagePackageManagerImportVariation(string package, string variation)
    {
        AddLog("BasePackage / PackageManager ImportVariation", $"Imported: {package} with variation {variation}");
    }

    private static void BasePackagePackageManagerRemove(string package)
    {
        AddLog("BasePackage / PackageManager Remove", $"Removed: {package}");
    }

    private static void BasePackagePackageManagerUpdate(string package)
    {
        AddLog("BasePackage / PackageManager Update", $"Updated: {package}");
    }

    private static void BasePackagePackageManagerItemSelected(string name)
    {
        AddLog("BasePackage / PackageManager ItemSelected", $"Selected: {name}");
    }

    private static void BasePackagePackageManagerOpen()
    {
        AddLog("BasePackage / PackageManager Open/Close", "Opened");
    }

    private static void BasePackagePackageManagerClose()
    {
        AddLog("BasePackage / PackageManager Open/Close", "Closed");
    }

    private static void BasePackageEditorThemeChanged(int newValue)
    {
        var theme = newValue switch
                    {
                        0 => "ActiveEditorTheme",
                        1 => "Dark",
                        var _ => "Light"
                    };

        AddLog("BasePackage / Changed EditorTheme", theme);
    }

    private static void BasePackageUseIconsChanged(bool newValue)
    {
        AddLog("BasePackage / Changed UseIcons", newValue ? "Enabled" : "Disabled");
    }

    private static void BasePackageTesterTokenChanged(string token)
    {
        AddLog("BasePackage / Changed TesterToken", $"Changed to: {token}");
    }

    private static void BasePackageBaseWindowPackageItemTabSelected(string name)
    {
        AddLog("BasePackage / BaseWindow PackageItemTabSelected", $"Selected: {name}");
    }

    private static void BasePackageBaseWindowSettingItemSelected(string name)
    {
        AddLog("BasePackage / BaseWindow SettingItemSelected", $"Selected: {name}");
    }

    private static void BasePackageBaseWindowInfoItemSelected(string name)
    {
        AddLog("BasePackage / BaseWindow InfoItemSelected", $"Selected: {name}");
    }

    private static void BasePackageBaseWindowPackageItemSelected(string name)
    {
        AddLog("BasePackage / BaseWindow PackageItemSelected", $"Selected: {name}");
    }

    private static void BasePackageBaseWindowSwitchTab(string tab)
    {
        AddLog("BasePackage / BaseWindow SwitchTab", $"Switched to: {tab}");
    }

    private static void BasePackageBaseWindowOpenWithLink(string link)
    {
        AddLog("BasePackage / BaseWindow OpenedWithLink", $"Opened with link: {link}");
    }

    private static void BasePackageBaseWindowClose()
    {
        AddLog("BasePackage / BaseWindow Open/Close", "Closed");
    }

    private static void BasePackageBaseWindowOpen()
    {
        AddLog("BasePackage / BaseWindow Open/Close", "Opened");
    }

    #endregion
}

}

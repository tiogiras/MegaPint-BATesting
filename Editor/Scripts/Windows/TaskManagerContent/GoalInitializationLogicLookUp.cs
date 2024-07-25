// TODO Commenting

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using MegaPint.Editor.Scripts.Drawer;
using MegaPint.Editor.Scripts.Internal;
using MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data;
using MegaPint.RepairScene.NonValidatable;
using MegaPint.SerializeReferenceDropdown.Editor;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent
{

internal static class GoalInitializationLogicLookUp
{
    private const string ExecuteTheAutoSaveMenuItem = "Execute The AutoSave MenuItem";
    private const string ClickMe = "Click Me";
    private const string OpenBaseWindow = "Open BaseWindow";
    private const string CameraCaptureRender = "Camera Capture Render";
    private const string CameraCaptureExport = "Camera Capture Export";
    private const string WindowCaptureRender = "Window Capture Render";
    private const string WindowCaptureExport = "Window Capture Export";
    private const string ChangeShortcutStateOfAnyCamera = "Change Shortcut State Of Any Camera";
    private const string TakeManualScreenshots = "Take Manual Screenshots";
    private const string TakeRuntimeScreenshots1 = "Take Runtime Screenshots I";
    private const string TakeScreenshotOfCameraCaptureConfig1 = "Take Screenshot Of Camera Capture Config I";
    private const string TakeRuntimeScreenshots2 = "Take Runtime Screenshots II";
    private const string TakeScreenshotOfCameraCaptureConfig2 = "Take Screenshot Of Camera Capture Config II";
    private const string AddRequirement = "Add Requirement";
    private const string RemoveRequirement = "Remove Requirement";
    private const string ChangeRequirement = "Change Requirement";
    private const string FixAnyIssue = "Fix Any Issue";
    private const string ImportRequirements = "Import Requirements";
    private const string ExportRequirements = "Export Requirements";
    private const string AddVSRequirement = "Validator Settings Add Requirement";
    private const string ChangeVSRequirement = "Validator Settings Change Requirement";
    private const string RemoveVSRequirement = "Validator Settings Remove Requirement";
    private const string FixAllIssuesInScene = "Fix All Issues In The Opened Scene";
    private const string FixTestValidationPrefab = "Fix The Test Validation Prefab";
    private const string OpenValidatorView = "Open Validator View";
    private const string RepairAScene1 = "Repair A Scene I";
    

    public static readonly Dictionary <string, Action> InitializationLookUp = new()
    {
        {ExecuteTheAutoSaveMenuItem, () => {ContextMenu.onMenuItemInvoked += OnExecuteTheAutoSaveMenuItem;}},
        {ClickMe, () => {SceneManagerTestProcedure.onWin += OnClickMe;}},
        {OpenBaseWindow, () => {ContextMenu.onMenuItemInvoked += OnOpenBaseWindow;}},
        {CameraCaptureRender, () => {CameraCaptureDrawer.onCameraCaptureRendered += OnCameraCaptureRendered;}},
        {CameraCaptureExport, () => {CameraCaptureDrawer.onCameraCaptureExported += OnCameraCaptureExported;}},
        {WindowCaptureRender, () => {WindowCapture.onRender += OnWindowCaptureRendered;}},
        {WindowCaptureExport, () => {WindowCapture.onExport += OnWindowCaptureExported;}},
        {ChangeShortcutStateOfAnyCamera, () => {ShortcutCapture.onChangeState += OnShortcutStateChanged;}},
        {TakeManualScreenshots, () => {TaskManager.onForceFinishTask += OnTakeManualScreenshots;}},
        {TakeRuntimeScreenshots1, () => {TaskManager.onForceFinishTask += OnTakRuntimeScreenshots1;}},
        {
            TakeScreenshotOfCameraCaptureConfig1,
            () => {TaskManager.onForceFinishTask += OnTakeScreenshotOfCameraCaptureConfig1;}
        },
        {TakeRuntimeScreenshots2, () => {TaskManager.onForceFinishTask += OnTakRuntimeScreenshots2;}},
        {
            TakeScreenshotOfCameraCaptureConfig2,
            () => {TaskManager.onForceFinishTask += OnTakeScreenshotOfCameraCaptureConfig2;}
        },
        {
            FixAnyIssue, () =>
            {
                ValidationDrawer.onFixAll += OnFixAnyIssue;
                ValidationDrawer.onIssueFixed += OnFixAnyIssue;
            }
        },
        {AddRequirement, () => {ValidatableMonoBehaviour.onRequirementsChanged += OnAddRequirement;}},
        {RemoveRequirement, () => {ValidatableMonoBehaviour.onRequirementsChanged += OnRemoveRequirement;}},
        {ChangeRequirement, () => {SerializeReferenceDropdownAdvancedDropdown.onSelectedItem += OnChangeRequirement;}},
        {ImportRequirements, () => {ValidatableMonoBehaviourDrawer.onImport += OnImportRequirements;}},
        {ExportRequirements, () => {ValidatableMonoBehaviourDrawer.onExport += OnExportRequirements;}},
        {AddVSRequirement, () => {ValidatorSettings.onRequirementsChanged += OnAddVSRequirement;}},
        {RemoveVSRequirement, () => {ValidatorSettings.onRequirementsChanged += OnRemoveVSRequirement;}},
        {
            ChangeVSRequirement,
            () => {SerializeReferenceDropdownAdvancedDropdown.onSelectedItem += OnChangeVSRequirement;}
        },
        {OpenValidatorView, () => {ValidatorView.onOpen += OnValidatorView;}},
        {FixTestValidationPrefab, () => {ValidatableMonoBehaviour.onValidated += OnFixTestValidationPrefab;}},
        {FixAllIssuesInScene, () => {SceneManagerValidatorView.onWin += OnFixAllIssuesInScene;}},
        {RepairAScene1, () => {SceneManager.onWin += OnRepairScene1;}}
    };

    public static readonly Dictionary <string, Action> DeInitializationLookUp = new()
    {
        {ExecuteTheAutoSaveMenuItem, () => {ContextMenu.onMenuItemInvoked -= OnExecuteTheAutoSaveMenuItem;}},
        {ClickMe, () => {SceneManagerTestProcedure.onWin -= OnClickMe;}},
        {OpenBaseWindow, () => {ContextMenu.onMenuItemInvoked -= OnOpenBaseWindow;}},
        {CameraCaptureRender, () => {CameraCaptureDrawer.onCameraCaptureRendered -= OnCameraCaptureRendered;}},
        {CameraCaptureExport, () => {CameraCaptureDrawer.onCameraCaptureExported -= OnCameraCaptureExported;}},
        {WindowCaptureRender, () => {WindowCapture.onRender -= OnWindowCaptureRendered;}},
        {WindowCaptureExport, () => {WindowCapture.onExport -= OnWindowCaptureExported;}},
        {ChangeShortcutStateOfAnyCamera, () => {ShortcutCapture.onChangeState -= OnShortcutStateChanged;}},
        {TakeManualScreenshots, () => {TaskManager.onForceFinishTask -= OnTakeManualScreenshots;}},
        {TakeRuntimeScreenshots1, () => {TaskManager.onForceFinishTask -= OnTakRuntimeScreenshots1;}},
        {
            TakeScreenshotOfCameraCaptureConfig1,
            () => {TaskManager.onForceFinishTask -= OnTakeScreenshotOfCameraCaptureConfig1;}
        },
        {TakeRuntimeScreenshots2, () => {TaskManager.onForceFinishTask -= OnTakRuntimeScreenshots2;}},
        {
            TakeScreenshotOfCameraCaptureConfig2,
            () => {TaskManager.onForceFinishTask -= OnTakeScreenshotOfCameraCaptureConfig2;}
        },
        {AddRequirement, () => {ValidatableMonoBehaviour.onRequirementsChanged -= OnAddRequirement;}},
        {RemoveRequirement, () => {ValidatableMonoBehaviour.onRequirementsChanged -= OnRemoveRequirement;}},
        {
            FixAnyIssue, () =>
            {
                ValidationDrawer.onFixAll -= OnFixAnyIssue;
                ValidationDrawer.onIssueFixed -= OnFixAnyIssue;
            }
        },
        {ChangeRequirement, () => {SerializeReferenceDropdownAdvancedDropdown.onSelectedItem -= OnChangeRequirement;}},
        {ImportRequirements, () => {ValidatableMonoBehaviourDrawer.onImport += OnImportRequirements;}},
        {ExportRequirements, () => {ValidatableMonoBehaviourDrawer.onExport += OnExportRequirements;}},
        {AddVSRequirement, () => {ValidatableMonoBehaviour.onRequirementsChanged -= OnAddVSRequirement;}},
        {RemoveVSRequirement, () => {ValidatableMonoBehaviour.onRequirementsChanged -= OnRemoveVSRequirement;}},
        {
            ChangeVSRequirement,
            () => {SerializeReferenceDropdownAdvancedDropdown.onSelectedItem -= OnChangeVSRequirement;}
        },
        {OpenValidatorView, () => {ValidatorView.onOpen -= OnValidatorView;}},
        {FixTestValidationPrefab, () => {ValidatableMonoBehaviour.onValidated -= OnFixTestValidationPrefab;}},
        {FixAllIssuesInScene, () => {SceneManagerValidatorView.onWin -= OnFixAllIssuesInScene;}},
        {RepairAScene1, () => {SceneManager.onWin -= OnRepairScene1;}}
    };

    #region Private Methods

    private static void MarkGoalAsDone(string goalName)
    {
        Goal goal = Goal.ActiveGoals.Find(goal => goal.title.Equals(goalName));
        Goal.MarkGoalAsDone(goal);
    }

    private static void MarkGoalAsDoneIf(string goalName, bool condition)
    {
        if (!condition)
            return;

        MarkGoalAsDone(goalName);
    }

    private static void OnAddRequirement(string _, bool added)
    {
        MarkGoalAsDoneIf(AddRequirement, added);
    }

    private static void OnAddVSRequirement(string _, bool added)
    {
        MarkGoalAsDoneIf(AddVSRequirement, added);
    }

    private static void OnCameraCaptureExported(string _)
    {
        MarkGoalAsDone(CameraCaptureExport);
    }

    private static void OnCameraCaptureRendered(string _)
    {
        MarkGoalAsDone(CameraCaptureRender);
    }

    private static void OnChangeRequirement(string _)
    {
        MarkGoalAsDone(ChangeRequirement);
    }

    private static void OnChangeVSRequirement(string _)
    {
        MarkGoalAsDone(ChangeVSRequirement);
    }

    private static void OnClickMe()
    {
        MarkGoalAsDone(ClickMe);
    }

    private static void OnExecuteTheAutoSaveMenuItem(ContextMenu.MenuItemSignature signature)
    {
        MarkGoalAsDoneIf(ExecuteTheAutoSaveMenuItem, signature.signature.Equals("AutoSave"));
    }

    private static void OnExportRequirements(string _)
    {
        MarkGoalAsDone(ExportRequirements);
    }

    private static void OnFixAllIssuesInScene()
    {
        MarkGoalAsDone(FixAllIssuesInScene);
    }

    private static void OnFixAnyIssue(string _, string __)
    {
        MarkGoalAsDone(FixAnyIssue);
    }

    private static void OnFixAnyIssue(string _)
    {
        MarkGoalAsDone(FixAnyIssue);
    }

    private static void OnFixTestValidationPrefab(string name, ValidationState state)
    {
        if (!name.Equals("Test Validation Prefab"))
            return;

        MarkGoalAsDoneIf(FixTestValidationPrefab, state == ValidationState.Ok);
    }

    private static void OnImportRequirements(string _)
    {
        MarkGoalAsDone(ImportRequirements);
    }

    private static void OnOpenBaseWindow(ContextMenu.MenuItemSignature signature)
    {
        MarkGoalAsDoneIf(
            OpenBaseWindow,
            signature.signature.Equals("Open") || signature.signature.Equals("Open (Link)"));
    }

    private static void OnRemoveRequirement(string _, bool added)
    {
        MarkGoalAsDoneIf(RemoveRequirement, !added);
    }

    private static void OnRemoveVSRequirement(string _, bool added)
    {
        MarkGoalAsDoneIf(RemoveVSRequirement, !added);
    }

    private static void OnRepairScene1()
    {
        MarkGoalAsDone(RepairAScene1);
    }

    private static void OnShortcutStateChanged(string _, bool __)
    {
        MarkGoalAsDone(ChangeShortcutStateOfAnyCamera);
    }

    private static void OnTakeManualScreenshots()
    {
        MarkGoalAsDone(TakeManualScreenshots);
    }

    private static void OnTakeScreenshotOfCameraCaptureConfig1()
    {
        MarkGoalAsDone(TakeScreenshotOfCameraCaptureConfig1);
    }

    private static void OnTakeScreenshotOfCameraCaptureConfig2()
    {
        MarkGoalAsDone(TakeScreenshotOfCameraCaptureConfig2);
    }

    private static void OnTakRuntimeScreenshots1()
    {
        MarkGoalAsDone(TakeRuntimeScreenshots1);
    }

    private static void OnTakRuntimeScreenshots2()
    {
        MarkGoalAsDone(TakeRuntimeScreenshots2);
    }

    private static void OnValidatorView()
    {
        MarkGoalAsDone(OpenValidatorView);
    }

    private static void OnWindowCaptureExported()
    {
        MarkGoalAsDone(WindowCaptureExport);
    }

    private static void OnWindowCaptureRendered(string _)
    {
        MarkGoalAsDone(WindowCaptureRender);
    }

    #endregion
}

}
#endif

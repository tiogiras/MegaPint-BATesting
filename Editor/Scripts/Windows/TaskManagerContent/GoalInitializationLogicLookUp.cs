// TODO Commenting

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using MegaPint.Editor.Scripts.Drawer;
using MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent
{

internal static class GoalInitializationLogicLookUp
{
    private const string ExecuteTheAutoSaveMenuItem = "Execute The AutoSave MenuItem";
    private const string OpenBaseWindow = "Open BaseWindow";
    private const string CameraCaptureRender = "Camera Capture Render";
    private const string CameraCaptureExport = "Camera Capture Export";
    private const string WindowCaptureRender = "Window Capture Render";
    private const string WindowCaptureExport = "Window Capture Export";
    private const string ChangeShortcutStateOfAnyCamera = "Change Shortcut State Of Any Camera";
    private const string TakeManualScreenshots = "Take Manual Screenshots";

    public static readonly Dictionary <string, Action> InitializationLookUp = new()
    {
        {ExecuteTheAutoSaveMenuItem, () => {ContextMenu.onMenuItemInvoked += OnExecuteTheAutoSaveMenuItem;}},
        {OpenBaseWindow, () => {ContextMenu.onMenuItemInvoked += OnOpenBaseWindow;}},
        {CameraCaptureRender, () => {CameraCaptureDrawer.onCameraCaptureRendered += OnCameraCaptureRendered;}},
        {CameraCaptureExport, () => {CameraCaptureDrawer.onCameraCaptureExported += OnCameraCaptureExported;}},
        {WindowCaptureRender, () => {WindowCapture.onRender += OnWindowCaptureRendered;}},
        {WindowCaptureExport, () => {WindowCapture.onExport += OnWindowCaptureExported;}},
        {ChangeShortcutStateOfAnyCamera, () => {ShortcutCapture.onChangeState += OnShortcutStateChanged;}},
        {TakeManualScreenshots, () => {TaskManager.onForceFinishTask += OnTakeManualScreenshots;}}
    };

    public static readonly Dictionary <string, Action> DeInitializationLookUp = new()
    {
        {ExecuteTheAutoSaveMenuItem, () => {ContextMenu.onMenuItemInvoked -= OnExecuteTheAutoSaveMenuItem;}},
        {OpenBaseWindow, () => {ContextMenu.onMenuItemInvoked -= OnOpenBaseWindow;}},
        {CameraCaptureRender, () => {CameraCaptureDrawer.onCameraCaptureRendered -= OnCameraCaptureRendered;}},
        {CameraCaptureExport, () => {CameraCaptureDrawer.onCameraCaptureExported -= OnCameraCaptureExported;}},
        {WindowCaptureRender, () => {WindowCapture.onRender -= OnWindowCaptureRendered;}},
        {WindowCaptureExport, () => {WindowCapture.onExport -= OnWindowCaptureExported;}},
        {ChangeShortcutStateOfAnyCamera, () => {ShortcutCapture.onChangeState -= OnShortcutStateChanged;}},
        {TakeManualScreenshots, () => {TaskManager.onForceFinishTask -= OnTakeManualScreenshots;}}
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

    private static void OnCameraCaptureExported(string _)
    {
        MarkGoalAsDone(CameraCaptureExport);
    }

    private static void OnCameraCaptureRendered(string _)
    {
        MarkGoalAsDone(CameraCaptureRender);
    }

    private static void OnExecuteTheAutoSaveMenuItem(ContextMenu.MenuItemSignature signature)
    {
        MarkGoalAsDoneIf(ExecuteTheAutoSaveMenuItem, signature.signature.Equals("AutoSave"));
    }

    private static void OnOpenBaseWindow(ContextMenu.MenuItemSignature signature)
    {
        MarkGoalAsDoneIf(
            OpenBaseWindow,
            signature.signature.Equals("Open") || signature.signature.Equals("Open (Link)"));
    }

    private static void OnShortcutStateChanged(string _, bool __)
    {
        MarkGoalAsDone(ChangeShortcutStateOfAnyCamera);
    }

    private static void OnTakeManualScreenshots()
    {
        MarkGoalAsDone(TakeManualScreenshots);
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

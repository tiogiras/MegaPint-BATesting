// TODO Commenting

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent
{

internal static class GoalInitializationLogicLookUp
{
    private const string ExecuteTheAutoSaveMenuItem = "Execute The AutoSave MenuItem";
    private const string OpenBaseWindow = "Open BaseWindow";

    public static readonly Dictionary <string, Action> InitializationLookUp = new()
    {
        {ExecuteTheAutoSaveMenuItem, () => {ContextMenu.onMenuItemInvoked += OnExecuteTheAutoSaveMenuItem;}},
        {OpenBaseWindow, () => {ContextMenu.onMenuItemInvoked += OnOpenBaseWindow;}}
    };

    public static readonly Dictionary <string, Action> DeInitializationLookUp = new()
    {
        {ExecuteTheAutoSaveMenuItem, () => {ContextMenu.onMenuItemInvoked -= OnExecuteTheAutoSaveMenuItem;}},
        {OpenBaseWindow, () => {ContextMenu.onMenuItemInvoked -= OnOpenBaseWindow;}}
    };

    #region Private Methods

    private static void OnExecuteTheAutoSaveMenuItem(ContextMenu.MenuItemSignature signature)
    {
        if (!signature.signature.Equals("AutoSave"))
            return;

        Goal goal = Goal.ActiveGoals.Find(goal => goal.title.Equals(ExecuteTheAutoSaveMenuItem));
        Goal.MarkGoalAsDone(goal);
    }

    private static void OnOpenBaseWindow(ContextMenu.MenuItemSignature signature)
    {
        if (!signature.signature.Equals("Open") && !signature.signature.Equals("Open (Link)"))
            return;

        Goal goal = Goal.ActiveGoals.Find(goal => goal.title.Equals(OpenBaseWindow));
        Goal.MarkGoalAsDone(goal);
    }

    #endregion
}

}
#endif

// TODO Commenting

#if UNITY_EDITOR
using System;
using MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data;
using UnityEditor;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent
{

internal static class GoalsLogic
{
    public static Action <Goal> onGoalDone;

    public static void MarkGoalAsDone(Goal goal)
    {
        goal.Done = true;
        onGoalDone?.Invoke(goal);
    }
}

}
#endif

// TODO Commenting

#if UNITY_EDITOR
using System;
using UnityEditor;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent
{

internal static class GoalsLogic
{
    public static Action <Goal> onGoalDone;

    public static void MarkGoalAsDone(Goal goal)
    {
        goal.done = true;
        EditorUtility.SetDirty(goal);
        
        onGoalDone?.Invoke(goal);
    }
}

}
#endif

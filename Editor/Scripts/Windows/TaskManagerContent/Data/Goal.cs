// TODO Commenting

#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

[CreateAssetMenu(fileName = "Goal", menuName = "MegaPint/Goal", order = 0)] // TODO remove this line
internal class Goal : ScriptableObject
{
    public static Action <Goal> onGoalDone;

    public bool Done
    {
        get => _done;
        set
        {
            _done = value;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }

    public string title;
    [TextArea] public string hint;

    [SerializeField] private bool _done;

    #region Public Methods

    public static void MarkGoalAsDone(Goal goal)
    {
        goal.Done = true;
        onGoalDone?.Invoke(goal);
    }

    public void ResetValues()
    {
        Done = false;
    }

    #endregion
}

}
#endif

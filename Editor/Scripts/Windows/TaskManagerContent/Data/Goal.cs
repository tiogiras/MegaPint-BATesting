// TODO Commenting

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

[CreateAssetMenu(fileName = "Goal", menuName = "MegaPint/Goal", order = 0)] // TODO remove this line
internal class Goal : ScriptableObject
{
    public static readonly List <Goal> ActiveGoals = new();

    public static Action <Goal> onGoalDone;

    public bool Done
    {
        get => _done;
        private set
        {
            _done = value;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }

    public string title;
    [TextArea] public string hint;
    public bool hasInitializationLogic;

    [SerializeField] private bool _done;

    #region Public Methods

    public static void MarkGoalAsDone(Goal goal)
    {
        goal.Done = true;

        ActiveGoals.Remove(goal);
        onGoalDone?.Invoke(goal);
    }

    public void ResetValues()
    {
        Done = false;
    }

    public void SetActive()
    {
        if (!ActiveGoals.Contains(this))
            ActiveGoals.Add(this);
    }

    #endregion
}

}
#endif

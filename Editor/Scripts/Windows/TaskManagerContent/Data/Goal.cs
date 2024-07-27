#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

/// <summary> Holds data about a goal </summary>
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

    /// <summary> Mark a goal as done </summary>
    /// <param name="goal"> Targeted goal </param>
    public static void MarkGoalAsDone(Goal goal)
    {
        goal.Done = true;

        ActiveGoals.Remove(goal);
        onGoalDone?.Invoke(goal);
    }

    /// <summary> Reset all values of the goal </summary>
    public void ResetValues()
    {
        Done = false;
    }

    /// <summary> Set the goal to active </summary>
    public void SetActive()
    {
        if (!ActiveGoals.Contains(this))
            ActiveGoals.Add(this);
    }

    #endregion
}

}
#endif

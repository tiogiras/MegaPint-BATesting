#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

/// <summary> Holds data about a goal </summary>
public class Goal : ScriptableObject
{
    public static readonly List <Goal> ActiveGoals = new();

    public static Action <Goal> onGoalDone;

    public bool Done
    {
        get
        {
            if (_doneInitialized)
                return _done;

            _done = SaveValues.TestData.GetValue(title, "0", false);
            _doneInitialized = true;

            return _done;
        }
        private set
        {
            SaveValues.TestData.SetValue(title, "0", value);
            _done = value;
        }
    }

    [SerializeField] private string _guid;

    public string title;
    [TextArea] public string hint;
    public bool hasInitializationLogic;

    private bool _done;
    private bool _doneInitialized;

    #region Unity Event Functions

    private void OnValidate()
    {
        _doneInitialized = false;
        
        if (!string.IsNullOrEmpty(_guid))
            return;
        
        _guid = Guid.NewGuid().ToString();
        
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
    }

    #endregion

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

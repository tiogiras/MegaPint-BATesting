#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

/// <summary> Stores all tasks and the current progress </summary>
internal class TaskManagerData : ScriptableObject
{
    public int TasksCount => _tasks.Count;

    public int CurrentTaskIndex
    {
        get
        {
            if (_currentTaskIndexInitialized)
                return _currentTaskIndex;

            _currentTaskIndex = SaveValues.TestData.GetValue("TM", "0", 0);
            _currentTaskIndexInitialized = true;

            return _currentTaskIndex;
        }
        set
        {
            SaveValues.TestData.SetValue("TM", "0", value);
            _currentTaskIndex = value;
        }
    }

    public List <Task> Tasks => _tasks;

    [SerializeField] private string _guid;
    [SerializeField] private List <Task> _tasks;
    
    private int _currentTaskIndex;
    private bool _currentTaskIndexInitialized;

    #region Unity Event Functions

    private void OnValidate()
    {
        _currentTaskIndexInitialized = false;
        
        if (!string.IsNullOrEmpty(_guid))
            return;
        
        _guid = Guid.NewGuid().ToString();
        
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
    }

    #endregion

    #region Public Methods

    /// <summary> Get the current active task </summary>
    /// <returns> Current task </returns>
    public Task CurrentTask()
    {
        return _tasks[CurrentTaskIndex];
    }

    /// <summary> Get the next task </summary>
    /// <returns> Next task </returns>
    public Task NextTask()
    {
        return CurrentTaskIndex == _tasks.Count ? null : _tasks[CurrentTaskIndex + 1];
    }

    /// <summary> Reset all tasks </summary>
    public void ResetValues()
    {
        CurrentTaskIndex = 0;

        foreach (Task task in _tasks)
            task.ResetValues();
    }

    #endregion
}

}
#endif

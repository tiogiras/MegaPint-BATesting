#if UNITY_EDITOR
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
        get => _currentTaskIndex;
        set
        {
            _currentTaskIndex = value;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }

    public List <Task> Tasks => _tasks;

    [SerializeField] private List <Task> _tasks;
    [SerializeField] private int _currentTaskIndex;

    #region Public Methods

    /// <summary> Get the current active task </summary>
    /// <returns> Current task </returns>
    public Task CurrentTask()
    {
        return _tasks[_currentTaskIndex];
    }

    /// <summary> Get the next task </summary>
    /// <returns> Next task </returns>
    public Task NextTask()
    {
        return _currentTaskIndex == _tasks.Count ? null : _tasks[_currentTaskIndex + 1];
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

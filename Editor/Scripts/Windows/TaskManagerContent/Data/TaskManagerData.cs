// TODO commenting

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

[CreateAssetMenu(
    fileName = "TaskManagerData",
    menuName = "MegaPint/TaskManagerData",
    order = 0)] // TODO remove this line
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

    public void ResetValues()
    {
        CurrentTaskIndex = 0;
        
        foreach (Task task in _tasks)
            task.ResetValues();
    }

    #region Public Methods

    public Task CurrentTask()
    {
        return _tasks[_currentTaskIndex];
    }

    #endregion
}

}
#endif

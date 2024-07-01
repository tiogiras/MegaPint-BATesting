// TODO commenting

#if UNITY_EDITOR
using System.Collections.Generic;
using MegaPint.Editor.Scripts;
using UnityEngine;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows.TaskManagerContent
{

[CreateAssetMenu(
    fileName = "TaskManagerData",
    menuName = "MegaPint/TaskManagerData",
    order = 0)] // TODO remove this line
internal class TaskManagerData : ScriptableObject
{
    [SerializeField] private List <Task> _tasks;

    public int TasksCount => _tasks.Count;
    public int currentTaskIndex;
    
    #region Public Methods

    public Task CurrentTask()
    {
        return _tasks[currentTaskIndex];
    }

    #endregion
}

}
#endif

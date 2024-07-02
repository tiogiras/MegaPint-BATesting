// TODO commenting

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

[CreateAssetMenu(fileName = "TaskData", menuName = "MegaPint/TaskData", order = 0)] // TODO remove this line
internal class Task : ScriptableObject
{
    public float NeededTime
    {
        get => _neededTime;
        set
        {
            _neededTime = value;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }

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

    public Chapter chapter;
    public string taskName;
    [TextArea] public string taskDescription;
    public List <Requirement> taskRequirements;
    public bool hasDoableTask;
    public SceneAsset scene;
    public bool startInPlayMode;
    public List <Goal> goals;

    [SerializeField] private bool _done;
    [SerializeField] private float _neededTime;

    #region Public Methods

    public void ResetValues()
    {
        foreach (Requirement requirement in taskRequirements)
            requirement.ResetValues();

        foreach (Goal goal in goals)
            goal.ResetValues();

        Done = false;
    }

    public void SetRequirementDone(int index, bool done)
    {
        taskRequirements[index].Done = done;
    }

    public void SetRequirementsDone(bool done)
    {
        for (var i = 0; i < taskRequirements.Count; i++)
            SetRequirementDone(i, done);
    }

    #endregion
}

}
#endif

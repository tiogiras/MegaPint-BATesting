// TODO commenting

#if UNITY_EDITOR
using System.Collections.Generic;
using MegaPint.Editor.Scripts.Windows.TaskManagerContent;
using UnityEditor;
using UnityEngine;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows.TaskManagerContent
{

[CreateAssetMenu(fileName = "TaskData", menuName = "MegaPint/TaskData", order = 0)] // TODO remove this line
internal class Task : ScriptableObject
{
    public Chapter chapter;
    public string taskName;
    [TextArea] public string taskDescription;
    public List <Requirement> taskRequirements;
    public bool hasDoableTask;
    public SceneAsset scene;

    public List <Goal> goals;

    public float neededTime;

    #region Public Methods

    public void SetRequirementDone(int index, bool done)
    {
        Requirement requirement = taskRequirements[index];

        requirement.done = done;
        EditorUtility.SetDirty(requirement);
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
